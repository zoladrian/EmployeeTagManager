namespace EmployeeTagManagerApp.Data.Interfaces
{
    public interface IDatabaseInitializer
    {
        Task InitializeAsync(string path);
    }
}