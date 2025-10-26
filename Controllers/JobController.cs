using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using JobPortalSystem.Data;
using JobPortalSystem.Models;

namespace JobPortalSystem.Controllers
{
    public class JobController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<JobController> _logger;

        public JobController(ApplicationDbContext context, ILogger<JobController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: Job
       public async Task<IActionResult> Index(string searchString, int? categoryId, string location, string type)
{
    // TEMPORARY DEBUG: Show ALL jobs without any filters
    var jobs = _context.Jobs
        .Include(j => j.Category)
        .Include(j => j.Employer)
        .AsQueryable();

    // Comment out the IsActive and LastDate filter temporarily
    // .Where(j => j.IsActive && j.LastDate >= DateTime.Now)

    // Load categories for filter dropdown
    ViewBag.Categories = await _context.Categories
        .Where(c => c.IsActive)
        .OrderBy(c => c.DisplayOrder)
        .ToListAsync();

    var jobList = await jobs
        .OrderByDescending(j => j.PostedDate)
        .ToListAsync();

    _logger.LogInformation($"Total jobs found: {jobList.Count}");

    return View(jobList);
}


        // GET: Job/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var job = await _context.Jobs
                .Include(j => j.Category)
                .Include(j => j.Employer)
                .Include(j => j.Applications)
                .FirstOrDefaultAsync(m => m.JobID == id);

            if (job == null)
            {
                return NotFound();
            }

            // Increment view count
            job.ViewCount++;
            await _context.SaveChangesAsync();

            return View(job);
        }

        // GET: Job/Search
        public IActionResult Search()
        {
            ViewBag.Categories = _context.Categories
                .Where(c => c.IsActive)
                .OrderBy(c => c.DisplayOrder)
                .ToList();

            return View();
        }
    }
}
