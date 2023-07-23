using EmployeeTagManagerApp.Data;
using EmployeeTagManagerApp.Data.Factories;
using EmployeeTagManagerApp.Data.Interfaces;
using EmployeeTagManagerApp.Data.Models;
using EmployeeTagManagerApp.Data.Validators;
using EmployeeTagManagerApp.Interfaces;
using EmployeeTagManagerApp.Modules.EditEmployeeModule.Views;
using EmployeeTagManagerApp.Modules.TableModule;
using EmployeeTagManagerApp.Services;
using EmployeeTagManagerApp.Services.EmployeeTagManagerApp.Services;
using EmployeeTagManagerApp.Services.Interfaces;
using EmployeeTagManagerApp.Views;
using FluentValidation;
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
            containerRegistry.Register<IValidator<Employee>, EmployeeValidator>();
            containerRegistry.Register<IValidator<Tag>, TagValidator>();
            containerRegistry.Register<IFileDialogService, OpenFileDialogService>();
            containerRegistry.Register<IEmployeeService, EmployeeService>();
            containerRegistry.Register<ITagService, TagService>();

            containerRegistry.RegisterScoped<IEmployeeFactory, EmployeeFactory>();
            containerRegistry.RegisterScoped<IDatabaseInitializer, DatabaseInitializer>();

            containerRegistry.RegisterSingleton<IEmployeeService, EmployeeService>();
            containerRegistry.RegisterSingleton<ManagerDbContext>(sp =>
            {
                var optionsBuilder = new DbContextOptionsBuilder<ManagerDbContext>();
                optionsBuilder.UseSqlServer("Server=localhost;Database=EmployeeTagManagerDatabase;Trusted_Connection=True;TrustServerCertificate=True;");
                return new ManagerDbContext(optionsBuilder.Options);
            });

            containerRegistry.RegisterDialog<EditEmployeeDialogView>("EditEmployeeDialog");
        }

        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            moduleCatalog.AddModule<TableModule>();
        }

        protected override async void OnInitialized()
        {
            base.OnInitialized();
        }
    }
}