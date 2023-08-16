using Entities;

namespace Entities.Interfaces
{
    public interface IPersonRepository
    {
        public Person GetPersonByTaxNumber(string taxNumber);
        public Person GetPersonByEmail(string email);        
        public Person GetPersonById(long id);        
        public void SaveNewPerson(Person person);
        public IEnumerable<Person> GetAllPerson();
        public void RemovePerson(Person person);
        public void UpdatePerson(Person person);        
        //public List<SubPersonModel> GetPersonsFilter(string firstNameFilter, string lastNameFilter, string taxNumberFilter, string emailFilter, string addressFilter,string userId);
    }
}
