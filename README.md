# 🚀 Job Portal System

A comprehensive web-based job portal built with **ASP.NET Core MVC** where companies can post jobs and job seekers can apply, featuring user authentication, role-based access control, resume uploads, and application tracking.

![ASP.NET Core](https://img.shields.io/badge/ASP.NET%20Core-9.0-blue)
![Entity Framework](https://img.shields.io/badge/Entity%20Framework-9.0-green)
![License](https://img.shields.io/badge/License-MIT-yellow)

## 📋 Table of Contents

- [Features](#features)
- [Technology Stack](#technology-stack)
- [Prerequisites](#prerequisites)
- [Installation](#installation)
- [Database Setup](#database-setup)
- [Configuration](#configuration)
- [Running the Application](#running-the-application)
- [User Roles](#user-roles)
- [Project Structure](#project-structure)
- [Screenshots](#screenshots)
- [Contributing](#contributing)
- [License](#license)

## ✨ Features

### For Job Seekers
- ✅ User registration and authentication
- ✅ Browse and search job listings with advanced filters
- ✅ Upload and manage resumes (PDF/DOC/DOCX)
- ✅ Apply to jobs with one click
- ✅ Track application status (Applied, Shortlisted, Rejected, Interviewed)
- ✅ Personalized dashboard with application statistics
- ✅ Email notifications for application updates
- ✅ Profile management

### For Employers
- ✅ Company registration and login
- ✅ Post and manage job listings (Create, Read, Update, Delete)
- ✅ View all applicants for each job
- ✅ Shortlist, reject, or schedule interviews
- ✅ Dashboard with hiring metrics
- ✅ Email notifications to applicants

### For Administrators
- ✅ Manage all users (Seekers & Employers)
- ✅ Monitor and manage all job postings
- ✅ System analytics and reporting
- ✅ Remove inappropriate content
- ✅ User role management

### Security & Validation
- 🔐 ASP.NET Core Identity with role-based authorization
- 🔐 Password encryption and secure authentication
- 🔐 Anti-CSRF tokens on all forms
- 🔐 File upload validation (type, size, extension)
- 🔐 SQL injection protection via Entity Framework

## 🛠️ Technology Stack

| Technology | Version | Purpose |
|------------|---------|---------|
| **ASP.NET Core MVC** | 9.0 | Web framework |
| **Entity Framework Core** | 9.0 | ORM for database operations |
| **SQLite** | 3.x | Database (Development) |
| **ASP.NET Identity** | 9.0 | Authentication & Authorization |
| **Bootstrap** | 5.3 | Responsive UI framework |
| **Font Awesome** | 6.0 | Icons |
| **SendGrid** | 9.29 | Email notifications |
| **C#** | 12.0 | Programming language |

## 📦 Prerequisites

Before you begin, ensure you have the following installed:

- [.NET 9.0 SDK](https://dotnet.microsoft.com/download/dotnet/9.0) or later
- [Visual Studio 2022](https://visualstudio.microsoft.com/) or [VS Code](https://code.visualstudio.com/)
- [Git](https://git-scm.com/)
- (Optional) [SQL Server](https://www.microsoft.com/sql-server) for production

## 🚀 Installation

### 1. Clone the Repository
git clone https://github.com/ayush08m/JobPortalSystem.git
cd JobPortalSystem

### 2. Restore NuGet Packages
dotnet restore


### 3. Install Required Tools
Install Entity Framework Core Tools
dotnet tool install --global dotnet-ef


## 🗄️ Database Setup

### Using SQLite (Default - Development)

Create initial migration
dotnet ef migrations add InitialCreate

Apply migrations and create database
dotnet ef database update

The SQLite database file (`JobPortal.db`) will be created in the project root.

### Using SQL Server (Production)

1. Update `appsettings.json`:

{
"ConnectionStrings": {
"DefaultConnection": "Server=YOUR_SERVER;Database=JobPortalDB;Trusted_Connection=True;MultipleActiveResultSets=true"
}
}

text

2. Update `Program.cs`:

// Change from UseSqlite to UseSqlServer
builder.Services.AddDbContext<ApplicationDbContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

text

3. Run migrations:

dotnet ef migrations add InitialCreate
dotnet ef database update

## ⚙️ Configuration

### 1. Email Settings (Optional)

To enable email notifications, update `appsettings.json`:

{
"SendGrid": {
"ApiKey": "YOUR_SENDGRID_API_KEY",
"FromEmail": "noreply@yourcompany.com",
"FromName": "Job Portal System"
}
}


Get your SendGrid API key from [SendGrid](https://sendgrid.com/).

## 🏃 Running the Application

### Development Mode

dotnet run


Or with hot reload:

dotnet watch run

The application will be available at:
- **HTTPS:** `https://localhost:5001`
- **HTTP:** `http://localhost:5000`

### Production Mode

dotnet publish -c Release -o ./publish
cd publish
dotnet JobPortalSystem.dll


## 👥 User Roles

The system supports three user roles:

| Role | Description | Access Level |
|------|-------------|--------------|
| **Admin** | System administrator | Full access to all features |
| **Employer** | Company/Recruiter | Post jobs, view applicants, manage applications |
| **Seeker** | Job applicant | Browse jobs, apply, track applications |


(Register as Employer or Seeker to test those roles)

<img width="1919" height="1023" alt="Screenshot 2025-10-26 201124" src="https://github.com/user-attachments/assets/8838ba4c-e195-4ae2-a7bb-13b9850f888a" />
<img width="1919" height="1079" alt="Screenshot 2025-10-26 201110" src="https://github.com/user-attachments/assets/d1a11e54-637e-4354-9f1f-099713bb9b13" />
<img width="1919" height="921" alt="Screenshot 2025-10-26 204336" src="https://github.com/user-attachments/assets/048f0e83-ceef-4c53-9566-eee67bdff701" />
<img width="1915" height="1018" alt="Screenshot 2025-10-26 204139" src="https://github.com/user-attachments/assets/52a9d40f-0927-4f85-9b3f-34560b848a22" />
<img width="1919" height="1021" alt="Screenshot 2025-10-26 203742" src="https://github.com/user-attachments/assets/24a8c966-24f9-4226-8810-b7e00fa36b01" />
<img width="1919" height="1012" alt="Screenshot 2025-10-26 203700" src="https://github.com/user-attachments/assets/e946a171-40e3-453a-b3af-f132afa2a8ff" />
<img width="1911" height="1014" alt="Screenshot 2025-10-26 203648" src="https://github.com/user-attachments/assets/86d6c2b7-8124-4c92-afb9-892d9cdeb3d6" />
<img width="1919" height="1018" alt="Screenshot 2025-10-26 203626" src="https://github.com/user-attachments/assets/991580d9-7511-47c7-ab58-8d8a8d2a577b" />
<img width="1918" height="1021" alt="Screenshot 2025-10-26 201151" src="https://github.com/user-attachments/assets/4ef5f3d0-6cbb-4910-9c53-81543030d0e9" />
<img width="1918" height="1020" alt="Screenshot 2025-10-26 201143" src="https://github.com/user-attachments/assets/2fe8cc24-e4e7-451b-b85e-e7d7afc20327" />

👨‍💻 Author

Ayush More

Github:@ayush08m

Project Link :https://github.com/ayush08m/JobPortalSystem
