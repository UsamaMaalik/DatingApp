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
    }
}