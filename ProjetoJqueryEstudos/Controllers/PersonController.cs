using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Web;
using ProjetoJqueryEstudos.Context;
using ProjetoJqueryEstudos.Entities;
using ProjetoJqueryEstudos.Models;
using ProjetoJqueryEstudos.Service;
using ProjetoJqueryEstudos.Transactions;
using ProjetoJqueryEstudos.UOW;
using ProjetoJqueryEstudos.Utils;
using System;
using System.Linq.Expressions;
using System.Net;
using System.Security.Claims;

public class PersonController : Controller
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly PersonTransaction _personTransaction;

    public PersonController(IUnitOfWork unitOfWork, PersonTransaction personTransaction)
    {
        _unitOfWork = unitOfWork;
        _personTransaction = personTransaction;
    }
    [HttpPost]
    public async Task<IActionResult> PersonCreateAction(SubPersonModel personModel)
    {
        try
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            long UserPersonId = _personTransaction.GetPersonIdByUserId(userId).Result;

            if (UserPersonId != 0)
            {
                SubPerson person = new SubPerson(UserPersonId, personModel.FirstName, personModel.LastName, personModel.Email);

                _personTransaction.AddNewSubPerson(person, UserPersonId);

                return Json(new { success = true });
            }

            return Json(new { success = false, message = "Campos preenchidos incorretamente!" });
        }
        catch (PortalException ex)
        {
            return Json(new { success = false, message = ex.Message });
        }
        catch
        {
            return Json(new { success = false, message = MessageService.ErrorServer });
        }
    }
    public IActionResult SaveAddresses([FromBody] List<Address> enderecos, long personId)
    {
        try
        {
            foreach (var item in enderecos)
            {
                item.PostalCode = item.PostalCode.Replace("-", "").Replace(".", "").Replace(" ", "");

                _unitOfWork.AddressService.AddressSave(item, personId, item.isPrincipalAddress);
            }
            return Ok();
        }
        catch
        {
            return Json(new { success = false, message = MessageService.ErrorServer });
        }
    }
    [Authorize]
    public IActionResult PersonList()
    {
        List<SubPersonModel> persons = new List<SubPersonModel>();

        return View(persons);
    }
    [HttpPost]
    [Authorize]
    public IActionResult GetPersonListFilter(string firstNameFilter, string lastNameFilter, string taxNumberFilter, string emailFilter, string addressFilter)
    {
        try
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
                return RedirectToAction("PersonList");

            List<SubPersonModel> personsFilter = _personTransaction.GetPersonByParameters(firstNameFilter, lastNameFilter, taxNumberFilter, emailFilter, addressFilter, userId);

            return View("PersonList", personsFilter);
        }
        catch
        {
            return Json(new { success = false, message = MessageService.ErrorServer });
        }
    }
    [Authorize(Roles = "Admin")]
    public IActionResult PersonEdit(long id)
    {
        return View(_unitOfWork.PersonService.GetPersonById(id));
    }
    public IActionResult GetAddressById(string id)
    {
        try
        {
            Address address = _unitOfWork.AddressService.GetAddressPrincipal(long.Parse(id), isPrincipal: true);

            if (address == null)
                throw new PortalException("pessoa sem endereço cadastrado!");

            return Json(new { success = true, data = address });
        }
        catch (PortalException ex)
        {
            return Json(new { success = ex.Sucess, message = ex.Message });
        }
        catch
        {
            return Json(new { success = false, message = MessageService.ErrorServer });
        }
    }
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public JsonResult PersonEditAction(Person person)
    {
        try
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.PersonService.UpdatePerson(person);
                return Json(new { sucess = true });
            }

            return Json(new { sucess = false });
        }
        catch
        {
            return Json(new { success = false, message = MessageService.ErrorServer });
        }
    }
    [HttpPost]
    [Authorize("Admin")]
    public IActionResult PersonDeleteAction(string personId)
    {
        try
        {
            if (ModelState.IsValid)
            {
                Person person = _unitOfWork.PersonService.GetPersonById(long.Parse(personId));

                if (person == null)
                    return Json(new { success = false, message = "Pessoa não encontrada" });

                _unitOfWork.PersonService.RemovePerson(person);
            }

            return Json(new { success = true, message = "Pessoa deletada com sucesso!" });
        }
        catch
        {
            return Json(new { success = false, message = MessageService.ErrorServer });
        }

    }
}
