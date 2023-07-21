using EmployeeTagManagerApp.Data.Interfaces;
using EmployeeTagManagerApp.Interfaces;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
