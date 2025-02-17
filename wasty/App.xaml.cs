// Lógica de interacción de la aplicación Wasty.
// Clase principal que hereda de Application, proporcionando la configuración básica para el ciclo de vida de la aplicación.
// Actualmente no contiene implementación adicional.

using System.Configuration;
using System.Data;
using System.Windows;
using System.Globalization;
using Microsoft.Extensions.DependencyInjection;
using wasty.Services;
using wasty.ViewModels;
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

            var mainWindow = Services.GetRequiredService<MainWindow>();
            mainWindow.Show();

            base.OnStartup(e);
        }

        private void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<NavigationService>(provider =>
            {
                var mainWindowViewModel = provider.GetRequiredService<MainWindowViewModel>();
                return new NavigationService(viewType => (UserControl)Activator.CreateInstance(viewType), mainWindowViewModel);
            });

            services.AddSingleton<MainWindowViewModel>();
            services.AddSingleton<SignupViewModel>();
            services.AddSingleton<LoginViewModel>();
            services.AddSingleton<MainViewModel>();
            services.AddHttpClient<ApiService>(client =>
            {
                client.BaseAddress = new Uri("http://localhost:5276/");
            });
        }

        //private void ConfigureServices(IServiceCollection services)
        //{
        //    // Definir la factoría para crear vistas dinámicamente
        //    Func<Type, UserControl> viewFactory = viewType => (UserControl)Activator.CreateInstance(viewType);

        //    // Registrar el ViewModel de la ventana principal
        //    services.AddSingleton<MainWindowViewModel>();

        //    // Registrar NavigationService después de que MainWindowViewModel esté registrado
        //    services.AddSingleton<NavigationService>(provider =>
        //    {
        //        var mainWindowViewModel = provider.GetRequiredService<MainWindowViewModel>();
        //        return new NavigationService(viewFactory, mainWindowViewModel);
        //    });

        //    // Registrar la MainWindow
        //    services.AddSingleton<MainWindow>(provider =>
        //    {
        //        var navigationService = provider.GetRequiredService<NavigationService>();
        //        return new MainWindow(navigationService);
        //    });
        //}

    }
}