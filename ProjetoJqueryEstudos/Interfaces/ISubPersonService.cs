using ProjetoJqueryEstudos.Entities;

namespace ProjetoJqueryEstudos.Interfaces
{
    public interface ISubPersonService
    {
        public List<SubPerson> GetAllSubPersonByPerson(long personId);
        public void AddNewSubPerson(SubPerson subPerson);
        public SubPerson GetSubPersonById(int subPersonId);
    }
}
