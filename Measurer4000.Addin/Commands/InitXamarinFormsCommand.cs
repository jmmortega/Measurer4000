using Measurer4000.Addin.Renderers;
using Measurer4000.Addin.Services;
using Measurer4000.Core.Services;
using Measurer4000.Core.Services.Interfaces;
using MonoDevelop.Components.Commands;
using Xamarin.Forms;

namespace Measurer4000.Addin.Commands
{
    public class InitXamarinFormsCommand : CommandHandler
    {
        protected override void Run()
        {
            Forms.Init();

            PlotViewRenderer.Init();

            ServiceLocator.Register<IDialogService>(new FileDialogService());
            ServiceLocator.Register<IMeasurerService>(new MeasureService(new FileManagerService()));
            ServiceLocator.Register<IWebBrowserTaskService>(new WebBrowserTaskService());
        }
    }
}