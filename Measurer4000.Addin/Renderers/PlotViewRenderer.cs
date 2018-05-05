using System.ComponentModel;
using Measurer4000.Addin.Controls;
using Measurer4000.Addin.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.GTK;

[assembly: ExportRenderer(typeof(PlotView), typeof(PlotViewRenderer))]
namespace Measurer4000.Addin.Renderers
{
    public class PlotViewRenderer : ViewRenderer<PlotView, OxyPlot.GtkSharp.PlotView>
    {
        static PlotViewRenderer()
        {
            Init();
        }

        public PlotViewRenderer()
        {
            // Do not delete
        }

        public static void Init()
        {
            PlotView.IsRendererInitialized = true;
        }

        protected override void OnElementChanged(ElementChangedEventArgs<PlotView> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null || Element == null)
            {
                return;
            }

            var plotView = new OxyPlot.GtkSharp.PlotView
            {
                Model = Element.Model,
                Controller = Element.Controller
            };

            SetNativeControl(plotView);
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (Element == null || Control == null)
            {
                return;
            }

            if (e.PropertyName == PlotView.ModelProperty.PropertyName)
            {
                Control.Model = Element.Model;
            }

            if (e.PropertyName == PlotView.ControllerProperty.PropertyName)
            {
                Control.Controller = Element.Controller;
            }
        }
    }
}