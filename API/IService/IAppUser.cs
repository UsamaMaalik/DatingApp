using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.IService
{
    public interface IAppUser
    {
         Task<IEnumerable<API.Entities.AppUser>> GetAppUsers();
    }
}