using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text.Json;
using System.Windows.Input;
using wasty.Models;
using wasty.Services;
using wasty.ViewModels;

namespace wasty.Utils
{
    public class Paginador<T> : INotifyPropertyChanged
    {
        private readonly ApiService _apiService;

        private string _URL;
        private int _paginaActual;
        private int _totalPaginas;
        private int _itemsPorPagina;
        private List<T> _todosLosItems;

        public ObservableCollection<T> ItemsPaginados { get; } = new();
        public ObservableCollection<T> TodosLosItemsOriginales { get; private set; }
        public ObservableCollection<T> ItemsOriginales { get; set; }

        public int PaginaActual => _paginaActual;

        public int TotalPaginas => _totalPaginas;

        public ICommand SiguienteCommand { get; }
        public ICommand AnteriorCommand { get; }

        public bool MostrarBotonAnterior => PaginaActual > 1;

        public bool MostrarBotonSiguiente => PaginaActual < TotalPaginas;

        public Paginador(ApiService apiService, string url, IEnumerable<T> items, int itemsPorPagina, int paginaActual, int totalPaginas)
        {
            _apiService = apiService ?? throw new ArgumentNullException(nameof(apiService));
            _URL = url;
            _itemsPorPagina = itemsPorPagina;
            _todosLosItems = items.ToList(); // Convertimos a lista para mayor control
            _paginaActual = paginaActual;
            _totalPaginas = totalPaginas;
            TodosLosItemsOriginales = new ObservableCollection<T>(_todosLosItems);
            ItemsOriginales = new ObservableCollection<T>(_todosLosItems);

            SiguienteCommand = new RelayCommand(_=> ChangePage("next"));
            AnteriorCommand = new RelayCommand(_=> ChangePage("previous"));

            ActualizarItemsPaginados();
        }

        private async Task ChangePage(string action)
        {
            if (action is "next" && PaginaActual < TotalPaginas)
                _paginaActual++;
            else if (action is "previous" && PaginaActual > 1)
                _paginaActual--;

            var result = await _apiService.RequestAsync("GET", $"{_URL}?PageNumber={_paginaActual}&PageSIze={_itemsPorPagina}", "");
            _todosLosItems = JsonSerializer.Deserialize<List<ClienteModel>>(result.datos);
            //_todosLosItems = items.ToList();
            ActualizarItemsPaginados();
        }

        private void ActualizarItemsPaginados()
        {
            ItemsPaginados.Clear();

            foreach (var item in _todosLosItems)
                ItemsPaginados.Add(item);

            // 🔁 Forzamos el refresco del DataGrid
            OnPropertyChanged(nameof(ItemsPaginados));
            OnPropertyChanged(nameof(TotalPaginas));
            OnPropertyChanged(nameof(PaginaActual));
            OnPropertyChanged(nameof(MostrarBotonAnterior));
            OnPropertyChanged(nameof(MostrarBotonSiguiente));
        }

        private void ActualizarTotalPaginas()
        {
            // Ahora recalculamos TotalPaginas correctamente al cambiar los elementos
            OnPropertyChanged(nameof(TotalPaginas));
        }

        public void RefrescarCon(IEnumerable<T> nuevosItems)
        {
            if (nuevosItems == null) throw new ArgumentNullException(nameof(nuevosItems));
            _todosLosItems = nuevosItems.ToList();
            ActualizarTotalPaginas();
            _paginaActual = 1; // Si los elementos cambian, resetear la página
            ActualizarItemsPaginados();
        }

        public void Reset()
        {
            // Restauramos la lista original de los elementos
            _todosLosItems = TodosLosItemsOriginales.ToList();
            ItemsOriginales = new ObservableCollection<T>(TodosLosItemsOriginales);

            // Reiniciamos la página y actualizamos la vista
            _paginaActual = 1;
            ActualizarItemsPaginados();
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string nombre) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nombre));
    }
}
