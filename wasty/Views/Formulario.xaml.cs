using System.Windows.Controls;
using wasty.ViewModels;


namespace wasty.Views
{
    public partial class Formulario : UserControl
    {
        public Formulario()
        {
            InitializeComponent();
            DataContext = new FormularioViewModel();
        }
    }
}
