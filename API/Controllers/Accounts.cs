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
        public async Task<ActionResult<API.DTOs.LoginRegisterDTO>> Register(API.Mvvm.Accounts.RegisterDTO dTO)
        {

            var data = await iAccounts.Register(dTO.UserName, dTO.Password);

            return data.result.Status ? data : BadRequest(data.result.Message);
        }
        [HttpPost("login")]
        public async Task<ActionResult<API.DTOs.LoginRegisterDTO>> Login(API.Mvvm.Accounts.RegisterDTO dTO)
        {
            var data = await iAccounts.Login(dTO);
            return data.result.Status ? data : Unauthorized(data.result.Message);
        }

        [HttpPost("testing")]
        public ActionResult<Common.Result> Testing(API.Mvvm.Accounts.RegisterDTO dTO)
        {
            var result = new Common.Result(){Message = dTO.UserName, Status = true};

            return result.Status ? result : BadRequest(result.Message);

        }
    }
}