using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Mvvm;

namespace API.IService
{
    public interface IAccounts
    {
        Task<API.DTOs.LoginRegisterDTO> Register(string userName, string password);
        Task<API.DTOs.LoginRegisterDTO> Login (API.Mvvm.Accounts.RegisterDTO dTO);
        Task<Common.Result> UsernameAlreadyExist(string userName);
    }
}