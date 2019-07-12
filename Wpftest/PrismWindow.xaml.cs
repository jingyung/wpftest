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
using Prism;
using Prism.Ioc;
using Prism.Regions;
using Wpftest.Views;

namespace Wpftest
{
    /// <summary>
    /// PrismWindow.xaml 的互動邏輯
    /// </summary>
    public partial class PrismWindow : Window
    {
        IContainerExtension _container;
        IRegionManager _regionManager;
        IRegion _region;
        ViewA _viewA;
        ViewB _viewB;
        public PrismWindow(IContainerExtension container, IRegionManager regionManager)
        {
            InitializeComponent();
            _container = container;
            _regionManager = regionManager;
            
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            _region.Activate(_viewA);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            //deactivate view a
            _region.Deactivate(_viewA);
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            //activate view b
            _region.Activate(_viewB);
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            //deactivate view b
            _region.Deactivate(_viewB);
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            _viewA = _container.Resolve<ViewA>();
            _viewB = _container.Resolve<ViewB>();
            _region = _regionManager.Regions["ContentRegion"];
            _region.Add(_viewA);
            _region.Add(_viewB);
  

        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            _region = _regionManager.Regions["ContentRegion"];
            _region.Remove(_viewA);
        }
    }
}
