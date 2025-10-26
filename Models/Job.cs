using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JobPortalSystem.Models
{
    public class Job
    {
        [Key]
        public int JobID { get; set; }

        [Required]
        public string EmployerID { get; set; } = string.Empty;

        [Required]
        [StringLength(200)]
        public string Title { get; set; } = string.Empty;

        [Required]
        [StringLength(5000)]
        public string Description { get; set; } = string.Empty;

        [Required]
        public int CategoryID { get; set; }

        [Required]
        [StringLength(100)]
        public string Location { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string Type { get; set; } = string.Empty;

        [StringLength(50)]
        public string? Experience { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? MinSalary { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? MaxSalary { get; set; }

        [StringLength(2000)]
        public string? Requirements { get; set; }

        [StringLength(2000)]
        public string? Benefits { get; set; }

        public DateTime PostedDate { get; set; } = DateTime.Now;

        public DateTime LastDate { get; set; }

        public bool IsActive { get; set; } = true;

        public string Status { get; set; } = "Open";

        public int ViewCount { get; set; } = 0;

        // Navigation Properties
        [ForeignKey("EmployerID")]
        public virtual ApplicationUser? Employer { get; set; }

        [ForeignKey("CategoryID")]
        public virtual Category? Category { get; set; }

        public virtual ICollection<Application>? Applications { get; set; }
    }
}
