using Prism.Commands;
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
using FlashOrder.Views;
using Flash.ViewModels;

namespace FlashOrder.ViewModels
{
    public class FlashOrderViewModel : BindableBase
    {
        public FlashOrderView View;
        IUnityContainer _container;
        private SymbolContract _symbolContract;
        public SymbolContract SymbolContract
        {

            get { return _symbolContract; }
            set
            {
                _symbolContract = value;

            }
        }

        public DelegateCommand ExecuteLoadDelegateCommand { get; private set; }
        public FlashOrderViewModel()
        {
            ExecuteLoadDelegateCommand = new DelegateCommand(ExecuteLoad);
        }
        public FlashOrderViewModel(IRegionManager regionManager, IUnityContainer container, IEventAggregator eventAggregator) : this()
        {
            _container = container;

        }
        private void ExecuteLoad()
        {
            //   ((FlashViewModel)View.FlashView.DataContext).SymbolContract = _symbolContract;
            ((FlashViewModel)((Flash.Views.FlashView)View.FlashView.Content).DataContext).SymbolContract = _symbolContract;
        }

    }
}
