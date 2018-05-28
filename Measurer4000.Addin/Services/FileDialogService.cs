using System;
using Measurer4000.Core.Models;
using Measurer4000.Core.Services.Interfaces;
using MonoDevelop.Components;
using MonoDevelop.Ide;
using Xwt;

namespace Measurer4000.Addin.Services
{
    public class FileDialogService : IDialogService
    {
        public void CreateDialog(EnumTypeDialog type, string text, string title = "")
        {
            switch (type)
            {
                case EnumTypeDialog.Information:
                    MessageService.ShowMessage(title, text);
                    break;
                case EnumTypeDialog.Warning:
                    MessageService.ShowWarning(title, text);
                    break;
                default:
                    MessageService.ShowMessage(title, text);
                    break;
            }
        }

        public void OpenFileDialog(Action<string> onFileDialogSuccess, Action<Exception> onFileDialogError)
        {
            using (var fileDialog = new Xwt.SelectFolderDialog("Browse For Solution"))
            {
                if (fileDialog.Run())
                {
                    onFileDialogSuccess?.Invoke(fileDialog.CurrentFolder);
                }
            }
        }
    }
}