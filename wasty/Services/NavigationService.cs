using System.Windows.Controls;
using wasty.ViewModels;

public class NavigationService
{
    private readonly Func<Type, UserControl> _viewFactory;
    private readonly MainWindowViewModel _mainWindowViewModel;

    public NavigationService(Func<Type, UserControl> viewFactory, MainWindowViewModel mainWindowViewModel)
    {
        _viewFactory = viewFactory;
        _mainWindowViewModel = mainWindowViewModel;
    }

    public UserControl NavigateTo<TView>() where TView : UserControl
    {
        var view = _viewFactory(typeof(TView));
        
        _mainWindowViewModel.CurrentView = view;
        
        return view;
    }
}
