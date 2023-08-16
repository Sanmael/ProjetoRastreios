using Entities;

namespace Entities.Interfaces
{
    public interface IAddressRepository
    {
        public void AddressSave(Address address, int personId, bool isPrincipalAddress = false);
        public void AddressRemove(Address address);
        public void AddressEdit(Address address);
        public Address GetAddressById(int id);
        public Address GetAddressPrincipal(int personId,bool isPrincipal);
        public Address GetAddressByCep(string cep, int personId);
    }
}
