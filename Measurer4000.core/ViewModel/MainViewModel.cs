using Measurer4000.Core.Models;
using Measurer4000.Core.Services;
using Measurer4000.Core.Utils;
using Measurer4000.Core.Services.Interfaces;
using System;
using System.Windows.Input;
using Measurer4000.Core.ViewModels.Base;
using OxyPlot;
using OxyPlot.Series;

namespace Measurer4000.Core.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private readonly IMeasurerService _measureService;
        private readonly IDialogService _fileDialogService;
        
        public MainViewModel()
        {
            _measureService = ServiceLocator.Get<IMeasurerService>();
            _fileDialogService = ServiceLocator.Get<IDialogService>();
        }

        private bool _isBusy;

        public bool IsBusy
        {
            get { return _isBusy; }
            set
            {
                _isBusy = value;
                RaiseProperty();
            }
        }

        private CodeStats _stats = new CodeStats();

        public CodeStats Stats
        {
            get { return _stats; }
            set { _stats = value; RaiseProperty(); }
        }

        private Solution _currentSolution;

        private PlotModel _androidPlotModel;

        public PlotModel AndroidPlotModel
        {
            get { return _androidPlotModel; }
            set
            {
                _androidPlotModel = value;
                RaiseProperty();
            }
        }

        private PlotModel _iosPlotModel;

        public PlotModel IosPlotModel
        {
            get { return _iosPlotModel; }
            set
            {
                _iosPlotModel = value;
                RaiseProperty();
            }
        }

        public ICommand CommandSelectFile
        {
            get
            {
                return new Command(() =>
                {
                    _fileDialogService.OpenFileDialog(
                        (solutionPath) => OpenSolutionPath(solutionPath),
                        (error) => ShowError(error));                        
                });
            }
        }

        public ICommand CommandShareLink
        {
            get
            {
                return new Command(() =>
                {
                    var url = ShareCodeReportUtils.CreateShareUrl(_stats);
                    ServiceLocator.Get<IWebBrowserTaskService>().Navigate(url);
                });
            }
        }

        private void OpenSolutionPath(string solutionPath)
        {
            if(solutionPath.ToLower().Contains("measurer")) 
            {
                ServiceLocator.Get<IWebBrowserTaskService>().Navigate(ShareCodeReportUtils.CreateExceptionUrl())
            }
            else
            {
                IsBusy = true;
                _currentSolution = _measureService.GetProjects(solutionPath);
                IsBusy = false; // just in case we later split this in 2 buttons
                MeasureSolution(_currentSolution);
            }
        }

        private void MeasureSolution(Solution solution)
        {
            IsBusy = true;
            _currentSolution = _measureService.Measure(solution);
            Stats = MeasureUtils.CalculateStats(_currentSolution);
            CreateAndroidPlot(Stats);
            CreateIOSPlot(Stats);
            IsBusy = false;
        }

        private void ShowError(Exception e)
        {
            _fileDialogService.OpenError(e, string.Empty);
        }

        private void CreateAndroidPlot(CodeStats codeStats)
        {
            AndroidPlotModel = new PlotModel
            {
                Title = "Android"
            };

            var pieSlice = new PieSeries
            {
                StrokeThickness = 2.0,
                InsideLabelPosition = 0.8,
                AngleSpan = 360,
                StartAngle = 0
            };

            pieSlice.Slices.Add(new PieSlice("Share", codeStats.ShareCodeInAndroid) { IsExploded = true, Fill = OxyColors.Green });
            pieSlice.Slices.Add(new PieSlice("Specific", codeStats.AndroidSpecificCode) { IsExploded = true, Fill = OxyColors.Red });

            AndroidPlotModel.Series.Add(pieSlice);
        }

        private void CreateIOSPlot(CodeStats codeStats)
        {
            IosPlotModel = new PlotModel
            {
                Title = "iOS"
            };

            var pieSlice = new PieSeries
            {
                StrokeThickness = 2.0,
                InsideLabelPosition = 0.8,
                AngleSpan = 360,
                StartAngle = 0
            };

            pieSlice.Slices.Add(new PieSlice("Share", codeStats.ShareCodeIniOS) { IsExploded = true, Fill = OxyColors.Green });
            pieSlice.Slices.Add(new PieSlice("Specific", codeStats.iOSSpecificCode) { IsExploded = true, Fill = OxyColors.Red });

            IosPlotModel.Series.Add(pieSlice);
        }
    }
}
