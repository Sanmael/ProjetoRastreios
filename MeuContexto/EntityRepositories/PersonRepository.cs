using Domain;
using Domain.Interfaces;
using System;
using System.Linq.Expressions;

namespace MeuContexto.EntityRepositories
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

        //    List<SubPersonModel> personModels = await repository.GetEntityByProcedure<Person>(sql).Select(x=> Mapper.MapToViewModel<Person, SubPersonModel>(x)).ToList();
            
        //    return personModels;
        //}

        public async Task<Person> GetPersonByTaxNumberAsync(string taxNumber)
        {
            Person PersonTaxNumber = await _repository.GetEntityAsync<Person>(x => x.TaxNumber == taxNumber);
            return PersonTaxNumber;
        }
        public async Task<Person> GetPersonByEmailAsync(string email)
        {
            Person PersonEmail = await _repository.GetEntityAsync<Person>(x => x.Email == email);
            return PersonEmail;
        }
        public async Task<Person> GetPersonByIdAsync(long id)
        {
            Person PersonById = await _repository.GetEntityAsync<Person>(x => x.PersonId == id);
            return PersonById;
        }

        public async Task SaveNewPersonAsync(Person person)
        {
            await _repository.SaveEntityAsync(person);
        }

        public async Task<IEnumerable<Person>> GetAllPersonAsync()
        {
            return await _repository.GetEntitys<Person>();
        }

        public async Task RemovePersonAsync(Person person)
        {
            await _repository.RemoveEntityAsync(person);
        }

        public async Task UpdatePersonAsync(Person person)
        {
            await _repository.UpdateEntityAsync(person);
        }
    }
}
