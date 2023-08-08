using ProjetoJqueryEstudos.Interfaces;

namespace ProjetoJqueryEstudos.UOW
{
    public interface IUnitOfWork
    {
        public IAddressService AddressService { get; }
        public IPersonService PersonService { get; }
        public IUserService UserService { get; }
        public ILoggerService LoggerService { get; }
        public ISubPersonService SubPersonService { get; }
        public ITrackingService TrackingService { get; }
        public ITrackingCodeEventsService TrackingCodeEvents { get; }
    }
}
