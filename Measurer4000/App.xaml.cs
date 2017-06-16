using Measurer4000.Services;
using Measurer4000.Core.Services;
using System.Windows;
using Measurer4000.Core.Services.Interfaces;

namespace Measurer4000
{
    /// <summary>
    /// Lógica de interacción para App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            ServiceLocator.Register<IDialogService>(new FileDialogService());
            ServiceLocator.Register<IMeasurerService>(new MeasureService(new FileManagerService()));
            ServiceLocator.Register<IWebBrowserTaskService>(new WebBrowserWPFTaskService());
        }
        
    }
}
