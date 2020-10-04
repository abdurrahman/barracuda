using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Barracuda.Application;
using Barracuda.Application.Message;
using Barracuda.Core;
using Barracuda.Domain;
using Barracuda.UnitTests.Utilities;
using NSubstitute;
using Xunit;

namespace Barracuda.UnitTests.ApplicationTests
{
    public class MessageServiceTests
    {
        private readonly IRepository<Message> _messageRepo;
        private readonly IObjectMapper _mapper;

        public MessageServiceTests()
        {
            _messageRepo = Substitute.For<IRepository<Message>>();
            _mapper = new MapsterObjectMapper();
        }

        [Fact]
        public async Task SendMessage_InvokeRepository_AsOnce()
        {
            var messageList = new List<Message>();
            _messageRepo.When(c => c.InsertAsync(Arg.Any<Message>()))
                .Do(c => messageList.Add(c.ArgAt<Message>(0)));

            var message = new MessageDto
            {
                Text = "New message",
                RecipientId = "recipient",
                SenderId = "senderId"
            };
            
            var messageService = new MessageService(_mapper, _messageRepo);
            await messageService.SendMessage(message);
            
            await _messageRepo.Received(1).InsertAsync(Arg.Any<Message>());

            Assert.Single(messageList);
            Assert.Equal(message.Text, messageList.First().Text);
        }

        [Fact]
        public async Task FindMessageById_InvokeRepository_AsOnce()
        {
            // Arrange
            var message = new Message {Id = 1, Text = "New message"};
            _messageRepo.FindByIdAsync(Arg.Any<int>()).Returns(message);
            
            var messageService = new MessageService(_mapper, _messageRepo);
            var response = await messageService.FindMessageById(message.Id);

            await _messageRepo.Received(1).FindByIdAsync(message.Id);

            Assert.Equal(message.Text, response.Text);
        }

        [Theory]
        [InlineData(true, true, 2)]
        [InlineData(true, false, 1)]
        [InlineData(false, true, 1)]
        [InlineData(false, false, 0)]
        public async Task GetMessages_ReturnUserMessages(bool hasSent, bool hasReceived, 
            int expectedMessageCount)
        {
            var userId = "13";
            var messageList = new List<Message>
            {
                new Message {Id = 3, SenderId = string.Empty, RecipientId = string.Empty}
            };

            if (hasSent)
            {
                messageList.Add(
                    new Message {Id = 1, SenderId = userId, RecipientId = string.Empty});
            }

            if (hasReceived)
            {
                messageList.Add(
                    new Message {Id = 2, SenderId = string.Empty, RecipientId = userId});
            }

            var queryable = new MockQueryable<Message>(messageList.AsQueryable());
            
            _messageRepo.TableNoTracking.Returns(queryable);
            
            var messageService = new MessageService(_mapper, _messageRepo);
            var response = await messageService.GetMessages(userId);
            
            Assert.Equal(expectedMessageCount, response.Count);
        }
    }
}