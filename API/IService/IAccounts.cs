using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Mvvm;

namespace API.IService
{
    public interface IAccounts
    {
        Task<Common.Result> Register(string userName, string password);
        Task<Common.Result> Login (API.Mvvm.Accounts.RegisterDTO dTO);
        Task<Common.Result> UsernameAlreadyExist(string userName);
    }
}