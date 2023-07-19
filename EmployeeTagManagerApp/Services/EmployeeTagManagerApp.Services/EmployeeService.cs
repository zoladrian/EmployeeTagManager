using EmployeeTagManagerApp.Services.Interfaces;

namespace EmployeeTagManagerApp.Services
{
    public class MessageService : IMessageService
    {
        public string GetMessage()
        {
            return "Hello from the Message Service";
        }
    }
}
