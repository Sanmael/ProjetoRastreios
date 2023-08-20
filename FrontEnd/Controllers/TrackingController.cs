using Microsoft.AspNetCore.Mvc;
using FrontEnd.Models;
using Service;
using Service.Models;
using Service.Transactions;
using Service.Utils;
using System.Security.Claims;

namespace FrontEnd.Controllers
{
    public class TrackingController : Controller
    {
        private readonly PersonTransaction _personTransaction;
        private readonly TrackingTransaction _trackingTransaction;

        public TrackingController(PersonTransaction personTransaction, TrackingTransaction trackingTransaction)
        {
            _personTransaction = personTransaction;
            _trackingTransaction = trackingTransaction;
        }
        [HttpPost]
        public async Task<IActionResult> NewTrackingCodeAsync(string code, string subPersonId)
        {
            try
            {
                _trackingTransaction.ValidateTrackingCode(code);

                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                bool validateSubPerson = _personTransaction.IsSubPersonOwnedByCurrentUserAsync(userId, long.Parse(subPersonId)).Result;

                bool validateCode = await _trackingTransaction.TrackingCodeExist(code);

                if (!validateSubPerson)
                    return Json(new { success = false, message = "person não correspondente!" });

                if (validateCode)
                    return Json(new { success = false, message = "codigo já adicionado!" });

                _trackingTransaction.AddNewTrackingCode(int.Parse(subPersonId), code);

                return Json(new { success = true });
            }
            catch (PortalException e)
            {
                return Json(new { success = false, message = e.Message });
            }
            catch
            {
                return Json(new { success = false, message = MessageService.ErrorServer });
            }
        }
        [HttpGet]
        public IActionResult GetTrackingBySubPerson(int subPersonId)
        {
            try
            {
                List<TrackingCodeModel> trackingCodes = _trackingTransaction.GetTrackingCodesByPersonAndTrackingCodeEventsNotNull(subPersonId);

                trackingCodes.ForEach(x => x.UpdateDate.ToShortDateString());

                if (trackingCodes.Any())
                {
                    return Json(new { success = true, data = trackingCodes });
                }

                return Json(new { success = false });
            }
            catch (Exception)
            {
                return Json(new { success = false });
            }
        }
        [HttpGet]
        public IActionResult GetTrackingEvents(int trackingCodeId)
        {
            try
            {
                List<TrackingCodeEventsModel> trackingCodes =  _trackingTransaction.GetTrackinEventsByIdAsync(trackingCodeId).Result;

                if (trackingCodes.Any())
                {
                    return Json(new { success = true, data = trackingCodes });
                }

                return Json(new { success = false });
            }
            catch (Exception)
            {
                return Json(new { success = false });
            }
        }
        public IActionResult DeleteTrackingCode(int trackingCodeId)
        {
            try
            {
                _trackingTransaction.DeleteTrackingCodeById(trackingCodeId);

                return Json(new { success = true });

            }
            catch
            {
                return Json(new { success = false });
            }
        }
    }
}
