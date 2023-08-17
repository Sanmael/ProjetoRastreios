using System.Net;
using System.Web;
using SendNotification.DTO;
using System.Globalization;
using Entities;
using MeuContexto.UOW;
using Newtonsoft.Json;
using Azure;
using System.Diagnostics.SymbolStore;
using System.Threading.Tasks;

namespace SendNotification.Service
{
    internal class SendEmail
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;

        public SendEmail(IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
        }

        public async Task GetCodesAsync()
        {
            List<TrackingCode> codes = _unitOfWork.TrackingService.GetTrackingCodeActive();

            IEnumerable<IGrouping<int, TrackingCode>> trackingCodes = codes.GroupBy(x => x.SubPersonId);
            //refatorar depois
            foreach (IGrouping<int, TrackingCode> trackingCode in trackingCodes)
            {
                SubPerson subPerson = _unitOfWork.SubPersonService.GetSubPersonById(trackingCode.Key);

                List<TrackingCode> trackingCodesSubPerson = trackingCode.Where(x => x.SubPersonId == subPerson.SubPersonId).ToList();

                await ProcessTrackingCodeAsync(trackingCodesSubPerson, subPerson);
            }
        }
        public async Task ProcessTrackingCodeAsync(List<TrackingCode> trackingCode, SubPerson subPerson)
        {
            foreach (TrackingCode code in trackingCode)
            {
                try
                {
                    string apiUrl = $"{_configuration["ApiConfig:Url"]}{code.Code}";

                    using (HttpClient httpClient = new HttpClient())
                    {
                        UriBuilder requestUri = new UriBuilder(apiUrl);

                        var query = HttpUtility.ParseQueryString(requestUri.Query);
                        query["user"] = _configuration["ApiConfig:User"];
                        query["token"] = _configuration["ApiConfig:Token"];
                        query["codigo"] = code.Code;

                        requestUri.Query = query.ToString();

                        HttpResponseMessage response = await httpClient.GetAsync(requestUri.ToString());

                        await ValidateResponseAsync(response, code, subPerson);
                    }
                }
                catch (Exception ex)
                {
                    _unitOfWork.LoggerService.LogError(ex, ex.Message);
                }
            }
        }
        public async Task ValidateResponseAsync(HttpResponseMessage response, TrackingCode trackingCode, SubPerson subPerson)
        {
            if (response.IsSuccessStatusCode)
            {
                string responseBody = await response.Content.ReadAsStringAsync();

                TrackingInfoDto trackingInfo = JsonConvert.DeserializeObject<TrackingInfoDto>(responseBody);

                TrackingCodeEvents lastEventAdd = _unitOfWork.TrackingCodeEvents.GetTrackingEventsByTrackingCodeId(trackingCode.TrackingCodeId).OrderBy(x => x.CreationDate).OrderByDescending(x => x.CreationDate).FirstOrDefault();

                UpdateAndValidateTrackingCodeStatusAsync(trackingCode, lastEventAdd, trackingInfo.eventos);

                if (trackingInfo.eventos != null && trackingInfo.eventos.Any())
                {
                    SaveNewEvents(trackingCode, trackingInfo.eventos, subPerson);

                    NexTry(trackingCode, 3);
                }
            }
            else if (response.StatusCode == HttpStatusCode.RequestTimeout || response.StatusCode == HttpStatusCode.InternalServerError)
                NexTry(trackingCode, 3);

            else
                SetError(trackingCode);
        }
        public void UpdateAndValidateTrackingCodeStatusAsync(TrackingCode trackingCode, TrackingCodeEvents lastEvent, List<EventDto> events)
        {
            if (lastEvent != null)
            {
                if (lastEvent.Status == "Objeto entregue ao destinatário")
                {
                    trackingCode.Status = TrackingCodeStatus.Delivered;
                    _unitOfWork.TrackingService.UpdateTrackingCode(trackingCode);
                    return;
                }

                events.RemoveAll(x => DateTime.Parse(x.data).Add(TimeSpan.Parse(x.hora)) <= lastEvent.CreationDate);
            }
            else if (lastEvent == null && !events.Any())
            {
                trackingCode.NumberOfTries++;

                if (trackingCode.NumberOfTries == 3)
                {
                    SetError(trackingCode);
                }

                NexTry(trackingCode, 1);
            }
        }
        public void SaveNewEvents(TrackingCode trackingCode, List<EventDto> events, SubPerson subPerson)
        {
            foreach (EventDto evento in events)
            {
                if (TimeSpan.TryParseExact(evento.hora, "hh\\:mm", CultureInfo.InvariantCulture, out TimeSpan horaMinuto) &&
                    DateTime.TryParseExact(evento.data, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime data))
                {
                    DateTime datetime = data.Add(horaMinuto);

                    TrackingCodeEvents trackingCodeEventModel = new TrackingCodeEvents()
                    {
                        TrakingCodeId = trackingCode.TrackingCodeId,
                        CreationDate = datetime,
                        Status = evento.status,
                        Local = evento.local
                    };

                    _unitOfWork.TrackingCodeEvents.NewTrackingCodeEvents(trackingCodeEventModel);
                }
            }

            //EventDto eventDto = events.First();

            //_unitOfWork.MailQueueRepository.AddNewMailQueue(subPerson.Email, eventDto.subStatus, )
        }
        public void NexTry(TrackingCode trackingCode, double hour)
        {
            trackingCode.NextSearch = DateTime.Now.AddHours(hour);
            _unitOfWork.TrackingService.UpdateTrackingCode(trackingCode);
        }
        public void SetError(TrackingCode trackingCode)
        {
            trackingCode.Status = TrackingCodeStatus.Error;
            _unitOfWork.TrackingService.UpdateTrackingCode(trackingCode);
        }
    }
}
