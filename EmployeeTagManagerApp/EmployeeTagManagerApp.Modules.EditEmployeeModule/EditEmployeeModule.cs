using EmployeeTagManagerApp.Core;
using EmployeeTagManagerApp.Modules.EditEmployeeModule.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

namespace EmployeeTagManagerApp.Modules.EditEmployeeModule
{
    public class EditEmployeeModule : IModule
    {
        private readonly IRegionManager _regionManager;

        public EditEmployeeModule(IRegionManager regionManager)
        {
            _regionManager = regionManager;
        }

        public void OnInitialized(IContainerProvider containerProvider)
        {
            _regionManager.RequestNavigate(RegionNames.EmployeeDialogRegion, "EditEmployeeDialogView");
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<EditEmployeeDialogView>();
        }
    }
}