using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity;
using Model;
using QuoteService;
using System.ComponentModel;
using System.Windows.Data;
using Prism.Commands;
using System.Data;

namespace Flash.ViewModels
{
    public class FlashViewModel : BindableBase
    {
        public Flash.Views.FlashView View;
        IUnityContainer _container;
        QuoteDataList _TickData;
        QuoteServiceBase _quote;
        public DelegateCommand ExecuteLoadDelegateCommand { get; private set; }
        public QuoteDataList TickData
        {
            set => SetProperty(ref _TickData, value);
            get => _TickData;
        }
        private SymbolContract _symbolContract;
        public SymbolContract SymbolContract
        {
            get { return _symbolContract; }
            set
            {
                _symbolContract = value;


            }
        }
        public FlashViewModel(IRegionManager regionManager, IUnityContainer container, IEventAggregator eventAggregator)
        {
            ExecuteLoadDelegateCommand = new DelegateCommand(ExecuteLoad);
            _container = container;
            _quote = _container.Resolve<QuoteServiceBase>();
            _TickData = new QuoteDataList();


        }
        private void ExecuteLoad()
        {
            //View.dgPrice.Items.SortDescriptions.Add(new SortDescription("Price", ListSortDirection.Descending));
            //TickData data = _quote.Query(_symbolContract, "");
            //TickData.init(data);
            //_quote.Subscribe(_symbolContract);
          
        }
        public void MiddleView()
        {
            View.dgPrice.ScrollIntoView(TickData.GetLastTrade());
        }
    }
}
