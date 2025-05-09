using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using wasty.Models;
using wasty.Services;

namespace wasty.ViewModels
{
    internal class TrasladosViewModel : INotifyPropertyChanged
    {
        private readonly NavigationService _navigationService;
        private readonly ApiService _apiService;

        public TrasladosViewModel(NavigationService navigationService, ApiService apiService)
        {
            _navigationService = navigationService;
            _apiService = apiService;

            LoadMockData();
        }

        private void LoadMockData()
        {
            Traslados = new ObservableCollection<TrasladoModel>
            {
                new TrasladoModel
                {
                    Codigo = "TR001",
                    FechaInicio = "2025-05-01",
                    FechaFin = "2025-05-02",
                    Transportista = "EcoTrans",
                    Chofer = "Juan Pérez",
                    Matricula = "1234-XYZ",
                    StatusTrasla = "En curso"
                },
                new TrasladoModel
                {
                    Codigo = "TR002",
                    FechaInicio = "2025-04-27",
                    FechaFin = "2025-04-29",
                    Transportista = "GreenWay",
                    Chofer = "Ana García",
                    Matricula = "5678-ABC",
                    StatusTrasla = "Finalizado"
                },
                new TrasladoModel
                {
                    Codigo = "TR003",
                    FechaInicio = "2025-05-08",
                    FechaFin = "2025-05-10",
                    Transportista = "Residuos Express",
                    Chofer = "Carlos López",
                    Matricula = "9101-GHI",
                    StatusTrasla = "Pendiente"
                }
            };
        }

        private ObservableCollection<TrasladoModel> _traslados;
        public ObservableCollection<TrasladoModel> Traslados
        {
            get => _traslados;
            set
            {
                _traslados = value;
                OnPropertyChanged();
            }
        }

        private TrasladoModel _clienteSeleccionado;
        public TrasladoModel ClienteSeleccionado
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
