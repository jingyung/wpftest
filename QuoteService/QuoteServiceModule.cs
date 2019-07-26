using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using Unity;
using Unity.Lifetime;

namespace QuoteService
{
    public class QuoteServiceModule : IModule
    {
        IUnityContainer _container;
        public QuoteServiceModule(IRegionManager regionManager, IUnityContainer container)
        {
            _container = container;
        }

        public void OnInitialized(IContainerProvider containerProvider)
        {
            _container.RegisterType<QuoteServiceBase, QuoteService>( new ContainerControlledLifetimeManager());
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
           
        }
    }
}
