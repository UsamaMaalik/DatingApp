using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.IService;
using Microsoft.EntityFrameworkCore;

namespace API.Mvvm
{
    public class AppUser: IAppUser
    {
        private readonly DataContext _context ;
        public AppUser(DataContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<API.Entities.AppUser>> GetAppUsers()
        {
            try
            {
                var data = await _context.AppUsers.ToListAsync();
                return data;
            }
            catch (System.Exception e)
            {
                
            }
            return null;
        }
    }
}