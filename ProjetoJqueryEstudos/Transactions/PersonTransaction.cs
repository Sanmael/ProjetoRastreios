using ProjetoJqueryEstudos.Entities;
using ProjetoJqueryEstudos.Models;
using ProjetoJqueryEstudos.UOW;
using System.Net;

namespace ProjetoJqueryEstudos.Transactions
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

                List<SubPersonModel> personsFilter = _unitOfWork.PersonService.GetPersonsFilter(firstNameFilter, lastNameFilter, taxNumberFilter, emailFilter, addressFilter, userId);

                return personsFilter;
            }
            catch (Exception ex)
            {
                _unitOfWork.LoggerService.LogError(ex, ex.Message);
                throw;
            }
        }
        public void ValidateIfExistPerson(Person person)
        {
            try
            {
                person.TaxNumber = person.TaxNumber.Replace("-", "").Replace(".", "").Replace(" ", "");

                if (_unitOfWork.PersonService.GetPersonByTaxNumber(person.TaxNumber) != null)
                    throw new PortalException("Cpf já está sendo usado");

                if (_unitOfWork.PersonService.GetPersonByEmail(person.Email) != null)
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
        public async Task SaveNewPersonAsync(Person person, Address address, string userId)
        {
            try
            {
                _unitOfWork.PersonService.SaveNewPerson(person);

                await _unitOfWork.UserService.UpdateUserIdentityPerson(userId, person.PersonId);

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
        public async Task<long> GetPersonIdByUserId(string userId)
        {
            try
            {
                long personId = await _unitOfWork.UserService.GetPersonIdByUserId(userId);

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
            long personid = await _unitOfWork.UserService.GetPersonIdByUserId(userId);

            List<SubPerson> subsPerson = _unitOfWork.SubPersonService.GetAllSubPersonByPerson(personid);

            return subsPerson.Where(x => x.SubPersonId == subPersonId).Any();
        }
        public bool TrackingCodeExist(string code)
        {
            TrackingCode trackingCode = _unitOfWork.TrackingService.GetTrackingCodeByCode(code);

            return trackingCode != null;
        }
        public void AddNewTrackingCode(int subPersonId, string code)
        {
            TrackingCode trackingCode = new TrackingCode(code, subPersonId);

            _unitOfWork.TrackingService.NewTrackingCode(trackingCode);
        }
        public void AddNewSubPerson(SubPerson person, long personId)
        {           
            _unitOfWork.SubPersonService.AddNewSubPerson(person);
        }
    }
}
