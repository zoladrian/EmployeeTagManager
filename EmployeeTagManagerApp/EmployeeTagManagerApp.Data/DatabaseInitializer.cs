using EmployeeTagManagerApp.Data.Interfaces;
using EmployeeTagManagerApp.Events;
using Microsoft.EntityFrameworkCore;
using Prism.Events;
using System.Text;

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

            try
            {
            if (!_dbContext.Employees.Any())
            {
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
            _eventAggregator.GetEvent<DatabaseChangedEvent>().Publish();
            }
            catch (Exception ex)
            {
                _eventAggregator.GetEvent<ErrorOccurredEvent>().Publish($"An error occurred while initializing the database: {ex.Message}");
            }
        }
    }
}