using EmployeeTagManagerApp.Data.Models;
using EmployeeTagManagerApp.Services.Interfaces;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Linq;
using System.Collections.ObjectModel;

namespace EmployeeTagManagerApp.Modules.EditEmployeeModule.ViewModels
{
    public class EditEmployeeDialogViewModel : BindableBase, IDialogAware
    {
        private Employee _employee;
        private IEmployeeService _employeeService;
        private ITagService _tagService;
        private readonly IDialogService _dialogService;
        private string _newTagName;
        private string _newTagDescription;
        private Tag _selectedTag;
        private ObservableCollection<Tag> _availableTags;
        private ObservableCollection<EmployeeTag> _employeeTags;

        public EditEmployeeDialogViewModel(IDialogService dialogService, IEmployeeService employeeService, ITagService tagService)
        {
            _employeeService = employeeService;
            _dialogService = dialogService;
            _tagService = tagService;
            AddTagCommand = new DelegateCommand(AddTag, CanAddTag)
                .ObservesProperty(() => NewTagName)
                .ObservesProperty(() => NewTagDescription);
            DeleteTagCommand = new DelegateCommand<Tag>(DeleteTag);
        }

        public Employee Employee
        {
            get { return _employee; }
            set { SetProperty(ref _employee, value); }
        }

        public ObservableCollection<EmployeeTag> EmployeeTags
        {
            get { return _employeeTags; }
            set { SetProperty(ref _employeeTags, value); }
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
        public DelegateCommand<Tag> DeleteTagCommand { get; }

        public string Title => "Edit Employee";

        public event Action<IDialogResult> RequestClose;

        public bool CanCloseDialog()
        {
            return true;
        }

        public void OnDialogClosed()
        {
            var dialogParameters = new DialogParameters
            {
                { "employee", Employee }
            };
            RequestClose?.Invoke(new DialogResult(ButtonResult.OK, dialogParameters));
        }

        public void OnDialogOpened(IDialogParameters parameters)
        {
            Employee = parameters.GetValue<Employee>("employee");
            EmployeeTags = new ObservableCollection<EmployeeTag>(Employee.EmployeeTags);
            LoadAvailableTags();
        }
        private async void UpdateTagDescription(Tag tag)
        {
            await _tagService.UpdateTagAsync(tag);
            LoadAvailableTags();
        }
        private async void LoadAvailableTags()
        {
            var tags = await _tagService.GetTagsAsync();
            AvailableTags = new ObservableCollection<Tag>(tags.Where(t => !EmployeeTags.Any(et => et.Tag.Id == t.Id)));
        }
        private async void AddTag()
        {
            var existingTag = AvailableTags.FirstOrDefault(t => t.Name == NewTagName);
            if (existingTag != null)
            {
                existingTag.Description = NewTagDescription;

                await _employeeService.AddTagToEmployeeAsync(Employee.Id, existingTag.Id);

                var updatedEmployee = await _employeeService.GetEmployeeByIdAsync(Employee.Id);
                EmployeeTags.Add(updatedEmployee.EmployeeTags.Last());
            }
            else
            {
                Tag newTag = new Tag { Name = NewTagName, Description = NewTagDescription };
                await _tagService.CreateTagAsync(newTag);

                await _employeeService.AddTagToEmployeeAsync(Employee.Id, newTag.Id);

                var updatedEmployee = await _employeeService.GetEmployeeByIdAsync(Employee.Id);
                EmployeeTags.Add(updatedEmployee.EmployeeTags.Last());

                AvailableTags.Add(newTag);
            }

            NewTagName = string.Empty;
            NewTagDescription = string.Empty;

            LoadAvailableTags();
        }



        private bool CanAddTag()
        {
            return !string.IsNullOrWhiteSpace(NewTagName) && !string.IsNullOrWhiteSpace(NewTagDescription);
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
    }
}
