using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text.Json;
using System.Windows.Documents;
using System.Windows.Documents.DocumentStructures;
using System.Windows.Input;
using System.Xml.Linq;
using wasty.Models;
using wasty.Services;
using wasty.ViewModels;
using wasty.Views;

public class StatisticsViewModel : INotifyPropertyChanged
{
    private readonly ApiService _apiService;
    private readonly NavigationService _navigationService;

    private readonly List<HeaderColumn> _headerColumn = HeaderColumn.ObtenerHeaders();
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

    private ObservableCollection<Dictionary<string, List<string>>> _filterableValues { get; set; }
    public ObservableCollection<Dictionary<string, List<string>>> FilterableValues
    {
        get => _filterableValues;
        set
        {
            _filterableValues = value;
            OnPropertyChanged(nameof(FilterableValues));
        }
    }

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
    public ICommand RemoveFieldCommand { get; }
    public ICommand ToggleExpandCommand { get; }


    public StatisticsViewModel(ApiService apiService, NavigationService navigationService)
    {
        _apiService = apiService;
        _navigationService = navigationService;

        VolverCommand = new RelayCommand<object>(_ => _navigationService.NavigateTo<MainView>());
        GenerateTableCommand = new RelayCommand<object>(_ => UpdateTable());
        ClearFiltersCommand = new RelayCommand<object>(_ => ClearFilters());
        ToggleExpandCommand = new RelayCommand<Field>(ToggleExpand);
        RemoveFieldCommand = new RelayCommand<Field>(RemoveField);

        AvailableFields = new ObservableCollection<Field>();

        SelectedFields = new ObservableCollection<Field>();
        FilteredData = new ObservableCollection<object>();

        // Diccionario de valores filtrables (simulación de datos desde la BD)
        FilterableValues = new ObservableCollection<Dictionary<string, List<string>>>();

        SelectedFilters = new Dictionary<string, List<string>>();
        Init("ClienteResiduo").GetAwaiter();
    }

    // Método para inicializar la vista modelo
    public async Task Init(string tabla)
    {
        AvailableFields = await GetData(tabla);
    }

    private async Task<ObservableCollection<Field>> GetData(string tabla)
    {
        JsonElement tokenElement = default;
        JsonElement fieldsElement = default;
        string fields = string.Empty;
        ObservableCollection<Field> fieldsList = new ObservableCollection<Field>();
        string token = string.Empty;
        var login = new
        {
            Email = "Pruebas123@pruebas.com",
            Contrasenia = "Pruebas123."
        };

        var auth = await _apiService.RequestAsync("POST", "auth/login", login);

        if (auth.TryGetProperty("datos", out JsonElement datosElement) && datosElement.TryGetProperty("token", out tokenElement))
            token = tokenElement.GetString();

        var result = await _apiService.RequestAsync("GET", $"estadisticas/columnas/{tabla}", "", token);

        if (result.TryGetProperty("datos", out fieldsElement))
            fields = fieldsElement.GetRawText();

        try
        {
            var fieldsNames = JsonSerializer.Deserialize<List<string>>(fields);

            foreach(var i in fieldsNames)
            {
                var header = _headerColumn.FirstOrDefault(s => i.Contains(s.Nombre));
                if (header is not null)
                {
                    fieldsList.Add(new Field(i, header.Icono, header.Color));
                }else 
                    fieldsList.Add(new Field(i, "FileAlertOutline", "#9C9C9C"));
            }

            return fieldsList;
        }
        catch (Exception ex)
        {
            return new ObservableCollection<Field>();
        }
    }
    public async Task OnSelectedFieldsChanged(string fieldName)
    {
        //if (e.Action == NotifyCollectionChangedAction.Add)
        //{
            //Field field = (Field)e.NewItems[0];
            List<string> values = new List<string>();

            JsonElement tokenElement = default;
            JsonElement fieldsElement = default;
            string json = string.Empty;
            string token = string.Empty;
            var login = new
            {
                Email = "Pruebas123@pruebas.com",
                Contrasenia = "Pruebas123."
            };

            var auth = await _apiService.RequestAsync("POST", "auth/login", login);

            if (auth.TryGetProperty("datos", out JsonElement datosElement) && datosElement.TryGetProperty("token", out tokenElement))
                token = tokenElement.GetString();

            var result = await _apiService.RequestAsync("GET", $"/estadisticas/datos?tabla=ClienteResiduo&campos={fieldName}", "", token);

            if (result.TryGetProperty("datos", out fieldsElement))
                json = fieldsElement.GetRawText();

            var fieldsNames = JsonSerializer.Deserialize<List<dynamic>>(json);

            try
            {
                foreach(JsonElement jElement in fieldsNames)
                {
                    if (jElement.TryGetProperty(fieldName, out JsonElement property))
                        values.Add(property.GetRawText());
                }
            }catch(Exception ex)
            {

            }
            FilterableValues.Add(
                new Dictionary<string, List<string>>
                {
                    { fieldName, values.Take(5).ToList() }
                }
            );
        //}
    }
    private void ToggleExpand(Field field)
    {
        foreach (var f in SelectedFields)
        {
            if (f != field) f.IsExpanded = false;
        }
        field.IsExpanded = !field.IsExpanded;
        OnPropertyChanged(nameof(SelectedFields));
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
    private void RemoveField(Field field)
    {
        if (SelectedFields.Contains(field))
        {
            SelectedFields.Remove(field);
            if (!AvailableFields.Any(f => f.Name == field.Name))
            {
                AvailableFields.Add(field);
            }
            OnPropertyChanged(nameof(SelectedFields));
            OnPropertyChanged(nameof(AvailableFields));
        }
    }

    private void ClearFilters()
    {
        foreach (var field in SelectedFields.ToList())
        {
            RemoveField(field);
        }
        SelectedFilters.Clear();
        OnPropertyChanged(nameof(SelectedFilters));
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
