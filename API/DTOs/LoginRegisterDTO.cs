using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Mvvm;

namespace API.DTOs
{
    public class LoginRegisterDTO
    {
        public string Username {get;set;}
        public string Token {get;set;}
        public Common.Result result {get;set;}
    }
}