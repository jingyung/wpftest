using Model;
using Prism.Commands;
using Prism.Events;
using Prism.Regions;
using QuoteService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Unity;
using wpf.control;

namespace Wpftest.ViewModels
{
    public class MainWindowViewModel
    {
        private KeyboardListener listener;
        public MdiContainer MdiContainer;
        IUnityContainer _container;
        QuoteServiceBase _quote;
        IRegionManager _regionManager;

        public DelegateCommand ExecuteLoadDelegateCommand { get; private set; }
        public DelegateCommand<object> ExecuteClickDelegateCommand { get; private set; }
        public MainWindowViewModel()
        {
            ExecuteLoadDelegateCommand = new DelegateCommand(ExecuteLoad);
            ExecuteClickDelegateCommand = new DelegateCommand<object>(ExecuteClick);
        }

        public MainWindowViewModel(IRegionManager regionManager, IUnityContainer container, IEventAggregator eventAggregator) : this()
        {
            _regionManager = regionManager;
            _container = container;

            Region r = new Region();
            r.Name = "test";
            _regionManager.Regions.Add(r);
        
        }

 
        private void Quote_Connected()
        {
            //_quote.Subscribe(new SymbolContract("HKF", "HSI", "201912", CPEnum.Future, ""));
        }
        private void Quote_Disconnected()
        {

        }
        private void ExecuteLoad()
        {
            _quote = _container.Resolve<QuoteServiceBase>();

            _quote.Connected += Quote_Connected;
            _quote.Disconnected += Quote_Disconnected;
            _quote.Connect("172.16.204.217", 7113);
            listener = new KeyboardListener();
            listener.KeyboardDownEvent += ListenerOnKeyPressed;




        }
        private void ListenerOnKeyPressed(object sender, KeyEventArgs e)
        {

        }
        private void ExecuteClick(object obj)
        {
            //for (int i = 0; i < 1; i++)
            //{
            //    FlashOrder.Views.FlashOrderView view = new FlashOrder.Views.FlashOrderView();
            //    ((FlashOrder.ViewModels.FlashOrderViewModel)view.DataContext).SymbolContract = new SymbolContract("HKF", "HSI", "201912", CPEnum.Future, "");
            //    MdiContainer.AddMDIChild(view);
            //}
            MdiContainer.AddMDIChild(new System.Windows.Controls.UserControl());

        }
    }
}
