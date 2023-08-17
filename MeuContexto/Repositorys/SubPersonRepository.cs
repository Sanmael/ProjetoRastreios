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

        public void AddNewSubPerson(SubPerson subPerson)
        {
            _repository.SaveEntity(subPerson);
        }
        public List<SubPerson> GetSubPersonsFilter(string sql)
        {          
            List<SubPerson> subPersons = _repository.GetEntityByProcedure<SubPerson>(sql).ToList();

            return subPersons;
        }
        public List<SubPerson> GetSubPersonsTrackingCodeSend()
        {
            string sql = "Exec GetSubPersonsTrackingCodeSend";

            List<SubPerson> subPersons = _repository.GetEntityByProcedure<SubPerson>(sql).ToList();

            return subPersons;
        }
        public void DeleteSubPerson(SubPerson subPerson)
        {
            _repository.RemoveEntity(subPerson);
        }

        public List<SubPerson> GetAllSubPersonByPerson(long personId)
        {
            return _repository.GetEntitys<SubPerson>(x => x.PersonId == personId).ToList();
        }

        public SubPerson GetSubPersonById(int subPersonId)
        {
            return _repository.GetEntity<SubPerson>(x => x.SubPersonId == subPersonId);
        }
        public SubPerson GetSubPersonByPersonAndSubPersonId(int subPersonId, int personId)
        {
            return _repository.GetEntity<SubPerson>(x => x.PersonId == personId && x.SubPersonId == subPersonId);
        }
    }
}
