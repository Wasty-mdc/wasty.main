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
using System.Windows.Media.Animation;

namespace wasty
{
    public partial class MainWindow : Window
    {
        private MainWindowViewModel _viewModel => DataContext as MainWindowViewModel;

        public MainWindow()
        {
            InitializeComponent();
            var vm = ((App)Application.Current).Services.GetService(typeof(MainWindowViewModel)) as MainWindowViewModel;
            DataContext = vm;
            vm.PropertyChanged += ViewModel_PropertyChanged;
        }
        private void Overlay_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (_viewModel != null && _viewModel.IsNotificationPanelOpen)
            {
                _viewModel.IsNotificationPanelOpen = false;
            }
        }


        private void ViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(MainWindowViewModel.IsNotificationPanelOpen))
            {
                var storyboardKey = _viewModel.IsNotificationPanelOpen ? "SlideInStoryboard" : "SlideOutStoryboard";
                var storyboard = (Storyboard)FindResource(storyboardKey);
                storyboard.Begin();
            }
        }

    }
}