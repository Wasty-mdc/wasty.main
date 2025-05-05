using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wasty.Models;
using wasty.Services;

namespace wasty.ViewModels
{ 
    public class TreatmentsPanelViewModel : INotifyPropertyChanged
    {
        private readonly ApiService _apiService;
        private readonly NavigationService _navigationService;

        public TreatmentsPanelViewModel(ApiService apiService, NavigationService navigationService)
        {
            _apiService = apiService;
            _navigationService = navigationService;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string name) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
