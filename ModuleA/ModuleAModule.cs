using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Mvvm;
using Prism.Regions;
using Unity;

namespace ModuleA
{
    public class ModuleAModule : IModule
    {
        public ModuleAModule(IRegionManager regionManager, IUnityContainer container)
        {

        }
        public void OnInitialized(IContainerProvider containerProvider)
        {
            var regionManager = containerProvider.Resolve<IRegionManager>();
            regionManager.RegisterViewWithRegion("ContentRegion", typeof(ModuleA.Views.ViewC));
    

        }
 

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {

        }
    }
}
