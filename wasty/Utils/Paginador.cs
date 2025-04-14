using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using wasty.ViewModels;

namespace wasty.Utils
{
    public class Paginador<T> : INotifyPropertyChanged
    {
        private int _paginaActual = 1;
        private readonly int _itemsPorPagina;
        private List<T> _todosLosItems;

        public ObservableCollection<T> ItemsPaginados { get; } = new();
        public ObservableCollection<T> TodosLosItemsOriginales { get; private set; }
        public ObservableCollection<T> ItemsOriginales { get; set; }

        public int PaginaActual
        {
            get => _paginaActual;
            set
            {
                var nuevaPagina = Math.Max(1, Math.Min(value, TotalPaginas)); // Evita fuera de rango
                if (_paginaActual != nuevaPagina)
                {
                    _paginaActual = nuevaPagina;
                    OnPropertyChanged(nameof(PaginaActual));
                    ActualizarItemsPaginados();
                    CommandManager.InvalidateRequerySuggested();
                }
            }
        }

        public int TotalPaginas => (int)Math.Ceiling((double)_todosLosItems.Count / _itemsPorPagina);

        public ICommand SiguienteCommand { get; }
        public ICommand AnteriorCommand { get; }

        public bool MostrarBotonAnterior => PaginaActual > 1;

        public bool MostrarBotonSiguiente => PaginaActual < TotalPaginas;

        public Paginador(IEnumerable<T> items, int itemsPorPagina = 25)
        {
            _itemsPorPagina = itemsPorPagina;
            _todosLosItems = items.ToList(); // Convertimos a lista para mayor control
            TodosLosItemsOriginales = new ObservableCollection<T>(_todosLosItems);
            ItemsOriginales = new ObservableCollection<T>(_todosLosItems);

            SiguienteCommand = new RelayCommand(_ => PaginaActual++, _ => PaginaActual < TotalPaginas);
            AnteriorCommand = new RelayCommand(_ => PaginaActual--, _ => PaginaActual > 1);

            ActualizarItemsPaginados();
        }

        private void ActualizarItemsPaginados()
        {
            ItemsPaginados.Clear();

            var items = _todosLosItems
                .Skip((_paginaActual - 1) * _itemsPorPagina)
                .Take(_itemsPorPagina)
                .ToList();

            foreach (var item in items)
                ItemsPaginados.Add(item);

            // 🔁 Forzamos el refresco del DataGrid
            OnPropertyChanged(nameof(ItemsPaginados));
            OnPropertyChanged(nameof(TotalPaginas));
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
            PaginaActual = 1; // Si los elementos cambian, resetear la página
            ActualizarItemsPaginados();
        }

        public void Reset()
        {
            // Restauramos la lista original de los elementos
            _todosLosItems = TodosLosItemsOriginales.ToList();
            ItemsOriginales = new ObservableCollection<T>(TodosLosItemsOriginales);

            // Reiniciamos la página y actualizamos la vista
            PaginaActual = 1;
            ActualizarItemsPaginados();
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string nombre) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nombre));
    }
}
