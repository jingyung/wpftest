using Unity;
using Prism.Unity;

using System.Windows;
using Prism.Modularity;
using Prism.Mvvm;

namespace BootstrapperShell
{
    class Bootstrapper : UnityBootstrapper
    {
        protected override DependencyObject CreateShell()
        {
            return Container.Resolve<Wpftest.PrismWindow>();
        }

        protected override void InitializeShell()
        {
            Application.Current.MainWindow.Show();
        }
        protected override void ConfigureModuleCatalog()
        {
            base.ConfigureModuleCatalog();
            this.ModuleCatalog.AddModule<ModuleA.ModuleAModule>();
            ViewModelLocationProvider.Register<ModuleA.Views.ViewC, ModuleA.ViewModels.ViewCViewMode9l>();


        }
    }
}