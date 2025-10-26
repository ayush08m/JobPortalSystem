using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JobPortalSystem.Models
{
    public class Application
    {
        [Key]
        public int ApplicationID { get; set; }

        [Required]
        public int JobID { get; set; }

        [Required]
        public string UserID { get; set; } = string.Empty;

        [Required]
        [StringLength(500)]
        public string ResumePath { get; set; } = string.Empty;

        [StringLength(2000)]
        public string? CoverLetter { get; set; }

        [Required]
        [StringLength(50)]
        public string Status { get; set; } = "Applied";

        public DateTime AppliedDate { get; set; } = DateTime.Now;

        public DateTime? UpdatedDate { get; set; }

        [StringLength(1000)]
        public string? EmployerNotes { get; set; }

        // Navigation Properties
        [ForeignKey("JobID")]
        public virtual Job? Job { get; set; }

        [ForeignKey("UserID")]
        public virtual ApplicationUser? User { get; set; }
    }
}
