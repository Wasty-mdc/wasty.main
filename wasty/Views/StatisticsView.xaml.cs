using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using wasty.ViewModels;

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
            DataContext = ((App)Application.Current).Services.GetService(typeof(StatisticsViewModel));
        }

        // Método para iniciar el arrastre de un campo
        private void Field_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                var field = ((Border)sender).DataContext as Field;
                if (field != null)
                {
                    DragDrop.DoDragDrop((DependencyObject)sender, field, DragDropEffects.Move);
                }
            }
        }

        // Permitir la acción de arrastre en la barra lateral
        private void SelectedFields_DragOver(object sender, DragEventArgs e)
        {
            e.Effects = DragDropEffects.Move;
        }

        // Manejo de la acción de soltar en la barra lateral (mueve el campo a los seleccionados)
        private void SelectedFields_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(Field)))
            {
                var field = (Field)e.Data.GetData(typeof(Field));
                var viewModel = (StatisticsViewModel)DataContext;

                if (!viewModel.SelectedFields.Contains(field))
                {
                    viewModel.SelectedFields.Add(field);
                    viewModel.AvailableFields.Remove(field);
                }
            }
        }
    }
}
