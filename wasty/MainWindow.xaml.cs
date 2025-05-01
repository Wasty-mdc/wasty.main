using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media.Effects;
using System.Windows.Media;
using wasty.ViewModels;
using System.Windows.Shell;
using System.Windows.Threading;
using wasty.Styles;
using Microsoft.Extensions.DependencyInjection;
using MahApps.Metro.Controls;

namespace wasty
{
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = ((App)Application.Current).Services.GetService(typeof(MainWindowViewModel));
        }

    }
}