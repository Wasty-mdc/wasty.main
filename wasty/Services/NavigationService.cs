using System;
using System.Windows;
using System.Windows.Controls;

namespace wasty.Services
{
    public class NavigationService
    {
        private readonly Func<Type, UserControl> _viewFactory;

        public NavigationService(Func<Type, UserControl> viewFactory)
        {
            _viewFactory = viewFactory;
        }

        public void NavigateTo<TView>() where TView : UserControl
        {
            var view = _viewFactory(typeof(TView));

            var mainWindow = (MainWindow)Application.Current.MainWindow;
            mainWindow.CurrentView.Content = view;
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