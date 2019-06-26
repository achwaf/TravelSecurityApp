using Plugin.Permissions.Abstractions;
using SecurityTravelApp.DependencyServices;
using SecurityTravelApp.Services;
using SecurityTravelApp.Utils;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SecurityTravelApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TapSliderComp : ContentView
    {
        private const double _fadeEffect = 0.5;
        private const int _animLength = 140;
        private const int SOSTextShift = -15;
        private Animation back_animation_tumb;
        private Animation back_animation_delaybar;
        private Stopwatch stopwatch = new Stopwatch();
        private int secondsToWaitConst = 5;
        private int secondsToWait = 5;
        private PanGestureRecognizer _panGesture = new PanGestureRecognizer();
        private View _gestureListener;
        private Boolean thumbOnTarget = false;
        private Boolean timerLaunched = false;
        private Boolean actionLaunched = false;
        private Timer timer;
        private AppManagementService appMngSrv;
        private CallService callSrv;

        public TapSliderComp()
        {
            InitializeComponent();
            _panGesture.PanUpdated += OnPanGestureUpdated;
            CornerRadius cornerRadius = Utilities.getOnPlatformValue<CornerRadius>(this.Resources["ThumbCornerRadius"]);
            _gestureListener = new BoxView { BackgroundColor = Color.White, Opacity = .02, CornerRadius = cornerRadius };
            _gestureListener.GestureRecognizers.Add(_panGesture);
            AbsoluteLayout.SetLayoutFlags(_gestureListener, AbsoluteLayoutFlags.SizeProportional);
            AbsoluteLayout.SetLayoutBounds(_gestureListener, new Rectangle(0, 0, 1, 1));
            TapSlider.Children.Add(_gestureListener);
            timer = new Timer();
            timer.Elapsed += new ElapsedEventHandler(OnTimeEventAsync);
            timer.Interval = 1000;

        }

        public void initializeConfig(ServiceFactory pSrvFactory)
        {
            appMngSrv = (AppManagementService)pSrvFactory.getService(ServiceType.AppManagement);
            callSrv = (CallService)pSrvFactory.getService(ServiceType.Call);
        }

        private void SOSGradientPaintSurface(object sender, SKPaintSurfaceEventArgs args)
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
                float middleY = (rect.Top + rect.Bottom) / 2;
                // Create linear gradient from upper-left to lower-right
                paint.Shader = SKShader.CreateLinearGradient(
                                    new SKPoint(rect.Left, middleY),
                                    new SKPoint(rect.Right, middleY),
                                    new SKColor[] { Color.FromHex("#9B0303").ToSKColor(), Color.FromHex("#DB0606").ToSKColor() },
                                    new float[] { 0, 1 },
                                    SKShaderTileMode.Clamp);

                // Draw the gradient on the rectangle
                canvas.DrawRect(rect, paint);
            }
        }


        async void OnPanGestureUpdated(object sender, PanUpdatedEventArgs e)
        {

            double slideLength = Width - Thumb.Width - Thumb.Margin.Right - Thumb.Margin.Left;

            switch (e.StatusType)
            {
                case GestureStatus.Started:
                    actionLaunched = false;
                    SliderBorder.FadeTo(1, _animLength);
                    CallIcon.FadeTo(1, _animLength);
                    secondsToWait = secondsToWaitConst;
                    TimerLabel.Text = secondsToWait.ToString();
                    MessageLaunch.Text = I18n.GetText(AppTextID.APPEL_DANS);
                    break;

                case GestureStatus.Running:
                    // Translate and ensure we don't pan beyond the wrapped user interface element bounds.
                    var x = Math.Max(0, e.TotalX);
                    if (x > slideLength)
                        x = slideLength;

                    // ratio of length slided on max max slideLength
                    var v = x / slideLength;

                    Thumb.TranslationX = x;
                    SOSText.TranslationX = v * SOSTextShift;
                    DelayBar.WidthRequest = x + Thumb.Width;

                    // if the thumb has reached the target launch the timer to count duration
                    if (Thumb.TranslationX >= Width - Thumb.Width - 20) // 20 pour marge de precision
                    {
                        thumbOnTarget = true;
                        if (timerLaunched)
                        {
                            // wait for time duration to end
                        }
                        else
                        {
                            // launch the timer 
                            timerLaunched = true;
                            timer.Start();

                            // show the message 
                            showMessage();
                        }
                    }
                    else
                    {
                        // the thumb moved from target
                        thumbOnTarget = false;

                        // hide the message
                        hideMessage();

                        if (timerLaunched)
                        {
                            if (actionLaunched)
                            {
                                // do nothing
                            }
                            else
                            {
                                secondsToWait = secondsToWaitConst;
                                TimerLabel.Text = secondsToWait.ToString();
                            }
                            timer.Stop();
                            timerLaunched = false;
                        }
                        else
                        {
                            secondsToWait = secondsToWaitConst;
                            TimerLabel.Text = secondsToWait.ToString();
                        }
                    }
                    break;

                case GestureStatus.Completed:

                    if (timerLaunched)
                    {
                        timer.Stop();
                        timerLaunched = false;
                    }

                    // hide the message
                    hideMessage();

                    var posX = Thumb.TranslationX;
                    back_animation_tumb = new Animation(d => Thumb.TranslationX = d, posX, 0, Easing.CubicIn);
                    back_animation_delaybar = new Animation(d => DelayBar.WidthRequest = d, DelayBar.Width, Thumb.Width, Easing.CubicIn);
                    // Reset translation applied during the pan
                    back_animation_delaybar.Commit(Thumb, "DelayBarBack", 16, _animLength);
                    back_animation_tumb.Commit(Thumb, "ThumbBack", 16, _animLength);

                    var taskAnimationSliderBorder = SliderBorder.FadeTo(.3, _animLength / 2);
                    var taskAnimationCallIcon = CallIcon.FadeTo(.4, _animLength / 2);
                    var taskAnimationShiftSOSTextBack = SOSText.TranslateTo(0, 0, _animLength);
                    await Task.WhenAll(taskAnimationSliderBorder, taskAnimationCallIcon, taskAnimationShiftSOSTextBack);
                    break;
            }
        }

        private async void hideMessage()
        {
            if (TimerLabel.IsVisible || MessageLaunch.IsVisible)
            {
                var taskAnimationLabel = TimerLabel.FadeTo(0, _animLength / 2);
                var taskAnimationMessage = MessageLaunch.FadeTo(0, _animLength / 2);
                await Task.WhenAll(taskAnimationLabel, taskAnimationMessage);
                TimerLabel.IsVisible = false;
                MessageLaunch.IsVisible = false;
            }
        }

        private async void showMessage()
        {
            TimerLabel.IsVisible = true;
            MessageLaunch.IsVisible = true;
            TimerLabel.Opacity = 0;
            MessageLaunch.Opacity = 0;
            await Task.Delay(800);
            TimerLabel.FadeTo(.5, _animLength / 2);
            MessageLaunch.FadeTo(.5, _animLength / 2);
        }

        private async void OnTimeEventAsync(object source, ElapsedEventArgs e)
        {
            if (secondsToWait <= 0)
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    // UI interaction goes here
                    TimerLabel.Text = "";
                    MessageLaunch.Text = I18n.GetText(AppTextID.SOS_EN_COURS);
                });

                // do action
                if (!actionLaunched)
                {
                    actionLaunched = true;
                    MessagingCenter.Send<TapSliderComp>(this, "SOS");
                    timer.Stop();
                }
            }
            else
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    // UI interaction goes here
                    TimerLabel.Text = secondsToWait--.ToString();
                });
            }

        }


    }
}