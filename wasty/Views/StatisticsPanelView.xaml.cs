using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using wasty.ViewModels;

namespace wasty.Views
{
    public partial class StatisticsPanelView : UserControl
    {
        public StatisticsPanelView()
        {
            InitializeComponent();
            DataContext = ((App)Application.Current).Services.GetService(typeof(StatisticsPanelViewModel));
        }
    }
}