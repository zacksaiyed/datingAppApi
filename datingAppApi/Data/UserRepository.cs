using AutoMapper;
using AutoMapper.QueryableExtensions;
using datingAppApi.DTOs;
using datingAppApi.Entities;
using datingAppApi.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace datingAppApi.Data
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext dataContext;
        private readonly IMapper mapper;

        public UserRepository(DataContext dataContext, IMapper mapper)
        {
            this.dataContext = dataContext;
            this.mapper = mapper;
        }

        public async Task<MemberDto> GetMemberAsync(string username)
        {
            return await dataContext.Users.Where(x => x.UserName == username).ProjectTo<MemberDto>(mapper.ConfigurationProvider).SingleOrDefaultAsync();
        }

        public async Task<IEnumerable<MemberDto>> GetMembersAsync()
        {
            return await dataContext.Users.ProjectTo<MemberDto>(mapper.ConfigurationProvider).ToListAsync();
        }

        public  async Task<AppUser> GetUserByIdAsync(int id)
        {
            return await dataContext.Users.FindAsync(id);
        }

        public  async Task<AppUser> GetUserByUsernameAsync(string username)
        {
            return await dataContext.Users.Include(p=>p.Photos).SingleOrDefaultAsync(x => x.UserName == username);
        }

        public async Task<IEnumerable<AppUser>> GetUsersAsync()
        {
            return await dataContext.Users.Include(p=>p.Photos).ToListAsync();
        }

        public async Task<bool> SaveAllAsync()
        {
            return await dataContext.SaveChangesAsync() > 0;
        }

        public void Update(AppUser user)
        {
            dataContext.Entry(user).State = EntityState.Modified;
        }
    }
}
