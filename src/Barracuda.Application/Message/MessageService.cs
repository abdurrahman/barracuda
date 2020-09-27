using System.Collections.Generic;
using System.Threading.Tasks;
using Barracuda.Core;
using Barracuda.Domain;

namespace Barracuda.Application.Message
{
    public class MessageService : IMessageService
    {
        private readonly IObjectMapper _mapper;
        private readonly IMessageRepository _messageRepository;
        
        public MessageService(IObjectMapper mapper, 
            IMessageRepository messageRepository)
        {
            _mapper = mapper;
            _messageRepository = messageRepository;
        }
        
        public void SendMessage(MessageDto message)
        {
            var entity = _mapper.MapTo<Domain.Message>(message);

            _messageRepository.InsertAsync(entity);
        }

        public async Task<IList<MessageDto>> GetMessages(string userId)
        {
            // Todo: Use pagination while retrieving all messages.
            var result = await _messageRepository.GetListAsync(c => c.SenderId == userId);

            return _mapper.MapTo<List<MessageDto>>(result);
        }

        public async Task<MessageDto> FindMessageById(int id)
        {
            var result = await _messageRepository.FindByIdAsync(id);

            return _mapper.MapTo<MessageDto>(result);
        }
    }
}