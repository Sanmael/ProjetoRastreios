using Entities.Interfaces;
using Microsoft.AspNetCore.Identity;
using MeuContexto.Context;
using MeuContexto.Service;
using ProjetoJqueryEstudos.Interfaces;
using ProjetoJqueryEstudos.Repositorie;

namespace MeuContexto.UOW
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IRepository _repository;
        private readonly IAddressRepository _iAddressService;
        private readonly IPersonRepository _iPersonService;
        private readonly IUserService _iuserServices;
        private readonly ILoggerRepository _iloggerService;
        private readonly ITrackingRepository _iTrackingService;
        private readonly ISubPersonRepository _iSubPersonService;
        private readonly ITrackingCodeEventsRepository _iTrackingCodeEvents;
        private readonly SignInManager<UserIdentity> _signInManager;
        private readonly UserManager<UserIdentity> _userManager;

        public UnitOfWork(AppDbContext appDbContext, SignInManager<UserIdentity> signInManager, UserManager<UserIdentity> userManager)
        {
            _repository = new Repository(appDbContext);
            _signInManager = signInManager;
            _userManager = userManager;
        }

        public UnitOfWork(AppDbContext appDbContext)
        {
            _repository = new Repository(appDbContext);
        }

        public IAddressRepository AddressService
        {
            get
            {
                return _iAddressService ?? new AddressRepository(_repository);
            }
        }

        public IPersonRepository PersonService
        {
            get
            {
                return _iPersonService ?? new PersonRepository(_repository);
            }
        }
        public IUserService UserService
        {
            get
            {
                return _iuserServices ?? new UserRepository(_signInManager, _userManager, _repository);
            }
        }
        public ILoggerRepository LoggerService
        {
            get
            {
                return _iloggerService ?? new LoggerService(_repository);
            }
        }
        public ISubPersonRepository SubPersonService
        {
            get
            {
                return _iSubPersonService ?? new SubPersonRepository(_repository);
            }
        }
        public ITrackingRepository TrackingService
        {
            get
            {
                return _iTrackingService ?? new TrackingRepository(_repository);
            }
        }
        public ITrackingCodeEventsRepository TrackingCodeEvents
        {
            get
            {
                return _iTrackingCodeEvents ?? new TrackingCodeEventsRepository(_repository);
            }
        }
    }
}


