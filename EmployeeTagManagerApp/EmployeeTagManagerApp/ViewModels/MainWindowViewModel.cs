using EmployeeTagManagerApp.Data.Interfaces;
using EmployeeTagManagerApp.Events;
using EmployeeTagManagerApp.Interfaces;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Windows;

namespace EmployeeTagManagerApp.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private readonly IFileDialogService _fileDialogService;
        private readonly IDatabaseInitializer _databaseInitializer;
        private readonly IEventAggregator _eventAggregator;

        public MainWindowViewModel(IFileDialogService fileDialogService, IDatabaseInitializer databaseInitializer, IEventAggregator eventAggregator)
        {
            _databaseInitializer = databaseInitializer;
            _fileDialogService = fileDialogService ?? throw new ArgumentNullException(nameof(fileDialogService));
            LoadCommand = new DelegateCommand(OnLoadCommandExecuted);
            _eventAggregator.GetEvent<ErrorOccurredEvent>().Subscribe(ShowError);
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
        private void ShowError(string error)
        {
            MessageBox.Show(error);
        }
    }
}