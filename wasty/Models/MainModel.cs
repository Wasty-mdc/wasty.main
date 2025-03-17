using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace wasty.Models
{
    public class MainModel : INotifyPropertyChanged
    {
        private object _currentView;

        // Propiedad que representa la vista actual
        public object CurrentView
        {
            get => _currentView;
            set
            {
                _currentView = value;
                OnPropertyChanged();
            }
        }


        // Evento para notificar cambios en las propiedades
        public event PropertyChangedEventHandler PropertyChanged;

        // Método para invocar el evento PropertyChanged
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}