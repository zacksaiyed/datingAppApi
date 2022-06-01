using datingAppApi.Data;
using datingAppApi.DTOs;
using datingAppApi.Entities;
using datingAppApi.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace datingAppApi.Controllers
{
    public class AccountController:BaseApiController
    {
        private readonly DataContext context;
        private readonly ITokenService tokenService;

        public AccountController(DataContext context, ITokenService tokenService)
        {
            this.context = context;
            this.tokenService = tokenService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto request)
        {
            if (await UserExists(request.userName)) return BadRequest("Usernmae is taken");
            
                using var hmac = new HMACSHA512();
                var user = new AppUser()
                {
                    UserName = request.userName.ToLower(),
                    PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(request.password)),
                    PasswordSalt = hmac.Key
                };

                context.Users.Add(user);
                await context.SaveChangesAsync();

                return new UserDto { UserName=user.UserName, Token= tokenService.CreateToken(user)};

            
        }

        [HttpPost("login")]
        private async Task<ActionResult<UserDto>> Login(LoginDto request)
        {
            var user = await context.Users.SingleOrDefaultAsync(x => x.UserName == request.Username);
            if (user == null) return Unauthorized("Invalid username");

            using var hmac = new HMACSHA512(user.PasswordSalt);

            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(request.Password));

            for (int i = 0; i < computedHash.Length; i++)
            {
                if (computedHash[i] != user.PasswordHash[i]) return Unauthorized("Invalid Password");
            }

            return new UserDto {UserName=user.UserName, Token=tokenService.CreateToken(user) };
        }

        private async Task<bool> UserExists(string userName)
        {
           return await context.Users.AnyAsync(x => x.UserName == userName.ToLower());
        }
    }
}
