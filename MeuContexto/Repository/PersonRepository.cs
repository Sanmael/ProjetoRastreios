using Entities;
using Entities.Interfaces;
using System;
using System.Linq.Expressions;

namespace MeuContexto.Service
{
    public class PersonRepository : IPersonRepository
    {
        private readonly IRepository _repository;

        public PersonRepository(IRepository repository)
        {
            _repository = repository;
        }

        //public List<SubPersonModel> GetPersonsFilter(string firstNameFilter, string lastNameFilter, string taxNumberFilter, string emailFilter, string addressFilter,string userId)
        //{
        //    var sql = $@"EXEC PersonFilter 
        //    @UserId = {(userId != null ? $"N'{userId}'" : "NULL")}, 
        //    @FirstName = {(firstNameFilter != null ? $"N'{firstNameFilter}'" : "NULL")}, 
        //    @LastName = {(lastNameFilter != null ? $"N'{lastNameFilter}'" : "NULL")}, 
        //    @TaxNumber = {(taxNumberFilter != null ? $"N'{taxNumberFilter}'" : "NULL")}, 
        //    @Email = {(emailFilter != null ? $"N'{emailFilter}'" : "NULL")}, 
        //    @PostalCode = {(addressFilter != null ? $"N'{addressFilter}'" : "NULL")}";

        //    sql = sql.Replace("\r\n", "");

        //    List<SubPersonModel> personModels = _repository.GetEntityByProcedure<Person>(sql).Select(x=> Mapper.MapToViewModel<Person, SubPersonModel>(x)).ToList();
            
        //    return personModels;
        //}

        public Person GetPersonByTaxNumber(string taxNumber)
        {
            Person PersonTaxNumber = _repository.GetEntity<Person>(x => x.TaxNumber == taxNumber);
            return PersonTaxNumber;
        }
        public Person GetPersonByEmail(string email)
        {
            Person PersonEmail = _repository.GetEntity<Person>(x => x.Email == email);
            return PersonEmail;
        }
        public Person GetPersonById(long id)
        {
            Person PersonById = _repository.GetEntity<Person>(x => x.PersonId == id);
            return PersonById;
        }

        public void SaveNewPerson(Person person)
        {
            _repository.SaveEntity(person);
        }

        public IEnumerable<Person> GetAllPerson()
        {
            return _repository.GetEntitys<Person>();
        }

        public void RemovePerson(Person person)
        {
            _repository.RemoveEntity(person);
        }

        public void UpdatePerson(Person person)
        {
            _repository.UpdateEntity(person);
        }
    }
}
