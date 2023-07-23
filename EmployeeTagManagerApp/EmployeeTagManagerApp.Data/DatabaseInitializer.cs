using Microsoft.EntityFrameworkCore;
using System.Text;
using EmployeeTagManagerApp.Data.Interfaces;
using Prism.Events;
using EmployeeTagManagerApp.Events;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using EmployeeTagManagerApp.Data.Models;

namespace EmployeeTagManagerApp.Data
{
    public class DatabaseInitializer : IDatabaseInitializer
    {
        private readonly ManagerDbContext _dbContext;
        private readonly IEmployeeFactory _employeeFactory;
        private readonly IEventAggregator _eventAggregator;

        public DatabaseInitializer(ManagerDbContext dbContext, IEmployeeFactory employeeFactory, IEventAggregator eventAggregator)
        {
            _dbContext = dbContext;
            _employeeFactory = employeeFactory;
            _eventAggregator = eventAggregator;
        }

        public async Task InitializeAsync(string path)
        {
            await _dbContext.Database.EnsureCreatedAsync();

            if (!_dbContext.Employees.Any())
            {
                string csvFileData;
                using (StreamReader sr = new StreamReader(path))
                {
                    csvFileData = await sr.ReadToEndAsync();
                }

                var csvLines = csvFileData.Split('\n').Skip(1);

                foreach (var line in csvLines)
                {
                    var values = line.Split(',');
                    var employee = _employeeFactory.Create(values);

                    _dbContext.Employees.Add(new Employee
                    {
                        Id = employee.Id,
                        Name = employee.Name,
                        Surname = employee.Surname,
                        Email = employee.Email,
                        Phone = employee.Phone
                    });
                }

                await _dbContext.SaveChangesAsync();
            }

            _eventAggregator.GetEvent<DatabaseChangedEvent>().Publish();
        }
    }
}
