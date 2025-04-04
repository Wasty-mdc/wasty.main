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
        private readonly int _itemsPorPagina;
        private readonly ObservableCollection<T> _todosLosItems;

        public ObservableCollection<T> ItemsPaginados { get; } = new();

        public int PaginaActual
        {
            get => _paginaActual;
            set
            {
                if (_paginaActual != value)
                {
                    _paginaActual = value;
                    OnPropertyChanged(nameof(PaginaActual));
                    ActualizarItemsPaginados();
                    CommandManager.InvalidateRequerySuggested(); // Refresca CanExecute
                }
            }
        }

        public int TotalPaginas => (int)Math.Ceiling((double)_todosLosItems.Count / _itemsPorPagina);

        public ICommand SiguienteCommand { get; }
        public ICommand AnteriorCommand { get; }

        public Paginador(IEnumerable<T> items, int itemsPorPagina = 10)
        {
            _itemsPorPagina = itemsPorPagina;
            _todosLosItems = new ObservableCollection<T>(items);

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

            OnPropertyChanged(nameof(TotalPaginas));
        }

        public void RefrescarDatos(IEnumerable<T> nuevosItems)
        {
            _todosLosItems.Clear();
            foreach (var item in nuevosItems)
                _todosLosItems.Add(item);

            PaginaActual = 1;
            ActualizarItemsPaginados();
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string nombre) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nombre));
    }
}
