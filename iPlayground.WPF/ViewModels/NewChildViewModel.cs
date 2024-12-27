using System;
using System.Windows.Input;
using iPlayground.Core.Interfaces.Services;
using iPlayground.Core.Models;
using System.Threading.Tasks;
using iPlayground.WPF.Services.Interfaces;

namespace iPlayground.WPF.ViewModels
{
    public class NewChildViewModel : BaseViewModel
    {
        private readonly ISessionService _sessionService;
        private readonly IParentService _parentService;
        private readonly INavigationService _navigationService;

        private string _firstName;
        private string _lastName;
        private string _parentName;
        private string _parentPhone;

        public string FirstName
        {
            get => _firstName;
            set
            {
                SetProperty(ref _firstName, value);
                CommandManager.InvalidateRequerySuggested();
            }
        }

        public string LastName
        {
            get => _lastName;
            set
            {
                SetProperty(ref _lastName, value);
                CommandManager.InvalidateRequerySuggested();
            }
        }

        public string ParentName
        {
            get => _parentName;
            set
            {
                SetProperty(ref _parentName, value);
                CommandManager.InvalidateRequerySuggested();
            }
        }

        public string ParentPhone
        {
            get => _parentPhone;
            set
            {
                SetProperty(ref _parentPhone, value);
                CommandManager.InvalidateRequerySuggested();
            }
        }

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        public NewChildViewModel(
            ISessionService sessionService,
            IParentService parentService,
            INavigationService navigationService)
        {
            _sessionService = sessionService;
            _parentService = parentService;
            _navigationService = navigationService;

            SaveCommand = new RelayCommand(SaveAsync, CanSave);
            CancelCommand = new RelayCommand(Cancel);
        }

        private bool CanSave()
        {
            return !string.IsNullOrWhiteSpace(FirstName) &&
                   !string.IsNullOrWhiteSpace(LastName) &&
                   !string.IsNullOrWhiteSpace(ParentName) &&
                   !string.IsNullOrWhiteSpace(ParentPhone);
        }

        private async void SaveAsync()
        {
            try
            {
                var parent = await _parentService.CreateParentAsync(ParentName, ParentPhone);

                var child = new Child
                {
                    FirstName = FirstName,
                    LastName = LastName,
                    ParentId = parent.Id
                };

                // TODO: Implement child creation and session start

                ClearForm();
                _navigationService.NavigateTo<ActiveSessionsViewModel>();
            }
            catch (Exception ex)
            {
                // TODO: Show error message
            }
        }

        private void Cancel()
        {
            ClearForm();
            _navigationService.NavigateTo<ActiveSessionsViewModel>();
        }

        private void ClearForm()
        {
            FirstName = string.Empty;
            LastName = string.Empty;
            ParentName = string.Empty;
            ParentPhone = string.Empty;
        }
    }
}