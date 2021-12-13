using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using API.Data;
using API.Entities;
using API.IService;
using Microsoft.EntityFrameworkCore;

namespace API.Mvvm
{
    public class Accounts : IAccounts
    {
        private readonly DataContext _context;
        public Accounts(DataContext context)
        {
            _context = context;
        }
        public class RegisterDTO
        {
            public string UserName { get; set; }
            public string Password { get; set; }
        }
        public async Task<Common.Result> Register(string userName, string password)
        {
            var result = new Common.Result();
            try
            {
                result = await UsernameAlreadyExist(userName);

                if(result.Status)
                {
                     using (var hmac = new HMACSHA512())
                {
                    var user = new Entities.AppUser()
                    {
                        UserName = userName,
                        PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password)),
                        PasswordSalt = hmac.Key
                    };
                    _context.AppUsers.Add(user);
                    await _context.SaveChangesAsync();
                    result.Status = true;
                    result.Message = $"{userName} Added Successfully";
                }
                }
            }
            catch (System.Exception e)
            {
                result.Status = false;
                result.Message = e.Message;
            }
            return result;
        }

        public async Task<Common.Result> UsernameAlreadyExist(string userName)
        {
            var result = new Common.Result();

            try
            {
                result.Status = await _context.AppUsers.AnyAsync( x => x.UserName.ToLower() == userName.ToLower() );
                result.Message = result.Status ? "Already Exist": "Not Exists";
                result.Status = !result.Status;
            }
            catch (System.Exception e)
            {
                result.Status = false;
                result.Message = e.Message;
            }
            return result;
        }
    }
}