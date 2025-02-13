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

            base.OnStartup(e);
        }

        private void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<NavigationService>(provider =>
            new NavigationService(viewType => (UserControl)Activator.CreateInstance(viewType)));

            services.AddSingleton<SignupViewModel>();
            services.AddSingleton<LoginViewModel>();
            services.AddHttpClient<ApiService>(client =>
            {
                client.BaseAddress = new Uri("http://localhost:5276/");
            });
        }
    }
}