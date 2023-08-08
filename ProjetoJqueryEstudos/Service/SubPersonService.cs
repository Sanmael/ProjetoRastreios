using ProjetoJqueryEstudos.Entities;
using ProjetoJqueryEstudos.Interfaces;

namespace ProjetoJqueryEstudos.Service
{
    public class SubPersonService : ISubPersonService
    {
        private readonly IRepository _repository;

        public SubPersonService(IRepository repository)
        {
            _repository = repository;
        }

        public void AddNewSubPerson(SubPerson subPerson)
        {
            _repository.SaveEntity(subPerson);
        }

        public List<SubPerson> GetAllSubPersonByPerson(long personId)
        {
            return _repository.GetEntitys<SubPerson>(x => x.PersonId == personId).ToList();
        }

        public SubPerson GetSubPersonById(int subPersonId)
        {
            return _repository.GetEntity<SubPerson>(x => x.SubPersonId == subPersonId);
        }
    }
}
