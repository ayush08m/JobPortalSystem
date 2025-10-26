using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JobPortalSystem.Models
{
    public class Notification
    {
        [Key]
        public int NotificationID { get; set; }

        [Required]
        public string UserID { get; set; } = string.Empty;

        [Required]
        [StringLength(200)]
        public string Subject { get; set; } = string.Empty;

        [Required]
        public string Body { get; set; } = string.Empty;

        [StringLength(50)]
        public string? EmailType { get; set; }

        public bool IsSent { get; set; } = false;

        public DateTime? SentDate { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        [StringLength(1000)]
        public string? ErrorMessage { get; set; }

        // Navigation Properties
        [ForeignKey("UserID")]
        public virtual ApplicationUser? User { get; set; }
    }
}
