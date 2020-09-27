using System.Collections.Generic;
using System.Threading.Tasks;

namespace Barracuda.Application.Message
{
    public interface IMessageService
    {
        void SendMessage(MessageDto message);

        Task<IList<MessageDto>> GetMessages(string userId);

        Task<MessageDto> FindMessageById(int id);
    }
}