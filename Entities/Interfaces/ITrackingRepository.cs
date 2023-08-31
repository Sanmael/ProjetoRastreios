
namespace Domain.Interfaces
{
    public interface ITrackingRepository
    {
        public Task NewTrackingCodeAsync(TrackingCode trackingCode);
        public Task UpdateTrackingCode(TrackingCode trackingCode);
        public Task DeleteTrackingCodeAsync(TrackingCode trackingCode);
        public Task DeleteTrackingCodeByIdAsync(long trackingCodeId);
        public Task<TrackingCode> GetTrackingCodeByCodeAsync(string code);
        public Task<List<TrackingCode>> GetTrackingCodeActiveAsync();
        public Task<List<TrackingCode>> GetTrackingCodeBySubPersonAsync(int subPersonId);
        public Task<List<TrackingCode>> GetTrackingCodeBySubPersonEventsAsync(int subPersonId);

    }
}
