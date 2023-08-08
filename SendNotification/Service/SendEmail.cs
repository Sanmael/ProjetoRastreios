using Humanizer;
using ProjetoJqueryEstudos.UOW;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using ProjetoJqueryEstudos.Entities;
using System.Text.Json;
using System.Web;
using Newtonsoft.Json;
using SendNotification.DTO;

namespace SendNotification.Service
{
    internal class SendEmail
    {
        private readonly IUnitOfWork _unitOfWork;

        public SendEmail(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task SendMailAsync()
        {
            IEnumerable<IGrouping<int, TrackingCode>> codes = _unitOfWork.TrackingService.GetTrackingCodeActive().GroupBy(x => x.SubPersonId);

            foreach (var code in codes)
            {
                SubPerson subPerson = _unitOfWork.SubPersonService.GetSubPersonById(code.Key);

                List<TrackingCode> trackingCodes = _unitOfWork.TrackingService.GetTrackingCodeBySubPerson((int)subPerson.SubPersonId);

                foreach (var trackingCode in trackingCodes)
                {
                    string apiUrl = $"https://api.linketrack.com/track/json?user=Samunickinho@gmail.com&token=930b33d20f9de2a0b83bc0740d5b236a415306c8ed4ae8cbf41111f70b23a9ef&codigo={trackingCode.Code}";

                    using (HttpClient httpClient = new HttpClient())
                    {
                        var requestUri = new UriBuilder(apiUrl);
                        var query = HttpUtility.ParseQueryString(requestUri.Query);
                        query["user"] = "Samunickinho@gmail.com";
                        query["token"] = "930b33d20f9de2a0b83bc0740d5b236a415306c8ed4ae8cbf41111f70b23a9ef";
                        query["codigo"] = trackingCode.Code;
                        requestUri.Query = query.ToString();

                        HttpResponseMessage response = await httpClient.GetAsync(requestUri.ToString());

                        if (response.IsSuccessStatusCode)
                        {
                            string responseBody = response.Content.ReadAsStringAsync().Result;

                            TrackingInfo trackingInfo = JsonConvert.DeserializeObject<TrackingInfo>(responseBody);

                            if (trackingInfo.eventos != null && trackingInfo.eventos.Count > 0)
                            {
                                Event lastEvent = trackingInfo.eventos.First();

                                if (lastEvent.status == "Objeto entregue ao destinatário")
                                {
                                    trackingCode.Status = TrackingCodeStatus.Delivered;
                                    _unitOfWork.TrackingService.UpdateTrackingCode(trackingCode);
                                    continue;
                                }

                                TrackingCodeEvents trackingCodeEventModel = new TrackingCodeEvents()
                                {
                                    TrakingCodeId = trackingCode.TrackingCodeId,
                                    CreationDate = DateTime.Now,
                                    Status = lastEvent.status,
                                    Local = lastEvent.local
                                };

                                _unitOfWork.TrackingCodeEvents.NewTrackingCodeEvents(trackingCodeEventModel);
                            }
                        }

                        trackingCode.NextSearch = DateTime.Now.AddHours(2);

                        _unitOfWork.TrackingService.UpdateTrackingCode(trackingCode);
                    }
                }
            }
        }
    }
}
