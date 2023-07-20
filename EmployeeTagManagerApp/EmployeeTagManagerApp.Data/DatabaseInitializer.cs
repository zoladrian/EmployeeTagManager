using EmployeeTagManagerApp.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using EmployeeTagManagerApp.Data.Models;
using EmployeeTagManagerApp.Abstraction;
using System.Reflection;
using System.Text;

namespace EmployeeTagManagerApp.Data
{
    public class DatabaseInitializer : IDatabaseInitializer
    {
        private readonly ManagerDbContext _dbContext;

        public DatabaseInitializer(ManagerDbContext dbContext)
        {
            _dbContext = dbContext;
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

                    var employee = new Employee
                    {
                        Id = int.Parse(values[0]),
                        Name = values[1].Replace("'", "''"),
                        Surname = values[2].Replace("'", "''"),
                        Email = values[3],
                        Phone = values[4]
                    };

                    sb.Append($"INSERT INTO [dbo].[Employees] (Id, Name, Surname, Email, Phone) VALUES ({employee.Id}, '{employee.Name}', '{employee.Surname}', '{employee.Email}', '{employee.Phone}');");
                }
                sb.Append("SET IDENTITY_INSERT [dbo].[Employees] OFF;");

                _dbContext.Database.ExecuteSqlRaw(sb.ToString());

            }
        }

    }
}
