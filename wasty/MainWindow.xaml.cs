// Lógica de interacción para la ventana principal de Wasty.
// Inicializa la ventana y establece el título de la aplicación como "Wasty".
// Actualmente no contiene implementación adicional más allá de la inicialización básica.

using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace wasty
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Title = "Wasty";
        }
    }
}