using HotChocolateExplorer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotChocolateExplorer.DBConfig
{
    public class SchoolContext : DbContext
    {
        private IDbContextTransaction _transaction = null;

        public SchoolContext(DbContextOptions<SchoolContext> options) : base(options)
        {

        }
        public virtual void Commit()
        {
            base.SaveChanges();
        }

        public virtual Task CommitAsync()
        {
            return base.SaveChangesAsync();
        }

        public void BeginTransaction()
        {
            _transaction = Database.BeginTransaction();
        }

        public void CommitTransaction()
        {
            if (_transaction == null)
            {
                return;
            }

            _transaction.Commit();
            _transaction.Dispose();
            _transaction = null;
        }

        public void RollbackTransaction()
        {
            if (_transaction == null)
            {
                return;
            }

            _transaction.Rollback();
            _transaction.Dispose();
            _transaction = null;
        }


        public DbSet<Student> Students { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }
        public DbSet<Course> Courses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Student>()
                .HasMany(t => t.Enrollments)
                .WithOne(t => t.Student)
                .HasForeignKey(t => t.StudentId);

            modelBuilder.Entity<Enrollment>()
                    .HasIndex(t => new
                    {
                        t.StudentId,
                        t.CourseId
                    })
                    .IsUnique();

            modelBuilder.Entity<Course>()
                        .HasMany(t => t.Enrollments)
                        .WithOne(t => t.Course)
                        .HasForeignKey(t => t.CourseId);
        }
    }
}
