using SecurityTravelApp.Models;
using SecurityTravelApp.Utils;
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
    public partial class DocComp : ContentView
    {

        private static int ColorSelector = 0;
        private Color bgColor;

        private Document Item;
        private List<String> lighterColors;


        public DocComp(Document pDoc)
        {
            InitializeComponent();
            initColors();
            Item = pDoc;

            // test
            SKCanvasView canvasView = new SKCanvasView();

            // set colors
            setBgColor();
            // date recieved and seen texts
            dateRecievedAndSeen();

            // filling texts
            Title.Text = Item.title;
            TheText.Text = Item.description;
            RegionText.Text = Item.region;
            DocType.Text = Item.fileExtension;

            // icon
            selectFileIcon();

            // apply language
            updateTXT();
        }

        private void initColors()
        {
            lighterColors = new List<string>();
            lighterColors.Add("7729bf12");
            lighterColors.Add("7708bdbd");
            lighterColors.Add("77f21b3f");
            lighterColors.Add("77ff9914");
            lighterColors.Add("770fa3b1");
            lighterColors.Add("77b5e2fa");
            lighterColors.Add("77eddea4");
            lighterColors.Add("77f7a072");
        }

        private void selectFileIcon()
        {
            switch (Item.type)
            {
                case DocumentType.Text: TypeIcon.Text = LineAwesomeIcons.FileText; break;
                case DocumentType.Link: TypeIcon.Text = LineAwesomeIcons.Link; break;
                case DocumentType.PDF: TypeIcon.Text = LineAwesomeIcons.FilePDF; break;
                case DocumentType.Word: TypeIcon.Text = LineAwesomeIcons.FileWord; break;
                case DocumentType.Excel: TypeIcon.Text = LineAwesomeIcons.FileExcel; break;
                case DocumentType.Image: TypeIcon.Text = LineAwesomeIcons.FilePicture; break;
                case DocumentType.PowerPoint: TypeIcon.Text = LineAwesomeIcons.FilePowerPoint; break;
                case DocumentType.Audio: TypeIcon.Text = LineAwesomeIcons.FileAudio; break;
                case DocumentType.Video: TypeIcon.Text = LineAwesomeIcons.FileVideo; break;
                case DocumentType.Other: TypeIcon.Text = LineAwesomeIcons.FileOther; break;
            }
        }

        private void dateRecievedAndSeen()
        {
            // date recieved and seen region
            if (Item.isSeen)
            {
                NewInfo.IsVisible = false;
                SeenInfo.IsVisible = true;

                // display time if seen today, display date elsewise
                if (Item.dateSeen.Date == DateTime.Today)
                {
                    TextSeen.Text = Item.dateSeen.ToShortTimeString();
                }
                else
                {
                    TextSeen.Text = Item.dateSeen.ToShortDateString();
                }
                Container.Opacity = .5;
            }
            else
            {
                Container.Opacity = 1;
                NewInfo.IsVisible = true;
                SeenInfo.IsVisible = false;
            }

            // display time if recieved today, display date elsewise
            if (Item.dateReceived.Date == DateTime.Today)
            {
                TextRecieved.Text = Item.dateReceived.ToShortTimeString();
            }
            else
            {
                TextRecieved.Text = Item.dateReceived.ToShortDateString();
            }
        }

        private void setBgColor()
        {
            // setting Bg color of the placeholder
            bgColor = Color.FromHex("#" + lighterColors[ColorSelector]);
            if (++ColorSelector >= lighterColors.Count)
            {
                ColorSelector = 0;
            }

            IconBackground.Color = bgColor;
            Footer.BackgroundColor = bgColor;

        }

        public void updateTXT()
        {
            NewAlertTXT.Text = I18n.GetText(AppTextID.NEW);
        }

        private void OnCanvasViewPaintSurface(object sender, SKPaintSurfaceEventArgs args)
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
                                    new SKColor[] { bgColor.ToSKColor(), Color.FromHex("#77000000").ToSKColor() },
                                    new float[] { 0, 1 },
                                    SKShaderTileMode.Repeat);

                // Draw the gradient on the rectangle
                canvas.DrawRect(rect, paint);
            }
        }
    }
}