using Domain;

namespace Domain.Interfaces
{
    public interface IPersonRepository
    {
        public Task<Person> GetPersonByTaxNumberAsync(string taxNumber);
        public Task<Person> GetPersonByEmailAsync(string email);        
        public Task<Person> GetPersonByIdAsync(long id);        
        public Task SaveNewPersonAsync(Person person);
        public Task<IEnumerable<Person>> GetAllPersonAsync();
        public Task RemovePersonAsync(Person person);
        public Task UpdatePersonAsync(Person person);        
        //public List<SubPersonModel> GetPersonsFilter(string firstNameFilter, string lastNameFilter, string taxNumberFilter, string emailFilter, string addressFilter,string userId);
    }
}
