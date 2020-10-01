using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Barracuda.Core;

namespace Barracuda.Domain
{
    public class ApplicationUser : IdentityUser, IAggregateRoot
    {
        public ApplicationUser()
        {
            Messages = new HashSet<Message>();
        }
        
        [MaxLength(100)]
        public string FirstName { get; set; }

        [MaxLength(100)]
        public string LastName { get; set; }
        
        public DateTime CreateDate { get; set; }

        public virtual ICollection<Message> Messages { get; set; }
    }
}