using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;
using System.Collections.Generic;
using System.Windows.Input;
using System.Windows.Data;

namespace wpf.control
{
    [ContentProperty("Children")]
    public class MdiContainer : UserControl
    {
        const int WindowOffset = 25;
        private static ResourceDictionary currentResourceDictionary;
        public static readonly DependencyProperty ThemeProperty = DependencyProperty.Register("Theme", typeof(ThemeType), typeof(MdiContainer), new UIPropertyMetadata(ThemeType.Aero, new PropertyChangedCallback(ThemeValueChanged)));
        public static readonly DependencyProperty MenuProperty = DependencyProperty.Register("Menu", typeof(UIElement), typeof(MdiContainer), new UIPropertyMetadata(null, new PropertyChangedCallback(MenuValueChanged)));
        public static readonly DependencyProperty MdiLayoutProperty = DependencyProperty.Register("MdiLayout", typeof(MdiLayout), typeof(MdiContainer), new UIPropertyMetadata(MdiLayout.ArrangeIcons, new PropertyChangedCallback(MdiLayoutValueChanged)));
        public static readonly DependencyProperty ActiveMdiChildProperty = DependencyProperty.Register("ActiveMdiChild", typeof(MdiChild), typeof(MdiContainer), new UIPropertyMetadata(null, new PropertyChangedCallback(ActiveMdiChildValueChanged)));
        internal static readonly DependencyProperty ButtonsProperty = DependencyProperty.Register("Buttons", typeof(Panel), typeof(MdiContainer), new UIPropertyMetadata(null, new PropertyChangedCallback(ButtonsValueChanged)));
        public ThemeType Theme { get { return (ThemeType)GetValue(ThemeProperty); } set { SetValue(ThemeProperty, value); } }
        public UIElement Menu { get { return (UIElement)GetValue(MenuProperty); } set { SetValue(MenuProperty, value); } }
        public MdiLayout MdiLayout { get { return (MdiLayout)GetValue(MdiLayoutProperty); } set { SetValue(MdiLayoutProperty, value); } }
        public MdiChild ActiveMdiChild { get { return (MdiChild)GetValue(ActiveMdiChildProperty); } internal set { SetValue(ActiveMdiChildProperty, value); } }
        internal Panel Buttons { get { return (Panel)GetValue(ButtonsProperty); } set { SetValue(ButtonsProperty, value); } }
        internal double InnerHeight { get { return ActualHeight; } }
        public ObservableCollection<MdiChild> Children { get; set; }
        private Canvas _windowCanvas;
        private Border _menu;
        private Border _buttons;
        private Panel _topPanel;
        private double _windowOffset;
        public MdiContainer()
        {
            Background = Brushes.DarkGray;
            Focusable = IsTabStop = false;

            Children = new ObservableCollection<MdiChild>();
            Children.CollectionChanged += Children_CollectionChanged;

            Grid gr = new Grid();
          
            gr.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            gr.RowDefinitions.Add(new RowDefinition());

            _topPanel = new DockPanel { Background = SystemColors.MenuBrush };
            _topPanel.Children.Add(_menu = new Border());
            DockPanel.SetDock(_menu, Dock.Left);
             _topPanel.Children.Add(_buttons = new Border());
            DockPanel.SetDock(_buttons, Dock.Right);
            _topPanel.SizeChanged += MdiContainer_SizeChanged;
            _topPanel.Children.Add(new UIElement());
            gr.Children.Add(_topPanel);

            ScrollViewer sv = new ScrollViewer
            {
                Content = _windowCanvas = new Canvas(),
                HorizontalScrollBarVisibility = ScrollBarVisibility.Auto,
                VerticalScrollBarVisibility = ScrollBarVisibility.Auto
            };
             gr.Children.Add(sv);
            Grid.SetRow(sv, 1);
           Content = gr;

          //  if (Environment.OSVersion.Version.Major > 5)
            //  ThemeValueChanged(this, new DependencyPropertyChangedEventArgs(ThemeProperty, Theme, ThemeType.Aero));
            // else
            //   ThemeValueChanged(this, new DependencyPropertyChangedEventArgs(ThemeProperty, Theme, ThemeType.Luna));
            ThemeValueChanged(this, new DependencyPropertyChangedEventArgs(ThemeProperty, Theme, ThemeType.Generic));
            Loaded += MdiContainer_Loaded;
            SizeChanged += MdiContainer_SizeChanged;
            KeyDown += new System.Windows.Input.KeyEventHandler(MdiContainer_KeyDown);
        }
        static void MdiContainer_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            MdiContainer mdiContainer = (MdiContainer)sender;
            if (mdiContainer.Children.Count < 2)
                return;
            switch (e.Key)
            {
                case Key.Tab:
                    if (e.KeyboardDevice.Modifiers == ModifierKeys.Control)
                    {
                        int minZindex = Panel.GetZIndex(mdiContainer.Children[0]);
                        foreach (MdiChild mdiChild in mdiContainer.Children)
                            if (Panel.GetZIndex(mdiChild) < minZindex)
                                minZindex = Panel.GetZIndex(mdiChild);
                        Panel.SetZIndex(mdiContainer.GetTopChild(), minZindex - 1);
                        mdiContainer.GetTopChild().Focus();
                        e.Handled = true;
                    }
                    break;
            }
        }
        private void MdiContainer_Loaded(object sender, RoutedEventArgs e)
        {
            Window wnd = Window.GetWindow(this);
            if (wnd != null)
            {
                wnd.Activated += MdiContainer_Activated;
                wnd.Deactivated += MdiContainer_Deactivated;
            }

            _windowCanvas.Width = _windowCanvas.ActualWidth;
            _windowCanvas.Height = _windowCanvas.ActualHeight;

            _windowCanvas.VerticalAlignment = VerticalAlignment.Top;
            _windowCanvas.HorizontalAlignment = HorizontalAlignment.Left;

            InvalidateSize();
        }
        private void MdiContainer_Activated(object sender, EventArgs e)
        {
            if (ActiveMdiChild == null)
                return;
            ActiveMdiChild.Focused = true;
        }
        private void MdiContainer_Deactivated(object sender, EventArgs e)
        {
            if (Children.Count == 0)
                return;

            for (int i = 0; i < Children.Count; i++)
                if (Children[i].WindowState != WindowState.Maximized)
                    Children[i].Focused = false;
        }
        private void MdiContainer_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (Children.Count == 0)
                return;

