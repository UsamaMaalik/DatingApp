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
        [HttpPost("login")]
        public async Task<ActionResult<Common.Result>> Login(API.Mvvm.Accounts.RegisterDTO dTO)
        {
            var result = await iAccounts.Login(dTO);
            return result.Status ? result : Unauthorized(result.Message);
        }

        [HttpPost("testing")]
        public ActionResult<Common.Result> Testing(API.Mvvm.Accounts.RegisterDTO dTO)
        {
            var result = new Common.Result(){Message = dTO.UserName, Status = true};

            return result.Status ? result : BadRequest(result.Message);

        }
    }
}