using System.IO;
using System.Linq;
using Measurer4000.Core.ViewModels;
using MonoDevelop.Ide;
using Xamarin.Forms;

namespace Measurer4000.Addin.Views
{
    public partial class MeasurerView : ContentPage
    {
        MainViewModel _viewModel;

        public MeasurerView()
        {
            InitializeComponent();

            _viewModel = new MainViewModel();
            BindingContext = _viewModel;
        }

		protected override void OnAppearing()
		{
            base.OnAppearing();

            var solutions = IdeApp.Workspace.GetAllSolutions();

            if (solutions.Any())
            {
                var solution = solutions.First();
                var solutionPath = Path.Combine(Directory.GetCurrentDirectory(), solution.FileName);
                _viewModel.MeasureSolutionByPath(solutionPath);
            }
		}
	}
}