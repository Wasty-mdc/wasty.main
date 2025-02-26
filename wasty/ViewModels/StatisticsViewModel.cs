﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text.Json;
using System.Windows.Input;
using wasty.Models;
using wasty.Services;
using wasty.ViewModels;
using wasty.Views;

public class StatisticsViewModel : INotifyPropertyChanged
{
    private readonly ApiService _apiService;
    private readonly NavigationService _navigationService;

    public ObservableCollection<Field> SelectedFields { get; set; }
    private ObservableCollection<object> _filteredData;
    public ObservableCollection<object> FilteredData
    {
        get => _filteredData;
        set
        {
            _filteredData = value;
            OnPropertyChanged(nameof(FilteredData));
        }
    }
    // Propiedad que representa la colección de clientes
    private ObservableCollection<Field> _availableFields { get; set; }
    public ObservableCollection<Field> AvailableFields
    {
        get => _availableFields;
        set
        {
            _availableFields = value;
            OnPropertyChanged(nameof(AvailableFields));
        }
    }

    public Dictionary<string, List<string>> FilterableValues { get; set; }
    private Dictionary<string, List<string>> _selectedFilters;
    public Dictionary<string, List<string>> SelectedFilters
    {
        get => _selectedFilters;
        set
        {
            _selectedFilters = value;
            OnPropertyChanged(nameof(SelectedFilters));
        }
    }

    public ICommand VolverCommand { get; }
    public ICommand GenerateTableCommand { get; }
    public ICommand ClearFiltersCommand { get; }
    public ICommand ToggleExpandCommand { get; }


    public StatisticsViewModel(ApiService apiService, NavigationService navigationService)
    {
        _apiService = apiService;
        _navigationService = navigationService;
        VolverCommand = new RelayCommand<object>(_ => _navigationService.NavigateTo<MainView>());
        GenerateTableCommand = new RelayCommand<object>(_ => UpdateTable());
        ClearFiltersCommand = new RelayCommand<object>(_ => ClearFilters());
        ToggleExpandCommand = new RelayCommand<Field>(ToggleExpand);

        AvailableFields = new ObservableCollection<Field>();

        SelectedFields = new ObservableCollection<Field>();
        FilteredData = new ObservableCollection<object>();

        // Diccionario de valores filtrables (simulación de datos desde la BD)
        FilterableValues = new Dictionary<string, List<string>>
        {
            { "Año", new List<string> { "2020", "2021", "2022", "2023", "2024", "2025" } },
            { "Cliente", new List<string> { "Empresa A", "Empresa B", "Empresa C", "Empresa D" } },
            { "Producto", new List<string> { "Papel", "Plástico", "Vidrio", "Metal" } },
            { "Categoría", new List<string> { "Reciclable", "No Reciclable" } },
            { "Estado", new List<string> { "Pendiente", "Completado", "Cancelado" } }
        };

        SelectedFilters = new Dictionary<string, List<string>>();
        Init().GetAwaiter();
    }

    // Método para inicializar la vista modelo
    private async Task Init()
    {
        AvailableFields = await GetData();
    }

    private async Task<ObservableCollection<Field>> GetData()
    {
        JsonElement tokenElement = default;
        JsonElement fieldsElement = default;
        ObservableCollection<Field> fieldsList = new ObservableCollection<Field>();
        string token = "";
        string fields = "";
        var login = new
        {
            Email = "Pruebas123@pruebas.com",
            Contrasenia = "Pruebas123."
        };

        var auth = await _apiService.RequestAsync("POST", "auth/login", login);

        if (auth.TryGetProperty("datos", out JsonElement datosElement) && datosElement.TryGetProperty("token", out tokenElement))
            token = tokenElement.GetString();

        var result = await _apiService.RequestAsync("GET", "data/tablas", "", token);

        if (result.TryGetProperty("datos", out fieldsElement))
            fields = fieldsElement.GetRawText();

        try
        {
            var fieldsNames = JsonSerializer.Deserialize<List<string>>(fields);

            foreach(var i in fieldsNames)
            {
                fieldsList.Add(new Field(i,"Calendar", "#6CD4FF"));
            }

            return fieldsList;
        }
        catch (Exception ex)
        {
            return new ObservableCollection<Field>();
        }
    }
    private void ToggleExpand(Field field)
    {
        field.IsExpanded = !field.IsExpanded;
    }
    private void UpdateTable()
    {
        var allData = GetAllData(); // Obtiene la data completa

        if (SelectedFields.Count == 0)
        {
            FilteredData.Clear();
            return;
        }

        if (SelectedFilters.Count == 0 || SelectedFilters.All(f => f.Value.Count == 0))
        {
            FilteredData = new ObservableCollection<object>(allData.Select(row =>
                SelectedFields.ToDictionary(field => field.Name, field => row.ContainsKey(field.Name) ? row[field.Name] : null)));
            return;
        }

        var filtered = allData
            .Where(row => SelectedFilters.All(filtro =>
                !row.ContainsKey(filtro.Key) || filtro.Value.Contains(row[filtro.Key].ToString())))
            .Select(row => SelectedFields.ToDictionary(field => field.Name, field => row.ContainsKey(field.Name) ? row[field.Name] : null));

        FilteredData.Clear();
        foreach (var item in filtered)
        {
            FilteredData.Add(item);
        }
    }

    private void ClearFilters()
    {
        SelectedFilters.Clear();
        OnPropertyChanged(nameof(SelectedFilters));
        UpdateTable();
    }

    private List<Dictionary<string, object>> GetAllData()
    {
        return new List<Dictionary<string, object>>
        {
            new Dictionary<string, object> { { "Año", "2023" }, { "Cliente", "Empresa A" }, { "Producto", "Papel" }, { "Categoría", "Reciclable" }, { "Estado", "Pendiente" }, { "Cantidad", 50 }, { "Precio", 2.5 }, { "Total", 125 } },
            new Dictionary<string, object> { { "Año", "2022" }, { "Cliente", "Empresa B" }, { "Producto", "Plástico" }, { "Categoría", "No Reciclable" }, { "Estado", "Completado" }, { "Cantidad", 100 }, { "Precio", 1.5 }, { "Total", 150 } }
        };
    }

    public event PropertyChangedEventHandler PropertyChanged;
    public void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
