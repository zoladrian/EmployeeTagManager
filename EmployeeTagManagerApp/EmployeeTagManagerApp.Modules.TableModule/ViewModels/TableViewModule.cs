using EmployeeTagManagerApp.Data.Models;
using EmployeeTagManagerApp.Services.Interfaces;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace EmployeeTagManagerApp.Modules.TableModule.ViewModels
{
    public class TableViewModel : BindableBase
    {
        private readonly IEmployeeService _employeeService;
        private readonly ITagService _tagService;
        private ObservableCollection<Employee> _employees;
        private Employee _selectedEmployee;

        public TableViewModel(IEmployeeService employeeService, ITagService tagService)
        {
            _employeeService = employeeService;
            _tagService = tagService;

            LoadDataCommand = new DelegateCommand(async () => await LoadDataAsync());
            EditCommand = new DelegateCommand<Employee>(EditEmployee, CanEditOrDelete).ObservesProperty(() => SelectedEmployee);
            DeleteCommand = new DelegateCommand<Employee>(DeleteEmployee, CanEditOrDelete).ObservesProperty(() => SelectedEmployee);
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
        }

        private void EditEmployee(Employee employee)
        {
            // Logic for editing the employee
        }

        private void DeleteEmployee(Employee employee)
        {
            // Logic for deleting the employee
        }

        private bool CanEditOrDelete(Employee employee)
        {
            return employee != null;
        }
    }

}
