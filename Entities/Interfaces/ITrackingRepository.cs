
namespace Entities.Interfaces
{
    public interface ITrackingRepository
    {
        public void NewTrackingCode(TrackingCode trackingCode);
        public void UpdateTrackingCode(TrackingCode trackingCode);
        public void DeleteTrackingCode(TrackingCode trackingCode);
        public void DeleteTrackingCodeById(long trackingCodeId);
        public TrackingCode GetTrackingCodeByCode(string code);
        public List<TrackingCode> GetTrackingCodeActive();
        public List<TrackingCode> GetTrackingCodeBySubPerson(int subPersonId);
        public List<TrackingCode> GetTrackingCodeBySubPersonEvents(int subPersonId);

    }
}