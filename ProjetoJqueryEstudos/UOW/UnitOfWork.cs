using Microsoft.AspNetCore.Identity;
using ProjetoJqueryEstudos.Context;
using ProjetoJqueryEstudos.Entities;
using ProjetoJqueryEstudos.Interfaces;
using ProjetoJqueryEstudos.Repositorie;
using ProjetoJqueryEstudos.Service;

namespace ProjetoJqueryEstudos.UOW
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IRepository _repository;
        private readonly IAddressService _iAddressService;
        private readonly IPersonService _iPersonService;
        private readonly IUserService _iuserServices;
        private readonly ILoggerService _iloggerService;
        private readonly ITrackingService _iTrackingService;
        private readonly ISubPersonService _iSubPersonService;
        private readonly ITrackingCodeEventsService _iTrackingCodeEvents;
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

        public IAddressService AddressService
        {
            get
            {
                return _iAddressService ?? new AddressService(_repository);
            }
        }

        public IPersonService PersonService
        {
            get
            {
                return _iPersonService ?? new PersonService(_repository);
            }
        }
        public IUserService UserService
        {
            get
            {
                return _iuserServices ?? new UserService(_signInManager, _userManager, _repository);
            }
        }
        public ILoggerService LoggerService
        {
            get
            {
                return _iloggerService ?? new LoggerService(_repository);
            }
        }
        public ISubPersonService SubPersonService
        {
            get
            {
                return _iSubPersonService ?? new SubPersonService(_repository);
            }
        }
        public ITrackingService TrackingService
        {
            get
            {
                return _iTrackingService ?? new TrackingService(_repository);
            }
        }
        public ITrackingCodeEventsService TrackingCodeEvents
        {
            get
            {
                return _iTrackingCodeEvents ?? new TrackingCodeEventsService(_repository);
            }
        }
    }
}


