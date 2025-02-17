using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows;
using wasty.ViewModels;
using wasty.Services;
using System.Windows.Controls;

namespace wasty
{
    public partial class App : Application
    {
        public IServiceProvider Services { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);
            Services = serviceCollection.BuildServiceProvider();

            // Obtener MainWindow correctamente desde el contenedor de servicios
            var mainWindow = Services.GetRequiredService<MainWindow>();
            mainWindow.Show();

            base.OnStartup(e);
        }

        private void ConfigureServices(IServiceCollection services)
        {
            // Registrar MainWindowViewModel
            services.AddSingleton<MainWindowViewModel>();

            // Definir la factoría para crear vistas dinámicamente
            Func<Type, UserControl> viewFactory = viewType => (UserControl)Activator.CreateInstance(viewType);

            // Registrar NavigationService después de MainWindowViewModel
            services.AddSingleton<NavigationService>(provider =>
            {
                var mainWindowViewModel = provider.GetRequiredService<MainWindowViewModel>();
                return new NavigationService(viewFactory, mainWindowViewModel);
            });

            //  Registrar MainWindow asegurando que reciba NavigationService
            services.AddSingleton<MainWindow>(provider =>
            {
                var navigationService = provider.GetRequiredService<NavigationService>();
                return new MainWindow(navigationService);
            });
        }

    }
}
