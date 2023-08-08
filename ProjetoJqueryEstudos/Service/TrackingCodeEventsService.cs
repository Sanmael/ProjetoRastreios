using ProjetoJqueryEstudos.Entities;
using ProjetoJqueryEstudos.Interfaces;

namespace ProjetoJqueryEstudos.Service
{
    public class TrackingCodeEventsService : ITrackingCodeEventsService
    {
        private readonly IRepository _repository;

        public TrackingCodeEventsService(IRepository repository)
        {
            _repository = repository;
        }
        public void NewTrackingCodeEvents(TrackingCodeEvents trackingCodeEvents)
        {
            _repository.SaveEntity(trackingCodeEvents);
        }

    }
}
