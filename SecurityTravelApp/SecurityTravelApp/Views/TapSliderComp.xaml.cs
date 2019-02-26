using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SecurityTravelApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TapSliderComp : ContentView
    {
        private const double _fadeEffect = 0.5;
        private const uint _animLength = 180;
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


        public event EventHandler SlideCompleted;

        public TapSliderComp()
        {
            InitializeComponent();
            _panGesture.PanUpdated += OnPanGestureUpdated;
            //SizeChanged += OnSizeChanged;

            _gestureListener = new ContentView { BackgroundColor = Color.White, Opacity = 0.05 };
            _gestureListener.GestureRecognizers.Add(_panGesture);
            AbsoluteLayout.SetLayoutFlags(_gestureListener, AbsoluteLayoutFlags.SizeProportional);
            AbsoluteLayout.SetLayoutBounds(_gestureListener, new Rectangle(0, 0, 1, 1));
            TapSlider.Children.Add(_gestureListener);
            timer = new Timer();
            timer.Elapsed += new ElapsedEventHandler(OnTimeEvent);
            timer.Interval = 1000;
        }

        async void OnPanGestureUpdated(object sender, PanUpdatedEventArgs e)
        {

            switch (e.StatusType)
            {
                case GestureStatus.Started:
                    break;

                case GestureStatus.Running:
                    // Translate and ensure we don't pan beyond the wrapped user interface element bounds.
                    var x = Math.Max(0, e.TotalX);
                    if (x > (Width - Thumb.Width))
                        x = (Width - Thumb.Width);

                    Thumb.TranslationX = x;
                    DelayBar.WidthRequest = x + Thumb.Width;

                    // if the thumb has reached the target launch the timer to count duration
                    if (Thumb.TranslationX >= Width - Thumb.Width - 10) // 10 pour marge de precision
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
                            TimerLabel.IsVisible = true;
                            MessageLaunch.FadeTo(1, _animLength);
                            TimerLabel.FadeTo(1, _animLength);
                        }
                    }
                    else
                    {
                        // the thumb moved from target
                        thumbOnTarget = false;
                        if (timerLaunched)
                        {
                            if (actionLaunched)
                            {
                                // do nothing
                            }
                            else
                            {
                                secondsToWait = secondsToWaitConst;
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

                    // hide the message
                    MessageLaunch.FadeTo(0, _animLength);
                    await TimerLabel.FadeTo(0, _animLength);
                    TimerLabel.IsVisible = false;

                    var posX = Thumb.TranslationX;
                    back_animation_tumb = new Animation(d => Thumb.TranslationX = d, posX, 0, Easing.CubicIn);
                    back_animation_delaybar = new Animation(d => DelayBar.WidthRequest = d, DelayBar.Width, Thumb.Width, Easing.CubicIn);
                    // Reset translation applied during the pan
                    back_animation_delaybar.Commit(Thumb, "DelayBarBack", 16, _animLength);
                    back_animation_tumb.Commit(Thumb, "ThumbBack", 16, _animLength);
                    //await Task.WhenAll(new Task[]{
                    ////TrackBar.FadeTo(1, _animLength),
                    //Thumb.TranslateTo(0, 0, _animLength * 2, Easing.CubicIn),

                    //});

                    if (posX >= (Width - Thumb.Width - 10/* keep some margin for error*/))
                        SlideCompleted?.Invoke(this, EventArgs.Empty);
                    break;
            }
        }

        private void OnTimeEvent(object source, ElapsedEventArgs e)
        {
            if (secondsToWait < 0)
            {
                // do action
                actionLaunched = true;
            }
            Device.BeginInvokeOnMainThread(() =>
            {
                // UI interaction goes here
                TimerLabel.Text = secondsToWait--.ToString();
            });
            
        }


    }
}