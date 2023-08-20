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
using MeuContexto.Context;
using Microsoft.EntityFrameworkCore;
using MeuContexto.Repositorie;
using System.Linq;

namespace SendNotification.Service
{
    internal class SendEmail
    {
        private IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;

        public SendEmail(IConfiguration configuration, IUnitOfWork unitOfWork)
        {
            _configuration = configuration;

            _unitOfWork = unitOfWork;/*new AbstractContext().AbstractContextReturn(RepositoryType.Dapper, _configuration["ConnectionStrings:DefaultConnections"]);*/
        }

        public async Task GetCodesAsync()
        {
            List<TrackingCode> codes = await _unitOfWork.TrackingService.GetTrackingCodeActiveAsync();

            IEnumerable<IGrouping<int, TrackingCode>> trackingCodes = codes.GroupBy(x => x.SubPersonId);

            foreach(var trackingCode in trackingCodes)
            {
                SubPerson subPerson = await _unitOfWork.SubPersonService.GetSubPersonByIdAsync(trackingCode.Key);

                List<TrackingCode> trackingCodesSubPerson = trackingCode.Where(x => x.SubPersonId == subPerson.SubPersonId).ToList();

                await ProcessTrackingCodeAsync(trackingCodesSubPerson, subPerson);
            }                                                                         
        }
        public async Task ProcessTrackingCodeAsync(List<TrackingCode> trackingCode, SubPerson subPerson)
        {
            foreach (TrackingCode trackingCod in trackingCode)
            {
                try
                {
                    string apiUrl = $"{_configuration["ApiConfig:Url"]}{trackingCod.Code}";

                    using (HttpClient httpClient = new HttpClient())
                    {
                        UriBuilder requestUri = new UriBuilder(apiUrl);

                        var query = HttpUtility.ParseQueryString(requestUri.Query);
                        query["user"] = _configuration["ApiConfig:User"];
                        query["token"] = _configuration["ApiConfig:Token"];
                        query["codigo"] = trackingCod.Code;

                        requestUri.Query = query.ToString();

                        HttpResponseMessage response = await httpClient.GetAsync(requestUri.ToString());

                        await ValidateResponseAsync(response, trackingCod, subPerson);
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

                TrackingCodeEvents lastEventAdd = await _unitOfWork.TrackingCodeEvents.GetLastTrackingCodeByTrackingCodeId(trackingCode.TrackingCodeId); 

                await UpdateAndValidateTrackingCodeStatusAsync(trackingCode, lastEventAdd, trackingInfo.eventos);

                if (trackingInfo.eventos != null && trackingInfo.eventos.Any())
                {
                    await SaveNewEventsAsync(trackingCode, trackingInfo.eventos, subPerson);

                    await NexTryAsync(trackingCode, 3);
                }
            }
            else if (response.StatusCode == HttpStatusCode.RequestTimeout || response.StatusCode == HttpStatusCode.InternalServerError)
                await NexTryAsync(trackingCode, 3);

            else
                await SetErrorAsync(trackingCode);
        }
        public async Task UpdateAndValidateTrackingCodeStatusAsync(TrackingCode trackingCode, TrackingCodeEvents lastEvent, List<EventDto> events)
        {
            if (lastEvent != null)
            {
                if (lastEvent.Status == "Objeto entregue ao destinatário")
                {
                    trackingCode.Status = TrackingCodeStatus.Delivered;
                    await _unitOfWork.TrackingService.UpdateTrackingCode(trackingCode);                    
                }

                events.RemoveAll(x => DateTime.Parse(x.data).Add(TimeSpan.Parse(x.hora)) <= lastEvent.CreationDate);
            }
            else if (lastEvent == null && !events.Any())
            {
                trackingCode.NumberOfTries++;

                if (trackingCode.NumberOfTries == 3)
                {
                    await SetErrorAsync(trackingCode);
                }

                await NexTryAsync(trackingCode, 1);
            }
        }
        public async Task SaveNewEventsAsync(TrackingCode trackingCode, List<EventDto> events, SubPerson subPerson)
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

                    await _unitOfWork.TrackingCodeEvents.NewTrackingCodeEventsAsync(trackingCodeEventModel);
                }
            }

            EventDto eventDto = events.First();

            await _unitOfWork.MailQueueRepository.AddNewMailQueueAsync(subPerson.Email, eventDto.status, "teste", "teste");
        }
        public async Task NexTryAsync(TrackingCode trackingCode, double hour)
        {
            trackingCode.NextSearch = DateTime.Now.AddHours(hour);
            await _unitOfWork.TrackingService.UpdateTrackingCode(trackingCode);
        }
        public async Task SetErrorAsync(TrackingCode trackingCode)
        {
            trackingCode.Status = TrackingCodeStatus.Error;
            await _unitOfWork.TrackingService.UpdateTrackingCode(trackingCode);
        }
    }
}
