using datingAppApi.Data;
using datingAppApi.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace datingAppApi.Controllers
{
    public class UsersController : BaseApiController
    {
        private readonly DataContext _context;

        public UsersController(DataContext context)
        {
            this._context = context;
        }

        [HttpPost("GetUsers")]
        public async Task<ActionResult<IEnumerable<AppUser>>> GetUsers()
        {
            var users = await _context.Users.ToListAsync();

            return users;
        }

        [HttpPost("GetUser")]
        public async Task<ActionResult<AppUser>> GetUser([FromBody] int id)
        {
            var users = await _context.Users.FindAsync(id);

            return users;
        }

    }
}
