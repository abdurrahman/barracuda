using System;
using System.Threading.Tasks;
using Barracuda.Application;
using Barracuda.Application.Message;
using Barracuda.Core;
using Barracuda.Domain;
using Moq;
using Xunit;

namespace Barracuda.UnitTests.ApplicationTests
{
    public class MessageServiceTests
    {
        private readonly Mock<IRepository<Message>> _mockMessageRepo;
        private readonly IObjectMapper _mapper;

        public MessageServiceTests()
        {
            _mockMessageRepo = new Mock<IRepository<Message>>();
            
            _mapper = new MapsterObjectMapper();
        }

        [Fact]
        public async Task ShouldInvokeRepositorySendMessageAsOnce()
        {
            var message = new Message
            {
                Text = "New message",
                RecipientId = It.IsAny<string>(),
                SenderId = It.IsAny<string>(),
                CreateDate = DateTime.Now
            };

            _mockMessageRepo.Setup(c => c.FindByIdAsync(It.IsAny<int>())).ReturnsAsync(message);
            
            var messageService = new MessageService(_mapper, _mockMessageRepo.Object);

            await messageService.SendMessage(new MessageDto
            {
                Text = message.Text,
                RecipientId = "recipient",
                SenderId = "senderId"
            });
            
            _mockMessageRepo.Verify(c => c.FindByIdAsync(It.IsAny<int>()), Times.Once);
        }
    }
}