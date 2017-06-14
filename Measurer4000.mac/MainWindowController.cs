﻿using System;

using Foundation;
using AppKit;

namespace Measurer4000.mac
{
    public partial class MainWindowController : NSWindowController
    {
        public MainWindowController(IntPtr handle) : base(handle)
        {
        }

        private System.IO.StreamReader file;
        [Export("initWithCoder:")]
        public MainWindowController(NSCoder coder) : base(coder)
        {
        }

        public MainWindowController() : base("MainWindow")
        {
        }
        public override void WindowDidLoad()
        {
			string path = "/Users/diegofafe/Documents/test.html";

			System.IO.Stream j = NSFileHandle.OpenRead(path).ReadDataToEndOfFile().AsStream();
            file = new System.IO.StreamReader(j);
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
            if (!file.EndOfStream) ShareLink.StringValue = file.ReadLine();
        }

        partial void ShareLinkClick(NSObject sender)
        {
            
        }
    }
}
