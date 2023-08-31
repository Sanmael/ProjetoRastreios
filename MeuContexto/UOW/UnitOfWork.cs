using Domain.Interfaces;
using MeuContexto;
using MeuContexto.Context;
using MeuContexto.DataEntityRepositories;
using MeuContexto.EntityRepositories;
using MeuContexto.UOW;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace MeuContexto.UOW
{
    public class UnitOfWork : IUnitOfWork
    {
        private IRepository _repository;
        private readonly IAddressRepository _iAddressService;
        private readonly IPersonRepository _iPersonService;
        private readonly IUserService _iuserServices;
        private readonly ILoggerRepository _iloggerService;
        private readonly ITrackingRepository _iTrackingService;
        private readonly ISubPersonRepository _iSubPersonService;
        private readonly ITrackingCodeEventsRepository _iTrackingCodeEvents;
        private readonly IMailQueueRepository _iMailQueueRepository;
        private readonly SignInManager<UserIdentity> _signInManager;
        private readonly UserManager<UserIdentity> _userManager;

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
        public IMailQueueRepository MailQueueRepository
        {
            get
            {
                return _iMailQueueRepository ?? new MailQueueRepository(_repository);
            }
        }


        public UnitOfWork(EntityContext appDbContext, SignInManager<UserIdentity> signInManager, UserManager<UserIdentity> userManager)
        {
            _repository = new EntityRepository(appDbContext);
            _signInManager = signInManager;
            _userManager = userManager;
        }
        //refatorar gambiarra depois
        public UnitOfWork(EntityContext appDbContext)
        {
            _repository = new EntityRepository(appDbContext);
        }
        public UnitOfWork()
        {
            string connection = "Data Source=DESKTOP-AUTOI40\\SQLEXPRESS;Initial Catalog=NomeDoBancoDeDados;Integrated Security=True;Encrypt=False";

            _repository = new DapperRepository(new DapperContext(connection));
        }
    }   
}



