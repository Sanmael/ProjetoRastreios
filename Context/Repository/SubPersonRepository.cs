using Entities;
using Entities.Interfaces;
using ProjetoJqueryEstudos.Interfaces;

namespace MeuContexto.Service
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
