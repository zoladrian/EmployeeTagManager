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
    } 
}