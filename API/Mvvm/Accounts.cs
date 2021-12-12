using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using API.Data;
using API.Entities;
using API.IService;

namespace API.Mvvm
{
    public class Accounts : IAccounts
    {
        private readonly DataContext _context;
        public Accounts(DataContext context)
        {
            _context = context;
        }
        public async Task<Common.Result> Register(string userName, string password)
        {
            var result = new Common.Result();
            try
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
            catch (System.Exception e)
            {
                result.Status = false;
                result.Message = e.Message;
            }
            return result;
        }
    }
}