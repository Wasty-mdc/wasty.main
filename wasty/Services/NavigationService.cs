using System;
using System.Windows;
using System.Windows.Controls;

namespace wasty.Services
{
    public class NavigationService
    {
        private readonly Func<Type, object> _viewFactory;

        public NavigationService(Func<Type, object> viewFactory)
        {
            _viewFactory = viewFactory;
        }

        public void NavigateTo<TView>()
        {
            //var view = _viewFactory(typeof(TView));

            //var mainWindow = (MainWindow)Application.Current.MainWindow;
            //mainWindow.CurrentView.Content = view;

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
                    mainWindow.CurrentView.Content = userControl;
                }
            }
        }
        public void NavigateTo<TView>(object parameter) where TView : UserControl 
        {
            var view = _viewFactory(typeof(TView));

            if (view is IParameterReceiver parameterReceiver)
            {
                parameterReceiver.ReceiveParameter(parameter);
            }

            var mainWindow = (MainWindow)Application.Current.MainWindow;
            mainWindow.CurrentView.Content = view;
        }
    }

    public interface IParameterReceiver
    {
        void ReceiveParameter(object parameter);
    }
}