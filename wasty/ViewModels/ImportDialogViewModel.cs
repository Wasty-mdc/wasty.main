using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Navigation;
using wasty.Models;
using wasty.Services;

namespace wasty.ViewModels
{
    public class ImportDialogViewModel : INotifyPropertyChanged
    {
        private readonly ExcelDataService _excelDataService;
        private readonly ApiService _apiService;

        public event PropertyChangedEventHandler PropertyChanged;

        public ImportDialogViewModel(ApiService apiService, ExcelDataService excelDataService)
        {
            _apiService = apiService;
            _excelDataService = excelDataService;

            Init();
        }

        public async Task Init()
        {
            var cargueClientes = await  _excelDataService.ReadXlsFromPath(AppDomain.CurrentDomain.BaseDirectory);
            await PostData(cargueClientes.ToJson());
        }

        private async Task<bool> PostData(string datos)
        {
            try
            {
                var result = await _apiService.RequestAsync("POST", $"clientes/batch", datos);
                var itemsList = JsonSerializer.Deserialize<ClienteModel>(result.datos);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
