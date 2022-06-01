using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace datingAppApi.Extensions
{
    public static class ClaimsPrincipleExtensions
    {
        public static string GetUsername(this ClaimsPrincipal claims)
        {
            return  claims.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            

        }
    }
}
