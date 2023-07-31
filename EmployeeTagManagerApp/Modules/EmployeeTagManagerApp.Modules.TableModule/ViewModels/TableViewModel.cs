using EmployeeTagManagerApp.Data.Models;
using EmployeeTagManagerApp.Services.Interfaces;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System.Collections.ObjectModel;
using EmployeeTagManagerApp.Events;
using System.Threading.Tasks;
using System.Windows.Input;
using Prism.Services.Dialogs;
using System.Windows;
using System.Linq;

namespace EmployeeTagManagerApp.Modules.TableModule.ViewModels
{
    public class TableViewModel : BindableBase
    {
        private readonly IEmployeeService _employeeService;
        private readonly IDialogService _dialogService;
        private readonly IEventAggregator _eventAggregator;
        private ObservableCollection<Employee> _employees;
        private Employee _selectedEmployee;

        public TableViewModel(IEmployeeService employeeService, IEventAggregator eventAggregator, IDialogService dialogService)
        {
            _employeeService = employeeService;
            _dialogService= dialogService;
            _eventAggregator = eventAggregator;

            _eventAggregator.GetEvent<DatabaseChangedEvent>().Subscribe(() => LoadDataAsync());
            EditCommand = new DelegateCommand<Employee>(EditEmployee).ObservesProperty(() => SelectedEmployee);
            DeleteCommand = new DelegateCommand<Employee>(DeleteEmployee).ObservesProperty(() => SelectedEmployee);
        }

        public ObservableCollection<Employee> Employees
        {
            get { return _employees; }
            set { SetProperty(ref _employees, value); }
        }

        public Employee SelectedEmployee
        {
            get { return _selectedEmployee; }
            set { SetProperty(ref _selectedEmployee, value); }
        }

        public ICommand LoadDataCommand { get; }
        public ICommand EditCommand { get; }
        public ICommand DeleteCommand { get; }

        private async Task LoadDataAsync()
        {
            var employees = await _employeeService.GetEmployeesAsync();
            Employees = new ObservableCollection<Employee>(employees);
            _eventAggregator.GetEvent<OnDataLoaded>().Publish();
        }

        private void EditEmployee(Employee employee)
        {
            var parameters = new DialogParameters
    {
        { "employee", employee }
    };

            _dialogService.ShowDialog("EditEmployeeDialog", parameters, async r =>
            {
                if (r.Result == ButtonResult.OK)
                {
                    var updatedEmployee = r.Parameters.GetValue<Employee>("employee");

                    var employeeToEdit = Employees.FirstOrDefault(e => e.Id == updatedEmployee.Id);
                    if (employeeToEdit != null)
                    {
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            int index = Employees.IndexOf(employeeToEdit);
                            Employees.Remove(employeeToEdit);
                            Employees.Insert(index, updatedEmployee);
                        });

                        await _employeeService.UpdateEmployeeAsync(updatedEmployee);
                    }
                }
            });
        }



        private async void DeleteEmployee(Employee employee)
        {
            if (employee == null) return;
            var result = MessageBox.Show("Are you sure you want to delete this employee?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                await _employeeService.DeleteEmployeeAsync(employee.Id);
                Application.Current.Dispatcher.Invoke(() =>
                {
                    Employees.Remove(employee);
                });
            }
        }
    }

}
