using System.ComponentModel.DataAnnotations;

namespace JobPortalSystem.ViewModels
{
    public class JobViewModel
    {
        public int JobID { get; set; }

        [Required]
        [StringLength(200)]
        [Display(Name = "Job Title")]
        public string Title { get; set; } = string.Empty;

        [Required]
        [StringLength(5000)]
        [Display(Name = "Job Description")]
        public string Description { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Category")]
        public int CategoryID { get; set; }

        [Required]
        [StringLength(100)]
        public string Location { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Job Type")]
        public string Type { get; set; } = string.Empty;

        [Display(Name = "Experience Required")]
        public string? Experience { get; set; }

        [Display(Name = "Minimum Salary")]
        public decimal? MinSalary { get; set; }

        [Display(Name = "Maximum Salary")]
        public decimal? MaxSalary { get; set; }

        [StringLength(2000)]
        public string? Requirements { get; set; }

        [StringLength(2000)]
        public string? Benefits { get; set; }

        [Required]
        [Display(Name = "Application Deadline")]
        public DateTime LastDate { get; set; }

        // For display purposes
        public string? EmployerName { get; set; }
        public string? CategoryName { get; set; }
        public DateTime PostedDate { get; set; }
        public int ApplicationCount { get; set; }
    }
}
