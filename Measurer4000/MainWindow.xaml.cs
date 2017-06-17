using System.Windows;
using Measurer4000.Core.ViewModels;

namespace Measurer4000
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            DataContext = new MainViewModel();
        }        
    }
}