using SkiaSharp;
using SkiaSharp.Views.Forms;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SecurityTravelApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BGComp : ContentView
    {

        public static readonly BindableProperty BGImageProperty = BindableProperty.Create("BGImage", typeof(String), typeof(BGComp), "BG1.png",
            BindingMode.TwoWay, propertyChanged: OnBGImageChanged);


        static void OnBGImageChanged(BindableObject bindable, object oldValue, object newValue)
        {
            BGComp owner = (BGComp)bindable;
            owner.decodeBitmap();
        }

        public string BGImage
        {
            get { return (string)GetValue(BGImageProperty); }
            set { SetValue(BGImageProperty, value); }
        }


        SKBitmap bitmap;
        public BGComp()
        {
            InitializeComponent();
            decodeBitmap();
        }

        public void decodeBitmap()
        {
            String resourceID = "SecurityTravelApp.Assets." + BGImage;
            Assembly assembly = GetType().GetTypeInfo().Assembly;
            using (Stream stream = assembly.GetManifestResourceStream(resourceID))
            {
                bitmap = SKBitmap.Decode(stream);
            }
        }


        private void BGPaintSurface(object sender, SKPaintSurfaceEventArgs args)
        {
            if (bitmap == null) return;
            double ratio = 1.0; 
            SKImageInfo info = args.Info;
            SKSurface surface = args.Surface;
            SKCanvas canvas = surface.Canvas;
            int desiredWidth = (int)Math.Round(info.Width * ratio);
            int desiredHeight = (int)Math.Round(info.Height * ratio);
            double ratioWidth = bitmap.Width * 1.0 / desiredWidth;
            double ratioHeight = bitmap.Height * 1.0 / desiredHeight;
            if (ratioWidth > ratioHeight)
            {
                desiredHeight = (int)Math.Round(bitmap.Height / ratioWidth);
            }
            else
            {
                desiredWidth = (int)Math.Round(bitmap.Width / ratioHeight);
            }


            SKImageInfo resizeInfo = new SKImageInfo(desiredWidth, desiredHeight);
            var resizedBitmap = bitmap.Resize(resizeInfo, SKFilterQuality.Medium);

            canvas.Clear();

            using (SKPaint paint = new SKPaint())
            {
                paint.Shader = SKShader.CreateBitmap(resizedBitmap, SKShaderTileMode.Repeat, SKShaderTileMode.Repeat);
                canvas.DrawRect(info.Rect, paint);
            }
        }

    }
}