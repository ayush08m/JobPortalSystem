using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace JobPortalSystem.ViewModels
{
    public class ApplicationViewModel
    {
        public int ApplicationID { get; set; }

        [Required]
        public int JobID { get; set; }

        [Required]
        [Display(Name = "Upload Resume")]
        public IFormFile? ResumeFile { get; set; }

        [StringLength(2000)]
        [Display(Name = "Cover Letter")]
        public string? CoverLetter { get; set; }

        // For display
        public string? JobTitle { get; set; }
        public string? CompanyName { get; set; }
        public string? Status { get; set; }
        public DateTime AppliedDate { get; set; }
    }
}
