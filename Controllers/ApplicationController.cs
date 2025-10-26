using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using JobPortalSystem.Data;
using JobPortalSystem.Models;
using JobPortalSystem.ViewModels;
using JobPortalSystem.Services;
using JobPortalSystem.Helpers;



namespace JobPortal.Controllers
{
    [Authorize(Roles = "Seeker")]
    public class ApplicationController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IWebHostEnvironment _environment;
        private readonly IEmailService _emailService;

        public ApplicationController(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            IWebHostEnvironment environment,
            IEmailService emailService)
        {
            _context = context;
            _userManager = userManager;
            _environment = environment;
            _emailService = emailService;
        }

        // GET: Application/Apply/5
        [HttpGet]
        public async Task<IActionResult> Apply(int jobId)
        {
            var job = await _context.Jobs
                .Include(j => j.Employer)
                .FirstOrDefaultAsync(j => j.JobID == jobId);

            if (job == null || !job.IsActive)
            {
                return NotFound();
            }

            var userId = _userManager.GetUserId(User);

            // Check if already applied
            var existingApplication = await _context.Applications
                .AnyAsync(a => a.JobID == jobId && a.UserID == userId);

            if (existingApplication)
            {
                TempData["Error"] = "You have already applied for this job.";
                return RedirectToAction("Details", "Job", new { id = jobId });
            }

            var model = new ApplicationViewModel
            {
                JobID = jobId,
                JobTitle = job.Title,
                CompanyName = $"{job.Employer.FirstName} {job.Employer.LastName}"
            };

            return View(model);
        }

        // POST: Application/Apply
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Apply(ApplicationViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Validate resume file
                if (!FileHelper.IsValidResumeFile(model.ResumeFile, out string errorMessage))
                {
                    ModelState.AddModelError("ResumeFile", errorMessage);
                    return View(model);
                }

                var userId = _userManager.GetUserId(User);
                var uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads", "resumes");

                try
                {
                    // Save resume file
                    var fileName = await FileHelper.SaveResumeAsync(model.ResumeFile, uploadsFolder, userId);
                    var relativePath = $"/uploads/resumes/{fileName}";

                    // Create application
                    var application = new Application
                    {
                        JobID = model.JobID,
                        UserID = userId,
                        ResumePath = relativePath,
                        CoverLetter = model.CoverLetter,
                        Status = "Applied",
                        AppliedDate = DateTime.Now
                    };

                    _context.Applications.Add(application);

                    // Save resume to Resumes table
                    var resume = new Resume
                    {
                        UserID = userId,
                        FileName = model.ResumeFile.FileName,
                        FilePath = relativePath,
                        FileSize = model.ResumeFile.Length,
                        FileType = Path.GetExtension(model.ResumeFile.FileName)
                    };

                    _context.Resumes.Add(resume);
                    await _context.SaveChangesAsync();

                    // Send confirmation email
                    var user = await _userManager.GetUserAsync(User);
                    var job = await _context.Jobs.FindAsync(model.JobID);
                    await _emailService.SendApplicationStatusEmailAsync(
                        user.Email,
                        $"{user.FirstName} {user.LastName}",
                        job.Title,
                        "Applied"
                    );

                    TempData["Success"] = "Application submitted successfully!";
                    return RedirectToAction("MyApplications");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Error submitting application: {ex.Message}");
                }
            }

            return View(model);
        }

        // GET: Application/MyApplications
        public async Task<IActionResult> MyApplications()
        {
            var userId = _userManager.GetUserId(User);

            var applications = await _context.Applications
                .Include(a => a.Job)
                .ThenInclude(j => j.Employer)
                .Include(a => a.Job.Category)
                .Where(a => a.UserID == userId)
                .OrderByDescending(a => a.AppliedDate)
                .ToListAsync();

            return View(applications);
        }

        // GET: Application/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userId = _userManager.GetUserId(User);

            var application = await _context.Applications
                .Include(a => a.Job)
                .ThenInclude(j => j.Employer)
                .Include(a => a.Job.Category)
                .FirstOrDefaultAsync(a => a.ApplicationID == id && a.UserID == userId);

            if (application == null)
            {
                return NotFound();
            }

            return View(application);
        }
    }
}
