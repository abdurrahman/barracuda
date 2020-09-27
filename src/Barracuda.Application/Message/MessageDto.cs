namespace Barracuda.Application.Message
{
    public class MessageDto
    {
        public int SenderId { get; set; }
        
        public int RecipientId { get; set; }
        
        public string Text { get; set; }
    }
}