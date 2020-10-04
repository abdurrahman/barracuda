using System;
using System.ComponentModel.DataAnnotations;
using Barracuda.Core;

namespace Barracuda.Domain.Entities
{
    public class UserBlock : BaseEntity
    {
        [MaxLength(50)]
        public string ComplaintUserId { get; set; }
        
        [MaxLength(50)]
        public string BlockedUserId { get; set; }
        
        public DateTime CreateDate { get; set; }
    }
}