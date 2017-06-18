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
                        (error) => _fileDialogService.CreateDialog(EnumTypeDialog.Error, error.Message));                        
                });
            }
        }

        public ICommand CommandShareLink
        {
            get
            {
                return new Command(() =>
                {
                    ServiceLocator.Get<IWebBrowserTaskService>().Navigate(ShareCodeReportUtils.CreateShareUrl(_stats));
                });
            }
        }

        private void OpenSolutionPath(string solutionPath)
        {
            if(solutionPath.ToLower().Contains("measurer")) 
            {
                _fileDialogService.CreateDialog(EnumTypeDialog.Warning
                    , "The measuring measurer is trying to measure a measuring measurer\nWill this measuring measurer be enough measurer to even measure a measuring measurer?"
                    , "Measurer Measurement Exception");
                ServiceLocator.Get<IWebBrowserTaskService>().Navigate(ShareCodeReportUtils.CreateExceptionUrl());
            }
            IsBusy = true;
            _currentSolution = _measureService.GetProjects(solutionPath);
            IsBusy = false; // just in case we later split this in 2 buttons
            MeasureSolution(_currentSolution);
        }

        private void MeasureSolution(Solution solution)
        {
            IsBusy = true;
            _currentSolution = _measureService.Measure(solution);
            Stats = MeasureUtils.CalculateStats(_currentSolution);
            CreateAndroidPlot(Stats);
            CreateIOSPlot(Stats);
            IsBusy = false;
            _fileDialogService.CreateDialog(EnumTypeDialog.Information
                    , "Consider sharing your applications stats clicking on bottom left link and filling the form\nData collected this way will be public accessible by the community"
                    , "Sharing");
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
