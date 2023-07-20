using EmployeeTagManagerApp.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using EmployeeTagManagerApp.Data.Models;
using System.Reflection;
using System.Text;
using EmployeeTagManagerApp.Data.Factory;
using EmployeeTagManagerApp.Data.Interfaces;

namespace EmployeeTagManagerApp.Data
{
    public class DatabaseInitializer : IDatabaseInitializer
    {
        private readonly ManagerDbContext _dbContext;
        private readonly IEmployeeFactory _employeeFactory;
        public DatabaseInitializer(ManagerDbContext dbContext, IEmployeeFactory employeeFactory)
        {
            _dbContext = dbContext;
            _employeeFactory = employeeFactory;
        }

        public async Task InitializeAsync()
        {
            await _dbContext.Database.EnsureCreatedAsync();

            if (!_dbContext.Employees.Any())
            {

                string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Data", "employes.csv");
                string csvFileData;
                using (StreamReader sr = new StreamReader(path))
                {
                    csvFileData = sr.ReadToEnd();
                }

                var csvLines = csvFileData.Split('\n').Skip(1);

                StringBuilder sb = new StringBuilder();
                sb.Append("SET IDENTITY_INSERT [dbo].[Employees] ON;");
                foreach (var line in csvLines)
                {
                    var values = line.Split(',');
                    var employee = _employeeFactory.Create(values);

                    sb.Append($"INSERT INTO [dbo].[Employees] (Id, Name, Surname, Email, Phone) VALUES ({employee.Id}, '{employee.Name}', '{employee.Surname}', '{employee.Email}', '{employee.Phone}');");
                }
                sb.Append("SET IDENTITY_INSERT [dbo].[Employees] OFF;");

                _dbContext.Database.ExecuteSqlRaw(sb.ToString());

            }
        }

    }
}
