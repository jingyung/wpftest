using Model;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Flash.Views
{
    /// <summary>
    /// FlashOrderView.xaml 的互動邏輯
    /// </summary>
    public partial class FlashView : UserControl
    {
        Flash.ViewModels.FlashViewModel _ViewModel;

        ScrollViewer gridScrollViewer;

        public FlashView()
        {
            InitializeComponent();
            if (this.DataContext == null) return;
            _ViewModel = (Flash.ViewModels.FlashViewModel)this.DataContext;
            ((Flash.ViewModels.FlashViewModel)this.DataContext).View = this; ;
            //    gridScrollViewer = GetScrollViewer(this.dgPrice);
            


        }
        public static ScrollViewer GetScrollViewer(UIElement element)
        {
            if (element == null) return null;

            ScrollViewer retour = null;
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(element) && retour == null; i++)
            {
                if (VisualTreeHelper.GetChild(element, i) is ScrollViewer)
                {
                    retour = (ScrollViewer)(VisualTreeHelper.GetChild(element, i));
                }
                else
                {
                    retour = GetScrollViewer(VisualTreeHelper.GetChild(element, i) as UIElement);
                }
            }
            return retour;
        }

        private void dgPrice_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
              dgPrice.ScrollIntoView(_ViewModel.TickData.GetLastTrade());
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            decimal Max = _ViewModel.TickData.Max(x => x.Price);

            for (int i = 1; i < 20; i++)
            {
                QuoteData Quote = new QuoteData();
                Quote.Price = Max + i;

                _ViewModel.TickData.AddTick(Quote.Price, Quote);
               
            }
      
        }
    }
}
