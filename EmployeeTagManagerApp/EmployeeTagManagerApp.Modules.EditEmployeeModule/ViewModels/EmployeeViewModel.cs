using EmployeeTagManagerApp.Data.Models;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeTagManagerApp.Modules.EditEmployeeModule.ViewModels
{
    public class EmployeeViewModel : BindableBase
    {
        private string _name;
        private string _surname;
        private string _email;
        private string _phone;
        private int _id;
        private ICollection<EmployeeTag> _employeeTags;
        public EmployeeViewModel(Employee employee)
        {
            _id = employee.Id;
            _name = employee.Name;
            _surname = employee.Surname;
            _email = employee.Email;
            _phone = employee.Phone;
            _employeeTags= employee.EmployeeTags;
        }

        public int Id { get => _id; }
        public string Name
        {
            get => _name;
            set
            {
                if (_name != value)
                {
                    _name = value;
                    RaisePropertyChanged(nameof(Name));
                }
            }
        }

        public string Surname
        {
            get => _surname;
            set
            {
                if (_surname != value)
                {
                    _surname = value;
                    RaisePropertyChanged(nameof(Surname));
                }
            }
        }

        public string Email
        {
            get => _email;
            set
            {
                if (_email != value)
                {
                    _email = value;
                    RaisePropertyChanged(nameof(Email));
                }
            }
        }

        public string Phone
        {
            get => _phone;
            set
            {
                if (_phone != value)
                {
                    _phone = value;
                    RaisePropertyChanged(nameof(Phone));
                }
            }
        }
        public ICollection<EmployeeTag> EmployeeTags { get => _employeeTags; } 
    }

}
