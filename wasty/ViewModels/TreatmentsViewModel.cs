using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using wasty.Models;
using wasty.Services;
using wasty.Utils;
using wasty.Views;

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


        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
