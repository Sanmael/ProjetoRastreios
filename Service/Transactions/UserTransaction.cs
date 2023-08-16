using Entities;
using MeuContexto;
using MeuContexto.Context;
using MeuContexto.UOW;
using System;

namespace Service.Transactions
{
    public class UserTransaction : IUserTransaction
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserTransaction(IUnitOfWork IUnitOfWork)
        {
            _unitOfWork = IUnitOfWork;
        }

        public async Task<bool> LoginAsync(string email, string password)
        {
            return await _unitOfWork.UserService.Login(email, password);
        }
        public async Task<bool> RegisterAsync(string email, string password)
        {
            return await _unitOfWork.UserService.Register(email, password);
        }
        public string GetUserIdByEmail(string email)
        {
            return _unitOfWork.UserService.GetUserByEmail(email).Id;
        }

        public Task<bool> Logout()
        {
            return _unitOfWork.UserService.Logout();
        }
    }
}

