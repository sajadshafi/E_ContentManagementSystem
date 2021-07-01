using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Wiser.API.Entities.Models;

namespace Wiser.API.Entities
{
    public class WiserContext : IdentityDbContext<
     SystemUser, SystemRole, string,
     IdentityUserClaim<string>, SystemUserRole, IdentityUserLogin<string>,
     IdentityRoleClaim<string>, IdentityUserToken<string>>
    {
        public WiserContext(DbContextOptions dbContextOptions) : base(dbContextOptions)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var type in modelBuilder.Model.GetEntityTypes())
            {
                if (typeof(IBaseModel).IsAssignableFrom(type.ClrType))
                    modelBuilder.SetSoftDeleteFilter(type.ClrType);
            }
            modelBuilder.Entity<Course>()
                .HasOne(c => c.CourseCategory)
                .WithMany(m => m.Courses)
                .HasForeignKey(f => f.CourseCategoryId);

            modelBuilder.Entity<Subject>()
                .HasOne(c => c.SubjectCategory)
                .WithMany(c => c.Subjects)
                .HasForeignKey(f => f.SubjectCategoryId);

            modelBuilder.Entity<Subject>()
                .HasOne(c => c.Department)
                .WithMany(c => c.Subjects)
                .HasForeignKey(f => f.DepartmentId);

            modelBuilder.Entity<SystemUser>()
                .HasOne(x => x.Department)
                .WithMany(x => x.SystemUsers)
                .HasForeignKey(x => x.DepartmentId)
                .IsRequired(false);

            modelBuilder.Entity<EContent>()
                .HasOne(x => x.Subject)
                .WithMany(s => s.EContents)
                .HasForeignKey(s => s.SubjectId);

            modelBuilder.Entity<EContent>()
                .HasOne(x => x.SystemUser)
                .WithMany(s => s.EContents)
                .HasForeignKey(s => s.UserId);

            modelBuilder.Entity<EFile>()
                .HasOne(x => x.EContent)
                .WithMany(s => s.EFiles)
                .HasForeignKey(s => s.EContentId);

            modelBuilder.Entity<SubjectAllotment>()
                .HasOne(x => x.Course)
                .WithMany(s => s.SubjectAllotments)
                .HasForeignKey(s => s.CourseId);

            base.OnModelCreating(modelBuilder);
        }
        public override int SaveChanges()
        {
            UpdateSoftDeleteStatuses();
            return base.SaveChanges();
        }
        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            UpdateSoftDeleteStatuses();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }
        private void UpdateSoftDeleteStatuses()
        {
            foreach (var entry in ChangeTracker.Entries())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.CurrentValues["IsDeleted"] = false;
                        break;
                    case EntityState.Deleted:
                        entry.State = EntityState.Modified;
                        entry.CurrentValues["IsDeleted"] = true;
                        break;
                }
            }
        }

        public DbSet<Institute> Institutes { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<CourseCategory> CourseCategories { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<SubjectCategory> SubjectCategories { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<EContent> EContents { get; set; }
        public DbSet<EFile> EFiles { get; set; }
        public DbSet<SubjectAllotment> SubjectAllotments { get; set; }

    }
}
