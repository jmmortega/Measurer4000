using Measurer4000.Core.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Measurer4000.Core.Models;

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
                CreateDialog(EnumTypeDialog.Error, e.Message);
            }
            
        }

        public void CreateDialog(EnumTypeDialog type, string text, string title = "")
        {
            MessageBoxImage messageImage;
            switch (type)
            {
                case EnumTypeDialog.Information:
                    messageImage = MessageBoxImage.Information;
                    break;
                case EnumTypeDialog.Warning:
                    messageImage = MessageBoxImage.Warning;
                    break;
                default:
                    messageImage = MessageBoxImage.Error;
                    break;
            }
            MessageBox.Show(text, title, MessageBoxButton.OK, messageImage);
        }
    }
}
