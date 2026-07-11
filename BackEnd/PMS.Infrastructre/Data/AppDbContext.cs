using Azure;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PMS.Domain.Entities;
using PMS.Infrastructre.Data.config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace PMS.Infrastructre.Data
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {


        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {


        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
       => optionsBuilder.UseSqlServer("Server=.;Database=PMSDB;Encrypt=False;Trusted_Connection=True;");

        public DbSet<User> Users => Set<User>();
        public DbSet<TaskItem> Tasks => Set<TaskItem>();
        public DbSet<Category> Categories => Set<Category>();
        public DbSet<Tag> Tags => Set<Tag>();
        public DbSet<TaskTag> TaskTags => Set<TaskTag>();
        public DbSet<TimeTracking> TimeTrackings => Set<TimeTracking>();
        public DbSet<NotificationSettings> NotificationSettings => Set<NotificationSettings>();

        // 🔔 Notifications
        public DbSet<Notification> Notifications => Set<Notification>();

        // 📅 Scheduling
        public DbSet<Schedule> Schedules => Set<Schedule>();
        public DbSet<ScheduleTask> ScheduleTasks => Set<ScheduleTask>();

        // 🤖 AI Module
        public DbSet<AiReport> AiReports => Set<AiReport>();
        public DbSet<UserProductivityStat> UserProductivityStats => Set<UserProductivityStat>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AiReportConfig).Assembly);
        }
    }
}
