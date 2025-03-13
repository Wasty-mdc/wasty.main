using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace wasty.Models
{
    class LoginModel: INotifyPropertyChanged
    {

        private string _email;
        //private bool _isAuthenticated;

        public string Email
        {
            get => _email;
            set
            {
                _email = value;
                OnPropertyChanged();
            }
        }
        //public bool IsAuthenticated
        //{
        //    get => _isAuthenticated;
        //    set
        //    {
        //        _isAuthenticated = value;
        //        OnPropertyChanged(nameof(IsAuthenticated));
        //    }
        //}

        // Evento para notificar cambios en las propiedades
        public event PropertyChangedEventHandler PropertyChanged;

        // Método para invocar el evento PropertyChanged
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
