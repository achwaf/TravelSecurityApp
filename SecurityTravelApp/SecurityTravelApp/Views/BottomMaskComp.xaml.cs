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
    public partial class BottomMaskComp : ContentView
    {
        public BottomMaskComp()
        {
            InitializeComponent();
        }


        private void BottomMaskPaintSurface(object sender, SKPaintSurfaceEventArgs args)
        {
            SKImageInfo info = args.Info;
            SKSurface surface = args.Surface;
            SKCanvas canvas = surface.Canvas;

            canvas.Clear();

            using (SKPaint paint = new SKPaint())
            {
                // Create a rectangke the size of the container
                SKRect rect = new SKRect(0, 0, info.Width, info.Height);

                float middleX = (rect.Right + rect.Left) / 2;
                // Create linear gradient from upper-left to lower-right
                paint.Shader = SKShader.CreateLinearGradient(
                                    new SKPoint(middleX, rect.Bottom),
                                    new SKPoint(middleX, rect.Top),
                                    new SKColor[] { Color.FromHex("#44000000").ToSKColor(), Color.FromHex("#00000000").ToSKColor() },
                                    new float[] { 0, 1 },
                                    SKShaderTileMode.Repeat);

                // Draw the gradient on the rectangle
                canvas.DrawRect(rect, paint);
            }
        }
    }
}