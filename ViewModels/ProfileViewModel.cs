using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace JobPortalSystem.ViewModels
{
    public class ProfileViewModel
    {
        public string? UserId { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Phone]
        [Display(Name = "Phone Number")]
        public string? PhoneNumber { get; set; }

        [StringLength(500)]
        public string? Bio { get; set; }

        [Display(Name = "Profile Picture")]
        public IFormFile? ProfilePictureFile { get; set; }

        public string? CurrentProfilePicture { get; set; }
    }
}
