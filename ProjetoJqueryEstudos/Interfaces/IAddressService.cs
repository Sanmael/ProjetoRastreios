using ProjetoJqueryEstudos.Entities;

namespace ProjetoJqueryEstudos.Interfaces
{
    public interface IAddressService
    {
        public void AddressSave(Address address, long personId, bool isPrincipalAddress = false);
        public void AddressRemove(Address address);
        public void AddressEdit(Address address);
        public Address GetAddressById(long id);
        public Address GetAddressPrincipal(long personId,bool isPrincipal);
        public Address GetAddressByCep(string cep, long personId);
    }
}
