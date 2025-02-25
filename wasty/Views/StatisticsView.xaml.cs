using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using wasty.Models;
using wasty.ViewModels;

namespace wasty.Views
{
    public partial class StatisticsView : UserControl
    {
        public StatisticsView()
        {
            InitializeComponent();
            DataContext = ((App)Application.Current).Services.GetService(typeof(StatisticsViewModel));
        }

        private void OnFieldMouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed && sender is Border border && border.DataContext is Field field)
            {
                DragDrop.DoDragDrop(border, field, DragDropEffects.Move);
            }
        }

        private void OnFieldsListDragOver(object sender, DragEventArgs e)
        {
            e.Effects = DragDropEffects.Move;
            e.Handled = true;
        }

        private void OnFieldsListDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(Field)))
            {
                var field = (Field)e.Data.GetData(typeof(Field));
                var viewModel = (StatisticsViewModel)DataContext;

                if (viewModel.SelectedFields.Contains(field))
                {
                    viewModel.SelectedFields.Remove(field);
                    viewModel.AvailableFields.Add(field);
                }
                else if (viewModel.AvailableFields.Contains(field))
                {
                    viewModel.AvailableFields.Remove(field);
                    viewModel.SelectedFields.Add(field);
                }
            }
        }
    }
}
