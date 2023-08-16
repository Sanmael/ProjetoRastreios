using Entities;
using Entities.Interfaces;

namespace MeuContexto.Service
{
    public class TrackingRepository : ITrackingRepository
    {
        private readonly IRepository repository;

        public TrackingRepository(IRepository repository)
        {
            this.repository = repository;
        }

        public void DeleteTrackingCode(TrackingCode trackingCode)
        {
            repository.RemoveEntity(trackingCode);
        }

        public List<TrackingCode> GetTrackingCodeActive()
        {
            return repository.GetEntitys<TrackingCode>(x => x.Status == TrackingCodeStatus.Active && x.NextSearch <= DateTime.Now).ToList();
        }

        public TrackingCode GetTrackingCodeByCode(string code)
        {
            return repository.GetEntity<TrackingCode>(x => x.Code.Equals(code));
        }

        public List<TrackingCode> GetTrackingCodeBySubPerson(int subPersonId)
        {
            return repository.GetEntitys<TrackingCode>(x => x.SubPersonId == subPersonId && x.Status == TrackingCodeStatus.Active && x.NextSearch <= DateTime.Now).ToList();
        }

        public List<TrackingCode> GetTrackingCodeBySubPersonEvents(int subPersonId)
        {
            var sql = $@"EXEC GetTrackingCodeBySubPersonId 
            @subPersonId = {(subPersonId != null ? $"N'{subPersonId}'" : "NULL")}";

            sql = sql.Replace("\r\n", "");

            return repository.GetEntityByProcedure<TrackingCode>(sql);
        }

        public void NewTrackingCode(TrackingCode trackingCode)
        {
            repository.SaveEntity<TrackingCode>(trackingCode);
        }


        public void UpdateTrackingCode(TrackingCode trackingCode)
        {
            repository.UpdateEntity<TrackingCode>(trackingCode);
        }
    }
}
