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
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public AdminController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Admin/Dashboard
        public async Task<IActionResult> Dashboard()
        {
            var model = new DashboardViewModel
            {
                TotalUsers = await _userManager.Users.CountAsync(),
                TotalJobsPosted = await _context.Jobs.CountAsync()
            };

            // Count users by role
            var employers = await _userManager.GetUsersInRoleAsync("Employer");
            var seekers = await _userManager.GetUsersInRoleAsync("Seeker");

            model.TotalEmployers = employers.Count;
            model.TotalSeekers = seekers.Count;

            return View(model);
        }

        // GET: Admin/ManageUsers
        public async Task<IActionResult> ManageUsers()
        {
            var users = await _userManager.Users.ToListAsync();
            return View(users);
        }

        // GET: Admin/ManageJobs
        public async Task<IActionResult> ManageJobs()
        {
            var jobs = await _context.Jobs
                .Include(j => j.Employer)
                .Include(j => j.Category)
                .OrderByDescending(j => j.PostedDate)
                .ToListAsync();

            return View(jobs);
        }

        // POST: Admin/DeactivateJob/5
        [HttpPost]
        public async Task<IActionResult> DeactivateJob(int id)
        {
            var job = await _context.Jobs.FindAsync(id);
            if (job != null)
            {
                job.IsActive = false;
                await _context.SaveChangesAsync();
                TempData["Success"] = "Job deactivated successfully!";
            }

            return RedirectToAction("ManageJobs");
        }

        // POST: Admin/DeactivateUser
        [HttpPost]
        public async Task<IActionResult> DeactivateUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                user.IsActive = false;
                await _userManager.UpdateAsync(user);
                TempData["Success"] = "User deactivated successfully!";
            }

            return RedirectToAction("ManageUsers");
        }

        // GET: Admin/Reports
        public async Task<IActionResult> Reports()
        {
            ViewBag.TotalJobs = await _context.Jobs.CountAsync();
            ViewBag.ActiveJobs = await _context.Jobs.Where(j => j.IsActive).CountAsync();
            ViewBag.TotalApplications = await _context.Applications.CountAsync();
            ViewBag.TotalCategories = await _context.Categories.CountAsync();

            return View();
        }
    }
}
