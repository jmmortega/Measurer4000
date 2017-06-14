using System;

using Foundation;
using AppKit;

namespace Measurer4000.mac
{
    public partial class MainWindowController : NSWindowController
    {
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
        public override void WindowDidLoad()
        {
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
            AmountOfFiles.StringValue = "FUNCIONA!";
        }

        partial void ShareLinkClick(NSObject sender)
        {
            System.IO.Stream j = NSFileHandle.OpenRead("/path").ReadDataToEndOfFile().AsStream();
        }
    }
}
