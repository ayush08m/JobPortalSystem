using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace JobPortalSystem.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [StringLength(100)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(100)]
        public string LastName { get; set; }

        [StringLength(200)]
        public string? ProfilePicture { get; set; }

        [StringLength(500)]
        public string? Bio { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public DateTime? ModifiedDate { get; set; }

        public bool IsActive { get; set; } = true;

        // Navigation Properties
        public virtual ICollection<Job>? Jobs { get; set; }
        public virtual ICollection<Application>? Applications { get; set; }
        public virtual ICollection<Resume>? Resumes { get; set; }
        public virtual ICollection<Notification>? Notifications { get; set; }
    }
}
