
using Entities;
using Entities.Interfaces;

namespace MeuContexto.Repositorys
{
    public class AddressRepository : IAddressRepository
    {
        private readonly IRepository _repository;
        public AddressRepository(IRepository repository)
        {
            _repository = repository;
        }

        public void AddressEdit(Address address)
        {
            throw new NotImplementedException();
        }

        public void AddressRemove(Address address)
        {
            throw new NotImplementedException();
        }

        public void AddressSave(Address address, int personId, bool isPrincipalAddress = false)
        {
            if (isPrincipalAddress)
            {
                VerifyPrincipalAddresAsync(personId);
            }
            address.PersonId = personId;
            address.isPrincipalAddress = isPrincipalAddress;
            _repository.SaveEntityAsync(address);
        }

        public async Task<Address> GetAddressByCepAsync(string cep, int personId)
        {
            try
            {
                Address address = await _repository.GetEntityAsync<Address>(x => x.PostalCode == cep && x.PersonId == personId);

                return address;
            }
            catch 
            {
                throw;
            }
        }

        public async Task<Address> GetAddressById(int id)
        {
            return await _repository.GetEntityAsync<Address>(x => x.PersonId == id);
        }

        public async Task<Address> GetAddressPrincipalAsync(int personId,bool isPrincipal)
        {
            return await _repository.GetEntityAsync<Address>(x => x.PersonId == personId && x.isPrincipalAddress == true);
        }

        private async Task VerifyPrincipalAddresAsync(int personId)
        {
            Address address = await _repository.GetEntityAsync<Address>(x => x.PersonId == personId && x.isPrincipalAddress == true);

            if (address != null)
            {
                address.isPrincipalAddress = false;
                await _repository.UpdateEntityAsync(address);
            }
        }
    }
}
