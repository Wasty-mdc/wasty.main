using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using wasty.Models;
using wasty.Services;

namespace wasty.ViewModels
{
    internal class TreatmentsViewModel : INotifyPropertyChanged
    {
        private readonly NavigationService _navigationService;
        private readonly ApiService _apiService;

        public TreatmentsViewModel(NavigationService navigationService, ApiService apiService)
        {
            _navigationService = navigationService;
            _apiService = apiService;

            LoadMockData();
        }

        private void LoadMockData()
        {
            Tratamientos = new ObservableCollection<Treatment>
            {
                new Treatment
                {
                    Codigo = "TRT-001",
                    Fecha = "2025-04-01",
                    Denominacion = "Tratamiento Químico A",
                    LER = "20 01 13",
                    OrigenDestino = "Planta Norte",
                    OperadorTraslados = "EcoTrans S.L.",
                    Activo = "Sí",
                    Notificar = "Notificar"
                },
                new Treatment
                {
                    Codigo = "TRT-002",
                    Fecha = "2025-04-05",
                    Denominacion = "Tratamiento Biológico B",
                    LER = "19 02 03",
                    OrigenDestino = "Centro Sur",
                    OperadorTraslados = "GreenWay Logística",
                    Activo = "No",
                    Notificar = "Notificar"
                },
                new Treatment
                {
                    Codigo = "TRT-003",
                    Fecha = "2025-04-10",
                    Denominacion = "Separación Mecánica",
                    LER = "19 12 10",
                    OrigenDestino = "Depósito Este",
                    OperadorTraslados = "Residuos Express",
                    Activo = "Sí",
                    Notificar = "Notificar"
                }
            };
        }

        private ObservableCollection<Treatment> _tratamientos;
        public ObservableCollection<Treatment> Tratamientos
        {
            get => _tratamientos;
            set
            {
                _tratamientos = value;
                OnPropertyChanged();
            }
        }

        private Treatment _clienteSeleccionado;
        public Treatment ClienteSeleccionado
        {
            get => _clienteSeleccionado;
            set
            {
                _clienteSeleccionado = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
