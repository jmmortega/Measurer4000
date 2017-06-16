using Measurer4000.Core.Services.Interfaces;
using System;
using System.Diagnostics;

namespace Measurer4000.Services
{
    public class WebBrowserWPFTaskService : IWebBrowserTaskService
    {
        public void Navigate(string url)
        {
            Process.Start(new ProcessStartInfo(url));
        }
    }
}
