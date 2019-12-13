using Unity;
using Prism.Unity;

using System.Windows;
using Prism.Modularity;
using Prism.Mvvm;
using QuoteService;
using Unity.Lifetime;
using Prism.Regions;
using wpf.control;
using CommonServiceLocator;

namespace BootstrapperShell
{
    class Bootstrapper : UnityBootstrapper
    {
        protected override DependencyObject CreateShell()
        {
          return ServiceLocator.Current.GetInstance<Wpftest.Views.MainWindow>();
            //return Container.Resolve<Wpftest.Views.MainWindow>();
        }
        protected override void ConfigureContainer()
        {
            base.ConfigureContainer();
           

        }

        protected override void InitializeShell()
        {
            this.Container.RegisterType<QuoteServiceBase, QuoteService.QuoteService>(new ContainerControlledLifetimeManager());
            Application.Current.MainWindow.Show();
        }
        protected override void ConfigureModuleCatalog()
        {

            this.ModuleCatalog.AddModule<ModuleA.ModuleAModule>();
             this.ModuleCatalog.AddModule<Flash.FlashModule>();
             this.ModuleCatalog.AddModule<FlashOrder.FlashOrderModule>();
            ViewModelLocationProvider.Register<ModuleA.Views.ViewC, ModuleA.ViewModels.ViewCViewMode9l>();
          
        }
        protected override RegionAdapterMappings ConfigureRegionAdapterMappings()
        {
            var map = base.ConfigureRegionAdapterMappings();
            map.RegisterMapping(typeof(MdiContainer), Container.Resolve<MdiRegionAdapter>());
            return map;
        }

    }
}