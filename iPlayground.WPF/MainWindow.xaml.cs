using MaterialDesignThemes.Wpf;
using System.Windows;
using iPlayground.WPF.Views;
using iPlayground.Core.Interfaces.Services;
using iPlayground.Core.Models;
using System;
using iPlayground.Core.Interfaces.Repositories;

namespace iPlayground.WPF
{
    public partial class MainWindow : Window
    {
        private readonly ActiveSessionsView _activeSessionsView;
        private readonly InActiveSessionsView _inActiveSessionsView;
 
        private readonly ISessionService _sessionService;
        private readonly IParentService _parentService;
        private readonly IChildRepository _childRepository;


        public MainWindow(
            ActiveSessionsView activeSessionsView,
            InActiveSessionsView inActiveSessionsView,
            ISessionService sessionService,
            IParentService parentService,
            IChildRepository childRepository
             )
           
        {
            InitializeComponent();
            _activeSessionsView = activeSessionsView;
            _inActiveSessionsView = inActiveSessionsView;
            _sessionService = sessionService;
            _parentService = parentService;
            _childRepository = childRepository;

            // Set initial view
            MainContent.Content = _activeSessionsView;
        }

        private void ActiveSessions_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = _activeSessionsView;
        }

        public async void NewChild_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new NewChildDialog(_sessionService, _parentService, _childRepository);
            var result = await DialogHost.Show(dialog, "RootDialog");

            if (result is bool success && success)
            {
                _activeSessionsView?.LoadActiveSessions();
            }
        }

        private void Reports_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = _inActiveSessionsView;
        }

        
    }
}