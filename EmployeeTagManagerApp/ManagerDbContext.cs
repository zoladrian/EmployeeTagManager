using System;
namespace EmployeeTagManagerApp.Data.Modelsnamespace EmployeeTagManagerApp.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Employee> Employees { get; set; }

        public DbSet<Tag> Tags { get; set; }
    } 
}