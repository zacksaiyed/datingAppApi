using datingAppApi.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace datingAppApi.Interfaces
{
    public interface ITokenService
    {
        string CreateToken(AppUser user);
    }
}
