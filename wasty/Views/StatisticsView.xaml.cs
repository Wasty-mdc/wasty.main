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
    /// Lógica de interacción para StatisticsView.xaml
    /// </summary>
    public partial class StatisticsView : UserControl
    {
        public StatisticsView()
        {
            InitializeComponent();
        }
        private void FieldsList_DragOver(object sender, DragEventArgs e)
        {
            e.Effects = DragDropEffects.Move;
        }

        private void FieldsList_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(string)))
            {
                string field = (string)e.Data.GetData(typeof(string));
                if (!((StatisticsViewModel)DataContext).SelectedFields.Contains(field))
                {
                    ((StatisticsViewModel)DataContext).SelectedFields.Add(field);
                    ((StatisticsViewModel)DataContext).AvailableFields.Remove(field);
                }
            }
        }

        private void SelectedList_DragOver(object sender, DragEventArgs e)
        {
            e.Effects = DragDropEffects.Move;
        }

        private void SelectedList_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(string)))
            {
                string field = (string)e.Data.GetData(typeof(string));
                if (!((StatisticsViewModel)DataContext).AvailableFields.Contains(field))
                {
                    ((StatisticsViewModel)DataContext).AvailableFields.Add(field);
                    ((StatisticsViewModel)DataContext).SelectedFields.Remove(field);
                }
            }
        }

    }
}
