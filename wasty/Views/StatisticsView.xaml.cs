﻿using System.Collections.Specialized;
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

            var viewModel = (StatisticsViewModel)DataContext;
            //viewModel.SelectedFields.CollectionChanged += OnSelectedFieldsChanged;
        }

        public void ReceiveParameter(object parameter)
        {
            var viewModel = (StatisticsViewModel)DataContext;
            viewModel.Init(parameter.ToString());
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

        private async void OnFieldsListDrop(object sender, DragEventArgs e)
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
                        await viewModel.OnSelectedFieldsChanged(field.Name, "REMOVE");
                        viewModel.AvailableFields.Add(field);
                    }
                }
                else if (viewModel.AvailableFields.Contains(field))
                {
                    viewModel.AvailableFields.Remove(field);

                    // Asegurar que se reusa la misma referencia en SelectedFields
                    if (!viewModel.SelectedFields.Any(f => f.Name == field.Name))
                    {
                        await viewModel.OnSelectedFieldsChanged(field.Name, "ADD");
                        viewModel.SelectedFields.Add(field);
                    }
                }

                // Notificar cambios
                //viewModel.OnPropertyChanged(nameof(viewModel.SelectedFields));
                //viewModel.OnPropertyChanged(nameof(viewModel.AvailableFields));
            }
        }
        private void OnSelectedFieldsChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            var viewModel = (StatisticsViewModel)DataContext;
            //viewModel.OnSelectedFieldsChanged(e);
        }




        ///////////////////////////////////////////////////////77
        private Point _startPoint;
        private void OnFieldMouseMove2(object sender, MouseEventArgs e)
        {
            Point mousePos = e.GetPosition(null);
            Vector diff = _startPoint - mousePos;

            if (e.LeftButton == MouseButtonState.Pressed &&
                (Math.Abs(diff.X) > SystemParameters.MinimumHorizontalDragDistance ||
                    Math.Abs(diff.Y) > SystemParameters.MinimumVerticalDragDistance))
            {
                // Get the dragged item
                var border = sender as Border;
                if (border == null) return;

                var data = border.DataContext;

                // Initialize the drag & drop operation
                DragDrop.DoDragDrop(border, data, DragDropEffects.Move);
            }
        }

        private void OnFieldDragOver(object sender, DragEventArgs e)
        {
            e.Effects = DragDropEffects.Move;
            e.Handled = true;
        }

        private void OnFieldDrop(object sender, DragEventArgs e)
        {
            //if (e.Data.GetDataPresent(typeof(Field)))
            //{
            //    var droppedData = e.Data.GetData(typeof(Field)) as Field;
            //    var target = ((Border)sender).DataContext as Field;

            //    // Handle the drop logic here, e.g., reordering items in the collection
            //    var viewModel = DataContext as Field;
            //    viewModel.MoveItem(droppedData, target);
            //}
        }
        private void OnFieldMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _startPoint = e.GetPosition(null);
        }
    }
}
