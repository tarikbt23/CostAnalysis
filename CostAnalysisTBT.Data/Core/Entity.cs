using System;
using System.ComponentModel.DataAnnotations;

namespace CostAnalysisTBT.Data.Core
{
    public abstract class Entity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;

        public DateTime? ChangedOn { get; set; }

        public bool IsDeleted { get; set; } = false;

        [Required]
        public int CreatedBy { get; set; }
    }
}
