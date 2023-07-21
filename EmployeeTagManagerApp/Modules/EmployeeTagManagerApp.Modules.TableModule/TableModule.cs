using EmployeeTagManagerApp.Core;
using EmployeeTagManagerApp.Modules.TableModule.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

namespace EmployeeTagManagerApp.Modules.TableModule
{
    public class TableModule : IModule
    {
        private readonly IRegionManager _regionManager;

        public TableModule(IRegionManager regionManager)
        {
            _regionManager = regionManager;
        }

        public void OnInitialized(IContainerProvider containerProvider)
        {
            _regionManager.RequestNavigate(RegionNames.TableRegion, "TableView");
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<TableView>();
        }
    }
}