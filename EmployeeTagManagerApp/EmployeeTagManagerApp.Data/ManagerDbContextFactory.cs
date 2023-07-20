using EmployeeTagManagerApp.Data;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;

namespace EmployeeTagManagerApp.Data
{
    public class ManagerDbContextFactory : IDesignTimeDbContextFactory<ManagerDbContext>
    {
        public ManagerDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ManagerDbContext>();
            optionsBuilder.UseSqlServer("Server=localhost;Database=EmployeeTagManagerDatabase;Trusted_Connection=True;TrustServerCertificate=True;");

            return new ManagerDbContext(optionsBuilder.Options);
        }
    }
}


