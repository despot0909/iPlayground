using System.Collections.ObjectModel;
using System.Windows.Input;
using iPlayground.Core.Interfaces.Services;
using iPlayground.Core.Models;
using iPlayground.WPF.Services.Interfaces;

namespace iPlayground.WPF.ViewModels
{
    public class ActiveSessionsViewModel : BaseViewModel
    {
        private readonly ISessionService _sessionService;
        private readonly INavigationService _navigationService;
        private ObservableCollection<Session> _activeSessions;

        public ObservableCollection<Session> ActiveSessions
        {
            get => _activeSessions;
            set => SetProperty(ref _activeSessions, value);
        }

        public ICommand StartNewSessionCommand { get; }

        public ActiveSessionsViewModel(ISessionService sessionService, INavigationService navigationService)
        {
            _sessionService = sessionService;
            _navigationService = navigationService;
            _activeSessions = new ObservableCollection<Session>();

            StartNewSessionCommand = new RelayCommand(() => _navigationService.NavigateTo<NewChildViewModel>());

            LoadActiveSessions();
        }

        private async void LoadActiveSessions()
        {
            var sessions = await _sessionService.GetActiveSessionsAsync();
            ActiveSessions = new ObservableCollection<Session>(sessions);
        }
    }
}