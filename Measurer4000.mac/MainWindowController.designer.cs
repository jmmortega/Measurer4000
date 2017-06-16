// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace Measurer4000.mac
{
	[Register ("MainWindowController")]
	partial class MainWindowController
	{
		[Outlet]
		AppKit.NSTextField AmountOfFiles { get; set; }

		[Outlet]
		AppKit.NSTextField AndroidFiles { get; set; }

		[Outlet]
		AppKit.NSTextField AndroidLOC { get; set; }

		[Outlet]
		AppKit.NSButton ButtonOpenFile { get; set; }

		[Outlet]
		AppKit.NSButton ButtonShareLink { get; set; }

		[Outlet]
		AppKit.NSTextField CodeFiles { get; set; }

		[Outlet]
		AppKit.NSTextField CoreLOC { get; set; }

		[Outlet]
		AppKit.NSTextField iOSFiles { get; set; }

		[Outlet]
		AppKit.NSTextField iOSLOC { get; set; }

		[Outlet]
		AppKit.NSProgressIndicator ProgressBar { get; set; }

		[Outlet]
		AppKit.NSTextField ShareCodeInAndroid { get; set; }

		[Outlet]
		AppKit.NSTextField ShareCodeIniOS { get; set; }

		[Outlet]
		AppKit.NSTextField ShareLink { get; set; }

		[Outlet]
		AppKit.NSTextField ShareUIInAndroid { get; set; }

		[Outlet]
		AppKit.NSTextField ShareUIIniOS { get; set; }

		[Outlet]
		AppKit.NSTextField TotalLOC { get; set; }

		[Outlet]
		AppKit.NSTextField TotalUILines { get; set; }

		[Outlet]
		AppKit.NSTextField UIFiles { get; set; }

		[Action ("ButtonOpenFileClick:")]
		partial void ButtonOpenFileClick (Foundation.NSObject sender);

		[Action ("ButtonShareLinkClick:")]
		partial void ButtonShareLinkClick (Foundation.NSObject sender);

		[Action ("ShareLinkClick:")]
		partial void ShareLinkClick (Foundation.NSObject sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (AmountOfFiles != null) {
				AmountOfFiles.Dispose ();
				AmountOfFiles = null;
			}

			if (AndroidFiles != null) {
				AndroidFiles.Dispose ();
				AndroidFiles = null;
			}

			if (AndroidLOC != null) {
				AndroidLOC.Dispose ();
				AndroidLOC = null;
			}

			if (ButtonOpenFile != null) {
				ButtonOpenFile.Dispose ();
				ButtonOpenFile = null;
			}

			if (CodeFiles != null) {
				CodeFiles.Dispose ();
				CodeFiles = null;
			}

			if (CoreLOC != null) {
				CoreLOC.Dispose ();
				CoreLOC = null;
			}

			if (iOSFiles != null) {
				iOSFiles.Dispose ();
				iOSFiles = null;
			}

			if (iOSLOC != null) {
				iOSLOC.Dispose ();
				iOSLOC = null;
			}

			if (ProgressBar != null) {
				ProgressBar.Dispose ();
				ProgressBar = null;
			}

			if (ShareCodeInAndroid != null) {
				ShareCodeInAndroid.Dispose ();
				ShareCodeInAndroid = null;
			}

			if (ShareCodeIniOS != null) {
				ShareCodeIniOS.Dispose ();
				ShareCodeIniOS = null;
			}

			if (ShareLink != null) {
				ShareLink.Dispose ();
				ShareLink = null;
			}

			if (ShareUIInAndroid != null) {
				ShareUIInAndroid.Dispose ();
				ShareUIInAndroid = null;
			}

			if (ShareUIIniOS != null) {
				ShareUIIniOS.Dispose ();
				ShareUIIniOS = null;
			}

			if (TotalLOC != null) {
				TotalLOC.Dispose ();
				TotalLOC = null;
			}

			if (TotalUILines != null) {
				TotalUILines.Dispose ();
				TotalUILines = null;
			}

			if (UIFiles != null) {
				UIFiles.Dispose ();
				UIFiles = null;
			}

			if (ButtonShareLink != null) {
				ButtonShareLink.Dispose ();
				ButtonShareLink = null;
			}
		}
	}
}
