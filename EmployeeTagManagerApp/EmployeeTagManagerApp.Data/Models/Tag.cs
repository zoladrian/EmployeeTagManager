namespace EmployeeTagManagerApp.Data.Models
{
    public class Tag
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public ICollection<EmployeeTag> EmployeeTags { get; set; } = new List<EmployeeTag>();
    }
}
