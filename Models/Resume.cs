using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JobPortalSystem.Models
{
    public class Resume
    {
        [Key]
        public int ResumeID { get; set; }

        [Required]
        public string UserID { get; set; } = string.Empty;

        [Required]
        [StringLength(200)]
        public string FileName { get; set; } = string.Empty;

        [Required]
        [StringLength(500)]
        public string FilePath { get; set; } = string.Empty;

        [Required]
        public long FileSize { get; set; }

        [Required]
        [StringLength(50)]
        public string FileType { get; set; } = string.Empty;

        public DateTime UploadDate { get; set; } = DateTime.Now;

        public bool IsDefault { get; set; } = false;

        [StringLength(4000)]
        public string? ParsedData { get; set; }

        // Navigation Properties
        [ForeignKey("UserID")]
        public virtual ApplicationUser? User { get; set; }
    }
}
