using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
namespace wpf.control
{
    public class CustomWindow : Window
    {
        public static readonly DependencyProperty ContentProperty = DependencyProperty.Register("Content", typeof(UIElement), typeof(CustomWindow));
        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register("Title", typeof(string), typeof(CustomWindow));
        public static readonly DependencyProperty IconProperty = DependencyProperty.Register("Icon", typeof(ImageSource), typeof(CustomWindow));
        public static readonly DependencyProperty ShowIconProperty = DependencyProperty.Register("ShowIcon", typeof(bool), typeof(CustomWindow), new UIPropertyMetadata(true));
        public static readonly DependencyProperty ResizableProperty = DependencyProperty.Register("Resizable", typeof(bool), typeof(CustomWindow), new UIPropertyMetadata(true));
        public static readonly DependencyProperty FocusedProperty = DependencyProperty.Register("Focused", typeof(bool), typeof(CustomWindow), new UIPropertyMetadata(false, new PropertyChangedCallback(FocusedValueChanged)));
        public static readonly DependencyProperty WindowOutSideBoxProperty = DependencyProperty.Register("WindowOutSideBox", typeof(bool), typeof(CustomWindow), new UIPropertyMetadata(false, new PropertyChangedCallback(WindowOutSideBoxValueChanged)));

        public bool WindowOutSideBox { get { return (bool)GetValue(WindowOutSideBoxProperty); } set { SetValue(WindowOutSideBoxProperty, value); } }

        public bool Resizable { get { return (bool)GetValue(ResizableProperty); } set { SetValue(ResizableProperty, value); } }
        public UIElement Content { get { return (UIElement)GetValue(ContentProperty); } set { SetValue(ContentProperty, value); } }
        public string Title { get { return (string)GetValue(TitleProperty); } set { SetValue(TitleProperty, value); } }
        public ImageSource Icon { get { return (ImageSource)GetValue(IconProperty); } set { SetValue(IconProperty, value); } }
        public bool Focused { get { return (bool)GetValue(FocusedProperty); } set { SetValue(FocusedProperty, value); } }
        private Button WindowOutSideButton;
        private Button minimizeButton;

        private Button maximizeButton;

        private Button closeButton;

