using Measurer4000.Services;
using System.Windows;

namespace Measurer4000
{
    /// <summary>
    /// Lógica de interacción para App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            ServiceLocator.Register<FileDialogService>(new FileDialogService());
            ServiceLocator.Register<MeasureService>(new MeasureService());
        }
        
    }
}
