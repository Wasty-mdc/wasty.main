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
using wasty.ViewModels;

namespace wasty.Utils
{
    /// <summary>
    /// Lógica de interacción para ImportDialog.xaml
    /// </summary>
    public partial class ImportDialog : Window
    {
        public ImportDialog()
        {
            InitializeComponent();
            DataContext = ((App)Application.Current).Services.GetService(typeof(ImportDialogViewModel));
        }
    }
}
