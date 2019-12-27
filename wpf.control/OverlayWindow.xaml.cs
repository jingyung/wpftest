using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace wpf.control
{

    class OverlayWindowDockingButton : IDropSurface
    {
        OverlayWindow _owner;
        public readonly Button _btnDock;
        public OverlayWindowDockingButton(Button btnDock, OverlayWindow owner) : this(btnDock, owner, true)
        {

        }
        public OverlayWindowDockingButton(Button btnDock, OverlayWindow owner, bool enabled)
        {
            _btnDock = btnDock;
            _owner = owner;
            Enabled = enabled;
        }

        public bool Enabled = true;



        #region IDropSurface Membri di

        public Rect SurfaceRectangle
        {
            get
            {
                if (!_owner.IsLoaded)
                    return new Rect();

                return new Rect(_btnDock.PointToScreen(new Point(0, 0)), new Size(_btnDock.ActualWidth, _btnDock.ActualHeight));
            }
        }

        public MdiChild getMdiChild
        {
            get
            {
                return null;
            }
        }
        public void OnDragEnter(Point point)
        {

        }

        public void OnDragOver(Point point)
        {

        }

        public void OnDragLeave(Point point)
        {

        }

        public bool OnDrop(Point point)
        {
            return false;
        }

        #endregion
    }

    /// <summary>
    /// OverlayWindow.xaml 的互動邏輯
    /// </summary>
    public partial class OverlayWindow : Window 
    {
        public List<IDropSurface> Surfaces;
        public OverlayWindow(List<IDropSurface> pSurfaces)
        {
            InitializeComponent();
            Surfaces = pSurfaces;
            OverlayWindowDockingButton owdBottom = new OverlayWindowDockingButton(btnDockPaneTop, this);
            if (!Surfaces.Contains(owdBottom))
                Surfaces.Add(owdBottom);

              owdBottom = new OverlayWindowDockingButton(btnDockPaneRight, this);
            if (!Surfaces.Contains(owdBottom))
                Surfaces.Add(owdBottom);

              owdBottom = new OverlayWindowDockingButton(btnDockPaneBottom, this);
            if (!Surfaces.Contains(owdBottom))
                Surfaces.Add(owdBottom);

              owdBottom = new OverlayWindowDockingButton(btnDockPaneLeft, this);
            if (!Surfaces.Contains(owdBottom))
                Surfaces.Add(owdBottom);
        }
 
      

        public void ShowOverlayPaneDockingOptions(Rect rectPane)
        {
            //  Rect rectPane = pane.SurfaceRectangle;

            // Point myScreenTopLeft = PointToScreen(new Point(0, 0));
            //   rectPane.Offset(-myScreenTopLeft.X, -myScreenTopLeft.Y); 
            Canvas.SetLeft(gridPaneRelativeDockingOptions, this.Width/2- gridPaneRelativeDockingOptions.Width/2);
            Canvas.SetTop(gridPaneRelativeDockingOptions, this.Height / 2 - gridPaneRelativeDockingOptions.Height / 2);
            gridPaneRelativeDockingOptions.Visibility = Visibility.Visible;

        }

    }
}
