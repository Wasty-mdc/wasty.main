using System.Windows;
using System.Windows.Controls;
using wasty.ViewModels;

namespace wasty.Views
{
    public partial class RentingView : UserControl
    {
        public RentingView()
        {
            InitializeComponent();
            DataContext = ((App)Application.Current).Services.GetService(typeof(RentingViewModel));
        }
    }
}
