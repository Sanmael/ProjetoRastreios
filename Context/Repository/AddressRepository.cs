
namespace MeuContexto.Service
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

        public void AddressSave(Address address, long personId, bool isPrincipalAddress = false)
        {
            if (isPrincipalAddress)
            {
                VerifyPrincipalAddres(personId);
            }
            address.PersonId = personId;
            address.isPrincipalAddress = isPrincipalAddress;
            _repository.SaveEntity(address);
        }

        public Address GetAddressByCep(string cep,long personId)
        {
            try
            {
                Address address = _repository.GetEntity<Address>(x => x.PostalCode == cep && x.PersonId == personId);

                return address;
            }
            catch 
            {
                throw;
            }
        }

        public Address GetAddressById(long id)
        {
            return _repository.GetEntity<Address>(x => x.PersonId == id);
        }

        public Address GetAddressPrincipal(long personId,bool isPrincipal)
        {
            return _repository.GetEntity<Address>(x => x.PersonId == personId && x.isPrincipalAddress == true);
        }

        private void VerifyPrincipalAddres(long personId)
        {
            Address address = _repository.GetEntity<Address>(x => x.PersonId == personId && x.isPrincipalAddress == true);

            if (address != null)
            {
                address.isPrincipalAddress = false;
                _repository.UpdateEntity(address);
            }
        }
    }
}
