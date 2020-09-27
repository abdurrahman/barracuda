using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Barracuda.Core
{
    /// <summary>
    /// Base class with integer identifier for entities
    /// </summary>
    [Serializable]
    public class BaseEntity : BaseEntity<int>
    {
    }
    
    /// <summary>
    /// Base class for entities
    /// </summary>
    [Serializable]
    public class BaseEntity<TKey> : IEntity<TKey>
    {
        /// <summary>
        /// Gets or sets the entity identifier
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual TKey Id { get; set; }
    }
}