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
    public class SeekerController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IWebHostEnvironment _environment;

        public SeekerController(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            IWebHostEnvironment environment)
        {
            _context = context;
            _userManager = userManager;
            _environment = environment;
        }

        // GET: Seeker/Dashboard
        public async Task<IActionResult> Dashboard()
        {
            var userId = _userManager.GetUserId(User);

            var applications = await _context.Applications
                .Include(a => a.Job)
                .Where(a => a.UserID == userId)
                .ToListAsync();

            var model = new DashboardViewModel
            {
                TotalApplications = applications.Count,
                PendingApplications = applications.Count(a => a.Status == "Applied"),
                ShortlistedApplications = applications.Count(a => a.Status == "Shortlisted"),
                RejectedApplications = applications.Count(a => a.Status == "Rejected"),
                RecentApplications = applications.OrderByDescending(a => a.AppliedDate).Take(5).ToList()
            };

            // Get recommended jobs
            var appliedCategories = applications
                .Select(a => a.Job.CategoryID)
                .Distinct()
                .ToList();

            model.RecommendedJobs = await _context.Jobs
                .Include(j => j.Category)
                .Include(j => j.Employer)
                .Where(j => j.IsActive && 
                           j.LastDate >= DateTime.Now &&
                           appliedCategories.Contains(j.CategoryID))
                .OrderByDescending(j => j.PostedDate)
                .Take(5)
                .ToListAsync();

            return View(model);
        }

        // GET: Seeker/Profile
        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            var user = await _userManager.GetUserAsync(User);

            var model = new ProfileViewModel
            {
                UserId = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Bio = user.Bio,
                CurrentProfilePicture = user.ProfilePicture
            };

            return View(model);
        }

        // POST: Seeker/Profile
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Profile(ProfileViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);

                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.PhoneNumber = model.PhoneNumber;
                user.Bio = model.Bio;

                // Handle profile picture upload
                if (model.ProfilePictureFile != null)
                {
                    var uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads", "profiles");
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    var uniqueFileName = $"{user.Id}_{DateTime.Now.Ticks}{Path.GetExtension(model.ProfilePictureFile.FileName)}";
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await model.ProfilePictureFile.CopyToAsync(stream);
                    }

                    user.ProfilePicture = $"/uploads/profiles/{uniqueFileName}";
                }

                var result = await _userManager.UpdateAsync(user);

                if (result.Succeeded)
                {
                    TempData["Success"] = "Profile updated successfully!";
                    return RedirectToAction("Profile");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View(model);
        }

        // GET: Seeker/Resumes
        public async Task<IActionResult> Resumes()
        {
            var userId = _userManager.GetUserId(User);

            var resumes = await _context.Resumes
                .Where(r => r.UserID == userId)
                .OrderByDescending(r => r.UploadDate)
                .ToListAsync();

            return View(resumes);
        }
    }
}
