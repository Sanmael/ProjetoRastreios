﻿
using Entities.Interfaces;

namespace MeuContexto.UOW
{
    public interface IUnitOfWork
    {
        public IAddressRepository AddressService { get; }
        public IPersonRepository PersonService { get; }
        public IUserService UserService { get; }
        public ILoggerRepository LoggerService { get; }
        public ISubPersonRepository SubPersonService { get; }
        public ITrackingRepository TrackingService { get; }
        public ITrackingCodeEventsRepository TrackingCodeEvents { get; }
        public IMailQueueRepository MailQueueRepository { get; }
    }
}
