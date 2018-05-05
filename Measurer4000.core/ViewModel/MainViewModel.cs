﻿using Measurer4000.Core.Models;
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

        public CodeStats Stats => _currentSolution.Stats;

        private Solution _currentSolution = new Solution();

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

		private PlotModel _uwpPlotModel;

        public PlotModel UwpPlotModel
        {
			get { return _uwpPlotModel; }
            set
            {
				_uwpPlotModel = value;
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
                    ServiceLocator.Get<IWebBrowserTaskService>().Navigate(ShareCodeReportUtils.CreateShareUrl(_currentSolution.Stats));
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
            RaiseProperty(nameof(Stats));
            
            CreateAndroidPlot(_currentSolution.Stats);
            CreateIOSPlot(_currentSolution.Stats);
            CreateUWPPlot(_currentSolution.Stats);

            IsBusy = false;
            _fileDialogService.CreateDialog(EnumTypeDialog.Information
                    , "Consider sharing your applications stats clicking on bottom left link and filling the form\nData collected this way will be public accessible by the community"
                    , "Sharing");
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

        private void CreateUWPPlot(CodeStats codeStats)
        {
            UwpPlotModel = new PlotModel
            {
                Title = "UWP"
            };

            var pieSlice = new PieSeries
            {
                StrokeThickness = 2.0,
                InsideLabelPosition = 0.8,
                AngleSpan = 360,
                StartAngle = 0
            };

            pieSlice.Slices.Add(new PieSlice("Share", codeStats.ShareCodeInUWP) { IsExploded = true, Fill = OxyColors.Green });
            pieSlice.Slices.Add(new PieSlice("Specific", codeStats.UWPSpecificCode) { IsExploded = true, Fill = OxyColors.Red });

            UwpPlotModel.Series.Add(pieSlice);
        }
    }
}
