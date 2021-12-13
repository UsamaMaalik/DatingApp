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
        public async Task<ActionResult<Common.Result>> Register(API.Mvvm.Accounts.RegisterDTO dTO)
        {
            var result = await iAccounts.Register(dTO.UserName, dTO.Password);

            return result.Status ? result : BadRequest(result.Message);
        }
    }
}