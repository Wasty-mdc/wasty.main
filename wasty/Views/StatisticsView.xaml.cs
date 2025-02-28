using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using wasty.Models;
using wasty.Services;
using wasty.ViewModels;

namespace wasty.Views
{
    public partial class StatisticsView : UserControl, IParameterReceiver
    {
        public StatisticsView()
        {
            InitializeComponent();
            DataContext = ((App)Application.Current).Services.GetService(typeof(StatisticsViewModel));
        }

        public void ReceiveParameter(object parameter)
        {
            var viewModel = (StatisticsViewModel)DataContext;
            //viewModel.Initialize(parameter);
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

                    // Asegurar que se reusa la misma referencia en AvailableFields
                    if (!viewModel.AvailableFields.Any(f => f.Name == field.Name))
                    {
                        viewModel.AvailableFields.Add(field);
                    }
                }
                else if (viewModel.AvailableFields.Contains(field))
                {
                    viewModel.AvailableFields.Remove(field);

                    // Asegurar que se reusa la misma referencia en SelectedFields
                    if (!viewModel.SelectedFields.Any(f => f.Name == field.Name))
                    {
                        viewModel.SelectedFields.Add(field);
                    }
                }

                // Notificar cambios
                viewModel.OnPropertyChanged(nameof(viewModel.SelectedFields));
                viewModel.OnPropertyChanged(nameof(viewModel.AvailableFields));
            }
        }
    }
}
