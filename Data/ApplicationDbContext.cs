using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using JobPortalSystem.Models;

namespace JobPortalSystem.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Job> Jobs { get; set; }
        public DbSet<Application> Applications { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Resume> Resumes { get; set; }
        public DbSet<Notification> Notifications { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure relationships and constraints
            modelBuilder.Entity<Job>()
                .HasOne(j => j.Employer)
                .WithMany(u => u.Jobs)
                .HasForeignKey(j => j.EmployerID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Application>()
                .HasOne(a => a.Job)
                .WithMany(j => j.Applications)
                .HasForeignKey(a => a.JobID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Application>()
                .HasOne(a => a.User)
                .WithMany(u => u.Applications)
                .HasForeignKey(a => a.UserID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Resume>()
                .HasOne(r => r.User)
                .WithMany(u => u.Resumes)
                .HasForeignKey(r => r.UserID)
                .OnDelete(DeleteBehavior.Cascade);

            // Seed initial categories
            modelBuilder.Entity<Category>().HasData(
                new Category { CategoryID = 1, CategoryName = "Software Development", Description = "Programming and development jobs", DisplayOrder = 1 },
                new Category { CategoryID = 2, CategoryName = "Web Development", Description = "Frontend and backend web development", DisplayOrder = 2 },
                new Category { CategoryID = 3, CategoryName = "Mobile Development", Description = "iOS and Android app development", DisplayOrder = 3 },
                new Category { CategoryID = 4, CategoryName = "Data Science", Description = "Data analysis and machine learning", DisplayOrder = 4 },
                new Category { CategoryID = 5, CategoryName = "DevOps", Description = "Infrastructure and deployment", DisplayOrder = 5 },
                new Category { CategoryID = 6, CategoryName = "UI/UX Design", Description = "User interface and experience design", DisplayOrder = 6 },
                new Category { CategoryID = 7, CategoryName = "Quality Assurance", Description = "Testing and QA roles", DisplayOrder = 7 },
                new Category { CategoryID = 8, CategoryName = "Project Management", Description = "Project and product management", DisplayOrder = 8 }
            );

            // Configure decimal precision
            modelBuilder.Entity<Job>()
                .Property(j => j.MinSalary)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Job>()
                .Property(j => j.MaxSalary)
                .HasColumnType("decimal(18,2)");

            // Create indexes for performance
            modelBuilder.Entity<Job>()
                .HasIndex(j => j.PostedDate);

            modelBuilder.Entity<Job>()
                .HasIndex(j => j.CategoryID);

            modelBuilder.Entity<Application>()
                .HasIndex(a => a.Status);
        }
    }
}
