using SecurityTravelApp.Services;
using SecurityTravelApp.ViewModels;
using SecurityTravelApp.Views.ViewsUtils;
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
    public partial class NavigationBarComp : ContentView, I18nable
    {
        private NavigationBarVM viewModel;
        private ServiceFactory srvFactory;


        public NavigationBarComp()
        {
            InitializeComponent();
        }

        public void initializeContent(ServiceFactory pSrvFactory, NavigationParams pParams)
        {
            srvFactory = pSrvFactory;
            viewModel = new NavigationBarVM(pSrvFactory);
            BindingContext = viewModel;
            populateLayout(pParams);
        }

        private void populateLayout(NavigationParams pParams)
        {
            // clear
            HorizontalLayout.Children.Clear();
            // add NavigationItemComps to the layout
            var i = 0;
            foreach (var item in viewModel.navigationItems)
            {
                // create the component
                Boolean highlight = false;
                if (pParams != null)
                {
                    highlight = item.target == pParams.navigationTarget;
                }
                var comp = new NavigationItemComp(srvFactory, item, highlight);

                // add it
                Grid.SetColumn(comp, i++);
                HorizontalLayout.Children.Add(comp);
            }
        }

        public void update(NavigationParams pParams)
        {
            int index = 0;
            NavigationItem vUpdateTargetItem = null;
            NavigationItemComp vUpdateHighlightComp = null;
            foreach (var el in HorizontalLayout.Children)
            {
                NavigationItemComp navItem = (NavigationItemComp)el;
                NavigationItem vItem = navItem.Item;
                if (vItem.target.Equals(pParams.navigationUpdateTarget))
                {
                    vUpdateTargetItem = vItem;
                    index = HorizontalLayout.Children.IndexOf(navItem);
                }
                if (vItem.target.Equals(pParams.navigationTarget))
                {
                    vUpdateHighlightComp = navItem;
                }

            }
            // we recreate the comp to update display, yeah it s lame but xamarin does not help either
            if (vUpdateTargetItem != null)
            {
                HorizontalLayout.Children[index] = new NavigationItemComp(srvFactory, vUpdateTargetItem, false);
            }
            if (vUpdateHighlightComp != null)
            {
                vUpdateHighlightComp.showAndFadeOutHighlight();
            }
        }

        public void updateTXT()
        {
            foreach (var el in HorizontalLayout.Children)
            {
                NavigationItemComp navItem = (NavigationItemComp)el;
                navItem.updateTXT();
            }
        }

        private void OnCanvasViewPaintSurface(object sender, SkiaSharp.Views.Forms.SKPaintSurfaceEventArgs args)
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
                float endY = (rect.Bottom + rect.Top) / 2;
                // Create linear gradient from upper-left to lower-right
                paint.Shader = SKShader.CreateLinearGradient(
                                    new SKPoint(middleX, rect.Bottom),
                                    new SKPoint(middleX, endY),
                                    new SKColor[] { Color.FromHex("#77999999").ToSKColor(), Color.FromHex("#00FFFFFF").ToSKColor() },
                                    new float[] { 0, 1 },
                                    SKShaderTileMode.Clamp);

                // Draw the gradient on the rectangle
                canvas.DrawRect(rect, paint);
            }
        }


    }

}