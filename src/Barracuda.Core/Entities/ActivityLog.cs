using System.ComponentModel.DataAnnotations;

namespace Barracuda.Core
{
    public class ActivityLog : BaseEntity
    {
        /// <summary>
        /// Gets or sets the activity log type identifier
        /// </summary>
        public ActivityLogType ActivityLogType { get; set; }

        /// <summary>
        /// Gets or sets the user identifier
        /// </summary>
        [Required]
        [MaxLength(450)] // Columns participating in a foreign key relationship must be defined with the same length and scale.
        public string UserId { get; set; }

        /// <summary>
        /// Gets or sets the IP address
        /// </summary>
        [MaxLength(15)]
        public virtual string IpAddress { get; set; }
    }
}