using JobPortalSystem.Models;

namespace JobPortalSystem.ViewModels
{
    public class DashboardViewModel
    {
        // For Seeker
        public int TotalApplications { get; set; }
        public int PendingApplications { get; set; }
        public int ShortlistedApplications { get; set; }
        public int RejectedApplications { get; set; }
        public List<Application>? RecentApplications { get; set; }
        public List<Job>? RecommendedJobs { get; set; }

        // For Employer
        public int TotalJobs { get; set; }
        public int ActiveJobs { get; set; }
        public int TotalApplicants { get; set; }
        public List<Job>? RecentJobs { get; set; }

        // For Admin
        public int TotalUsers { get; set; }
        public int TotalEmployers { get; set; }
        public int TotalSeekers { get; set; }
        public int TotalJobsPosted { get; set; }
    }
}
