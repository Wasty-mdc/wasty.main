using System;
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
            if (_mainWindowViewModel == null)
            {
                throw new Exception("MainWindowViewModel no está correctamente inicializado en NavigationService.");
            }

            var view = _viewFactory(typeof(TView));
            if (view == null)
            {
                throw new Exception($"No se pudo crear la vista {typeof(TView).Name}");
            }

            _mainWindowViewModel.CurrentView = view;
        }
    }
}
