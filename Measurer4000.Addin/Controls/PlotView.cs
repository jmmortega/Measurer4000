using System;
using OxyPlot;
using Xamarin.Forms;

namespace Measurer4000.Addin.Controls
{
    public class PlotView : View
    {
        public static readonly BindableProperty ControllerProperty = BindableProperty.Create(nameof(Controller), typeof(PlotController), typeof(PlotView));

        public static readonly BindableProperty ModelProperty = BindableProperty.Create(nameof(Model), typeof(PlotModel), typeof(PlotView));

        public PlotView()
        {
            if (!IsRendererInitialized)
            {
                var message = "Renderer is not initialized.";
                switch (Device.RuntimePlatform)
                {
                    case Device.UWP:
                        message +=
                            "\nRemember to add `OxyPlot.Xamarin.Forms.Platform.UWP.PlotViewRenderer.Init();` after `Xamarin.Forms.Forms.Init(e);` in the Universal Windows app project.";
                        break;
                    case Device.Android:
                        message +=
                            "\nRemember to add `OxyPlot.Xamarin.Forms.Platform.Android.PlotViewRenderer.Init();` after `Xamarin.Forms.Forms.Init();` in the Android app project.";
                        break;
                    case Device.iOS:
                        message +=
                            "\nRemember to add `OxyPlot.Xamarin.Forms.Platform.iOS.PlotViewRenderer.Init();` after `Xamarin.Forms.Forms.Init();` in the iOS app project.";
                        break;
                    case Device.GTK:
                        message +=
                            "\nRemember to add `OxyPlot.Xamarin.Forms.Platform.GTK.PlotViewRenderer.Init();` after `Xamarin.Forms.Forms.Init();` in the GTK app project.";
                        break;
                }
                throw new InvalidOperationException(message);
            }
        }

        public PlotModel Model
        {
            get { return (PlotModel)this.GetValue(ModelProperty); }
            set { this.SetValue(ModelProperty, value); }
        }

        public PlotController Controller
        {
            get { return (PlotController)this.GetValue(ControllerProperty); }
            set { this.SetValue(ControllerProperty, value); }
        }

        public static bool IsRendererInitialized { get; set; }
    }
}