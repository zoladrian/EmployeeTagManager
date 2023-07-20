using EmployeeTagManagerApp.Abstraction;
using EmployeeTagManagerApp.Data;
using EmployeeTagManagerApp.Modules.ModuleName;
using EmployeeTagManagerApp.Services;
using EmployeeTagManagerApp.Services.Interfaces;
using EmployeeTagManagerApp.Views;
using Microsoft.EntityFrameworkCore;
using Prism.Ioc;
using Prism.Modularity;
using System.Windows;

namespace EmployeeTagManagerApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }
        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<IEmployeeService, EmployeeService>();
            containerRegistry.RegisterSingleton<IDatabaseInitializer, DatabaseInitializer>();
            containerRegistry.RegisterSingleton<ManagerDbContext>(sp =>
            {
                var optionsBuilder = new DbContextOptionsBuilder<ManagerDbContext>();
                optionsBuilder.UseSqlServer("Server=localhost;Database=EmployeeTagManagerDatabase;Trusted_Connection=True;TrustServerCertificate=True;");
                return new ManagerDbContext(optionsBuilder.Options);
            });
        }

        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            moduleCatalog.AddModule<ModuleNameModule>();

        }
        protected override async void OnInitialized()
        {
            var databaseInitializer = Container.Resolve<IDatabaseInitializer>();
            await databaseInitializer.InitializeAsync();

            base.OnInitialized();
        }

    }
}
