﻿using System;

using Foundation;
using AppKit;
using Measurer4000.Core.Services.Interfaces;
using Measurer4000.Core.Services;
using Measurer4000.mac.Services;
using Measurer4000.Core.Models;
using System.Linq;
using System.Globalization;

namespace Measurer4000.mac
{
    public partial class MainWindowController : NSWindowController
    {
        private readonly IDialogService _fileDialogService = new FileDialogService();
        private readonly MeasureService _measureService = new MeasureService();
        private Solution _currentSolution;
        public MainWindowController(IntPtr handle) : base(handle)
        {
        }

        [Export("initWithCoder:")]
        public MainWindowController(NSCoder coder) : base(coder)
        {
        }

        public MainWindowController() : base("MainWindow")
        {
        }

        public override void WindowWillLoad()
        {
            base.WindowWillLoad();
        }
        public override void WindowDidLoad()
        {
            _measureService.FileManagerService = new FileManagerService();
            ProgressBar.IsDisplayedWhenStopped = false;
            base.WindowDidLoad();
        }

        public override void AwakeFromNib()
        {
            base.AwakeFromNib();
        }

        public new MainWindow Window
        {
            get { return (MainWindow)base.Window; }
        }

        partial void ButtonOpenFileClick(Foundation.NSObject sender)
        {
            _fileDialogService.OpenFileDialog(
                        (solutionPath) => OpenSolutionPath(solutionPath),
                        (error) => ShowError(error));
        }

        partial void ShareLinkClick(NSObject sender)
        {

        }

        private void ShowError(Exception e)
        {
            _fileDialogService.OpenError(e, string.Empty);
        }

        private void OpenSolutionPath(string solutionPath)
        {
            ButtonOpenFile.Enabled = false;
            ProgressBar.StartAnimation(this);
            _currentSolution = _measureService.GetProjects(solutionPath);
            ButtonOpenFile.Enabled = true;
            ProgressBar.StopAnimation(this);
            MeasureSolution(_currentSolution);
        }

        private void MeasureSolution(Solution solution)
        {
            ButtonOpenFile.Enabled = false;
            ProgressBar.StartAnimation(this);
            _currentSolution = _measureService.Measure(solution);
            _currentSolution.Stats = new CodeStats()
            {
                ShareCodeInAndroid = Math.Round(((double)_currentSolution.Projects.Where(x => x.Platform == EnumPlatform.Core).SelectMany(x => x.Files).Where(x => x.IsUserInterface == false).Sum(x => x.LOC)
                    / (_currentSolution.Projects.Where(x => x.Platform == EnumPlatform.Android).SelectMany(x => x.Files).Where(x => x.IsUserInterface == false).Sum(x => x.LOC)
                    + _currentSolution.Projects.Where(x => x.Platform == EnumPlatform.Core).SelectMany(x => x.Files).Where(x => x.IsUserInterface == false).Sum(x => x.LOC))
                    ) * 100, 2),
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
            AmountOfFiles.StringValue = _currentSolution.Stats.AmountOfFiles.ToString();
            AndroidFiles.StringValue = _currentSolution.Stats.AndroidFiles.ToString();
            AndroidLOC.StringValue = _currentSolution.Stats.TotalLinesInAndroid.ToString();
            CodeFiles.StringValue = _currentSolution.Stats.CodeFiles.ToString();
            CoreLOC.StringValue = _currentSolution.Stats.TotalLinesCore.ToString();
            iOSFiles.StringValue = _currentSolution.Stats.iOSFiles.ToString();
            iOSLOC.StringValue = _currentSolution.Stats.TotalLinesIniOS.ToString();
            ShareCodeInAndroid.StringValue = _currentSolution.Stats.ShareCodeInAndroid.ToString("F2", CultureInfo.InvariantCulture);
            ShareCodeIniOS.StringValue = _currentSolution.Stats.ShareCodeIniOS.ToString("F2", CultureInfo.InvariantCulture);
            ShareUIInAndroid.StringValue = _currentSolution.Stats.ShareUIInAndroid.ToString("F2", CultureInfo.InvariantCulture);
            ShareUIIniOS.StringValue = _currentSolution.Stats.ShareUIIniOS.ToString("F2", CultureInfo.InvariantCulture);
            TotalLOC.StringValue = _currentSolution.Stats.TotalLinesOfCode.ToString();
            TotalUILines.StringValue = _currentSolution.Stats.TotalLinesOfUI.ToString();
            UIFiles.StringValue = _currentSolution.Stats.UIFiles.ToString();
            ButtonOpenFile.Enabled = true;
            ProgressBar.StopAnimation(this);
        }
    }
}
