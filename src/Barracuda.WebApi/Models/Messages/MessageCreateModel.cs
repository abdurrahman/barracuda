namespace Barracuda.WebApi.Models.Messages
{
    public class MessageCreateModel
    {
        public int SenderId { get; set; }
        
        public int RecipientId { get; set; }
        
        public string Text { get; set; }
    }
}