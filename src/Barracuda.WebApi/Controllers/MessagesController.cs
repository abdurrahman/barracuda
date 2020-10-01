using System;
using System.Threading.Tasks;
using Barracuda.Application.Message;
using Barracuda.WebApi.Hubs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace Barracuda.WebApi.Controllers
{
    public class MessagesController : BaseController
    {
        private readonly IMessageService _messageService;
        private readonly IHubContext<MessageHub> _hubContext;

        public MessagesController(IMessageService messageService, 
            IHubContext<MessageHub> hubContext)
        {
            _messageService = messageService;
            _hubContext = hubContext;
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
        public async Task<IActionResult> SendMessage([FromBody] MessageDto model)
        {
            await _hubContext.Clients.Client(model.RecipientId).SendAsync("NewMessage", DateTime.Now);
            
            await _messageService.SendMessage(new MessageDto
            {
                Text = model.Text,
                RecipientId = model.RecipientId,
                SenderId = UserId
            });
            
            return Ok();
        }

        // PUT api/messages/1
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMessage([FromRoute] int id, [FromBody] MessageDto model)
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