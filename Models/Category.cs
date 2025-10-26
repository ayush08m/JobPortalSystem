using System.ComponentModel.DataAnnotations;

namespace JobPortalSystem.Models
{
    public class Category
    {
        [Key]
        public int CategoryID { get; set; }

        [Required]
        [StringLength(100)]
        public string CategoryName { get; set; } = string.Empty;

        [StringLength(500)]
        public string? Description { get; set; }

        public bool IsActive { get; set; } = true;

        public int DisplayOrder { get; set; }

        // Navigation Properties
        public virtual ICollection<Job>? Jobs { get; set; }
    }
}
