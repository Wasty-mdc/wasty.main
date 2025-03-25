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
using wasty.Helpers;
using wasty.Models;
using Wasty.ViewModels;


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

            base.OnStartup(e);
        }

        private void ConfigureServices(IServiceCollection services)
        {
            // Definir la factoría para crear vistas dinámicamente
            Func<Type, UserControl> viewFactory = viewType => (UserControl)Activator.CreateInstance(viewType);

            services.AddSingleton<IsolatedStorageHelper>();

            services.AddHttpClient<ApiService>(client =>
            {
                client.BaseAddress = new Uri("http://localhost:5276/");
            });
            services.AddTransient<NavigationService>(provider =>
                new NavigationService(viewType => (object)Activator.CreateInstance(viewType))
            );
            services.AddSingleton<AuthService>();
            services.AddSingleton<SessionService>();

            services.AddSingleton<AuthModel>();

            services.AddSingleton<MainWindowViewModel>();
            services.AddTransient<SignupViewModel>();
            services.AddTransient<LoginViewModel>();
            services.AddTransient<MainViewModel>();
            services.AddTransient<StatisticsViewModel>();
            services.AddTransient<StatisticsPanelViewModel>();
            services.AddTransient<ClientViewModel>();
            services.AddTransient<ClientPanelViewModel>();
            services.AddTransient<ResiduosViewModel>();
            services.AddTransient<ResiduosPanelViewModel>();

            services.AddHttpClient<ApiService>(client =>
            {
                client.BaseAddress = new Uri("http://localhost:5276/");
            });
        }
    }
}