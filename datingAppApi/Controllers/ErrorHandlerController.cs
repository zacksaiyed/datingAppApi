using datingAppApi.Data;
using datingAppApi.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace datingAppApi.Controllers
{
    public class ErrorHandlerController : BaseApiController
    {
        private readonly DataContext context;

        public ErrorHandlerController(DataContext context)
        {
            this.context = context;
        }
        [HttpPost("auth")]
        [Authorize]
        public ActionResult<string> GetSecret()
        {
            return "secret Text";
        }

        [HttpPost("not-found")]
        public ActionResult<AppUser> GetNotFound()
        {
            var thing = context.Users.Find(-1);
            if (thing == null) return NotFound();

            return Ok(thing);
        }

        [HttpPost("server-error")]
        public ActionResult<string> GetServerError()
        {
            var thing = context.Users.Find(-1);
            var thingToReturn = thing.ToString();

            return thingToReturn;
        }

        [HttpPost("bad-request")]
        public ActionResult<string> GetBadRequest()
        {
            
            return BadRequest("It was a bad request");
        }
    }
}
