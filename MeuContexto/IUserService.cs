
namespace MeuContexto
{
    public interface IUserService
    {
        public Task<bool> Register(string email, string password);
        public Task<bool> Login(string email, string password);
        public Task<bool> Logout();
        public Task UpdateUserIdentityPerson(string userId, int personId);
        public Task<int> GetPersonIdByUserId(string userId);
        public UserIdentity GetUserByEmail(string email);
    }
}
