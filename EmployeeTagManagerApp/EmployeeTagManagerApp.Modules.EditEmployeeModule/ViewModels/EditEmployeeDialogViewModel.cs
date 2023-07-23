using EmployeeTagManagerApp.Data.Models;
using EmployeeTagManagerApp.Services.Interfaces;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Linq;
using System.Collections.ObjectModel;
using EmployeeTagManagerApp.Data.Validators;
using FluentValidation;
using System.Collections.Generic;

namespace EmployeeTagManagerApp.Modules.EditEmployeeModule.ViewModels
{
    public class EditEmployeeDialogViewModel : BindableBase, IDialogAware
    {
        private EmployeeViewModel _employee;
        private readonly IEmployeeService _employeeService;
        private readonly ITagService _tagService;
        private readonly EmployeeValidator _employeeValidator;
        private readonly TagValidator _tagValidator;
        private string _newTagName;
        private string _newTagDescription;
        private Tag _selectedTag;
        private ObservableCollection<Tag> _availableTags;
        private ObservableCollection<EmployeeTag> _employeeTags;

        public EditEmployeeDialogViewModel(IEmployeeService employeeService, ITagService tagService, TagValidator tagValidator, EmployeeValidator employeeValidator)
        {
            _employeeService = employeeService;
            _tagService = tagService;
            AddTagCommand = new DelegateCommand(AddTag, CanAddTag)
                .ObservesProperty(() => NewTagName)
                .ObservesProperty(() => NewTagDescription)
                .ObservesProperty(() => SelectedTag);
            OkCommand = new DelegateCommand(OnOk, CanOk)
                .ObservesProperty(() => Employee.Name)
                .ObservesProperty(() => Employee.Surname)
                .ObservesProperty(() => Employee.Email)
                .ObservesProperty(() => Employee.Phone);
            DeleteTagCommand = new DelegateCommand<Tag>(DeleteTag);
            _tagValidator = tagValidator;
            _employeeValidator= employeeValidator;
        }

        public ObservableCollection<EmployeeTag> EmployeeTags
        {
            get { return _employeeTags; }
            set { SetProperty(ref _employeeTags, value); }
        }

        public EmployeeViewModel Employee
        {
            get { return _employee; }
            set { SetProperty(ref _employee, value); }
        }

        public ObservableCollection<Tag> AvailableTags
        {
            get { return _availableTags; }
            set { SetProperty(ref _availableTags, value); }
        }
        public string NewTagName
        {
            get { return _newTagName; }
            set { SetProperty(ref _newTagName, value); }
        }

        public string NewTagDescription
        {
            get { return _newTagDescription; }
            set
            {
                if (SetProperty(ref _newTagDescription, value))
                {
                    if (SelectedTag != null && SelectedTag.Name == NewTagName)
                    {
                        SelectedTag.Description = value;
                        UpdateTagDescription(SelectedTag);
                    }
                }
            }
        }

        public Tag SelectedTag
        {
            get { return _selectedTag; }
            set
            {
                if (SetProperty(ref _selectedTag, value) && value != null)
                {
                    NewTagDescription = value.Description;
                }
            }
        }
        public DelegateCommand AddTagCommand { get; }

        public DelegateCommand OkCommand { get; }
        public DelegateCommand<Tag> DeleteTagCommand { get; }

        public string Title => "Edit Employee";

        public event Action<IDialogResult> RequestClose;

        public bool CanCloseDialog()
        {
            return true;
        }

        public void OnDialogClosed()
        {
        }

        public void OnDialogOpened(IDialogParameters parameters)
        {
            Employee = new EmployeeViewModel(parameters.GetValue<Employee>("employee"));
            EmployeeTags = new ObservableCollection<EmployeeTag>(Employee.EmployeeTags);
            LoadAvailableTags();
        }
        private void OnOk()
        {
            Employee employeeToUpdate = new Employee();
            UpdateEmployee(employeeToUpdate);

            var dialogParameters = new DialogParameters
            {
                { "employee", employeeToUpdate }
            };
            RequestClose?.Invoke(new DialogResult(ButtonResult.OK, dialogParameters));
        }
        private async void UpdateTagDescription(Tag tag)
        {
            await _tagService.UpdateTagAsync(tag);
            LoadAvailableTags();
        }
        private async void LoadAvailableTags()
        {
            var tags = await _tagService.GetTagsAsync();
            AvailableTags = new ObservableCollection<Tag>(tags.Where(t => !EmployeeTags.Any(et => et?.Tag.Id == t.Id)));
        }
        private async void AddTag()
        {
            Tag tagToAdd = null;
            var existingTag = AvailableTags.FirstOrDefault(t => t.Name == NewTagName);
            if (existingTag != null)
            {
                existingTag.Description = NewTagDescription;
                tagToAdd = existingTag;
            }
            else
            {
                Tag newTag = new Tag { Name = NewTagName, Description = NewTagDescription };
                await _tagService.CreateTagAsync(newTag);
                AvailableTags.Add(newTag);
                tagToAdd = newTag;
            }

            await _employeeService.AddTagToEmployeeAsync(Employee.Id, tagToAdd.Id);
            var updatedEmployee = await _employeeService.GetEmployeeByIdAsync(Employee.Id);
            var newEmployeeTag = updatedEmployee.EmployeeTags.LastOrDefault(et => et.Tag.Id == tagToAdd.Id);
            if (newEmployeeTag != null)
            {
                EmployeeTags.Add(newEmployeeTag);
            }

            NewTagName = string.Empty;
            NewTagDescription = string.Empty;

            LoadAvailableTags();
        }
        private bool CanOk()
        {
            if (Employee != null)
            {
                var validationResult = _employeeValidator.Validate(ToEmployee());
                return validationResult.IsValid;
            }
            return false;
        }

        private bool CanAddTag()
        {
            if (!string.IsNullOrWhiteSpace(NewTagName) && !string.IsNullOrWhiteSpace(NewTagDescription))
            {
                var tag = new Tag { Name = NewTagName, Description = NewTagDescription };
                var validationResult = _tagValidator.Validate(tag);
                return validationResult.IsValid;
            }
            return false;
        }

        private async void DeleteTag(Tag tag)
        {
            await _employeeService.RemoveTagFromEmployeeAsync(Employee.Id, tag.Id);
            var tagToRemove = EmployeeTags.FirstOrDefault(t => t.Tag.Id == tag.Id);
            if (tagToRemove != null)
            {
                EmployeeTags.Remove(tagToRemove);
                LoadAvailableTags();
            }
        }
        public void UpdateEmployee(Employee employeeToUpdate)
        {
            employeeToUpdate.Id = Employee.Id;
            employeeToUpdate.Name = Employee.Name;
            employeeToUpdate.Surname = Employee.Surname;
            employeeToUpdate.Email = Employee.Email;
            employeeToUpdate.Phone = Employee.Phone;
            employeeToUpdate.EmployeeTags = new List<EmployeeTag>(_employeeTags.Select(et => new EmployeeTag
            {
                EmployeeId = et.EmployeeId,
                TagId = et.TagId
            }));
        }
        public Employee ToEmployee()
        {
            return new Employee
            {
                Id = Employee.Id,
                Name = Employee.Name,
                Surname = Employee.Surname,
                Email = Employee.Email,
                Phone = Employee.Phone,
                EmployeeTags = this.EmployeeTags
            };
        }
    }
}
