using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
            [Required]
            public string UserName { get; set; }
            [Required]
            public string Password { get; set; }
        }
        public async Task<Common.Result> Register(string userName, string password)
        {
            var result = new Common.Result();
            try
            {
                result = await UsernameAlreadyExist(userName);

                if (result.Status)
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
                result.Status = await _context.AppUsers.AnyAsync(x => x.UserName.ToLower() == userName.ToLower());
                result.Message = result.Status ? "Already Exist" : "Not Exists";
                result.Status = !result.Status;
            }
            catch (System.Exception e)
            {
                result.Status = false;
                result.Message = e.Message;
            }
            return result;
        }

        public async Task<Common.Result> Login(RegisterDTO dTO)
        {
            var result = new Common.Result();
            try
            {
                var data = await _context.AppUsers.SingleOrDefaultAsync(x => x.UserName.ToLower() == dTO.UserName.ToLower());
                if(data == null)
                {
                    result.Status = false;
                    result.Message = "Invalid Username";
                    return result;
                }
                result = HashVerification(dTO.Password,data.PasswordSalt,data.PasswordHash);
            }
            catch (System.Exception e)
            {
                result.Status = false;
                result.Message = e.Message;
            }
            return result;
        }

        public Common.Result HashVerification(string Password, byte[] passwordSalt, byte[] passwordHash)
        {
            var result = new Common.Result();
            bool IsValidPassword = true;
            try
            {
                using (var hmac = new HMACSHA512(passwordSalt))
                {
                    var hash =  hmac.ComputeHash(Encoding.UTF8.GetBytes(Password));
                    if (hash.Length == passwordHash.Length && hash.Length > 0)
                    {
                        for (int i = 0; i < hash.Length; i++)
                        {
                            if (hash[i] != passwordHash[i])
                            {
                                IsValidPassword = false;
                                break;
                            }
                        }
                        if (IsValidPassword)
                        {
                            result.Status = true;
                            result.Message = "Authorized";
                        }
                        else
                        {
                            result.Status = false;
                            result.Message = "Invalid password";
                        }
                    }
                    else
                    {
                        result.Status = false;
                        result.Message = "Invalid password";
                    }
                }
            }
            catch (System.Exception e)
            {
                result.Status = false;
                result.Message = e.Message;
            }
            return  result;
        }
    }
}