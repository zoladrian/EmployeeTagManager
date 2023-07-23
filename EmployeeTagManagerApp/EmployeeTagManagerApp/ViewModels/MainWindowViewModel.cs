using EmployeeTagManagerApp.Data.Interfaces;
using EmployeeTagManagerApp.Interfaces;
using Prism.Commands;
using Prism.Mvvm;
using System;

namespace EmployeeTagManagerApp.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private readonly IFileDialogService _fileDialogService;
        private readonly IDatabaseInitializer _databaseInitializer;

        public MainWindowViewModel(IFileDialogService fileDialogService, IDatabaseInitializer databaseInitializer)
        {
            _databaseInitializer = databaseInitializer;
            _fileDialogService = fileDialogService ?? throw new ArgumentNullException(nameof(fileDialogService));
            LoadCommand = new DelegateCommand(OnLoadCommandExecuted);
        }

        public DelegateCommand LoadCommand { get; }

        private void OnLoadCommandExecuted()
        {
            string filePath = _fileDialogService.ShowOpenFileDialog();
            if (filePath != null)
            {
                _databaseInitializer.InitializeAsync(filePath);
            }
        }
    }
}