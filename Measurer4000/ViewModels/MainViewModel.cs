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
            _measureService.Measure(solutionPath);
        }

        private void ShowError(Exception e)
        {
            _fileDialogService.OpenError(e, string.Empty);
        }
    }
}
