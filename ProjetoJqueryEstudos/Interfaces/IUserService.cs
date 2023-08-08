using ProjetoJqueryEstudos.Entities;

namespace ProjetoJqueryEstudos.Interfaces
{
    public interface IUserService
    {
        public Task<bool> Register(string email, string password);
        public Task<bool> Login(string email, string password);
        public Task<bool> Logout();
        public Task UpdateUserIdentityPerson(string userId, long personId);
        public Task<long> GetPersonIdByUserId(string userId);
        public UserIdentity GetUserByEmail(string email);
    }
}
