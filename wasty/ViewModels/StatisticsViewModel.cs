using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using wasty.Services;
using wasty.ViewModels;
using wasty.Views;

public class StatisticsViewModel : INotifyPropertyChanged
{
    private readonly NavigationService _navigationService;

    public ObservableCollection<string> AvailableFields { get; set; }
    public ObservableCollection<string> SelectedFields { get; set; }
    public ObservableCollection<object> FilteredData { get; set; }
    public ICommand VolverCommand { get; }

    public StatisticsViewModel(NavigationService navigationService)
    {
        _navigationService = navigationService;
        VolverCommand = new RelayCommand(_ => _navigationService.NavigateTo<MainView>());


        // Campos disponibles en la base de datos o modelo
        AvailableFields = new ObservableCollection<string>
        {
            "Fecha", "Cliente", "Producto", "Cantidad", "Precio", "Total"
        };

        SelectedFields = new ObservableCollection<string>();
        FilteredData = new ObservableCollection<object>();
    }

    // Método para actualizar la tabla cuando cambian los campos seleccionados
    private void UpdateTable()
    {
        // Aquí filtras los datos de acuerdo con los campos seleccionados
        // Suponiendo que tienes una lista de objetos con todas las columnas posibles
        var allData = GetAllData(); // Obtiene la data completa

        // Filtra solo los campos seleccionados
        var filtered = allData.Select(row =>
            SelectedFields.ToDictionary(field => field, field => row[field]));

        FilteredData.Clear();
        foreach (var item in filtered)
        {
            FilteredData.Add(item);
        }
    }

    private List<Dictionary<string, object>> GetAllData()
    {
        // Simulación de datos (esto normalmente vendría de una base de datos)
        return new List<Dictionary<string, object>>
        {
            new Dictionary<string, object> { { "Fecha", "2025-02-24" }, { "Cliente", "Empresa A" }, { "Producto", "Papel" }, { "Cantidad", 50 }, { "Precio", 2.5 }, { "Total", 125 } },
            new Dictionary<string, object> { { "Fecha", "2025-02-23" }, { "Cliente", "Empresa B" }, { "Producto", "Plástico" }, { "Cantidad", 100 }, { "Precio", 1.5 }, { "Total", 150 } }
        };
    }

    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
