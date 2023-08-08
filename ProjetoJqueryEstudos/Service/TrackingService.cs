using ProjetoJqueryEstudos.Entities;
using ProjetoJqueryEstudos.Interfaces;

namespace ProjetoJqueryEstudos.Service
{
    public class TrackingService : ITrackingService
    {
        private readonly IRepository repository;

        public TrackingService(IRepository repository)
        {
            this.repository = repository;
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
