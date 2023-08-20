namespace Service
{
    public interface IUserTransaction
    {
        Task<bool> LoginAsync(string email, string password);
        Task<bool> RegisterAsync(string email, string password);
        Task<bool> Logout();
        Task<string > GetUserIdByEmailAsync(string email);
    }
}
