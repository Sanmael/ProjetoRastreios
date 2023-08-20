using Entities;
using Entities.Interfaces;

namespace MeuContexto.Repositorys
{
    public class SubPersonRepository : ISubPersonRepository
    {
        private readonly IRepository _repository;

        public SubPersonRepository(IRepository repository)
        {
            _repository = repository;
        }

        public async Task AddNewSubPersonAsync(SubPerson subPerson)
        {
            await _repository.SaveEntityAsync(subPerson);
        }
        public async Task<List<SubPerson>> GetSubPersonsFilter(string sql)
        {          
            List<SubPerson> subPersons = await _repository.GetEntityByProcedure<SubPerson>(sql);

            return subPersons;
        }
        public async Task<List<SubPerson>> GetSubPersonsTrackingCodeSendAsync()
        {
            string sql = "Exec GetSubPersonsTrackingCodeSend";

            List<SubPerson> subPersons = await _repository.GetEntityByProcedure<SubPerson>(sql);

            return subPersons;
        }
        public async Task DeleteSubPersonAsync(SubPerson subPerson)
        {
            await _repository.RemoveEntityAsync(subPerson);
        }

        public async Task<List<SubPerson>> GetAllSubPersonByPersonAsync(long personId)
        {
            return await _repository.GetEntitys<SubPerson>(x => x.PersonId == personId);
        }

        public async Task<SubPerson> GetSubPersonByIdAsync(int subPersonId)
        {
            return await _repository.GetEntityAsync<SubPerson>(x => x.SubPersonId == subPersonId);
        }
        public async Task<SubPerson> GetSubPersonByPersonAndSubPersonId(int subPersonId, int personId)
        {
            return await _repository.GetEntityAsync<SubPerson>(x => x.PersonId == personId && x.SubPersonId == subPersonId);
        }
    }
}
