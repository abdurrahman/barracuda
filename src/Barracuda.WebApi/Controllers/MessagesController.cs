using System.Threading.Tasks;
using Barracuda.Application.Message;
using Barracuda.WebApi.Models.Messages;
using Microsoft.AspNetCore.Mvc;

namespace Barracuda.WebApi.Controllers
{
    public class MessagesController : BaseController
    {
        private readonly IMessageService _messageService;

        public MessagesController(IMessageService messageService)
        {
            _messageService = messageService;
        }

        // GET api/messages
        [HttpGet]
        public async Task<IActionResult> GetMessages()
        {
            return Ok(await _messageService.GetMessages(UserId));
        }

        // GET api/messages/1
        [HttpGet("{id}")]
        public async Task<IActionResult> GetMessage([FromRoute] int id)
        {
            var result = await _messageService.FindMessageById(id);
            
            return Ok(result);
        }

        // POST api/messages
        [HttpPost]
        public async Task<IActionResult> CreateMessage([FromBody] MessageCreateModel model)
        {
            await Task.FromResult(0);
            return Ok();
        }

        // PUT api/messages/1
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMessage([FromRoute] int id, [FromBody] MessageUpdateModel model)
        {
            await Task.FromResult(0);
            return Ok();
        }
        
        // DELETE api/messages/1
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMessage([FromRoute] int id)
        {
            await Task.FromResult(0);
            return Ok();
        }
    }
}