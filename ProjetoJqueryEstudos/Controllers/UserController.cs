using Microsoft.AspNetCore.Mvc;
using ProjetoJqueryEstudos.Entities;
using ProjetoJqueryEstudos.Interfaces;
using ProjetoJqueryEstudos.Models;
using ProjetoJqueryEstudos.Service;
using ProjetoJqueryEstudos.Transactions;
using ProjetoJqueryEstudos.UOW;
using ProjetoJqueryEstudos.Utils;
using System.Net;
using System.Security.Claims;
using System;

namespace ProjetoJqueryEstudos.Controllers
{
    public class UserController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly PersonTransaction _personTransaction;

        public UserController(IUnitOfWork unitOfWork, PersonTransaction personTransaction)
        {
            _unitOfWork = unitOfWork;
            _personTransaction = personTransaction;
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model)
        {
            try
            {
                bool result = await _unitOfWork.UserService.Login(model.Email, model.Password);

                return Json(new { Success = result, RedirectUrl = "https://localhost:7186/Home/IsLogged" });
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
                bool result = await _unitOfWork.UserService.Register(model.Email, model.Password);

                if (result)
                {
                    Person person = new Person(model.FirstName, model.LastName,model.TaxNumber,model.Email);

                    UserIdentity userId = _unitOfWork.UserService.GetUserByEmail(model.Email);

                    _personTransaction.ValidateIfExistPerson(person);

                    Address address = new Address(person.PersonId,model.City,model.State, model.PostalCode,model.Neighborhood,model.PublicPlace, isPrincipalAddress :true);

                    await _personTransaction.SaveNewPersonAsync(person, address, userId.Id);

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

        public async Task<IActionResult> Logout()
        {
            try
            {
                if (User?.Identity?.Name == null)
                    throw new PortalException("Não há usuario conectado!");

                await _unitOfWork.UserService.Logout();

                return Redirect("/Person/PersonList");
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
