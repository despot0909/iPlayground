using iPlayground.WPF.Services.Interfaces;
using iPlayground.WPF.ViewModels;
using System.Diagnostics;
using System.Windows.Input;

public class MainViewModel : BaseViewModel
{
    private readonly INavigationService _navigationService;
    private object _currentView;

    public object CurrentView
    {
        get => _currentView;
        set
        {
            Debug.WriteLine($"Setting CurrentView to: {value?.GetType().Name}");
            SetProperty(ref _currentView, value);
        }
    }

    public ICommand ShowActiveSessionsCommand { get; }
    public ICommand ShowNewChildCommand { get; }
    public ICommand ShowReportsCommand { get; }

    public MainViewModel(INavigationService navigationService)
    {
        _navigationService = navigationService;

        ShowActiveSessionsCommand = new RelayCommand(() =>
        {
            Debug.WriteLine("ShowActiveSessionsCommand executed");
            _navigationService.NavigateTo<ActiveSessionsViewModel>();
        });

        ShowNewChildCommand = new RelayCommand(() =>
        {
            Debug.WriteLine("ShowNewChildCommand executed");
            _navigationService.NavigateTo<NewChildViewModel>();
        });

        ShowReportsCommand = new RelayCommand(() =>
        {
            Debug.WriteLine("ShowReportsCommand executed");
        });

        // Set initial view
        Debug.WriteLine("Setting initial view");
        _navigationService.NavigateTo<ActiveSessionsViewModel>();
    }
}