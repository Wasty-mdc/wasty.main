using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using wasty.Models;
using wasty.Services;
using Wasty.ViewModels;

namespace wasty.Views
{
    /// <summary>
    /// Lógica de interacción para ResiduosPanelView.xaml
    /// </summary>
    public partial class ResiduosPanelView : Window, IParameterReceiver
    {
        public ResiduosPanelView()
        {
            InitializeComponent();
            DataContext = ((App)Application.Current).Services.GetService(typeof(ResiduosPanelViewModel));
        }

        public void ReceiveParameter(object parameter)
        {
            var viewModel = (ResiduosPanelViewModel)DataContext;
            viewModel.Init((ResiduoModel)parameter);
        }
    }
}
