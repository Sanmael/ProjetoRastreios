
namespace Domain.Interfaces
{
    public interface ITrackingCodeEventsRepository
    {
        public Task NewTrackingCodeEventsAsync(TrackingCodeEvents trackingCodeEvents);
        public Task DeleteTrackingEventsAsync(TrackingCodeEvents trackingCodeEvents);
        public Task<List<TrackingCodeEvents>> GetTrackingEventsByTrackingCodeIdAsync(long trackingCode);
        public Task<TrackingCodeEvents> GetLastTrackingCodeByTrackingCodeId(long trackingCodeid);

    }
}
