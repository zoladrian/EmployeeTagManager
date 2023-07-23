using EmployeeTagManagerApp.Services;
using EmployeeTagManagerApp.Data;
using EmployeeTagManagerApp.Data.Models;
using EmployeeTagManagerApp.Services.Interfaces;

namespace EmployeeTagManagerApp.Tests
{
    public class EmployeeServiceTests
    {
        private IEmployeeService _employeeService;
        private ManagerDbContext _dbContext;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<ManagerDbContext>()
                .UseInMemoryDatabase(databaseName: "EmployeeDatabase")
                .Options;

            _dbContext = new ManagerDbContext(options);
            var mockEventAggregator = Substitute.For<IEventAggregator>();

            _employeeService = new EmployeeService(_dbContext, mockEventAggregator);
        }

        [Test]
        public async Task GetEmployeesAsync_EmployeesExist_ReturnsEmployees()
        {
            // Arrange
            var employees = new[]
            {
                new Employee { Id = 1, Name = "John", Surname = "Doe", Email = "john.doe@example.com", Phone = "123456789" },
                new Employee { Id = 2, Name = "Jane", Surname = "Doe", Email = "jane.doe@example.com", Phone = "987654321" }
            };

            await _dbContext.Employees.AddRangeAsync(employees);
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _employeeService.GetEmployeesAsync();

            // Assert
            Assert.AreEqual(employees.Length, result.Count());

            // Cleanup
            _dbContext.Employees.RemoveRange(employees);
            await _dbContext.SaveChangesAsync();
        }

        [Test]
        public async Task UpdateEmployeeAsync_EmployeeExists_UpdatesEmployee()
        {
            // Arrange
            var employee = new Employee { Id = 1, Name = "John", Surname = "Doe", Email = "john.doe@example.com", Phone = "123456789" };
            _dbContext.Employees.Add(employee);
            await _dbContext.SaveChangesAsync();

            employee.Name = "UpdatedJohn";
            employee.Email = "updated.john.doe@example.com";

            // Act
            await _employeeService.UpdateEmployeeAsync(employee);

            // Assert
            var updatedEmployee = _dbContext.Employees.FirstOrDefault(e => e.Id == employee.Id);
            Assert.AreEqual("UpdatedJohn", updatedEmployee.Name);
            Assert.AreEqual("updated.john.doe@example.com", updatedEmployee.Email);

            // Cleanup
            _dbContext.Employees.Remove(employee);
            await _dbContext.SaveChangesAsync();
        }

        [Test]
        public async Task DeleteEmployeeAsync_EmployeeExists_DeletesEmployee()
        {
            // Arrange
            var employee = new Employee { Id = 1, Name = "John", Surname = "Doe", Email = "john.doe@example.com", Phone = "123456789" };
            _dbContext.Employees.Add(employee);
            await _dbContext.SaveChangesAsync();

            // Act
            await _employeeService.DeleteEmployeeAsync(employee.Id);

            // Assert
            Assert.AreEqual(0, _dbContext.Employees.Count());
        }

        [Test]
        public async Task AddTagToEmployeeAsync_TagDoesNotExist_AddsTag()
        {
            // Arrange
            var employee = new Employee { Id = 1, Name = "John", Surname = "Doe", Email = "john.doe@example.com", Phone = "123456789" };
            var tag = new Tag { Id = 1, Name = "Tag1", Description = "Description1" };

            _dbContext.Employees.Add(employee);
            _dbContext.Tags.Add(tag);
            await _dbContext.SaveChangesAsync();

            // Act
            await _employeeService.AddTagToEmployeeAsync(employee.Id, tag.Id);

            // Assert
            var result = _dbContext.EmployeeTags.FirstOrDefault(et => et.EmployeeId == employee.Id && et.TagId == tag.Id);
            Assert.IsNotNull(result);

            // Cleanup
            _dbContext.EmployeeTags.Remove(result);
            _dbContext.Employees.Remove(employee);
            _dbContext.Tags.Remove(tag);
            await _dbContext.SaveChangesAsync();
        }

        [Test]
        public async Task RemoveTagFromEmployeeAsync_TagExists_RemovesTag()
        {
            // Arrange
            var employee = new Employee { Id = 1, Name = "John", Surname = "Doe", Email = "john.doe@example.com", Phone = "123456789" };
            var tag = new Tag { Id = 1, Name = "Tag1", Description = "Description1" };

            _dbContext.Employees.Add(employee);
            _dbContext.Tags.Add(tag);
            await _dbContext.SaveChangesAsync();

            var employeeTag = new EmployeeTag { EmployeeId = employee.Id, TagId = tag.Id };
            _dbContext.EmployeeTags.Add(employeeTag);
            await _dbContext.SaveChangesAsync();

            // Act
            await _employeeService.RemoveTagFromEmployeeAsync(employee.Id, tag.Id);

            // Assert
            var result = _dbContext.EmployeeTags.FirstOrDefault(et => et.EmployeeId == employee.Id && et.TagId == tag.Id);
            Assert.IsNull(result);

            // Cleanup
            _dbContext.Employees.Remove(employee);
            _dbContext.Tags.Remove(tag);
            await _dbContext.SaveChangesAsync();
        }

        [Test]
        public async Task GetTagsForEmployeeAsync_TagExistsForEmployee_ReturnsTags()
        {
            // Arrange
            var employee = new Employee { Id = 1, Name = "John", Surname = "Doe", Email = "john.doe@example.com", Phone = "123456789" };
            var tags = new[]
            {
        new Tag { Id = 1, Name = "Tag1", Description = "Description1" },
        new Tag { Id = 2, Name = "Tag2", Description = "Description2" }
    };

            _dbContext.Employees.Add(employee);
            await _dbContext.Tags.AddRangeAsync(tags);
            await _dbContext.SaveChangesAsync();

            var employeeTags = tags.Select(tag => new EmployeeTag { EmployeeId = employee.Id, TagId = tag.Id });
            await _dbContext.EmployeeTags.AddRangeAsync(employeeTags);
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _employeeService.GetTagsForEmployeeAsync(employee.Id);

            // Assert
            Assert.AreEqual(tags.Length, result.Count());
        }

        [TearDown]
        public void Cleanup()
        {
            _dbContext.Database.EnsureDeleted();
        }

    }
}
