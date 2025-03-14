﻿using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using wasty.Services;
using wasty.Views;
using wasty.Models;
using System.Text.Json;

namespace wasty.ViewModels
{
    public class StatisticsPanelViewModel : INotifyPropertyChanged
    {
        private readonly ApiService _apiService;
        private readonly NavigationService _navigationService;
        public ICommand NavigateToStatisticsCommand { get; }

        private ObservableCollection<BotonPanelStats> _botones;
        public ObservableCollection<BotonPanelStats> Botones
        {
            get => _botones;
            set
            {
                _botones = value;
                OnPropertyChanged(nameof(Botones));
            }
        }

        public StatisticsPanelViewModel(ApiService apiService, NavigationService navigationService)
        {
            _apiService = apiService;
            _navigationService = navigationService;
            NavigateToStatisticsCommand = new RelayCommand<object>(NavigateToStatistics);
            Botones = new ObservableCollection<BotonPanelStats>();
            Init().GetAwaiter();
        }

        private void NavigateToStatistics(object parameter)
        {
            _navigationService.NavigateTo<StatisticsView>(parameter);
        }

        private async Task Init()
        {
            Botones = await GetData();
        }

        private async Task<ObservableCollection<BotonPanelStats>> GetData()
        {
            JsonElement itemsElement = default;
            string items = "";
            try
            {
                var result = await _apiService.RequestAsync("GET", "estadisticas/paneles", "");

                var itemsList = JsonSerializer.Deserialize<ObservableCollection<BotonPanelStats>>(result.datos);

                return itemsList;
            }
            catch (Exception ex)
            {
                return new ObservableCollection<BotonPanelStats>();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
