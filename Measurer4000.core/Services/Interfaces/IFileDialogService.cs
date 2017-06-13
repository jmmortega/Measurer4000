using System;

namespace Measurer4000.core.Services.Interfaces
{
    public interface IDialogService
    {
        void OpenFileDialog(Action<string> onFileDialogSuccess, Action<Exception> onFileDialogError);
        void OpenError(Exception exceptionError, string messageError);
    }
}
