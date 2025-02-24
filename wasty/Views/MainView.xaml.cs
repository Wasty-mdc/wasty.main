using System.Windows;
using System.Windows.Controls;
using wasty.Services;

namespace wasty.Views
{
    public partial class MainView : UserControl
    {
        public MainView()
        {
            InitializeComponent();
            // Establecer el DataContext al MainViewModel
            DataContext = ((App)Application.Current).Services.GetService(typeof(MainViewModel));
        }
    }
}