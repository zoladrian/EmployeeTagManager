using EmployeeTagManagerApp.Services.Interfaces;

namespace EmployeeTagManagerApp.Services
{
    public class EmployeeService : IEmployeeService
    {
        public string GetMessage()
        {
            return "Hello from the Message Service";
        }
    }
}
