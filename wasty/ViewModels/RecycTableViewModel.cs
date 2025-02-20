﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace wasty.ViewModels
{
    public class RecycTableViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Residuo> Residuos { get; set; }
        public ICommand ToggleStickyColumnCommand { get; }
        public ICommand ToggleColumnVisibilityCommand { get; }

        private Dictionary<string, bool> _stickyColumns;
        private Dictionary<string, bool> _hiddenColumns;
        private int _frozenColumnCount;

        public int FrozenColumnCount
        {
            get => _frozenColumnCount;
            set
            {
                _frozenColumnCount = value;
                OnPropertyChanged();
            }
        }

        public RecycTableViewModel()
        {
            Residuos = new ObservableCollection<Residuo>(GetMockData());
            _stickyColumns = new Dictionary<string, bool>();
            _hiddenColumns = new Dictionary<string, bool>();
            ToggleStickyColumnCommand = new RelayCommand<string>(ToggleStickyColumn);
            ToggleColumnVisibilityCommand = new RelayCommand<string>(ToggleColumnVisibility);
        }

        private void ToggleStickyColumn(string columnName)
        {
            if (string.IsNullOrEmpty(columnName)) return;

            if (_stickyColumns.ContainsKey(columnName))
            {
                _stickyColumns[columnName] = !_stickyColumns[columnName];
            }
            else
            {
                _stickyColumns[columnName] = true;
            }

            UpdateFrozenColumns();
        }

        private void UpdateFrozenColumns()
        {
            FrozenColumnCount = _stickyColumns.Values.Count(v => v);
        }

        private void ToggleColumnVisibility(string columnName)
        {
            if (string.IsNullOrEmpty(columnName)) return;

            if (_hiddenColumns.ContainsKey(columnName))
            {
                _hiddenColumns[columnName] = !_hiddenColumns[columnName];
            }
            else
            {
                _hiddenColumns[columnName] = true;
            }
            OnPropertyChanged("HiddenColumns");
        }

        public bool IsColumnVisible(string columnName)
        {
            return !_hiddenColumns.TryGetValue(columnName, out bool isHidden) || !isHidden;
        }

        private List<Residuo> GetMockData()
        {
            return new List<Residuo>
            {
                new Residuo { Codigo = "R001", Tipo = "Plástico", Peso = 120, Estado = "Procesado", Ubicacion = "Planta A", FechaRecoleccion = DateTime.Now, EmpresaGestora = "EcoResiduos S.A.", Destino = "Reciclaje" },
                new Residuo { Codigo = "R002", Tipo = "Vidrio", Peso = 85, Estado = "Pendiente", Ubicacion = "Planta B", FechaRecoleccion = DateTime.Now, EmpresaGestora = "GreenWaste Ltd.", Destino = "Reciclaje" },
                new Residuo { Codigo = "R003", Tipo = "Papel", Peso = 150, Estado = "Procesado", Ubicacion = "Planta C", FechaRecoleccion = DateTime.Now, EmpresaGestora = "RecyclePlus", Destino = "Reutilización" }
            };
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class Residuo
    {
        public string Codigo { get; set; }
        public string Tipo { get; set; }
        public double Peso { get; set; }
        public string Estado { get; set; }
        public string Ubicacion { get; set; }
        public DateTime FechaRecoleccion { get; set; }
        public string EmpresaGestora { get; set; }
        public string Destino { get; set; }
    }
}
