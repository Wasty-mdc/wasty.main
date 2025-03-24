using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using wasty.Models;
using wasty.Services;
using wasty.Views;

namespace wasty.ViewModels
{
    public class ResiduosViewModel : INotifyPropertyChanged
    {
        private readonly ApiService _apiService;
        private readonly NavigationService _navigationService;

        public ObservableCollection<Residuo> Residuos { get; set; }

        private Residuo _residuoSeleccionado;
        public Residuo ResiduoSeleccionado
        {
            get => _residuoSeleccionado;
            set
            {
                _residuoSeleccionado = value;
                OnPropertyChanged(nameof(ResiduoSeleccionado));
            }
        }

        public ICommand AbrirDetalleResiduoCommand { get; }
        public ICommand VolverAlMenuCommand { get; }

        public ResiduosViewModel(ApiService apiService, NavigationService navigationService)
        {
            _apiService = apiService;
            _navigationService = navigationService;

            // Simulación de datos (puedes sustituirlo con llamadas reales al API)
            Residuos = new ObservableCollection<Residuo>
            {
                new Residuo { Codigo = 1, Denominacion = "Aceite usado*", LER = "130205", Fecha = DateTime.Now, Origen = "MANUEL DIAZ CHILET", Destino = "METALLS DEL CAMP", OperadorTraslado = "ECO TITAN S.L.", Activo = true },
                new Residuo { Codigo = 2, Denominacion = "Baterías de plomo*", LER = "160601", Fecha = DateTime.Now, Origen = "MANUEL DIAZ CHILET", Destino = "METALLS DEL CAMP", OperadorTraslado = "ECO TITAN S.L.", Activo = true }
            };

            AbrirDetalleResiduoCommand = new RelayCommand(_ => AbrirDetalleResiduo(), _ => ResiduoSeleccionado != null);
            VolverAlMenuCommand = new RelayCommand(_ => VolverAlMenu());
        }

        private void AbrirDetalleResiduo()
        {
           // var panel = new ResiduosPanelView(ResiduoSeleccionado, _apiService, _navigationService);
           // panel.ShowDialog();
        }

        private void VolverAlMenu()
        {
            _navigationService.NavigateTo<MainView>();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
