using Measurer4000.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Measurer4000.Services
{
    public class FileDialogService : IDialogService
    {
        public void OpenFileDialog(Action<string> onFileDialogSuccess, Action<Exception> onFileDialogError)
        {
            try
            {
                Microsoft.Win32.OpenFileDialog fileDialog = new Microsoft.Win32.OpenFileDialog();
                fileDialog.Filter = "Solution Files (*.sln)|*.sln";

                if (fileDialog.ShowDialog() == true)
                {
                    onFileDialogSuccess?.Invoke(fileDialog.FileName);
                }
            }
            catch(Exception e)
            {
                OpenError(e, string.Empty);
            }
            
        }

        public void OpenError(Exception exceptionError, string messageError)
        {
            MessageBox.Show($"Error: {exceptionError.Message} , {messageError}");
        }
    }
}
