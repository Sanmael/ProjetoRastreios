using Domain;

namespace Domain.Interfaces
{
    public interface IAddressRepository
    {
        public void AddressSave(Address address, int personId, bool isPrincipalAddress = false);
        public void AddressRemove(Address address);
        public void AddressEdit(Address address);
        public Task<Address> GetAddressById(int id);
        public Task<Address> GetAddressPrincipalAsync(int personId,bool isPrincipal);
        public Task<Address> GetAddressByCepAsync(string cep, int personId);
    }
}
