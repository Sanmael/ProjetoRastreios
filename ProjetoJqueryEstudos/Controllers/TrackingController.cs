using Microsoft.AspNetCore.Mvc;
using ProjetoJqueryEstudos.Transactions;
using System.Security.Claims;

namespace ProjetoJqueryEstudos.Controllers
{
    public class TrackingController : Controller
    {
        private readonly PersonTransaction _personTransaction;

        public TrackingController(PersonTransaction personTransaction)
        {
            _personTransaction = personTransaction;
        }
        [HttpPost]
        public IActionResult NewTrackingCode(string code, string subPersonId)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                bool validateSubPerson = _personTransaction.IsSubPersonOwnedByCurrentUserAsync(userId, long.Parse(subPersonId)).Result;

                bool validateCode = _personTransaction.TrackingCodeExist(code);

                if (!validateSubPerson || validateCode)
                {
                    return Json(new { success = false });
                }

                _personTransaction.AddNewTrackingCode(int.Parse(subPersonId), code);

                return Json(new { success = true });
            }
            catch
            {
                return Json(new { success = false });
            }
        }
    }
}
