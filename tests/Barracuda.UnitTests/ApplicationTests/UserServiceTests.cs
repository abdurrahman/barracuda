using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Barracuda.Application;
using Barracuda.Application.Users;
using Barracuda.Core;
using Barracuda.Domain;
using Barracuda.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using NSubstitute;
using Xunit;

namespace Barracuda.UnitTests.ApplicationTests
{
    public class UserServiceTests
    {
        private readonly IRepository<UserBlock> _userBlockRepo;
        private readonly IObjectMapper _mapper;
        private readonly IUserStore<ApplicationUser> _userStore;

        public UserServiceTests()
        {
            _userBlockRepo = Substitute.For<IRepository<UserBlock>>();
            _mapper = new MapsterObjectMapper();
            _userStore = Substitute.For<IUserStore<ApplicationUser>>();
        }

        [Fact]
        public async Task AddUserBlock_InvokeRepository_AsOnce()
        {
            var userBlockList = new List<UserBlock>();
            _userBlockRepo.When(c => c.InsertAsync(Arg.Any<UserBlock>()))
                .Do(c => userBlockList.Add(c.ArgAt<UserBlock>(0)));
            
            var userBlock = new UserBlockDto
            {
                BlockedUserId = "blockUserId",
                ComplaintUserId = "complaintUserId"
            };

            var userService = new UserService(_userStore, _userBlockRepo, _mapper);
            await userService.BlockUser(userBlock);

            await _userBlockRepo.Received(1).InsertAsync(Arg.Any<UserBlock>());

            Assert.Single(userBlockList);
            Assert.Equal(userBlock.BlockedUserId, userBlockList.First().BlockedUserId);
        }
    }
}