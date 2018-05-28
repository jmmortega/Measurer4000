using Measurer4000.Addin.Views;
using Xamarin.Forms.Platform.GTK.Extensions;

namespace Measurer4000.Addin.Windows
{
    public class MeasurerWindow: Gtk.Window
    {
        public MeasurerWindow()
            : base(Gtk.WindowType.Toplevel)
        {
            Title = "Measurer4000";
            WindowPosition = Gtk.WindowPosition.Center;

            var page = new MeasurerView();

            Add(page.CreateContainer());

            SetSizeRequest(800, 480);
        }
    }
}