using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Collections.Generic;

namespace wpf.control
{
    [ContentProperty("Content")]
    public class MdiChild : Control, IDropSurface
    {
        public List<IDropSurface> Surfaces;

        public OverlayWindow OverlayWindow;

        public Rect SurfaceRectangle
        {
            get
            {
                double y = this.PointToScreen(this.Position).Y * Utility.GetDpiRatio() - this.Position.Y;
                double x = this.PointToScreen(this.Position).X * Utility.GetDpiRatio() - this.Position.X;
                return new Rect(new Point(x, y), new Size(ActualWidth, ActualHeight));
            }
        }

        public MdiChild getMdiChild
        {

            get { return this; }
        }

        internal const int MinimizedWidth = 160;
        internal const int MinimizedHeight = 29;
        public static readonly DependencyProperty ContentProperty = DependencyProperty.Register("Content", typeof(UIElement), typeof(MdiChild));
        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register("Title", typeof(string), typeof(MdiChild), new UIPropertyMetadata(""));
        public static readonly DependencyProperty IconProperty = DependencyProperty.Register("Icon", typeof(ImageSource), typeof(MdiChild));
        public static readonly DependencyProperty ShowIconProperty = DependencyProperty.Register("ShowIcon", typeof(bool), typeof(MdiChild), new UIPropertyMetadata(true));
        public static readonly DependencyProperty ResizableProperty = DependencyProperty.Register("Resizable", typeof(bool), typeof(MdiChild), new UIPropertyMetadata(true));
        public static readonly DependencyProperty FocusedProperty = DependencyProperty.Register("Focused", typeof(bool), typeof(MdiChild), new UIPropertyMetadata(false, new PropertyChangedCallback(FocusedValueChanged)));
        public static readonly DependencyProperty MinimizeBoxProperty = DependencyProperty.Register("MinimizeBox", typeof(bool), typeof(MdiChild), new UIPropertyMetadata(true, new PropertyChangedCallback(MinimizeBoxValueChanged)));
        public static readonly DependencyProperty MaximizeBoxProperty = DependencyProperty.Register("MaximizeBox", typeof(bool), typeof(MdiChild), new UIPropertyMetadata(true, new PropertyChangedCallback(MaximizeBoxValueChanged)));
        public static readonly DependencyProperty WindowOutSideBoxProperty = DependencyProperty.Register("WindowOutSideBox", typeof(bool), typeof(MdiChild), new UIPropertyMetadata(false, new PropertyChangedCallback(WindowOutSideBoxValueChanged)));
        public static readonly DependencyProperty CloseBoxProperty = DependencyProperty.Register("CloseBox", typeof(bool), typeof(MdiChild), new UIPropertyMetadata(true, new PropertyChangedCallback(CloseBoxValueChanged)));
        public static readonly DependencyProperty WindowStateProperty = DependencyProperty.Register("WindowState", typeof(WindowState), typeof(MdiChild), new UIPropertyMetadata(WindowState.Normal, new PropertyChangedCallback(WindowStateValueChanged)));
        public static readonly DependencyProperty PositionProperty = DependencyProperty.Register("Position", typeof(Point), typeof(MdiChild), new UIPropertyMetadata(new Point(-1, -1), new PropertyChangedCallback(PositionValueChanged)));
        public static readonly RoutedEvent ClosingEvent = EventManager.RegisterRoutedEvent("Closing", RoutingStrategy.Bubble, typeof(ClosingEventArgs), typeof(MdiChild)); public static readonly RoutedEvent ClosedEvent = EventManager.RegisterRoutedEvent("Closed", RoutingStrategy.Bubble, typeof(RoutedEventArgs), typeof(MdiChild));
        public UIElement Content { get { return (UIElement)GetValue(ContentProperty); } set { SetValue(ContentProperty, value); } }
        public string Title { get { return (string)GetValue(TitleProperty); } set { SetValue(TitleProperty, value); } }
        public ImageSource Icon { get { return (ImageSource)GetValue(IconProperty); } set { SetValue(IconProperty, value); } }
        public bool ShowIcon { get { return (bool)GetValue(ShowIconProperty); } set { SetValue(ShowIconProperty, value); } }
        public bool Resizable { get { return (bool)GetValue(ResizableProperty); } set { SetValue(ResizableProperty, value); } }
        public bool Focused { get { return (bool)GetValue(FocusedProperty); } set { SetValue(FocusedProperty, value); } }
        public bool MinimizeBox { get { return (bool)GetValue(MinimizeBoxProperty); } set { SetValue(MinimizeBoxProperty, value); } }
        public bool MaximizeBox { get { return (bool)GetValue(MaximizeBoxProperty); } set { SetValue(MaximizeBoxProperty, value); } }
        public bool WindowOutSideBox { get { return (bool)GetValue(WindowOutSideBoxProperty); } set { SetValue(WindowOutSideBoxProperty, value); } }
        public bool CloseBox { get { return (bool)GetValue(CloseBoxProperty); } set { SetValue(CloseBoxProperty, value); } }
        public WindowState WindowState { get { return (WindowState)GetValue(WindowStateProperty); } set { SetValue(WindowStateProperty, value); } }
        public Point Position { get { return (Point)GetValue(PositionProperty); } set { SetValue(PositionProperty, value); } }
        public Panel Buttons
        {
            //get { return (Panel)GetValue(ButtonsProperty); }
            //private set { SetValue(ButtonsProperty, value); }
            get;
            private set;
        }
        private new Thickness Margin { set { } }
        public event RoutedEventHandler Closing
        {
            add { AddHandler(ClosingEvent, value); }
            remove { RemoveHandler(ClosingEvent, value); }
        }

