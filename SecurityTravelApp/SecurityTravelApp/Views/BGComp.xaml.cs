using SkiaSharp;
using SkiaSharp.Views.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
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
            string value = (string)newValue;
            owner.bitmap = SKBitmap.Decode(value);
        }

        public string BGImage
        {
            get { return (string) GetValue(BGImageProperty); }
            set { SetValue(BGImageProperty, value); }
        }


        SKBitmap bitmap;
        public BGComp()
        {
            InitializeComponent();
            bitmap = SKBitmap.Decode(BGImage);
        }


        private void BGPaintSurface(object sender, SKPaintSurfaceEventArgs args)
        {
            if (bitmap == null) return;
            SKImageInfo info = args.Info;
            SKSurface surface = args.Surface;
            SKCanvas canvas = surface.Canvas;

            canvas.Clear();

            using (SKPaint paint = new SKPaint())
            {
                paint.Shader = SKShader.CreateBitmap(bitmap, SKShaderTileMode.Repeat, SKShaderTileMode.Repeat);
                canvas.DrawRect(info.Rect, paint);
            }
        }

    }
}