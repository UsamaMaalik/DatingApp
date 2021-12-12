using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using API.IService;
using API.Mvvm;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace API.Controllers
{
    public class Accounts : BaseApiController
    {
        private readonly IAccounts iAccounts;
        public Accounts(IAccounts iAccounts)
        {
            this.iAccounts = iAccounts;
        }
        [HttpPost("register")]
        public async Task<Common.Result> Register(string userName, string password)
        {
            var result = await iAccounts.Register(userName, password);
            return result;
        }

    }
}