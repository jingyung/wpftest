﻿using Model;
using Prism.Commands;
using Prism.Events;
using Prism.Regions;
using QuoteService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity;
using wpf.control;

namespace Wpftest.ViewModels
{
    public class MainWindowViewModel
    {
        public Dictionary<string, Flash.ViewModels.FlashViewModel> FlashViewModelCollection = new Dictionary<string, Flash.ViewModels.FlashViewModel>();
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
            _quote = _container.Resolve<QuoteServiceBase>();
            _quote.Connected += Quote_Connected;
            _quote.Disconnected += Quote_Disconnected;
            _quote.Connect("172.16.204.217", 7113);
            _quote.TickDataBid += _quote_TickDataBid;
            _quote.TickDataOffer += _quote_TickDataOffer;
            _quote.TickDataTrade += _quote_TickDataTrade;
            _quote.TickDataHighLow += _quote_TickDataHighLow;
            Region r = new Region();
            r.Name = "test";
            _regionManager.Regions.Add(r);
        }


        private void Quote_Connected()
        {
            _quote.Subscribe(new SymbolContract("HKF", "HSI", "201908", CPEnum.Future, ""));
        }
        private void Quote_Disconnected()
        {

        }
        private void ExecuteLoad()
        {

        }
        private void ExecuteClick(object obj)
        {
            FlashOrder.Views.FlashOrderView view = new FlashOrder.Views.FlashOrderView();
            ((FlashOrder.ViewModels.FlashOrderViewModel)view.DataContext).SymbolContract = new SymbolContract("HKF", "HSI", "201908", CPEnum.Future, "");
            _regionManager.Regions["test"].Add(view, "FlashOrder" + _regionManager.Regions["test"].Views.Count().ToString(), true);
            FlashViewModelCollection.Add(Guid.NewGuid().ToString(), (Flash.ViewModels.FlashViewModel)((Flash.Views.FlashView)view.FlashView.Content).DataContext);
            MdiContainer.Children.Add(new MdiChild { Content = view });
 
        }


        private void _quote_TickDataHighLow(TickDataHighLow val)
        {

        }

        private void _quote_TickDataTrade(TickDataTrade val)
        {
            Dictionary<string, Flash.ViewModels.FlashViewModel>.Enumerator e = FlashViewModelCollection.GetEnumerator();
            while (e.MoveNext())
            {
                Flash.ViewModels.FlashViewModel item = e.Current.Value;
                item.TickData.UpdateLastTrade(val);
                item.MiddleView();
            }

        }

        private void _quote_TickDataOffer(TickDataOffer val)
        {
            Dictionary<string, Flash.ViewModels.FlashViewModel>.Enumerator e = FlashViewModelCollection.GetEnumerator();
            while (e.MoveNext())
            {
                Flash.ViewModels.FlashViewModel item = e.Current.Value;
                item.TickData.UpdateOffer(val);
            }
        }

        private void _quote_TickDataBid(TickDataBid val)
        {
            Dictionary<string, Flash.ViewModels.FlashViewModel>.Enumerator e = FlashViewModelCollection.GetEnumerator();
            while (e.MoveNext())
            {
                Flash.ViewModels.FlashViewModel item = e.Current.Value;
                item.TickData.UpdateBid(val);
            }
        }


    }
}
