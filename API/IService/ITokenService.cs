using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.IService
{
    public interface ITokenService
    {
        string CreateToken (API.Entities.AppUser user );
    }
}