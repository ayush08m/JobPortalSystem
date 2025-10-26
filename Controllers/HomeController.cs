using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using JobPortalSystem.Data;
using JobPortalSystem.Models;
using JobPortalSystem.ViewModels;
using System.Diagnostics;




namespace JobPortal.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ApplicationDbContext context, ILogger<HomeController> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            // Get latest jobs for home page
            var latestJobs = await _context.Jobs
                .Include(j => j.Category)
                .Include(j => j.Employer)
                .Where(j => j.IsActive && j.LastDate >= DateTime.Now)
                .OrderByDescending(j => j.PostedDate)
                .Take(6)
                .ToListAsync();

            ViewBag.LatestJobs = latestJobs;

            // Get statistics
            ViewBag.TotalJobs = await _context.Jobs.Where(j => j.IsActive).CountAsync();
            ViewBag.TotalCategories = await _context.Categories.Where(c => c.IsActive).CountAsync();

            return View();
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }

    public class ErrorViewModel
    {
        public string RequestId { get; set; }
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}
