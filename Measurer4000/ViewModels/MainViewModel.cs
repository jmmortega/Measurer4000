using Measurer4000.Models;
using Measurer4000.Services;
using Measurer4000.Services.Interfaces;
using Measurer4000.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;


namespace Measurer4000.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private readonly IMeasurerService _measureService;
        private readonly IDialogService _fileDialogService;

        public MainViewModel()
        {
            _measureService = ServiceLocator.Get<MeasureService>();
            _fileDialogService = ServiceLocator.Get<FileDialogService>();
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

        private void OpenSolutionPath(string solutionPath)
        {
            _currentSolution = _measureService.GetProjects(solutionPath);
            MeasureSolution(_currentSolution);
        }

        private void MeasureSolution(Solution solution)
        {
            _currentSolution = _measureService.Measure(solution);
            Stats = new CodeStats() {
                AmountOfFiles = 0,
                CodeFiles = 0,
                UIFiles = 0,
                TotalLinesOfCode = 0,
                TotalLinesOfUI = 0,
                AndroidFiles = 0,
                iOSFiles = 0,
                TotalLinesInAndroid = 0,
                TotalLinesIniOS = 0
            };
        }

        private void ShowError(Exception e)
        {
            _fileDialogService.OpenError(e, string.Empty);
        }
    }
}
