using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using JobPortalSystem.Data;
using JobPortalSystem.Models;
using JobPortalSystem.ViewModels;
using JobPortalSystem.Services;

namespace JobPortalSystem.Controllers
{
    [Authorize(Roles = "Employer")]
    public class EmployerController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailService _emailService;

        public EmployerController(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            IEmailService emailService)
        {
            _context = context;
            _userManager = userManager;
            _emailService = emailService;
        }

        // GET: Employer/Dashboard
        public async Task<IActionResult> Dashboard()
        {
            var userId = _userManager.GetUserId(User);

            var jobs = await _context.Jobs
                .Include(j => j.Category)
                .Include(j => j.Applications)
                .Where(j => j.EmployerID == userId)
                .OrderByDescending(j => j.PostedDate)
                .ToListAsync();

            var model = new DashboardViewModel
            {
                TotalJobs = jobs.Count,
                ActiveJobs = jobs.Count(j => j.IsActive),
                TotalApplicants = jobs.Sum(j => j.Applications?.Count ?? 0),
                RecentJobs = jobs.Take(5).ToList()
            };

            return View(model);
        }

        // GET: Employer/PostJob
        [HttpGet]
        public IActionResult PostJob()
        {
            ViewBag.Categories = _context.Categories
                .Where(c => c.IsActive)
                .OrderBy(c => c.DisplayOrder)
                .ToList();

            return View();
        }

        // POST: Employer/PostJob
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PostJob(JobViewModel model)
        {
            if (ModelState.IsValid)
            {
                var job = new Job
                {
                    EmployerID = _userManager.GetUserId(User) ?? string.Empty,
                    Title = model.Title,
                    Description = model.Description,
                    CategoryID = model.CategoryID,
                    Location = model.Location,
                    Type = model.Type,
                    Experience = model.Experience,
                    MinSalary = model.MinSalary,
                    MaxSalary = model.MaxSalary,
                    Requirements = model.Requirements,
                    Benefits = model.Benefits,
                    LastDate = model.LastDate,
                    PostedDate = DateTime.Now,
                    IsActive = true,
                    Status = "Open"
                };

                _context.Jobs.Add(job);
                await _context.SaveChangesAsync();

                TempData["Success"] = "Job posted successfully!";
                return RedirectToAction("Dashboard");
            }

            ViewBag.Categories = _context.Categories
                .Where(c => c.IsActive)
                .OrderBy(c => c.DisplayOrder)
                .ToList();

            return View(model);
        }

        // GET: Employer/ManageJobs
        public async Task<IActionResult> ManageJobs()
        {
            var userId = _userManager.GetUserId(User);

            var jobs = await _context.Jobs
                .Include(j => j.Category)
                .Include(j => j.Applications)
                .Where(j => j.EmployerID == userId)
                .OrderByDescending(j => j.PostedDate)
                .ToListAsync();

            return View(jobs);
        }

        // GET: Employer/EditJob/5
        [HttpGet]
        public async Task<IActionResult> EditJob(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userId = _userManager.GetUserId(User);
            var job = await _context.Jobs.FirstOrDefaultAsync(j => j.JobID == id && j.EmployerID == userId);

            if (job == null)
            {
                return NotFound();
            }

            var model = new JobViewModel
            {
                JobID = job.JobID,
                Title = job.Title,
                Description = job.Description,
                CategoryID = job.CategoryID,
                Location = job.Location,
                Type = job.Type,
                Experience = job.Experience,
                MinSalary = job.MinSalary,
                MaxSalary = job.MaxSalary,
                Requirements = job.Requirements,
                Benefits = job.Benefits,
                LastDate = job.LastDate
            };

            ViewBag.Categories = _context.Categories
                .Where(c => c.IsActive)
                .OrderBy(c => c.DisplayOrder)
                .ToList();

            return View(model);
        }

        // POST: Employer/EditJob/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditJob(int id, JobViewModel model)
        {
            if (id != model.JobID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var userId = _userManager.GetUserId(User);
                var job = await _context.Jobs.FirstOrDefaultAsync(j => j.JobID == id && j.EmployerID == userId);

                if (job == null)
                {
                    return NotFound();
                }

                job.Title = model.Title;
                job.Description = model.Description;
                job.CategoryID = model.CategoryID;
                job.Location = model.Location;
                job.Type = model.Type;
                job.Experience = model.Experience;
                job.MinSalary = model.MinSalary;
                job.MaxSalary = model.MaxSalary;
                job.Requirements = model.Requirements;
                job.Benefits = model.Benefits;
                job.LastDate = model.LastDate;

                await _context.SaveChangesAsync();

                TempData["Success"] = "Job updated successfully!";
                return RedirectToAction("ManageJobs");
            }

            ViewBag.Categories = _context.Categories
                .Where(c => c.IsActive)
                .OrderBy(c => c.DisplayOrder)
                .ToList();

            return View(model);
        }

        // POST: Employer/DeleteJob/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteJob(int id)
        {
            var userId = _userManager.GetUserId(User);
            var job = await _context.Jobs.FirstOrDefaultAsync(j => j.JobID == id && j.EmployerID == userId);

            if (job != null)
            {
                job.IsActive = false;
                await _context.SaveChangesAsync();
                TempData["Success"] = "Job deleted successfully!";
            }

            return RedirectToAction("ManageJobs");
        }

        // GET: Employer/ViewApplicants/5
        public async Task<IActionResult> ViewApplicants(int jobId)
        {
            var userId = _userManager.GetUserId(User);

            var job = await _context.Jobs
                .Include(j => j.Applications)
                .ThenInclude(a => a.User)
                .FirstOrDefaultAsync(j => j.JobID == jobId && j.EmployerID == userId);

            if (job == null)
            {
                return NotFound();
            }

            return View(job);
        }

        // POST: Employer/UpdateApplicationStatus
        [HttpPost]
        public async Task<IActionResult> UpdateApplicationStatus(int applicationId, string status)
        {
            var application = await _context.Applications
                .Include(a => a.User)
                .Include(a => a.Job)
                .FirstOrDefaultAsync(a => a.ApplicationID == applicationId);

            if (application != null)
            {
                application.Status = status;
                application.UpdatedDate = DateTime.Now;
                await _context.SaveChangesAsync();

                // Send email notification
                if (application.User?.Email != null && application.Job?.Title != null)
                {
                    await _emailService.SendApplicationStatusEmailAsync(
                        application.User.Email,
                        $"{application.User.FirstName} {application.User.LastName}",
                        application.Job.Title,
                        status
                    );
                }

                TempData["Success"] = "Application status updated successfully!";
            }

            return RedirectToAction("ViewApplicants", new { jobId = application?.JobID ?? 0 });
        }
    }
}
