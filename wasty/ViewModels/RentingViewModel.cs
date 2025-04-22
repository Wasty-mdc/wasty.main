using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using wasty.Models;
using wasty.Services;

namespace wasty.ViewModels
{
    internal class RentingViewModel : INotifyPropertyChanged
    {
        private readonly NavigationService _navigationService;
        private readonly ApiService _apiService;

        public RentingViewModel(NavigationService navigationService, ApiService apiService)
        {
            _navigationService = navigationService;
            _apiService = apiService;

            LoadMockData();
        }

        private void LoadMockData()
        {
            Rentings = new ObservableCollection<Renting>
            {
                new Renting
                {
                    Codigo = "RNT-001",
                    NombreCliente = "Cliente A",
                    NIFCliente = "12345678A",
                    TipoMaquina = "Excavadora",
                    FechaAcuerdo = "2025-04-01",
                    InicioAcuerdo = "2025-04-05",
                    FinalAcuerdo = "2025-10-05"
                },
                new Renting
                {
                    Codigo = "RNT-002",
                    NombreCliente = "Cliente B",
                    NIFCliente = "87654321B",
                    TipoMaquina = "Carretilla",
                    FechaAcuerdo = "2025-03-15",
                    InicioAcuerdo = "2025-04-01",
                    FinalAcuerdo = "2025-09-30"
                }
            };
        }

        private ObservableCollection<Renting> _rentings;
        public ObservableCollection<Renting> Rentings
        {
            get => _rentings;
            set
            {
                _rentings = value;
                OnPropertyChanged();
            }
        }

        private Renting _rentingSeleccionado;
        public Renting RentingSeleccionado
        {
            get => _rentingSeleccionado;
            set
            {
                _rentingSeleccionado = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
