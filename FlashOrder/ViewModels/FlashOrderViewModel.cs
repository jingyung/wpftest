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
namespace FlashOrder.ViewModels
{
    public class FlashOrderViewModel : BindableBase
    {
        TickDataList _TickData;
        public TickDataList TickData
        {
            set => SetProperty(ref _TickData, value);
            get => _TickData;
        }
        public FlashOrderViewModel(IRegionManager regionManager, IUnityContainer container, IEventAggregator eventAggregator)
        {
            
        }
    }
}
