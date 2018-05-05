using System.Diagnostics;
using Measurer4000.Core.Services.Interfaces;

namespace Measurer4000.Addin.Services
{
    public class WebBrowserTaskService : IWebBrowserTaskService
    {
        public void Navigate(string url)
        {
            Process.Start(new ProcessStartInfo(url));
        }
    }
}