        WindowState NonMaximizedState { get; set; }
        private StackPanel buttonsPanel;
        public MdiContainer Container { get; set; }
        static CustomWindow()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CustomWindow), new FrameworkPropertyMetadata(typeof(CustomWindow)));
        }
        public CustomWindow()
        {
            this.Loaded += CustomWindow_Loaded;
            this.AllowsTransparency = true;
            this.WindowStyle = WindowStyle.None;
        }
        private void CustomWindow_Loaded(object sender, RoutedEventArgs e)
        {

        }
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            minimizeButton = (Button)Template.FindName("MinimizeButton", this);
            maximizeButton = (Button)Template.FindName("MaximizeButton", this);
            closeButton = (Button)Template.FindName("CloseButton", this);
            buttonsPanel = (StackPanel)Template.FindName("ButtonsPanel", this);

            WindowOutSideButton = (Button)Template.FindName("WindowOutSideBoxButton", this);
            if (WindowOutSideButton != null)
            {
                WindowOutSideButton.Click += WindowOutSideButton_Click;
            }
            if (minimizeButton != null)
                minimizeButton.Click += MinimizeButton_Click;

            if (maximizeButton != null)
                maximizeButton.Click += MaximizeButton_Click;

            if (closeButton != null)
                closeButton.Click += CloseButton_Click;

            Thumb dragThumb = (Thumb)Template.FindName("DragThumb", this);

            if (dragThumb != null)
            {
                dragThumb.DragStarted += Thumb_DragStarted;
                dragThumb.DragDelta += DragThumb_DragDelta;

                dragThumb.MouseDoubleClick += (sender, e) =>
                {
                    if (WindowState == WindowState.Minimized)
                        minimizeButton_Click(null, null);
                    else if (WindowState == WindowState.Normal)
                        maximizeButton_Click(null, null);
                };
            }

            Thumb resizeLeft = (Thumb)Template.FindName("ResizeLeft", this);
            Thumb resizeTopLeft = (Thumb)Template.FindName("ResizeTopLeft", this);
            Thumb resizeTop = (Thumb)Template.FindName("ResizeTop", this);
            Thumb resizeTopRight = (Thumb)Template.FindName("ResizeTopRight", this);
            Thumb resizeRight = (Thumb)Template.FindName("ResizeRight", this);
            Thumb resizeBottomRight = (Thumb)Template.FindName("ResizeBottomRight", this);
            Thumb resizeBottom = (Thumb)Template.FindName("ResizeBottom", this);
            Thumb resizeBottomLeft = (Thumb)Template.FindName("ResizeBottomLeft", this);
            if (resizeLeft != null)
            {
                resizeLeft.DragStarted += (sendor, e) => { Thumb_DragStarted(sendor, e); };
                resizeLeft.DragDelta += ResizeLeft_DragDelta;
            }

            if (resizeTop != null)
            {
                resizeTop.DragStarted += Thumb_DragStarted;
                resizeTop.DragDelta += ResizeTop_DragDelta;
            }

            if (resizeRight != null)
            {
                resizeRight.DragStarted += Thumb_DragStarted;
                resizeRight.DragDelta += ResizeRight_DragDelta;
            }

            if (resizeBottom != null)
            {
                resizeBottom.DragStarted += Thumb_DragStarted;
                resizeBottom.DragDelta += ResizeBottom_DragDelta;
            }

            if (resizeTopLeft != null)
            {
                resizeTopLeft.DragStarted += Thumb_DragStarted;

                resizeTopLeft.DragDelta += (sender, e) =>
                {
                    ResizeTop_DragDelta(null, e);
                    ResizeLeft_DragDelta(null, e);

                };
            }

            if (resizeTopRight != null)
            {
                resizeTopRight.DragStarted += Thumb_DragStarted;

                resizeTopRight.DragDelta += (sender, e) =>
                {
                    ResizeTop_DragDelta(null, e);
                    ResizeRight_DragDelta(null, e);
                };
            }

            if (resizeBottomRight != null)
            {
                resizeBottomRight.DragStarted += Thumb_DragStarted;

                resizeBottomRight.DragDelta += (sender, e) =>
                {
                    ResizeBottom_DragDelta(null, e);
                    ResizeRight_DragDelta(null, e);
                };
            }

            if (resizeBottomLeft != null)
            {
                resizeBottomLeft.DragStarted += Thumb_DragStarted;

                resizeBottomLeft.DragDelta += (sender, e) =>
                {
                    ResizeBottom_DragDelta(null, e);
                    ResizeLeft_DragDelta(null, e);
                };
            }
        }

        private void ResizeLeft_DragDelta(object sender, DragDeltaEventArgs e)
        {
            if (Width - e.HorizontalChange < MinWidth)
                return;

            double newLeft = e.HorizontalChange;

            if (this.Left + newLeft < 0)
                newLeft = 0 - this.Left;
            Width -= newLeft;
            this.Left += newLeft;

        }
        private void ResizeTop_DragDelta(object sender, DragDeltaEventArgs e)
        {
            if (Height - e.VerticalChange < MinHeight)
                return;
            double newTop = e.VerticalChange;
            if (this.Top + newTop < 0)
                newTop = 0 - this.Top;
            Height -= newTop;
            this.Top += newTop;
        }
        private void ResizeRight_DragDelta(object sender, DragDeltaEventArgs e)
        {
            if (Width + e.HorizontalChange < MinWidth)
                return;
            Width += e.HorizontalChange;
        }
        private void ResizeBottom_DragDelta(object sender, DragDeltaEventArgs e)
        {
            if (Height + e.VerticalChange < MinHeight)
                return;
            Height += e.VerticalChange;
        }

        private void DragThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            if (WindowState == WindowState.Maximized)
                return;
            double newLeft = this.Left + e.HorizontalChange,
                newTop = this.Top + e.VerticalChange;
            if (newLeft < 0)
                newLeft = 0;
            if (newTop < 0)
                newTop = 0;
            this.Left = newLeft;
            this.Top = newTop;
 
        }

        private void Thumb_DragStarted(object sender, DragStartedEventArgs e)
        {
            if (!Focused)
                Focused = true;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void MaximizeButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.WindowState != WindowState.Maximized)
            {
                this.WindowState = WindowState.Maximized;
            }
            else this.WindowState = WindowState.Normal;
        }

        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.WindowState != WindowState.Minimized)
            {
                this.WindowState = WindowState.Minimized;
            }
            else this.WindowState = WindowState.Minimized;
        }

        private void WindowOutSideButton_Click(object sender, RoutedEventArgs e)
        {
            this.WindowOutSideBox = this.WindowOutSideBox ? false : true;
        }

        private static void FocusedValueChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if ((bool)e.NewValue == (bool)e.OldValue)
                return;

            CustomWindow win = (CustomWindow)sender;
            if ((bool)e.NewValue)
            {
                win.Dispatcher.BeginInvoke(new Func<IInputElement, IInputElement>(Keyboard.Focus), System.Windows.Threading.DispatcherPriority.ApplicationIdle, win.Content);
                win.RaiseEvent(new RoutedEventArgs(GotFocusEvent, win));
            }
            else
            {
                if (win.WindowState == WindowState.Maximized)
                    win.Unmaximize();
                win.RaiseEvent(new RoutedEventArgs(LostFocusEvent, win));
            }
        }

        internal void Unmaximize()
        {
            if (WindowState == WindowState.Maximized)
                WindowState = NonMaximizedState;
        }

        private void minimizeButton_Click(object sender, RoutedEventArgs e)
        {
            if (WindowState == WindowState.Minimized)
                WindowState = WindowState.Normal;
            else
                WindowState = WindowState.Minimized;
        }
        private void maximizeButton_Click(object sender, RoutedEventArgs e)
        {
            if (WindowState == WindowState.Maximized)
                WindowState = WindowState.Normal;
            else
                WindowState = WindowState.Maximized;
        }
        private static void WindowOutSideBoxValueChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            CustomWindow t = (CustomWindow)sender;
            bool open = (bool)e.NewValue;
            if (!open)
            {
                t.WindowChange();
            }
            else
            {

            }
        }
        public void WindowChange()
        {
            if (!WindowOutSideBox)
            {
                MdiChild mdiChild = new MdiChild();
                mdiChild.Content = this.Content;
                mdiChild.Container = this.Container;
                mdiChild.Title = this.Title;
                mdiChild.WindowOutSideBox = false;
                mdiChild.Width = this.Width;
                mdiChild.Height = this.Height;
                Container.Children.Add(mdiChild);

                Point locationFromScreen = Container.PointToScreen(new Point(0, 0));
                double container_screen_x = locationFromScreen.X * Utility.GetDpiRatio();
                double container_screen_y = locationFromScreen.Y * Utility.GetDpiRatio();
                double diff_x =  this.Left- container_screen_x;
                double diff_y =     this.Top - container_screen_y;
                Point p = new Point(  diff_x,   diff_y);


                Canvas.SetTop(mdiChild, p.Y < 0 ? 0 : p.Y);

                Canvas.SetLeft(mdiChild, p.X < 0 ? 0 : p.X);

                this.Close(); Container.InvalidateSize();
            }
            else
            {

            }
        }

    }
}
