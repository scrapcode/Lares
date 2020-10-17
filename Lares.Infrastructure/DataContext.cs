using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

using Lares.Entities;
using System.Linq;
using Lares.Interfaces;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Threading;
using System.Threading.Tasks;

namespace Lares.Infrastructure
{
    public class DataContext : IdentityDbContext<User>
    {
        public DataContext(DbContextOptions<DataContext> options)
            : base(options)
        {
        }

        /*
         * Dataset Definitions
         */
        // public DbSet<User> Users { get; set; }
        public DbSet<Property> Property { get; set; }
        public DbSet<Asset> Assets { get; set; }
        public DbSet<WorkTask> WorkTasks { get; set; }
        public DbSet<TimeEntry> TimeEntries { get; set; }
        public DbSet<MaterialEntry> MaterialEntries { get; set; }
        public DbSet<Resource> Resources { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Property>().ToTable("Property");
            modelBuilder.Entity<Asset>().ToTable("Asset");
            modelBuilder.Entity<WorkTask>().ToTable("WorkTask");
            modelBuilder.Entity<TimeEntry>().ToTable("TimeEntry");
            modelBuilder.Entity<MaterialEntry>().ToTable("MaterialEntry");
            modelBuilder.Entity<Resource>().ToTable("Resource");
        }

        // OnSaveChanges Overrides
        public override int SaveChanges()
        {
            var entries = ChangeTracker.Entries()
                .Where(e => e.Entity is ITrackable &&
                (e.State == EntityState.Added ||
                e.State == EntityState.Modified));

            foreach (var entry in entries)
            {
                ((ITrackable)entry.Entity).UpdatedOn = DateTime.Now;

                if (entry.State == EntityState.Added)
                {
                    ((ITrackable)entry.Entity).CreatedOn = DateTime.Now;
                }    
            }

            return base.SaveChanges();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            var entries = ChangeTracker.Entries()
                .Where(e => e.Entity is ITrackable &&
                (e.State == EntityState.Added ||
                e.State == EntityState.Modified));

            foreach (var entry in entries)
            {
                ((ITrackable)entry.Entity).UpdatedOn = DateTime.Now;

                if (entry.State == EntityState.Added)
                {
                    ((ITrackable)entry.Entity).CreatedOn = DateTime.Now;
                }
            }

            return await base.SaveChangesAsync(true, cancellationToken);
        }
    }
}
