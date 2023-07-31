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
            _eventAggregator = eventAggregator;
            _fileDialogService = fileDialogService ?? throw new ArgumentNullException(nameof(fileDialogService));
            LoadCommand = new DelegateCommand(OnLoadCommandExecuted);
            _eventAggregator.GetEvent<ErrorOccurredEvent>().Subscribe(ShowError);
            _eventAggregator.GetEvent<OnDataLoaded>().Subscribe(OnDataLoaded);
        }

        private bool _isLoadButtonEnabled = true;
        private bool _isLoadingVisible;
        public bool IsLoadButtonEnabled
        {
            get { return _isLoadButtonEnabled; }
            set { SetProperty(ref _isLoadButtonEnabled, value); }
        }

        public bool IsLoadingVisible
        {
            get { return _isLoadingVisible; }
            set { SetProperty(ref _isLoadingVisible, value); }
        }
        public DelegateCommand LoadCommand { get; }

        private async void OnLoadCommandExecuted()
        {
            string filePath = _fileDialogService.ShowOpenFileDialog();
            if (filePath != null)
            {
                IsLoadButtonEnabled = false;
                IsLoadingVisible = true;

                await _databaseInitializer.InitializeAsync(filePath);
            }
        }
        private void OnDataLoaded()
        {
            IsLoadButtonEnabled = true;
            IsLoadingVisible = false;
        }
        private void ShowError(string error)
        {
            MessageBox.Show(error);
            OnDataLoaded();
        }
    }
}