using System;
using Barracuda.Core;

namespace Barracuda.Domain
{
    public class Message : BaseEntity, IAggregateRoot
    {
        public Message()
        {
            CreateDate = DateTime.UtcNow;
        }
        public string SenderId { get; set; }
        
        public string RecipientId { get; set; }
        
        public string Text { get; set; }
        public DateTime CreateDate { get; set; }
        
        public ApplicationUser User { get; set; }
    }
}