using EmployeeTagManagerApp.Interfaces;
using Microsoft.Win32;

namespace EmployeeTagManagerApp
{
    public class OpenFileDialogService : IFileDialogService
    {
        public string ShowOpenFileDialog()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "CSV Files (*.csv)|*.csv|All Files (*.*)|*.*",
                FilterIndex = 1
            };
            return (openFileDialog.ShowDialog() == true) ? openFileDialog.FileName : null;
        }
    }
}