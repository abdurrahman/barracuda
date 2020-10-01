using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Barracuda.Core;
using Microsoft.EntityFrameworkCore;

namespace Barracuda.Application.Message
{
    public class MessageService : IMessageService
    {
        private readonly IObjectMapper _mapper;
        private readonly IRepository<Domain.Message> _messageRepository;
        
        public MessageService(IObjectMapper mapper, 
            IRepository<Domain.Message> messageRepository)
        {
            _mapper = mapper;
            _messageRepository = messageRepository;
        }
        
        public async Task SendMessage(MessageDto message)
        {
            var entity = _mapper.MapTo<Domain.Message>(message);

            await _messageRepository.InsertAsync(entity);
        }

        public async Task<IList<MessageDto>> GetMessages(string userId)
        {
            // Todo: Use pagination while retrieving all messages.
            var result = await _messageRepository.TableNoTracking
                .Where(c => (c.SenderId == userId || c.RecipientId == userId) && (c.RecipientId == userId || c.SenderId == userId))
                .ToListAsync();

            return _mapper.MapTo<List<MessageDto>>(result);
        }

        public async Task<MessageDto> FindMessageById(int id)
        {
            var result = await _messageRepository.FindByIdAsync(id);

            return _mapper.MapTo<MessageDto>(result);
        }
    }
}