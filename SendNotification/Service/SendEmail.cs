using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Web;
using SendNotification.DTO;
using Microsoft.AspNetCore.Http;
using System.Globalization;
using Entities;
using MeuContexto.UOW;
using Newtonsoft.Json;
using Microsoft.Extensions.Logging;

namespace SendNotification.Service
{
    internal class SendEmail
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly int MaxDegreeOfParallelism = 10;
        private readonly IConfiguration _configuration;

        public SendEmail(IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
        }

        public async Task GetCodesAsync()
        {
            List<TrackingCode> codes = _unitOfWork.TrackingService.GetTrackingCodeActive();

            //refatorar depois
            ParallelOptions parallelOptions = new ParallelOptions();
            parallelOptions.MaxDegreeOfParallelism = 10;

            Parallel.ForEach(codes, parallelOptions, code =>
            {
                ProcessTrackingCodeAsync(code);
            });
        }

        public async Task ProcessTrackingCodeAsync(TrackingCode trackingCode)
        {
            try
            {
                string apiUrl = $"{_configuration["ApiConfig:Url"]}{trackingCode.Code}";

                using (HttpClient httpClient = new HttpClient())
                {
                    UriBuilder requestUri = new UriBuilder(apiUrl);

                    var query = HttpUtility.ParseQueryString(requestUri.Query);
                    query["user"] = _configuration["ApiConfig:User"];
                    query["token"] = _configuration["ApiConfig:Token"];
                    query["codigo"] = trackingCode.Code;

                    requestUri.Query = query.ToString();

                    HttpResponseMessage response = await httpClient.GetAsync(requestUri.ToString());

                    if (response.IsSuccessStatusCode)
                    {
                        string responseBody = response.Content.ReadAsStringAsync().Result;

                        TrackingInfoDto trackingInfo = JsonConvert.DeserializeObject<TrackingInfoDto>(responseBody);

                        TrackingCodeEvents lastEventAdd = _unitOfWork.TrackingCodeEvents.GetTrackingEventsByTrackingCodeId(trackingCode.TrackingCodeId).OrderBy(x => x.CreationDate).OrderByDescending(x => x.CreationDate).FirstOrDefault();

                        UpdateAndValidateTrackingCodeStatusAsync(trackingCode, lastEventAdd, trackingInfo.eventos);

                        if (trackingInfo.eventos != null && trackingInfo.eventos.Any())
                        {
                            SaveNewEvents(trackingCode, trackingInfo.eventos);

                            NexTry(trackingCode, 3);
                        }
                    }
                    else if (response.StatusCode == HttpStatusCode.RequestTimeout || response.StatusCode == HttpStatusCode.InternalServerError)
                        NexTry(trackingCode, 3);

                    else
                        SetError(trackingCode);
                }
            }
            catch (Exception ex)
            {
                _unitOfWork.LoggerService.LogError(ex, ex.Message);
            }
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

                return;
            }
        }
        public void SaveNewEvents(TrackingCode trackingCode, List<EventDto> events)
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
