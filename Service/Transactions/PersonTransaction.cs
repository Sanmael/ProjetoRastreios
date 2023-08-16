using Entities;
using MeuContexto.Context;
using MeuContexto.UOW;
using ProjetoJqueryEstudos.Models;
using Service.Mapper;
using Service.Models;
using System.Net;

namespace Service.Transactions
{
    public class PersonTransaction
    {
        private readonly IUnitOfWork _unitOfWork;

        public PersonTransaction(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public List<SubPersonModel> GetPersonByParameters(string firstNameFilter, string lastNameFilter, string taxNumberFilter, string emailFilter, string addressFilter, string userId)
        {
            try
            {
                addressFilter = addressFilter != null ? addressFilter.Replace("-", "").Replace(".", "").Replace(" ", "").Replace("\t", "") : null;
                taxNumberFilter = taxNumberFilter != null ? taxNumberFilter.Replace("-", "").Replace(".", "").Replace(" ", "").Replace("\t", "") : null;
                firstNameFilter = firstNameFilter != null ? firstNameFilter.Replace(" ", "").Replace("\t", "").ToLower() : null;
                lastNameFilter = lastNameFilter != null ? lastNameFilter.Replace(" ", "").Replace("\t", "").ToLower() : null;
                emailFilter = emailFilter != null ? emailFilter.Replace(" ", "").Replace("\t", "").ToLower() : null;

                var sql = $@"EXEC PersonFilter 
                @UserId = {(userId != null ? $"N'{userId}'" : "NULL")}, 
                @FirstName = {(firstNameFilter != null ? $"N'{firstNameFilter}'" : "NULL")}, 
                @LastName = {(lastNameFilter != null ? $"N'{lastNameFilter}'" : "NULL")}, 
                @TaxNumber = {(taxNumberFilter != null ? $"N'{taxNumberFilter}'" : "NULL")}, 
                @Email = {(emailFilter != null ? $"N'{emailFilter}'" : "NULL")}, 
                @PostalCode = {(addressFilter != null ? $"N'{addressFilter}'" : "NULL")}".Replace("\r\n", "");

                List<SubPersonModel> personsFilter = _unitOfWork.SubPersonService.GetPersonsByProc(sql).Select(x => Mappers.MapToViewModel<SubPerson, SubPersonModel>(x)).ToList();
                personsFilter.ForEach(x => x.TaxNumber = _unitOfWork.PersonService.GetPersonById(x.PersonId).TaxNumber);

                return personsFilter;
            }
            catch (Exception ex)
            {
                _unitOfWork.LoggerService.LogError(ex, ex.Message);
                throw;
            }
        }
        public void ValidateIfExistPerson(string taxNumber, string email)
        {
            try
            {
                if (_unitOfWork.PersonService.GetPersonByTaxNumber(taxNumber) != null)
                    throw new PortalException("Cpf já está sendo usado");

                if (_unitOfWork.PersonService.GetPersonByEmail(email) != null)
                    throw new PortalException("Email já está sendo usado");
            }
            catch (PortalException pt)
            {
                throw;
            }
            catch (Exception ex)
            {
                _unitOfWork.LoggerService.LogError(ex, ex.Message);
            }
        }

        public async Task SaveNewPersonAsync(PersonModel personModel, string userId, string city, string state, string postalCode, string neighborhood, string publicPlace, bool isPrincipalAddress = false)
        {
            try
            {
                Person person = Mappers.MapToViewModel<PersonModel, Person>(personModel);

                _unitOfWork.PersonService.SaveNewPerson(person);

                await _unitOfWork.UserService.UpdateUserIdentityPerson(userId, person.PersonId);

                Address address = new Address(person.PersonId, city, state, postalCode, neighborhood, publicPlace, isPrincipalAddress: true);

                _unitOfWork.AddressService.AddressSave(address, person.PersonId, isPrincipalAddress: true);
            }
            catch (PortalException pt)
            {
                throw;
            }
            catch (Exception ex)
            {
                _unitOfWork.LoggerService.LogError(ex, ex.Message);
            }
        }
        public async Task<int> GetPersonIdByUserId(string userId)
        {
            try
            {
                int personId = await _unitOfWork.UserService.GetPersonIdByUserId(userId);

                if (personId == 0)
                    throw new PortalException("Person não encontada");

                return personId;
            }
            catch (PortalException pt)
            {
                throw;
            }
            catch (Exception ex)
            {
                _unitOfWork.LoggerService.LogError(ex, ex.Message);
                throw;
            }
        }
        public long GetPersonIdByUserId(long userId)
        {
            try
            {
                long personId = _unitOfWork.PersonService.GetPersonById(userId).PersonId;

                return personId;
            }
            catch (PortalException pt)
            {
                throw;
            }
            catch (Exception ex)
            {
                _unitOfWork.LoggerService.LogError(ex, ex.Message);
                throw;
            }
        }
        public async Task<bool> IsSubPersonOwnedByCurrentUserAsync(string userId, long subPersonId)
        {
            int personid = await _unitOfWork.UserService.GetPersonIdByUserId(userId);

            List<SubPerson> subsPerson = _unitOfWork.SubPersonService.GetAllSubPersonByPerson(personid);

            return subsPerson.Where(x => x.SubPersonId == subPersonId).Any();
        }
      
        public void AddNewSubPerson(SubPersonModel subPerson)
        {
            SubPerson person = Mappers.MapToViewModel<SubPersonModel, SubPerson>(subPerson);
            _unitOfWork.SubPersonService.AddNewSubPerson(person);
        }
       
    }
}
