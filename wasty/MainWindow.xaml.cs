using System.Windows;
using wasty.ViewModels;

namespace wasty
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            // Establecer el DataContext al MainWindowViewModel
            DataContext = ((App)Application.Current).Services.GetService(typeof(MainWindowViewModel));
        }
    }
}