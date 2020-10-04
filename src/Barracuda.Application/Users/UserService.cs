using System;
using System.Threading;
using System.Threading.Tasks;
using Barracuda.Core;
using Barracuda.Domain;
using Barracuda.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Barracuda.Application.Users
{
    public class UserService : IUserService
    {
        private readonly IObjectMapper _mapper;
        private readonly IRepository<UserBlock> _userBlockRepository;
        private readonly IUserStore<ApplicationUser> _userStore;

        public UserService(IUserStore<ApplicationUser> userStore,
            IRepository<UserBlock> userBlockRepository,
            IObjectMapper mapper)
        {
            _userStore = userStore;
            _userBlockRepository = userBlockRepository;
            _mapper = mapper;
        }

        public async Task<ApplicationUser> GetUserByUserName(string username)
            => await _userStore.FindByNameAsync(username, CancellationToken.None);

        public async Task BlockUser(UserBlockDto model)
        {
            var entity = _mapper.MapTo<UserBlock>(model);
            entity.CreateDate = DateTime.UtcNow;

            await _userBlockRepository.InsertAsync(entity);
        }
    }
}