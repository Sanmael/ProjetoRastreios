using Entities;
using Entities.Interfaces;

namespace MeuContexto.Repositorys
{
    public class TrackingCodeEventsRepository : ITrackingCodeEventsRepository
    {
        private readonly IRepository _repository;

        public TrackingCodeEventsRepository(IRepository repository)
        {
            _repository = repository;
        }
        public void NewTrackingCodeEvents(TrackingCodeEvents trackingCodeEvents)
        {
            _repository.SaveEntity(trackingCodeEvents);
        }
        public void DeleteTrackingEvents(TrackingCodeEvents trackingCodeEvents)
        {
            _repository.RemoveEntity(trackingCodeEvents);
        }

        public List<TrackingCodeEvents> GetTrackingEventsByTrackingCodeId(long trackingCode)
        {
            return _repository.GetEntitys<TrackingCodeEvents>(x => x.TrakingCodeId == trackingCode).ToList();
        }

    }
}
