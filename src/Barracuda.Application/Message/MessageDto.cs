namespace Barracuda.Application.Message
{
    public class MessageDto
    {
        public string SenderId { get; set; }
        
        public string RecipientId { get; set; }
        
        public string Text { get; set; }
    }
}