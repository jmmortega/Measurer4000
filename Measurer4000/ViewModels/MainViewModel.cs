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
                ShareCodeInAndroid = Math.Round(((double) _currentSolution.Projects.Where(x => x.Platform == EnumPlatform.Core).SelectMany(x => x.Files).Where(x => x.IsUserInterface == false).Sum(x => x.LOC) 
                    / (_currentSolution.Projects.Where(x => x.Platform == EnumPlatform.Android).SelectMany(x => x.Files).Where(x => x.IsUserInterface == false).Sum(x => x.LOC)
                    + _currentSolution.Projects.Where(x => x.Platform == EnumPlatform.Core).SelectMany(x => x.Files).Where(x => x.IsUserInterface == false).Sum(x => x.LOC))
                    ) *100,2),
                ShareCodeIniOS = Math.Round(((double)_currentSolution.Projects.Where(x => x.Platform == EnumPlatform.Core).SelectMany(x => x.Files).Where(x => x.IsUserInterface == false).Sum(x => x.LOC)
                    / (_currentSolution.Projects.Where(x => x.Platform == EnumPlatform.iOS).SelectMany(x => x.Files).Where(x => x.IsUserInterface == false).Sum(x => x.LOC)
                    + _currentSolution.Projects.Where(x => x.Platform == EnumPlatform.Core).SelectMany(x => x.Files).Where(x => x.IsUserInterface == false).Sum(x => x.LOC))
                    ) * 100, 2),
                ShareUIInAndroid = Math.Round(((double)_currentSolution.Projects.Where(x => x.Platform == EnumPlatform.Core).SelectMany(x => x.Files).Where(x => x.IsUserInterface == true).Sum(x => x.LOC)
                    / (_currentSolution.Projects.Where(x => x.Platform == EnumPlatform.Android).SelectMany(x => x.Files).Where(x => x.IsUserInterface == true).Sum(x => x.LOC)
                    + _currentSolution.Projects.Where(x => x.Platform == EnumPlatform.Core).SelectMany(x => x.Files).Where(x => x.IsUserInterface == true).Sum(x => x.LOC))
                    ) * 100, 2),
                ShareUIIniOS = Math.Round(((double)_currentSolution.Projects.Where(x => x.Platform == EnumPlatform.Core).SelectMany(x => x.Files).Where(x => x.IsUserInterface == true).Sum(x => x.LOC)
                    / (_currentSolution.Projects.Where(x => x.Platform == EnumPlatform.iOS).SelectMany(x => x.Files).Where(x => x.IsUserInterface == true).Sum(x => x.LOC)
                    + _currentSolution.Projects.Where(x => x.Platform == EnumPlatform.Core).SelectMany(x => x.Files).Where(x => x.IsUserInterface == true).Sum(x => x.LOC))
                    ) * 100, 2),

                AmountOfFiles = _currentSolution.Projects.SelectMany(p => p.Files).Count(),
                CodeFiles = _currentSolution.Projects.SelectMany(x => x.Files).Count(x => x.IsUserInterface == false),
                UIFiles = _currentSolution.Projects.SelectMany(x => x.Files).Count(x => x.IsUserInterface == true),
                TotalLinesOfCode = _currentSolution.Projects.SelectMany(x => x.Files).Where(x => x.IsUserInterface == false).Sum(x => x.LOC),
                TotalLinesOfUI = _currentSolution.Projects.SelectMany(x => x.Files).Where(x => x.IsUserInterface == true).Sum(x => x.LOC),
                AndroidFiles = _currentSolution.Projects.Where(x => x.Platform == EnumPlatform.Android).SelectMany(x => x.Files).Count(),
                iOSFiles = _currentSolution.Projects.Where(x => x.Platform == EnumPlatform.iOS).SelectMany(x => x.Files).Count(),
                TotalLinesCore = _currentSolution.Projects.Where(x => x.Platform == EnumPlatform.Core).SelectMany(x => x.Files).Sum(x => x.LOC),
                TotalLinesInAndroid = _currentSolution.Projects.Where(x => x.Platform == EnumPlatform.Android).SelectMany(x => x.Files).Sum(x => x.LOC),
                TotalLinesIniOS = _currentSolution.Projects.Where(x => x.Platform == EnumPlatform.iOS).SelectMany(x => x.Files).Sum(x => x.LOC)
            };
        }

        private void ShowError(Exception e)
        {
            _fileDialogService.OpenError(e, string.Empty);
        }
    }
}
