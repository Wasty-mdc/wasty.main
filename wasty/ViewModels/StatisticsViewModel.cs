using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text.Json;
using System.Windows.Documents;
using System.Windows.Documents.DocumentStructures;
using System.Windows.Input;
using System.Xml.Linq;
using Microsoft.VisualBasic;
using OpenTK.Input;
using wasty.Models;
using wasty.Services;
using wasty.Utils;
using wasty.ViewModels;
using wasty.Views;
using static MaterialDesignThemes.Wpf.Theme.ToolBar;

public class StatisticsViewModel : INotifyPropertyChanged
{
    private readonly ApiService _apiService;
    private readonly NavigationService _navigationService;
    private string Tabla;

    private readonly List<HeaderColumn> _headerColumn = HeaderColumn.ObtenerHeaders();
    public ObservableCollection<Field> SelectedFields { get; set; }
    private DataView _filteredData;
    public DataView FilteredData
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

    private ObservableCollection<Dictionary<string, List<CheckBoxItemModel>>> _filterableValues { get; set; }
    public ObservableCollection<Dictionary<string, List<CheckBoxItemModel>>> FilterableValues
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
        FilteredData = new DataView();

        // Diccionario de valores filtrables (simulación de datos desde la BD)
        FilterableValues = new ObservableCollection<Dictionary<string, List<CheckBoxItemModel>>>();

        SelectedFilters = new Dictionary<string, List<string>>();
        Init("ClienteResiduo").GetAwaiter();
    }

    // Método para inicializar la vista modelo
    public async Task Init(string tabla)
    {
        Tabla = tabla;
        AvailableFields = await GetFields(tabla);
    }

    private async Task<ObservableCollection<Field>> GetFields(string tabla)
    {
        string fields = string.Empty;
        ObservableCollection<Field> fieldsList = new ObservableCollection<Field>();

        var result = await _apiService.RequestAsync("GET", $"estadisticas/columnas/{tabla}", "");

        try
        {
            var fieldsNames = JsonSerializer.Deserialize<List<string>>(result.datos);

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
    public async Task OnSelectedFieldsChanged(string fieldName, string action)
    {
        List<string> values = new List<string>();

        var result = await _apiService.RequestAsync("GET", $"/estadisticas/datos?tabla={Tabla}&campos={fieldName}", "");

        var records = JsonSerializer.Deserialize<List<dynamic>>(result.datos);

        foreach(JsonElement jElement in records)
        {
            if (jElement.TryGetProperty(fieldName, out JsonElement property))
                values.Add(property.GetRawText());
        }
        if (action is "ADD")
        {
            List<CheckBoxItemModel> valuesCheckBox = new List<CheckBoxItemModel>();
            foreach(string value in values.OrderBy(s => s))
            {
                valuesCheckBox.Add(new CheckBoxItemModel
                {
                    Value = value.Replace("\"","")
                });
            }

            FilterableValues.Add(
                new Dictionary<string, List<CheckBoxItemModel>>{
                    { fieldName, valuesCheckBox }
                }
            );
        } else
        {
            var itemToRemove = FilterableValues.FirstOrDefault(dict => dict.ContainsKey(fieldName));
            FilterableValues.Remove(itemToRemove);
        }
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
    private async Task UpdateTable()
    {
        var dataView = new DataView();
        var campos = string.Join(",", SelectedFields.Select(t => t.Name));
        var filtros = string.Empty;

        if (!string.IsNullOrWhiteSpace(campos))
        {
            DataTable dataTable = new DataTable();
            ObservableCollection<Field> fieldsList = new ObservableCollection<Field>();

            var result = await _apiService.RequestAsync("GET", $"estadisticas/datos/?tabla=ClienteResiduo&campos={campos}", "");
            try
            {
                JsonElement.ArrayEnumerator jsonArray = result.datos.EnumerateArray();

                if (jsonArray.Any())
                {
                    // Obtener las propiedades del primer objeto para definir las columnas
                    foreach (var property in jsonArray.First().EnumerateObject())
                    {
                        dataTable.Columns.Add(property.Name);
                    }

                    // Añadir filas al DataTable
                    foreach (var jsonObject in jsonArray)
                    {
                        var row = dataTable.NewRow();
                        foreach (var property in jsonObject.EnumerateObject())
                        {
                            row[property.Name] = property.Value.ToString();
                        }
                        dataTable.Rows.Add(row);
                    }
                }

                dataView = dataTable.DefaultView;
                dataView.Sort = $"{SelectedFields.Select(t => t.Name).FirstOrDefault()} ASC";

                foreach (var campo in SelectedFields.Select(t => t.Name))
                {
                    var filtroName = campo;
                    string filtroValues = string.Empty;

                    foreach (var item in FilterableValues.FirstOrDefault(s => s.ContainsKey(campo)))
                    {
                        filtroValues = string.Join(", ", item.Value.Where(s => s.IsChecked).Select(s => $"'{s.Value}'"));
                    }
                    filtros += $"{filtroName} IN ({filtroValues}) AND ";
                }

                filtros = Utils.RemoveLastWord(filtros);

                dataView.RowFilter = filtros;
            }
            catch(Exception ex)
            {
            }
        }

        FilteredData = dataView;
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
        FilterableValues.Clear();
        OnPropertyChanged(nameof(SelectedFilters));
    }

    public event PropertyChangedEventHandler PropertyChanged;
    public void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
