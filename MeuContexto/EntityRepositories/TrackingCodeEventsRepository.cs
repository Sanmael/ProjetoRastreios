using Domain;
using Domain.Interfaces;

namespace MeuContexto.EntityRepositories
{
    public class TrackingCodeEventsRepository : ITrackingCodeEventsRepository
    {
        private readonly IRepository _repository;

        public TrackingCodeEventsRepository(IRepository repository)
        {
            _repository = repository;
        }
        public async Task NewTrackingCodeEventsAsync(TrackingCodeEvents trackingCodeEvents)
        {
            await _repository.SaveEntityAsync(trackingCodeEvents);
        }
        public async Task DeleteTrackingEventsAsync(TrackingCodeEvents trackingCodeEvents)
        {
            await _repository.RemoveEntityAsync(trackingCodeEvents);
        }

        public async Task<List<TrackingCodeEvents>> GetTrackingEventsByTrackingCodeIdAsync(long trackingCode)
        {
            return await _repository.GetEntitys<TrackingCodeEvents>(x => x.TrakingCodeId == trackingCode);
        }

        public async Task<TrackingCodeEvents> GetLastTrackingCodeByTrackingCodeId(long trackingCodeid)
        {
            var teste = await _repository.GetEntityByProcedure<TrackingCodeEvents>(proc: $"select top 1 * from TrackingCodeEvents where TrakingCodeId = {trackingCodeid} order by CreationDate desc" , parameters: null);

            return teste.FirstOrDefault();
        }
    }
}