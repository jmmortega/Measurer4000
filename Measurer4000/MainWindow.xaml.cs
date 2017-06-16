using System.Windows;
using System.Windows.Navigation;
using System.Diagnostics;
using Measurer4000.Core.ViewModels;

namespace Measurer4000
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            DataContext = new MainViewModel();
        }        
    }
}