        public event RoutedEventHandler Closed
        {
            add { AddHandler(ClosedEvent, value); }
            remove { RemoveHandler(ClosedEvent, value); }
        }
        private Button WindowOutSideButton;
        private Button minimizeButton;

        private Button maximizeButton;

        private Button closeButton;

        private StackPanel buttonsPanel;

        public MdiContainer Container { get; set; }

        private Rect originalDimension;

        private Point minimizedPosition = new Point(-1, -1);

        WindowState NonMaximizedState { get; set; }



        static MdiChild()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MdiChild), new FrameworkPropertyMetadata(typeof(MdiChild)));
        }
        public MdiChild(MdiContainer pContainer)
        {
            Focusable = IsTabStop = false;
            Loaded += MdiChild_Loaded;
            GotFocus += MdiChild_GotFocus;
            KeyDown += MdiChild_KeyDown;
            Container = pContainer;
            OverlayWindow = new OverlayWindow(Container.Surfaces);

        }
        private void MdiChild_Loaded(object sender, RoutedEventArgs e)
        {
            FrameworkElement currentControl = this;

            while (currentControl != null && currentControl.GetType() != typeof(MdiContainer))
                currentControl = (FrameworkElement)currentControl.Parent;

            if (currentControl != null)
                Container = (MdiContainer)currentControl;
            //else throw new Exception("Unable to find MdiContainer parent.");
        }
        static void MdiChild_KeyDown(object sender, KeyEventArgs e)
        {
            MdiChild mdiChild = (MdiChild)sender;
            switch (e.Key)
            {
                case Key.F4:
                    if (e.KeyboardDevice.Modifiers == ModifierKeys.Control)
                    {
                        mdiChild.Close();
                        e.Handled = true;
                    }
                    break;
            }
        }

        private void MdiChild_GotFocus(object sender, RoutedEventArgs e)
        {
            Focus();
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
                minimizeButton.Click += minimizeButton_Click;

            if (maximizeButton != null)
                maximizeButton.Click += maximizeButton_Click;

            if (closeButton != null)
                closeButton.Click += closeButton_Click;

            Thumb dragThumb = (Thumb)Template.FindName("DragThumb", this);

            if (dragThumb != null)
            {
                dragThumb.DragStarted += Thumb_DragStarted;
                dragThumb.DragDelta += dragThumb_DragDelta;
                dragThumb.DragCompleted += DragThumb_DragCompleted;

                dragThumb.MouseDoubleClick += (sender, e) =>
                {
                    if (WindowState == WindowState.Minimized)
                        minimizeButton_Click(null, null);
                    else if (WindowState == WindowState.Normal)
                        maximizeButton_Click(null, null);
                    else if (WindowState == WindowState.Maximized)
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
                resizeLeft.DragStarted += Thumb_DragStarted;
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

                    Container.InvalidateSize();
                };
            }

            if (resizeTopRight != null)
            {
                resizeTopRight.DragStarted += Thumb_DragStarted;

                resizeTopRight.DragDelta += (sender, e) =>
                {
                    ResizeTop_DragDelta(null, e);
                    ResizeRight_DragDelta(null, e);

                    Container.InvalidateSize();
                };
            }

            if (resizeBottomRight != null)
            {
                resizeBottomRight.DragStarted += Thumb_DragStarted;

                resizeBottomRight.DragDelta += (sender, e) =>
                {
                    ResizeBottom_DragDelta(null, e);
                    ResizeRight_DragDelta(null, e);

                    Container.InvalidateSize();
                };
            }

            if (resizeBottomLeft != null)
            {
                resizeBottomLeft.DragStarted += Thumb_DragStarted;

                resizeBottomLeft.DragDelta += (sender, e) =>
                {
                    ResizeBottom_DragDelta(null, e);
                    ResizeLeft_DragDelta(null, e);

                    Container.InvalidateSize();
                };
            }

            MinimizeBoxValueChanged(this, new DependencyPropertyChangedEventArgs(MinimizeBoxProperty, true, MinimizeBox));
            MaximizeBoxValueChanged(this, new DependencyPropertyChangedEventArgs(MaximizeBoxProperty, true, MaximizeBox));
            CloseBoxValueChanged(this, new DependencyPropertyChangedEventArgs(CloseBoxProperty, true, CloseBox));
        }



        private void WindowOutSideButton_Click(object sender, RoutedEventArgs e)
        {
            if (WindowOutSideBox)
                WindowOutSideBox = false;
            else
                WindowOutSideBox = true;
        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);
            Focused = true;
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
        private void closeButton_Click(object sender, RoutedEventArgs e)
        {
            ClosingEventArgs eventArgs = new ClosingEventArgs(ClosingEvent);
            RaiseEvent(eventArgs);

            if (eventArgs.Cancel)
                return;
            Close();
            RaiseEvent(new RoutedEventArgs(ClosedEvent));
        }
        private void Thumb_DragStarted(object sender, DragStartedEventArgs e)
        {
            if (!Focused)
                Focused = true;
        }
        private void ResizeLeft_DragDelta(object sender, DragDeltaEventArgs e)
        {
            if (Width - e.HorizontalChange < MinWidth)
                return;

            double newLeft = e.HorizontalChange;

            if (Position.X + newLeft < 0)
                newLeft = 0 - Position.X;

            Width -= newLeft;
            Position = new Point(Position.X + newLeft, Position.Y);

            if (sender != null)
                Container.InvalidateSize();
        }
        private void ResizeTop_DragDelta(object sender, DragDeltaEventArgs e)
        {
            if (Height - e.VerticalChange < MinHeight)
                return;

            double newTop = e.VerticalChange;

            if (Position.Y + newTop < 0)
                newTop = 0 - Position.Y;

            Height -= newTop;
            Position = new Point(Position.X, Position.Y + newTop);

            if (sender != null)
                Container.InvalidateSize();
        }
        private void ResizeRight_DragDelta(object sender, DragDeltaEventArgs e)
        {
            if (Width + e.HorizontalChange < MinWidth)
                return;
            Width += e.HorizontalChange;
            if (sender != null)
                Container.InvalidateSize();
        }
        private void ResizeBottom_DragDelta(object sender, DragDeltaEventArgs e)
        {
            if (Height + e.VerticalChange < MinHeight)
                return;
            Height += e.VerticalChange;
            if (sender != null)
                Container.InvalidateSize();
        }
        private void dragThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            if (WindowState == WindowState.Maximized)
                return;

            double newLeft = Position.X + e.HorizontalChange,
                newTop = Position.Y + e.VerticalChange;

            if (newLeft < 0)
                newLeft = 0;
            if (newTop < 0)
                newTop = 0;
            Point p = new Point(PointToScreen(Position).X * Utility.GetDpiRatio() - Position.X, PointToScreen(Position).Y * Utility.GetDpiRatio() - Position.Y);

            foreach (IDropSurface surface in Container.Surfaces)
            {
                if (surface != this)
                    if (surface.SurfaceRectangle.IntersectsWith(SurfaceRectangle))
                    {
                        if (surface.getMdiChild != null)
                        {
                            surface.getMdiChild.OverlayWindow.Left = surface.getMdiChild.PointToScreen(surface.getMdiChild.Position).X * Utility.GetDpiRatio() - surface.getMdiChild.Position.X;
                            surface.getMdiChild.OverlayWindow.Top = surface.getMdiChild.PointToScreen(surface.getMdiChild.Position).Y * Utility.GetDpiRatio() - surface.getMdiChild.Position.Y;
                            surface.getMdiChild.OverlayWindow.Height = surface.getMdiChild.ActualHeight;
                            surface.getMdiChild.OverlayWindow.Width = surface.getMdiChild.Width;

                            surface.getMdiChild.OverlayWindow.ShowOverlayPaneDockingOptions(surface.SurfaceRectangle);
                            surface.getMdiChild.OverlayWindow.Show();
                        }
                        else
                        {
                            surface.OnDragOver(p);
                        }
                    }

            }
            this.Title = $"({newLeft},{newTop}) ({PointToScreen(Position).X * Utility.GetDpiRatio()},{PointToScreen(Position).Y * Utility.GetDpiRatio()})";
            Position = new Point(newLeft, newTop);

            Container.InvalidateSize();
        }
        private void DragThumb_DragCompleted(object sender, DragCompletedEventArgs e)
        {
            foreach (IDropSurface surface in Container.Surfaces)
            {
                if (surface != this)
                    if (surface.getMdiChild != null)
                    {
                        if (surface.SurfaceRectangle.IntersectsWith(SurfaceRectangle))
                        {
                            surface.getMdiChild.OverlayWindow.Hide();
                        }
                        else surface.getMdiChild.OverlayWindow.Hide();
                    }
              
            }
        }

        private static void PositionValueChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if ((Point)e.NewValue == (Point)e.OldValue)
                return;

            MdiChild mdiChild = (MdiChild)sender;
            Point newPosition = (Point)e.NewValue;

            Canvas.SetTop(mdiChild, newPosition.Y < 0 ? 0 : newPosition.Y);
            Canvas.SetLeft(mdiChild, newPosition.X < 0 ? 0 : newPosition.X);
        }
        private static void FocusedValueChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if ((bool)e.NewValue == (bool)e.OldValue)
                return;
            MdiChild mdiChild = (MdiChild)sender;
            if ((bool)e.NewValue)
            {
                mdiChild.Dispatcher.BeginInvoke(new Func<IInputElement, IInputElement>(Keyboard.Focus), System.Windows.Threading.DispatcherPriority.ApplicationIdle, mdiChild.Content);
                mdiChild.RaiseEvent(new RoutedEventArgs(GotFocusEvent, mdiChild));
            }
            else
            {
                if (mdiChild.WindowState == WindowState.Maximized)
                    mdiChild.Unmaximize();
                mdiChild.RaiseEvent(new RoutedEventArgs(LostFocusEvent, mdiChild));
            }
        }
        private static void MinimizeBoxValueChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            MdiChild mdiChild = (MdiChild)sender;
            bool visible = (bool)e.NewValue;
            if (visible)
            {
                bool maximizeVisible = true;

                if (mdiChild.maximizeButton != null)
                    maximizeVisible = mdiChild.maximizeButton.Visibility == Visibility.Visible;

                if (mdiChild.minimizeButton != null)
                    mdiChild.minimizeButton.IsEnabled = true;

                if (!maximizeVisible)
                {
                    if (mdiChild.maximizeButton != null)
                        mdiChild.minimizeButton.Visibility = Visibility.Visible;

                    if (mdiChild.maximizeButton != null)
                        mdiChild.maximizeButton.Visibility = Visibility.Visible;
                }
            }
            else
            {
                bool maximizeEnabled = true;
                if (mdiChild.maximizeButton != null)
                    maximizeEnabled = mdiChild.maximizeButton.IsEnabled;

                if (mdiChild.minimizeButton != null)
                    mdiChild.minimizeButton.IsEnabled = false;

                if (!maximizeEnabled)
                {
                    if (mdiChild.minimizeButton != null)
                        mdiChild.minimizeButton.Visibility = Visibility.Hidden;

                    if (mdiChild.maximizeButton != null)
                        mdiChild.maximizeButton.Visibility = Visibility.Hidden;
                }
            }
        }

        private static void MaximizeBoxValueChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            MdiChild mdiChild = (MdiChild)sender;
            bool visible = (bool)e.NewValue;
            if (visible)
            {
                bool minimizeVisible = true;

                if (mdiChild.minimizeButton != null)
                    minimizeVisible = mdiChild.minimizeButton.Visibility == Visibility.Visible;

                if (mdiChild.maximizeButton != null)
                    mdiChild.maximizeButton.IsEnabled = true;

                if (!minimizeVisible)
                {
                    if (mdiChild.maximizeButton != null)
                        mdiChild.minimizeButton.Visibility = Visibility.Visible;

                    if (mdiChild.maximizeButton != null)
                        mdiChild.maximizeButton.Visibility = Visibility.Visible;
                }
            }
            else
            {
                bool minimizeEnabled = true;

                if (mdiChild.minimizeButton != null)
                    minimizeEnabled = mdiChild.minimizeButton.IsEnabled;

                if (mdiChild.maximizeButton != null)
                    mdiChild.maximizeButton.IsEnabled = false;

                if (!minimizeEnabled)
                {
                    if (mdiChild.maximizeButton != null)
                        mdiChild.minimizeButton.Visibility = Visibility.Hidden;

                    if (mdiChild.maximizeButton != null)
                        mdiChild.maximizeButton.Visibility = Visibility.Hidden;
                }
            }
        }
        private static void CloseBoxValueChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            MdiChild mdiChild = (MdiChild)sender;
            bool visible = (bool)e.NewValue;

            if (visible)
            {
                if ((mdiChild.closeButton != null) && (mdiChild.closeButton.Visibility != Visibility.Visible))
                    mdiChild.closeButton.Visibility = Visibility.Visible;
            }
            else
            {
                if ((mdiChild.closeButton != null) && (mdiChild.closeButton.Visibility == Visibility.Visible))
                    mdiChild.closeButton.Visibility = Visibility.Collapsed;
            }
        }
        private static void WindowStateValueChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            MdiChild mdiChild = (MdiChild)sender;
            MdiContainer mdiContainer = mdiChild.Container;
            WindowState previousWindowState = (WindowState)e.OldValue;
            WindowState windowState = (WindowState)e.NewValue;
            if (mdiChild.Container == null ||
                previousWindowState == windowState)
                return;
            if (previousWindowState == WindowState.Maximized)
            {
                if (mdiContainer.ActiveMdiChild.WindowState != WindowState.Maximized)
                {
                    for (int i = 0; i < mdiContainer.Children.Count; i++)
                    {
                        if (mdiContainer.Children[i] != mdiChild &&
                                mdiContainer.Children[i].WindowState == WindowState.Maximized &&
                                mdiContainer.Children[i].MaximizeBox)
                            mdiContainer.Children[i].WindowState = WindowState.Normal;
                    }
                    ScrollViewer sv = (ScrollViewer)((Grid)mdiContainer.Content).Children[1];
                    sv.HorizontalScrollBarVisibility = ScrollBarVisibility.Auto;
                    sv.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
                }
            }
            if (previousWindowState == WindowState.Minimized)
                mdiChild.minimizedPosition = mdiChild.Position;
            switch (windowState)
            {
                case WindowState.Normal:
                    {
                        mdiChild.Position = new Point(mdiChild.originalDimension.X, mdiChild.originalDimension.Y);
                        mdiChild.Width = mdiChild.originalDimension.Width;
                        mdiChild.Height = mdiChild.originalDimension.Height;
                    }
                    break;
                case WindowState.Minimized:
                    {
                        if (previousWindowState == WindowState.Normal)
                            mdiChild.originalDimension = new Rect(mdiChild.Position.X, mdiChild.Position.Y, mdiChild.ActualWidth, mdiChild.ActualHeight);

                        double newLeft, newTop;
                        if (mdiChild.minimizedPosition.X >= 0 || mdiChild.minimizedPosition.Y >= 0)
                        {
                            newLeft = mdiChild.minimizedPosition.X;
                            newTop = mdiChild.minimizedPosition.Y;
                        }
                        else
                        {
                            List<Rect> minimizedWindows = new List<Rect>();
                            for (int i = 0; i < mdiContainer.Children.Count; i++)
                            {
                                MdiChild child = mdiContainer.Children[i];
                                if (child != mdiChild &&
                                    child.WindowState == WindowState.Minimized)
                                    minimizedWindows.Add(new Rect(child.Position.X, mdiContainer.InnerHeight - child.Position.Y, child.Width, child.Height));
                            }
                            Rect newWindowPlace;
                            bool occupied = true;
                            int count = 0,
                                capacity = Convert.ToInt32(mdiContainer.ActualWidth) / MdiChild.MinimizedWidth;
                            do
                            {
                                int row = count / capacity + 1,
                                    col = count % capacity;
                                newTop = MdiChild.MinimizedHeight * row;
                                newLeft = MdiChild.MinimizedWidth * col;
                                newWindowPlace = new Rect(newLeft, newTop, MdiChild.MinimizedWidth, MdiChild.MinimizedHeight);
                                occupied = false;
                                foreach (Rect rect in minimizedWindows)
                                {
                                    Rect intersection = rect;
                                    intersection.Intersect(newWindowPlace);
                                    if (intersection != Rect.Empty && intersection.Width > 0 && intersection.Height > 0)
                                    {
                                        occupied = true;
                                        break;
                                    }
                                }
                                count++;

                                // TODO: handle negative Canvas coordinates somehow.
                                if (newTop < 0)
                                {
                                    // ugly workaround for now.
                                    newTop = 0;
                                    occupied = false;
                                }

                            } while (occupied);

                            newTop = mdiContainer.InnerHeight - newTop;
                        }

                        mdiChild.Position = new Point(newLeft, newTop);

                        mdiChild.Width = MdiChild.MinimizedWidth;
                        mdiChild.Height = MdiChild.MinimizedHeight;
                    }
                    break;
                case WindowState.Maximized:
                    {
                        if (previousWindowState == WindowState.Normal)
                            mdiChild.originalDimension = new Rect(mdiChild.Position.X, mdiChild.Position.Y, mdiChild.ActualWidth, mdiChild.ActualHeight);
                        mdiChild.NonMaximizedState = previousWindowState;

                        mdiChild.Position = new Point(0, 0);
                        mdiChild.Width = mdiContainer.ActualWidth;
                        mdiChild.Height = mdiContainer.InnerHeight - 2; // ContentBorder.BorderThickness="1" in template

                    }
                    break;
            }

            mdiContainer.InvalidateSize();
        }

        private static void WindowOutSideBoxValueChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {

            MdiChild mdiChild = (MdiChild)sender;
            bool open = (bool)e.NewValue;
            if (open)
            {

                CustomWindow t = new CustomWindow();
                t.Title = mdiChild.Title;
                t.Container = mdiChild.Container;
                t.Content = mdiChild.Content;
                t.WindowOutSideBox = true;
                t.Width = mdiChild.Width;
                t.Height = mdiChild.Height;
                t.Top = mdiChild.PointToScreen(mdiChild.Position).Y * Utility.GetDpiRatio() - mdiChild.Position.Y;
                t.Left = mdiChild.PointToScreen(mdiChild.Position).X * Utility.GetDpiRatio() - mdiChild.Position.X;
                t.Show();
                mdiChild.WindowChange();
            }
            else
            {
                mdiChild.WindowChange();
            }
        }
        public new void Focus()
        {
            Container.ActiveMdiChild = this;
        }
        internal void Unmaximize()
        {
            if (WindowState == WindowState.Maximized)
                WindowState = NonMaximizedState;
        }
        public void WindowChange()
        {
            if (WindowOutSideBox)
                Container.Children.Remove(this);
            else
                Container.Children.Add(this);
        }
        public void Close()
        {
            if (Buttons != null)
                Buttons.Children.Clear();
            Container.Children.Remove(this);

        }

        void IDropSurface.OnDragEnter(Point point)
        {

        }

        void IDropSurface.OnDragOver(Point point)
        {

        }

        void IDropSurface.OnDragLeave(Point point)
        {

        }

        bool IDropSurface.OnDrop(Point point)
        {
            return false;
        }
    }
}