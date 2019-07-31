using Unity;
using Prism.Unity;

using System.Windows;
using Prism.Modularity;
using Prism.Mvvm;
using QuoteService;
using Unity.Lifetime;

namespace BootstrapperShell
{
    class Bootstrapper : UnityBootstrapper
    {
        protected override DependencyObject CreateShell()
        {
            return Container.Resolve<Wpftest.Views.MainWindow>();
        }
        protected override void ConfigureContainer()
        {
            base.ConfigureContainer();
      this.Container .RegisterType<QuoteServiceBase, QuoteService.QuoteService>(new ContainerControlledLifetimeManager());
        }

        protected override void InitializeShell()
        {
            Application.Current.MainWindow.Show();
        }
        protected override void ConfigureModuleCatalog()
        {
          
            this.ModuleCatalog.AddModule<ModuleA.ModuleAModule>();
            this.ModuleCatalog.AddModule<Flash.FlashModule>();
            this.ModuleCatalog.AddModule<FlashOrder.FlashOrderModule >();
            ViewModelLocationProvider.Register<ModuleA.Views.ViewC, ModuleA.ViewModels.ViewCViewMode9l>();
         // this.ModuleCatalog.AddModule<QuoteService.QuoteServiceModule>();


        }
    }
}