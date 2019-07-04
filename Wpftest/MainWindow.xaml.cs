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
namespace Wpftest
{
    /// <summary>
    /// MainWindow.xaml 的互動邏輯
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
 
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            Container.Children.Add(new MdiChild { Content = new Label { Content = "Normal window" }, Title = "Window " });
        }


     


    }
}
