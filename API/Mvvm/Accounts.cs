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
        private readonly ITokenService tokenService;
        public Accounts(DataContext context,ITokenService _tokenSerivce )
        {
            _context = context;
            tokenService = _tokenSerivce;
        }
        public class RegisterDTO
        {
            [Required]
            public string UserName { get; set; }
            [Required]
            public string Password { get; set; }
        }
        public async Task<API.DTOs.LoginRegisterDTO> Register(string userName, string password)
        {
            var result = new Common.Result();
            var data = new Entities.AppUser();
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
                        data.UserName = user.UserName;
                        data.PasswordHash = user.PasswordHash;
                        data.PasswordSalt = user.PasswordSalt;
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
            return new API.DTOs.LoginRegisterDTO(){ Username = data.UserName, Token = tokenService.CreateToken(data),result= result } ;
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

        public async Task<API.DTOs.LoginRegisterDTO> Login(RegisterDTO dTO)
        {
            var result = new Common.Result();
            var user = new Entities.AppUser();
            try
            {
                var data = await _context.AppUsers.SingleOrDefaultAsync(x => x.UserName.ToLower() == dTO.UserName.ToLower());
                if(data == null)
                {
                    result.Status = false;
                    result.Message = "Invalid Username";
                }
                result = HashVerification(dTO.Password,data.PasswordSalt,data.PasswordHash);
                if(result.Status)
                {
                    user.UserName = data.UserName;
                    user.PasswordHash = data.PasswordHash;
                    user.PasswordSalt = data.PasswordSalt;
                }
            }
            catch (System.Exception e)
            {
                result.Status = false;
                result.Message = e.Message;
            }
            var dtoTemp = new API.DTOs.LoginRegisterDTO(){ Username = user.UserName, Token = result.Status ? tokenService.CreateToken(user) : "" ,result = result};
            return dtoTemp ;
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