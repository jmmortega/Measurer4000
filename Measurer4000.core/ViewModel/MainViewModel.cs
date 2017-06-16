using Measurer4000.Core.Models;
using Measurer4000.Core.Services;
using Measurer4000.Core.Utils;
using Measurer4000.Core.Services.Interfaces;
using Measurer4000.Services;
using Measurer4000.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Diagnostics;
using System.Windows.Navigation;


namespace Measurer4000.Core.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private readonly MeasureService _measureService;
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
            IsBusy = false;
        }

        private void ShowError(Exception e)
        {
            _fileDialogService.OpenError(e, string.Empty);
        }
    }
}
