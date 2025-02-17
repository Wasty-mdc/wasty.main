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
            base.OnStartup(e);

            try
            {
                var serviceCollection = new ServiceCollection();
                ConfigureServices(serviceCollection);
                Services = serviceCollection.BuildServiceProvider();

                // Verificar que MainWindow se obtenga correctamente
                var mainWindow = Services.GetRequiredService<MainWindow>();
                if (mainWindow == null)
                {
                    throw new Exception("No se pudo obtener una instancia de MainWindow desde el contenedor de dependencias.");
                }

                mainWindow.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al iniciar la aplicación: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Current.Shutdown();
            }
        }

        private void ConfigureServices(IServiceCollection services)
        {
            // Definir la factoría para crear vistas dinámicamente
            Func<Type, UserControl> viewFactory = viewType => (UserControl)Activator.CreateInstance(viewType);

            // Registrar MainWindowViewModel
            services.AddSingleton<MainWindowViewModel>();

            // Registrar NavigationService después de MainWindowViewModel
            services.AddSingleton<NavigationService>(provider =>
            {
                var mainWindowViewModel = provider.GetRequiredService<MainWindowViewModel>();
                return new NavigationService(viewFactory, mainWindowViewModel);
            });

            // Registrar correctamente MainWindow
            services.AddSingleton<MainWindow>(provider =>
            {
                var navigationService = provider.GetRequiredService<NavigationService>();
                return new MainWindow(navigationService);
            });
        }
    }
}
