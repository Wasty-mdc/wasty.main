using System;
using System.Windows;
using System.Windows.Controls;
using wasty.ViewModels;

namespace wasty.Services
{
    public class NavigationService
    {
        private readonly Func<Type, UserControl> _viewFactory;
        private readonly MainWindowViewModel _mainWindowViewModel;

        public NavigationService(Func<Type, UserControl> viewFactory, MainWindowViewModel mainWindowViewModel)
        {
            _viewFactory = viewFactory;
            _mainWindowViewModel = mainWindowViewModel;
        }

        public void NavigateTo<TView>() where TView : UserControl
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                var view = _viewFactory(typeof(TView));
                _mainWindowViewModel.CurrentView = view;
            });
        }

    }
}
