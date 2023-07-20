using EmployeeTagManagerApp.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace EmployeeTagManagerApp.Data
{
    public class ManagerDbContext : DbContext
    {
        public ManagerDbContext(DbContextOptions<ManagerDbContext> options) : base(options)
        {
        }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<EmployeeTag> EmployeeTags { get; set; }  // Dodaj to

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Zdefiniuj relacje wiele-do-wielu
            modelBuilder.Entity<EmployeeTag>()
                .HasKey(et => new { et.EmployeeId, et.TagId });  // Klucz złożony

            modelBuilder.Entity<EmployeeTag>()
                .HasOne(et => et.Employee)
                .WithMany(e => e.EmployeeTags)
                .HasForeignKey(et => et.EmployeeId);  // Zdefiniowanie klucza obcego

            modelBuilder.Entity<EmployeeTag>()
                .HasOne(et => et.Tag)
                .WithMany(t => t.EmployeeTags)
                .HasForeignKey(et => et.TagId);  // Zdefiniowanie klucza obcego
        }
    }
}
