using System;
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
        private int _itemsPorPagina;
        private readonly ObservableCollection<T> _todosLosItems;
        public int ItemsPorPagina
        {
            get => _itemsPorPagina;
            set
            {
                if (value > 0 && value != _itemsPorPagina)
                {
                    _itemsPorPagina = value;
                    PaginaActual = 1; // Opcional: volver a la primera página
                    ActualizarItemsPaginados();
                    OnPropertyChanged(nameof(ItemsPorPagina));
                }
            }
        }

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

        public bool MostrarBotonAnterior
        {
            get => PaginaActual > 1;
        }

        public bool MostrarBotonSiguiente
        {
            get => PaginaActual < TotalPaginas;
        }

        public Paginador(IEnumerable<T> items, int itemsPorPagina = 25)
        {
            _itemsPorPagina = itemsPorPagina;
            _todosLosItems = new ObservableCollection<T>(items);
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
                .Take(_itemsPorPagina);

            foreach (var item in items)
                ItemsPaginados.Add(item);

            // 🔁 Forzamos el refresco del DataGrid
            OnPropertyChanged(nameof(ItemsPaginados));
            OnPropertyChanged(nameof(TotalPaginas));
            OnPropertyChanged(nameof(MostrarBotonAnterior));
            OnPropertyChanged(nameof(MostrarBotonSiguiente));
        }

        public void RefrescarCon(IEnumerable<T> nuevosItems)
        {
            _todosLosItems.Clear();
            foreach (var item in nuevosItems)
                _todosLosItems.Add(item);

            PaginaActual = 1;
            ActualizarItemsPaginados();
        }

        public void Reset()
        {
            _todosLosItems.Clear();
            foreach (var item in TodosLosItemsOriginales)
                _todosLosItems.Add(item);

            // 🔁 Restauramos también la referencia original
            ItemsOriginales = new ObservableCollection<T>(TodosLosItemsOriginales);

            // 🔒 Reiniciamos página y actualizamos correctamente
            PaginaActual = 1;
            ActualizarItemsPaginados();
        }



        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string nombre) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nombre));
    }
}
