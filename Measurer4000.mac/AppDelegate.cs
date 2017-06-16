using AppKit;
using Foundation;
using Measurer4000.Core.Services;
using Measurer4000.Core.Services.Interfaces;
using Measurer4000.mac.Services;

namespace Measurer4000.mac
{
    [Register("AppDelegate")]
    public class AppDelegate : NSApplicationDelegate
    {
        MainWindowController mainWindowController;

        public AppDelegate()
        {
        }

        public override void DidFinishLaunching(NSNotification notification)
        {
            ServiceLocator.Register<IDialogService>(new FileDialogService());
            ServiceLocator.Register<IMeasurerService>(new MeasureService(new FileManagerService()));
            ServiceLocator.Register<IWebBrowserTaskService>(new WebBrowserMacTaskService());

            mainWindowController = new MainWindowController();
            mainWindowController.Window.MakeKeyAndOrderFront(this);
        }

        public override void WillTerminate(NSNotification notification)
        {
            // Insert code here to tear down your application
        }
    }
}
