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
using Unity;

namespace Wpftest.ViewModels
{
    public class MainWindowViewModel
    {
        IUnityContainer _container;
        QuoteServiceBase _quote;

        public DelegateCommand ExecuteLoadDelegateCommand { get; private set; }
        public MainWindowViewModel()
        {
            ExecuteLoadDelegateCommand = new DelegateCommand(ExecuteLoad);
        }
        private void ExecuteLoad()
        {
             
        }
        public MainWindowViewModel(IRegionManager regionManager, IUnityContainer container, IEventAggregator eventAggregator) : this()
        {
            _container = container;
            _quote = _container.Resolve<QuoteServiceBase>();
            _quote.Connected += Quote_Connected;
            _quote.Disconnected += Quote_Disconnected;
            _quote.Connect("172.16.204.217", 7113);


        }
        private void Quote_Connected()
        {
            _quote.Subscribe(new SymbolContract("HKF", "HSI", "201907", CPEnum.Future, ""));
        }
        private void Quote_Disconnected()
        {

        }
 
    }
}
