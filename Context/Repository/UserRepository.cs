using Entities.Interfaces;
using Microsoft.AspNetCore.Identity;
using ProjetoJqueryEstudos.Interfaces;

namespace MeuContexto.Service
{
    public class UserRepository : IUserService
    {
        private readonly SignInManager<UserIdentity> _signInManager;
        private readonly UserManager<UserIdentity> _userManager;
        private readonly IRepository repository;

        public UserRepository(SignInManager<UserIdentity> signInManager, UserManager<UserIdentity> userManager, IRepository repository)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            this.repository = repository;
        }

        public async Task<bool> Login(string username, string password)
        {
            try
            {

                UserIdentity? user = await _userManager.FindByNameAsync(username);

                if (user == null)
                    throw new PortalException("Usuario não encontrado");

                SignInResult isSucessLogin = await _signInManager.PasswordSignInAsync(username, password, isPersistent: false, false);

                if (!isSucessLogin.Succeeded)
                    throw new PortalException("Senha incorreta!");

                return true;
            }
            catch
            {
                throw;
            }

        }

        public async Task<bool> Logout()
        {
            try
            {
                await _signInManager.SignOutAsync();

                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> Register(string email, string password)
        {
            try
            {
                UserIdentity user = new UserIdentity() { UserName = email, Email = email };

                IdentityResult result = await _userManager.CreateAsync(user, password);

                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return true;
                }
                else
                    return false;
            }
            catch
            {
                return false;
            }
        }

        public async Task UpdateUserIdentityPerson(string userId, long personId)
        {
            UserIdentity userIdentity = await _userManager.FindByIdAsync(userId);

            userIdentity.PersonId = personId;

            _userManager.UpdateAsync(userIdentity);
        }
        public async Task<long> GetPersonIdByUserId(string userId)
        {
            UserIdentity userIdentity = await _userManager.FindByIdAsync(userId);
            return (long)userIdentity.PersonId;
        }

        public UserIdentity GetUserByEmail(string email)
        {
            return repository.GetEntity<UserIdentity>(x=> x.Email == email);
        }
    }
}
