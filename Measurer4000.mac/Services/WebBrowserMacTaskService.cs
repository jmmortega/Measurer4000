using AppKit;
using Foundation;
using Measurer4000.Core.Services.Interfaces;

namespace Measurer4000.mac.Services
{
    public class WebBrowserMacTaskService : IWebBrowserTaskService
    {
        public void Navigate(string url)
        {            
            NSWorkspace.SharedWorkspace.OpenUrl(new NSUrl(url));
        }
    }
}
