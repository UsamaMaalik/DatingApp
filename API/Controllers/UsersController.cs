
using System.Collections.Generic;
using API.Entities;
using API.IService;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
   
    public class UsersController : BaseApiController
    {
        public UsersController(IAppUser iAppUser) : base(iAppUser)
        {
        }

        [HttpGet]
        public async Task<IEnumerable<API.Entities.AppUser>> GetAppUsers()
        {
            var data = await _iAppUser.GetAppUsers();
            return data;
        }

        [HttpGet("{id}")]
        public async Task <AppUser> GetAppUsers(int Id)
        {
            var data = await _iAppUser.GetAppUsers();
            return data.FirstOrDefault(x => x.Id ==Id );
        }

    }
}