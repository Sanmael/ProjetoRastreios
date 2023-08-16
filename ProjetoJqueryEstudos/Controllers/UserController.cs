using Microsoft.AspNetCore.Mvc;
using ProjetoJqueryEstudos.Models;
using System.Net;
using System.Security.Claims;
using System;
using Service.Transactions;
using Service;
using Service.Models;
using Entities;
using Service.Utils;

namespace ProjetoJqueryEstudos.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserTransaction _userTransaction;
        private readonly PersonTransaction _personTransaction;
        public UserController(IUserTransaction userTransaction, PersonTransaction personTransaction)
        {
            _userTransaction = userTransaction;
            _personTransaction = personTransaction;
        }
        [HttpPost]
        public IActionResult Login(LoginModel model)
        {
            try
            {
                bool result = _userTransaction.LoginAsync(model.Email, model.Password).Result;

                return Json(new { Success = result, RedirectUrl = "Home/IsLogged" });
            }
            catch (PortalException ex)
            {
                return Json(new { Success = ex.Sucess, Message = ex.Message });
            }
            catch
            {
                return Json(new { success = false, message = MessageService.ErrorServer });
            }
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (!ModelState.IsValid)
                throw new PortalException(ModelState.Values.Where(x => x.Errors.Any()).First().Errors.First().ErrorMessage);

            try
            {
                PersonModel person = new PersonModel(model.FirstName, model.LastName, model.TaxNumber, model.Email);

                person.TaxNumber = person.TaxNumber.Replace("-", "").Replace(".", "").Replace(" ", "");

                _personTransaction.ValidateIfExistPerson(person.TaxNumber, person.Email);

                bool result = _userTransaction.RegisterAsync(model.Email, model.Password).Result;

                if (result)
                {
                    string userId = _userTransaction.GetUserIdByEmail(model.Email);

                    await _personTransaction.SaveNewPersonAsync(person, userId, model.City, model.State, model.PostalCode, model.Neighborhood, model.PublicPlace, isPrincipalAddress: true);

                    return Json(new { success = true });
                }

                return Json(new { success = false, message = "Campos preenchidos incorretamente!" });
            }
            catch (PortalException ex)
            {
                return Json(new { Success = ex.Sucess, Message = ex.Message });
            }
            catch
            {
                return Json(new { success = false, message = MessageService.ErrorServer });
            }
        }
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            try
            {
                if (User?.Identity?.Name == null)
                    throw new PortalException("Não há usuario conectado!");

                await _userTransaction.Logout();

                return Redirect("/Home/Index");
            }
            catch (PortalException ex)
            {
                return Json(new { Success = ex.Sucess, Message = ex.Message });
            }
            catch
            {
                return Json(new { success = false, message = MessageService.ErrorServer });
            }
        }

    }
}
