
 namespace Domain.Interfaces
{
    public interface ISubPersonRepository
    {
        public Task<List<SubPerson>> GetAllSubPersonByPersonAsync(long personId);
        public Task AddNewSubPersonAsync(SubPerson subPerson);
        public Task DeleteSubPersonAsync(SubPerson subPerson);
        public Task<SubPerson> GetSubPersonByIdAsync(int subPersonId);
        public Task<SubPerson> GetSubPersonByPersonAndSubPersonId(int subPersonId, int personId);
        public Task<List<SubPerson>> GetSubPersonsFilter(string sql);       
    }
}
