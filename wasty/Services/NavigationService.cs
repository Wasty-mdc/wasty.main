using System;
using System.Windows;
using System.Windows.Controls;

namespace wasty.Services
{
    public class NavigationService
    {
        private readonly Stack<object> _history = new Stack<object>();
        private object _currentView;
        private readonly Func<Type, object> _viewFactory;

        public NavigationService(Func<Type, object> viewFactory)
        {
            _viewFactory = viewFactory;
        }

        public void NavigateTo<TView>()
        {
            var view = _viewFactory(typeof(TView));

            if (view is Window window)
            {
                window.ShowDialog();
            }
            else if (view is UserControl userControl)
            {
                var mainWindow = (MainWindow)Application.Current.MainWindow;
                if (mainWindow != null)
                {
                    _history.Push(userControl);
                    mainWindow.CurrentView.Content = userControl;
                }
            }
        }
        public void NavigateTo<TView>(object parameter)
        {
            var view = _viewFactory(typeof(TView));

            if (view is IParameterReceiver parameterReceiver)
            {
                parameterReceiver.ReceiveParameter(parameter);
            }

            if (view is Window window)
            {
                window.Show();
            }
            else if (view is UserControl userControl)
            {
                var mainWindow = (MainWindow)Application.Current.MainWindow;
                if (mainWindow != null)
                {
                    _history.Push(userControl);
                    mainWindow.CurrentView.Content = userControl;
                }
            }
        }
        public void GoBack()
        {
            if (_history.Count > 1)
            {
                _history.Pop();
                var previousView = _history.Peek();
                var mainWindow = (MainWindow)Application.Current.MainWindow;
                if (mainWindow != null)
                {
                    mainWindow.CurrentView.Content = previousView;
                }
            }
        }
    }

    public interface IParameterReceiver
    {
        void ReceiveParameter(object parameter);
    }
}