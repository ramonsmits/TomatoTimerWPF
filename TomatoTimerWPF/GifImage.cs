using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;

namespace TomatoTimerWPF
{
    class GifImage : Image
    {
        public static readonly DependencyProperty FrameIndexProperty =
            DependencyProperty.Register(
                "FrameIndex",
                typeof(int),
                typeof(GifImage),
                new UIPropertyMetadata(0, FrameIndexChanged));

        public static readonly DependencyProperty AutoStartProperty =
            DependencyProperty.Register(
                "AutoStart",
                typeof(bool),
                typeof(GifImage),
                new UIPropertyMetadata(true, AutoStartChanged));

        public static readonly DependencyProperty GifSourceProperty =
            DependencyProperty.Register(
                "GifSource",
                typeof(string),
                typeof(GifImage),
                new UIPropertyMetadata(string.Empty, GifSourceChanged));

        public static readonly DependencyProperty DurationPerFrameProperty =
            DependencyProperty.Register(
                "DurationPerFrameMS",
                typeof(int),
                typeof(GifImage),
                new UIPropertyMetadata(100, null));

        Int32Animation animation;
        GifBitmapDecoder decoder;
        bool isInitialized;

        static GifImage()
        {
            VisibilityProperty.OverrideMetadata(
                typeof(GifImage),
                new FrameworkPropertyMetadata(VisibilityChanged));
        }

        public int FrameIndex
        {
            get => (int)GetValue(FrameIndexProperty);
            set => SetValue(FrameIndexProperty, value);
        }

        public bool AutoStart
        {
            get => (bool)GetValue(AutoStartProperty);
            set => SetValue(AutoStartProperty, value);
        }

        public string GifSource
        {
            get => (string)GetValue(GifSourceProperty);
            set => SetValue(GifSourceProperty, value);
        }

        public int DurationPerFrameMS
        {
            get => (int)GetValue(DurationPerFrameProperty);
            set => SetValue(DurationPerFrameProperty, value);
        }

        void Initialize()
        {
            decoder = new GifBitmapDecoder(
                new Uri("pack://application:,,," + GifSource),
                BitmapCreateOptions.PreservePixelFormat,
                BitmapCacheOption.Default);
            int frameCount = decoder.Frames.Count;
            animation = new Int32Animation(
                0,
                decoder.Frames.Count - 1,
                new Duration(new TimeSpan(0, 0, 0, 0, frameCount * 1000)))
                {RepeatBehavior = RepeatBehavior.Forever};
            Source = decoder.Frames[0];
            isInitialized = true;
            if (AutoStart && Visibility == Visibility.Visible)
            {
                StartAnimation();
            }
        }

        static void VisibilityChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            GifImage gifImage = sender as GifImage;
            if (gifImage == null)
            {
                return;
            }
            if ((Visibility)e.NewValue == Visibility.Visible)
            {
                gifImage.StartAnimation();
            }
            else
            {
                gifImage.StopAnimation();
            }
        }

        static void FrameIndexChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var gifImage = sender as GifImage;
            if (gifImage != null)
            {
                gifImage.Source = gifImage.decoder.Frames[(int)e.NewValue];
            }
        }

        static void AutoStartChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if ((bool)e.NewValue)
                ((GifImage)sender).StartAnimation();
        }

        static void GifSourceChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            ((GifImage)sender).Initialize();
        }

        public void StartAnimation()
        {
            if (!isInitialized)
                Initialize();
            BeginAnimation(FrameIndexProperty, animation);
        }

        public void StopAnimation()
        {
            BeginAnimation(FrameIndexProperty, null);
        }
    }
}