            for (int i = 0; i < Children.Count; i++)
            {
                MdiChild mdiChild = Children[i];
                if (mdiChild.WindowState == WindowState.Maximized)
                {
                    mdiChild.Width = ActualWidth;
                    mdiChild.Height = ActualHeight;
                }
                if (mdiChild.WindowState == WindowState.Minimized)
                {
                    mdiChild.Position = new Point(mdiChild.Position.X, mdiChild.Position.Y + e.NewSize.Height - e.PreviousSize.Height);
                }
            }
        }
        private void Children_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    {
                        MdiChild mdiChild = Children[e.NewStartingIndex],
                            topChild = ActiveMdiChild;

                        if (topChild != null && topChild.WindowState == WindowState.Maximized)
                            mdiChild.Loaded += (s, a) => mdiChild.WindowState = WindowState.Maximized;
                        mdiChild.Loaded += (s, a) => ActiveMdiChild = mdiChild;

                        if (mdiChild.Position.X < 0 || mdiChild.Position.Y < 0)
                            mdiChild.Position = new Point(_windowOffset, _windowOffset);
                        _windowCanvas.Children.Add(mdiChild);

                        _windowOffset += WindowOffset;
                        if (_windowOffset + mdiChild.Width > ActualWidth)
                            _windowOffset = 0;
                        if (_windowOffset + mdiChild.Height > ActualHeight)
                            _windowOffset = 0;
                    }
                    break;
                case NotifyCollectionChangedAction.Remove:
                    {
                        MdiChild oldChild = (MdiChild)e.OldItems[0];
                        _windowCanvas.Children.Remove(oldChild);
                        MdiChild newChild = GetTopChild();

                        ActiveMdiChild = newChild;
                        if (newChild != null && oldChild.WindowState == WindowState.Maximized)
                            newChild.WindowState = WindowState.Maximized;
                    }
                    break;
                case NotifyCollectionChangedAction.Reset:
                    _windowCanvas.Children.Clear();
                    break;
            }
            InvalidateSize();
        }
        internal void InvalidateSize()
        {
            Point largestPoint = new Point(0, 0);

            for (int i = 0; i < Children.Count; i++)
            {
                MdiChild mdiChild = Children[i];

                Point farPosition = new Point(mdiChild.Position.X + mdiChild.Width, mdiChild.Position.Y + mdiChild.Height);

                if (farPosition.X > largestPoint.X)
                    largestPoint.X = farPosition.X;

                if (farPosition.Y > largestPoint.Y)
                    largestPoint.Y = farPosition.Y;
            }

            if (_windowCanvas.Width != largestPoint.X)
                _windowCanvas.Width = largestPoint.X;

            if (_windowCanvas.Height != largestPoint.Y)
                _windowCanvas.Height = largestPoint.Y;
        }
        internal MdiChild GetTopChild()
        {
            if (Children.Count < 1)
                return null;

            int index = 0, maxZindex = Panel.GetZIndex(Children[0]);
            for (int i = 1, zindex; i < Children.Count; i++)
            {
                zindex = Panel.GetZIndex(Children[i]);
                if (zindex > maxZindex)
                {
                    maxZindex = zindex;
                    index = i;
                }
            }
            return Children[index];
        }
        private static void ThemeValueChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            MdiContainer mdiContainer = (MdiContainer)sender;
            ThemeType themeType = (ThemeType)e.NewValue;

            bool max_mode = mdiContainer.ActiveMdiChild != null && mdiContainer.ActiveMdiChild.WindowState == WindowState.Maximized;
            if (max_mode)
                mdiContainer.ActiveMdiChild.WindowState = WindowState.Normal;

            if (currentResourceDictionary != null)
                Application.Current.Resources.MergedDictionaries.Remove(currentResourceDictionary);

            switch (themeType)
            {
                case ThemeType.Luna:
                    Application.Current.Resources.MergedDictionaries.Add(currentResourceDictionary = new ResourceDictionary { Source = new Uri(@"/wpf.control;component/Themes/Luna.xaml", UriKind.Relative) });
                    break;
                case ThemeType.Aero:
                    Application.Current.Resources.MergedDictionaries.Add(currentResourceDictionary = new ResourceDictionary { Source = new Uri(@"/wpf.control;component/Themes/Aero.xaml", UriKind.Relative) });
                    break;
            }

            //if (max_mode)
            //    mdiContainer.ActiveMdiChild.WindowState = WindowState.Maximized;
        }
        private static void MenuValueChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            MdiContainer mdiContainer = (MdiContainer)sender;
            UIElement menu = (UIElement)e.NewValue;

          //  mdiContainer._menu.Child = menu;
        }
        private static void MdiLayoutValueChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            MdiContainer mdiContainer = (MdiContainer)sender;
            MdiLayout value = (MdiLayout)e.NewValue;

            if (value == MdiLayout.ArrangeIcons ||
                mdiContainer.Children.Count < 1)
                return;

            // 1. WindowState.Maximized -> WindowState.Normal
            List<MdiChild> minimizedWindows = new List<MdiChild>(),
                normalWindows = new List<MdiChild>();
            foreach (MdiChild mdiChild in mdiContainer.Children)
                switch (mdiChild.WindowState)
                {
                    case WindowState.Minimized:
                        minimizedWindows.Add(mdiChild);
                        break;
                    case WindowState.Maximized:
                        mdiChild.WindowState = WindowState.Normal;
                        normalWindows.Add(mdiChild);
                        break;
                    default:
                        normalWindows.Add(mdiChild);
                        break;
                }

            minimizedWindows.Sort(new MdiChildComparer());
            normalWindows.Sort(new MdiChildComparer());

            // 2. Arrange minimized windows
            double containerHeight = mdiContainer.InnerHeight;
            for (int i = 0; i < minimizedWindows.Count; i++)
            {
                MdiChild mdiChild = minimizedWindows[i];
                int capacity = Convert.ToInt32(mdiContainer.ActualWidth) / MdiChild.MinimizedWidth,
                    row = i / capacity + 1,
                    col = i % capacity;
                containerHeight = mdiContainer.InnerHeight - MdiChild.MinimizedHeight * row;
                double newLeft = MdiChild.MinimizedWidth * col;
                mdiChild.Position = new Point(newLeft, containerHeight);
            }

            // 3. Resize & arrange normal windows
            switch (value)
            {
                case MdiLayout.Cascade:
                    {
                        double newWidth = mdiContainer.ActualWidth * 0.58, // should be non-linear formula here
                            newHeight = containerHeight * 0.67,
                            windowOffset = 0;
                        foreach (MdiChild mdiChild in normalWindows)
                        {
                            if (mdiChild.Resizable)
                            {
                                mdiChild.Width = newWidth;
                                mdiChild.Height = newHeight;
                            }
                            mdiChild.Position = new Point(windowOffset, windowOffset);

                            windowOffset += WindowOffset;
                            if (windowOffset + mdiChild.Width > mdiContainer.ActualWidth)
                                windowOffset = 0;
                            if (windowOffset + mdiChild.Height > containerHeight)
                                windowOffset = 0;
                        }
                    }
                    break;
                case MdiLayout.TileHorizontal:
                    {
                        int cols = (int)Math.Sqrt(normalWindows.Count),
                            rows = normalWindows.Count / cols;

                        List<int> col_count = new List<int>(); // windows per column
                        for (int i = 0; i < cols; i++)
                        {
                            if (normalWindows.Count % cols > cols - i - 1)
                                col_count.Add(rows + 1);
                            else
                                col_count.Add(rows);
                        }

                        double newWidth = mdiContainer.ActualWidth / cols,
                            newHeight = containerHeight / col_count[0],
                            offsetTop = 0,
                            offsetLeft = 0;

                        for (int i = 0, col_index = 0, prev_count = 0; i < normalWindows.Count; i++)
                        {
                            if (i >= prev_count + col_count[col_index])
                            {
                                prev_count += col_count[col_index++];
                                offsetLeft += newWidth;
                                offsetTop = 0;
                                newHeight = containerHeight / col_count[col_index];
                            }

                            MdiChild mdiChild = normalWindows[i];
                            if (mdiChild.Resizable)
                            {
                                mdiChild.Width = newWidth;
                                mdiChild.Height = newHeight;
                            }
                            mdiChild.Position = new Point(offsetLeft, offsetTop);
                            offsetTop += newHeight;
                        }
                    }
                    break;
                case MdiLayout.TileVertical:
                    {
                        int rows = (int)Math.Sqrt(normalWindows.Count),
                            cols = normalWindows.Count / rows;

                        List<int> col_count = new List<int>(); // windows per column
                        for (int i = 0; i < cols; i++)
                        {
                            if (normalWindows.Count % cols > cols - i - 1)
                                col_count.Add(rows + 1);
                            else
                                col_count.Add(rows);
                        }

                        double newWidth = mdiContainer.ActualWidth / cols,
                            newHeight = containerHeight / col_count[0],
                            offsetTop = 0,
                            offsetLeft = 0;

                        for (int i = 0, col_index = 0, prev_count = 0; i < normalWindows.Count; i++)
                        {
                            if (i >= prev_count + col_count[col_index])
                            {
                                prev_count += col_count[col_index++];
                                offsetLeft += newWidth;
                                offsetTop = 0;
                                newHeight = containerHeight / col_count[col_index];
                            }

                            MdiChild mdiChild = normalWindows[i];
                            if (mdiChild.Resizable)
                            {
                                mdiChild.Width = newWidth;
                                mdiChild.Height = newHeight;
                            }
                            mdiChild.Position = new Point(offsetLeft, offsetTop);
                            offsetTop += newHeight;
                        }
                    }
                    break;
            }
            mdiContainer.InvalidateSize();
            mdiContainer.MdiLayout = MdiLayout.ArrangeIcons;
        }
        private static void ActiveMdiChildValueChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            MdiContainer mdiContainer = (MdiContainer)sender;
            MdiChild newChild = (MdiChild)e.NewValue,
                oldChild = (MdiChild)e.OldValue;

            if (newChild == null || newChild == oldChild)
                return;

            if (oldChild != null && oldChild.WindowState == WindowState.Maximized)
                newChild.WindowState = WindowState.Maximized;

            int maxZindex = 0;
            for (int i = 0; i < mdiContainer.Children.Count; i++)
            {
                int zindex = Panel.GetZIndex(mdiContainer.Children[i]);
                if (zindex > maxZindex)
                    maxZindex = zindex;
                if (mdiContainer.Children[i] != newChild)
                    mdiContainer.Children[i].Focused = false;
                else
                    newChild.Focused = true;
            }

            Panel.SetZIndex(newChild, maxZindex + 1);

            if (mdiContainer.MdiChildTitleChanged != null)
                mdiContainer.MdiChildTitleChanged(mdiContainer, new RoutedEventArgs());
        }
        private static void ButtonsValueChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            MdiContainer mdiContainer = (MdiContainer)sender;
            Panel panel = (Panel)e.NewValue;

            mdiContainer._buttons.Child = panel;

            if (mdiContainer.MdiChildTitleChanged != null)
                mdiContainer.MdiChildTitleChanged(mdiContainer, new RoutedEventArgs());
        }

        internal class MdiChildComparer : IComparer<MdiChild>
        {
            #region IComparer<MdiChild> Members

            public int Compare(MdiChild x, MdiChild y)
            {
                return -1 * Canvas.GetZIndex(x).CompareTo(Canvas.GetZIndex(y));
            }

            #endregion
        }
        public event RoutedEventHandler MdiChildTitleChanged;
    }
}