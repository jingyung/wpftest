using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using wpf.control;
using Prism.Unity;
using Prism.Regions;
using Unity;
using Prism.Events;
using Model;
using QuoteService;
namespace Wpftest.Views
{
    /// <summary>
    /// MainWindow.xaml 的互動邏輯
    /// </summary>
    public partial class MainWindow : Window
    {
 
        public MainWindow()
        {
            InitializeComponent();
            ((Wpftest.ViewModels.MainWindowViewModel)this.DataContext).MdiContainer = MdiContainer;
        }
 
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
     
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        { 
            this.MdiContainer.MdiLayout = MdiLayout.TileVertical;

        }

  
    }
}
