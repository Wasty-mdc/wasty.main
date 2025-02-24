using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using wasty.Services;
using wasty.ViewModels;
using wasty.Views;

public class StatisticsViewModel : INotifyPropertyChanged
{
    private readonly NavigationService _navigationService;

    public ObservableCollection<Field> AvailableFields { get; set; }
    public ObservableCollection<Field> SelectedFields { get; set; }
    public ObservableCollection<object> FilteredData { get; set; }
    public ICommand VolverCommand { get; }
    public ICommand GenerateTableCommand { get; }

    public StatisticsViewModel(NavigationService navigationService)
    {
        _navigationService = navigationService;
        VolverCommand = new RelayCommand(_ => _navigationService.NavigateTo<MainView>());
        GenerateTableCommand = new RelayCommand(_ => UpdateTable());

        // Mock Data - Campos disponibles con icono y background
        AvailableFields = new ObservableCollection<Field>
        {
            new Field("Fecha", "Calendar", "LightBlue"),
            new Field("Cliente", "Account", "LightGreen"),
            new Field("Producto", "PackageVariant", "LightYellow"),
            new Field("Cantidad", "Counter", "LightPink"),
            new Field("Precio", "CurrencyUsd", "LightCyan"),
            new Field("Total", "Calculator", "LightGray")
        };

        SelectedFields = new ObservableCollection<Field>();
        FilteredData = new ObservableCollection<object>();
    }

    private void UpdateTable()
    {
        var allData = GetAllData(); // Obtiene la data completa

        var filtered = allData.Select(row =>
            SelectedFields.ToDictionary(field => field.Name, field => row[field.Name]));

        FilteredData.Clear();
        foreach (var item in filtered)
        {
            FilteredData.Add(item);
        }
    }

    private List<Dictionary<string, object>> GetAllData()
    {
        return new List<Dictionary<string, object>>
        {
            new Dictionary<string, object> { { "Fecha", "2025-02-24" }, { "Cliente", "Empresa A" }, { "Producto", "Papel" }, { "Cantidad", 50 }, { "Precio", 2.5 }, { "Total", 125 } },
            new Dictionary<string, object> { { "Fecha", "2025-02-23" }, { "Cliente", "Empresa B" }, { "Producto", "Plástico" }, { "Cantidad", 100 }, { "Precio", 1.5 }, { "Total", 150 } },
            new Dictionary<string, object> { { "Fecha", "2025-02-22" }, { "Cliente", "Empresa C" }, { "Producto", "Vidrio" }, { "Cantidad", 30 }, { "Precio", 3.0 }, { "Total", 90 } },
            new Dictionary<string, object> { { "Fecha", "2025-02-21" }, { "Cliente", "Empresa D" }, { "Producto", "Metal" }, { "Cantidad", 20 }, { "Precio", 5.0 }, { "Total", 100 } }
        };
    }

    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}

public class Field
{
    public string Name { get; set; }
    public string Icon { get; set; }
    public string Background { get; set; }

    public Field(string name, string icon, string background)
    {
        Name = name;
        Icon = icon;
        Background = background;
    }
}
