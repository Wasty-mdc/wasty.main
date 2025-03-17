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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace wasty.Views
{
    /// <summary>
    /// Lógica de interacción para ClientPanelView.xaml
    /// </summary>
    public partial class ClientPanelView : UserControl
    {
        public ClientPanelView()
        {
            InitializeComponent();
        }
        private void GrupoPrincipal_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (GrupoSecundario == null) return;

            GrupoSecundario.Items.Clear();
            GrupoSecundario.IsEnabled = true;

            string selectedGroup = (GrupoPrincipal.SelectedItem as ComboBoxItem)?.Content.ToString();
            List<string> secondaryOptions = new List<string>();

            switch (selectedGroup)
            {
                case "Talleres":
                    secondaryOptions.AddRange(new string[] {
                    "Mecánico", "Aceite", "Camiones", "Lavadero", "Mecánico y Planchas",
                    "Motos", "Plancha y Pintura", "Plancha, Pintura y Mecánico", "Transportes"
                });
                    break;
                case "Planchas":
                    GrupoSecundario.IsEnabled = false;
                    break;
                case "Gestores":
                    secondaryOptions.AddRange(new string[] {
                    "Aceite Gestor", "Cartón", "Lodos", "Metales", "Palets", "Plástico",
                    "Residuos Peligrosos", "Residuos Voluminosos", "Vidrio"
                });
                    break;
                case "Industrias":
                    secondaryOptions.AddRange(new string[] {
                    "Aceite Industrial", "Cartón", "Contenedores", "Curtido Pieles",
                    "Material Quirúrgico", "Metales", "Plástico", "Otros"
                });
                    break;
                case "Talleres Aceite":
                    GrupoSecundario.IsEnabled = false;
                    break;
                case "Sin asignar":
                    secondaryOptions.Add("Sin asignar");
                    break;
            }

            foreach (var option in secondaryOptions)
            {
                GrupoSecundario.Items.Add(new ComboBoxItem { Content = option });
            }
        }

    }
}
