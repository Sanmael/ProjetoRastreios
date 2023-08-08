using ProjetoJqueryEstudos.Entities;

namespace ProjetoJqueryEstudos.Interfaces
{
    public interface ITrackingService
    {
        public void NewTrackingCode(TrackingCode trackingCode);
        public void UpdateTrackingCode(TrackingCode trackingCode);
        public TrackingCode GetTrackingCodeByCode(string code);
        public List<TrackingCode> GetTrackingCodeActive();
        public List<TrackingCode> GetTrackingCodeBySubPerson(int subPersonId);
    }
}