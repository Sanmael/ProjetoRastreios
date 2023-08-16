
namespace Entities.Interfaces
{
    public interface ITrackingCodeEventsRepository
    {
        public void NewTrackingCodeEvents(TrackingCodeEvents trackingCodeEvents);
        public void DeleteTrackingEvents(TrackingCodeEvents trackingCodeEvents);
        public List<TrackingCodeEvents> GetTrackingEventsByTrackingCodeId(long trackingCode);

    }
}
