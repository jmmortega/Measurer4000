using System;
using System.Globalization;
using AppKit;
using CoreGraphics;
using Foundation;
using Measurer4000.Core.ViewModels;
using OxyPlot.Xamarin.Mac;

namespace Measurer4000.mac
{
	public partial class MainWindowController : NSWindowController
    {
        private MainViewModel _dataContext;

        private PlotView iosPlot;
        private PlotView androidPlot;
        private PlotView uwpPlot;

        
        public MainWindowController(IntPtr handle) : base(handle)
        {            
        }

        [Export("initWithCoder:")]
        public MainWindowController(NSCoder coder) : base(coder)
        {
        }

        public MainWindowController() : base("MainWindow")
        {
            _dataContext = new MainViewModel();
            _dataContext.PropertyChanged -= DataContextPropertyChanged;
            _dataContext.PropertyChanged += DataContextPropertyChanged;


        }

        private void InitPlotView()
        {
            iosPlot = new PlotView(new CGRect(0,0, iOSPlotView.Frame.Width, iOSPlotView.Frame.Height));
            iOSPlotView.AddSubview(iosPlot);

            androidPlot = new PlotView(new CGRect(0, 0, AndroidPlotView.Frame.Width, AndroidPlotView.Frame.Height));
            AndroidPlotView.AddSubview(androidPlot);


            uwpPlot = new PlotView(new CGRect(0, 0, UWPPlotView.Frame.Width, UWPPlotView.Frame.Height));
            UWPPlotView.AddSubview(uwpPlot);


        }

        private void DataContextPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if(e.PropertyName == "IsBusy")
            {
                ButtonOpenFile.Enabled = !_dataContext.IsBusy;
                ButtonShareLink.Enabled = !_dataContext.IsBusy;
                if(_dataContext.IsBusy)
                {
                    ProgressBar.StartAnimation(this);
                    
                }
                else
                {
                    ProgressBar.StopAnimation(this);
                    GatherValuesFromSolution();
                }
            }
        }

        protected override void Dispose(bool disposing)
        {
            _dataContext.PropertyChanged -= DataContextPropertyChanged;
            base.Dispose(disposing);
        }

        public override void WindowDidLoad()
        {
            ButtonShareLink.Enabled = true;
            base.WindowDidLoad();

            InitPlotView();
        }
        
        public new MainWindow Window
        {
            get { return (MainWindow)base.Window; }
        }

        partial void ButtonOpenFileClick(NSObject sender)
        {
            _dataContext.CommandSelectFile.Execute(null);
        }

        partial void ButtonShareLinkClick(NSObject sender)
        {
            _dataContext.CommandShareLink.Execute(null);
        }
                
        private void GatherValuesFromSolution()
        {                                    
            AmountOfFiles.StringValue = _dataContext.Stats.AmountOfFiles.ToString();
			CodeFiles.StringValue = _dataContext.Stats.CodeFiles.ToString();
			TotalLOC.StringValue = _dataContext.Stats.TotalLinesOfCode.ToString();
            TotalUILines.StringValue = _dataContext.Stats.TotalLinesOfUI.ToString();
            UIFiles.StringValue = _dataContext.Stats.UIFiles.ToString();                     

			CoreLOC.StringValue = _dataContext.Stats.TotalLinesCore.ToString();

			AndroidFiles.StringValue = _dataContext.Stats.AndroidFiles.ToString();
            AndroidLOC.StringValue = _dataContext.Stats.TotalLinesInAndroid.ToString();
			ShareCodeInAndroid.StringValue = _dataContext.Stats.ShareCodeInAndroid.ToString("F2", CultureInfo.InvariantCulture);
			AndroidSpecificCode.StringValue = _dataContext.Stats.AndroidSpecificCode.ToString("F2", CultureInfo.InvariantCulture);         
            
            iOSFiles.StringValue = _dataContext.Stats.iOSFiles.ToString();
            iOSLOC.StringValue = _dataContext.Stats.TotalLinesIniOS.ToString();
			ShareCodeIniOS.StringValue = _dataContext.Stats.ShareCodeIniOS.ToString("F2", CultureInfo.InvariantCulture);
			iOSSpecificCode.StringValue = _dataContext.Stats.iOSSpecificCode.ToString("F2", CultureInfo.InvariantCulture);

			UWPFiles.StringValue = _dataContext.Stats.UWPFiles.ToString();
			UWPLOC.StringValue = _dataContext.Stats.TotalLinesInUWP.ToString();
			ShareCodeInUWP.StringValue = _dataContext.Stats.ShareCodeInUWP.ToString("F2", CultureInfo.InvariantCulture);
			UWPSpecificCode.StringValue = _dataContext.Stats.UWPSpecificCode.ToString("F2", CultureInfo.InvariantCulture);

            iosPlot.Model = _dataContext.IosPlotModel;
            androidPlot.Model = _dataContext.AndroidPlotModel;
            uwpPlot.Model = _dataContext.UwpPlotModel;
        }
    }
}
