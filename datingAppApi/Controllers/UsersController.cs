using AutoMapper;
using datingAppApi.Data;
using datingAppApi.DTOs;
using datingAppApi.Entities;
using datingAppApi.Extensions;
using datingAppApi.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace datingAppApi.Controllers
{
    [Authorize]
    public class UsersController : BaseApiController
    {
        private readonly IUserRepository userRepository;
        private readonly IMapper mapper;
        private readonly IPhotoService photoService;

        public UsersController(IUserRepository userRepository,IMapper mapper, IPhotoService photoService)
        {
            this.userRepository = userRepository;
            this.mapper = mapper;
            this.photoService = photoService;
        }

        [HttpPost("GetUsers")]
        public async Task<ActionResult<IEnumerable<MemberDto>>> GetUsers()
        {
            var users = await userRepository.GetMembersAsync();
            
            return Ok(users);
        }

        [HttpPost("GetUser",Name ="GetUser")]
        public async Task<ActionResult<MemberDto>> GetUser([FromBody] GetUserDto request)
        {
            var users = await userRepository.GetMemberAsync(request.UserName);

            return users;
        }

        [HttpPost("UpdateUser")]
        public async Task<ActionResult> UpdateUser(MemberUpdateDto request)
        {
            var user = await userRepository.GetUserByUsernameAsync(User.GetUsername());

            mapper.Map(request, user);

            userRepository.Update(user);

            if (await userRepository.SaveAllAsync()) return NoContent();

            return BadRequest("Failed to update request");
        }


        [HttpPost("AddPhoto")]
        public async Task<ActionResult<PhotoDto>> AddPhoto(IFormFile file)
        {
            var user = await userRepository.GetUserByUsernameAsync(User.GetUsername());

            var result = await photoService.AddPhotoAsync(file);

            if (result.Error != null)
            {
                return BadRequest(result.Error.Message);
            }

            var photo = new Photo()
            {
                URL= result.SecureUrl.AbsoluteUri,
                PublicId=result.PublicId
            };

            if (user.Photos.Count==0)
            {
                photo.IsMain = true;
            }

            user.Photos.Add(photo);

            if (await userRepository.SaveAllAsync())
            {

                return CreatedAtRoute("GetUser",new GetUserDto(){UserName=User.GetUsername()} ,mapper.Map<PhotoDto>(photo));
            }

            return BadRequest("Failed to upload image");

        }

        [HttpPost("SetMainPhoto/{photoId}")]
        public async Task<ActionResult> SetMainPhoto(int photoId)
        {
            var user = await userRepository.GetUserByUsernameAsync(User.GetUsername());

            var photos = user.Photos.FirstOrDefault(x => x.Id == photoId);
            if (photos.IsMain) return BadRequest("This is already your main photo");

            var currentMain = user.Photos.FirstOrDefault(x => x.IsMain);
            if (currentMain != null) currentMain.IsMain = false;

            photos.IsMain = true;

            if (await userRepository.SaveAllAsync()) return NoContent();

            return BadRequest("Failed to set main photo");
        }



    }
}
