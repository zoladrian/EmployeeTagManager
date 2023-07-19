using EmployeeTagManagerApp.Core.Mvvm;
using EmployeeTagManagerApp.Services.Interfaces;
using Prism.Regions;

namespace EmployeeTagManagerApp.Modules.ModuleName.ViewModels
{
    public class ViewAViewModel : RegionViewModelBase
    {
        private string _message;
        public string Message
        {
            get { return _message; }
            set { SetProperty(ref _message, value); }
        }

        public ViewAViewModel(IRegionManager regionManager) :
            base(regionManager)
        {
        }

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            //do something
        }
    }
}
