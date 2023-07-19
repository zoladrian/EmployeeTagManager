using System;
namespace EmployeeTagManagerApp.Data.Models
{
    public class Employee
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public ICollection<Tag> Tags { get; set; } = new List<Tag>();
    }
}
