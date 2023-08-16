
 namespace Entities.Interfaces
{
    public interface ISubPersonRepository
    {
        public List<SubPerson> GetAllSubPersonByPerson(long personId);
        public void AddNewSubPerson(SubPerson subPerson);
        public void DeleteSubPerson(SubPerson subPerson);
        public SubPerson GetSubPersonById(int subPersonId);
        public SubPerson GetSubPersonByPersonAndSubPersonId(int subPersonId, int personId);
        public List<SubPerson> GetPersonsByProc(string sql);

    }
}
