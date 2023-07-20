namespace EmployeeTagManagerApp.Abstraction
{
    public interface IDatabaseInitializer
    {
        Task InitializeAsync();
    }
}