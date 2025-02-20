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

        public UserControl NavigateTo<TView>() where TView : UserControl
        {
            var view = _viewFactory(typeof(TView));

            var mainWindow = (MainWindow)Application.Current.MainWindow;
            mainWindow.contentView.Content = view;

            return view; // Ahora devuelve la vista para que pueda asignarse a CurrentView
        }
    }
}
