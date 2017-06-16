using AppKit;
using Measurer4000.Core.Services;
using Measurer4000.mac.Services;

namespace Measurer4000.mac
{
    static class MainClass
    {
        static void Main(string[] args)
        {
            ServiceLocator.Register<FileDialogService>(new FileDialogService());
            ServiceLocator.Register<MeasureService>(new MeasureService(new FileManagerService()));
            NSApplication.Init();
            NSApplication.Main(args);
        }
    }
}
