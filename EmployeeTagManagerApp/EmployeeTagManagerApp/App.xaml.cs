using EmployeeTagManagerApp.Modules.ModuleName;
using EmployeeTagManagerApp.Services;
using EmployeeTagManagerApp.Services.Interfaces;
using EmployeeTagManagerApp.Views;
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
            containerRegistry.RegisterSingleton<IMessageService, EmployeeService>();
        }

        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            moduleCatalog.AddModule<ModuleNameModule>();
        }
    }
}
