using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using wasty.Models;
using wasty.Services;
using wasty.ViewModels;
using wasty.Views;

public class StatisticsViewModel : INotifyPropertyChanged
{
    private readonly NavigationService _navigationService;

    public ObservableCollection<Field> AvailableFields { get; set; }
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

    public StatisticsViewModel(NavigationService navigationService)
    {
        _navigationService = navigationService;
        VolverCommand = new RelayCommand<object>(_ => _navigationService.NavigateTo<MainView>());
        GenerateTableCommand = new RelayCommand<object>(_ => UpdateTable());
        ClearFiltersCommand = new RelayCommand<object>(_ => ClearFilters());

        // Mock Data - Campos disponibles con icono y background
        AvailableFields = new ObservableCollection<Field>
        {
            new Field("Año", "Calendar", "LightBlue", true),
            new Field("Cliente", "Account", "LightGreen", true),
            new Field("Producto", "PackageVariant", "LightYellow", true),
            new Field("Categoría", "Tag", "LightGoldenrodYellow", true),
            new Field("Estado", "CheckboxMarked", "LightCoral", true),
            new Field("Cantidad", "Counter", "LightPink"),
            new Field("Precio", "CurrencyUsd", "LightCyan"),
            new Field("Total", "Calculator", "LightGray")
        };

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
    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
