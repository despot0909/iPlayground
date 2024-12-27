using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using iPlayground.Core.Interfaces.Repositories;
using iPlayground.Core.Interfaces.Services;
using iPlayground.Core.Models;
using MaterialDesignThemes.Wpf;

namespace iPlayground.WPF.Views
{
    public partial class NewChildDialog : UserControl
    {
        private readonly ISessionService _sessionService;
        private readonly IParentService _parentService;
        private readonly IChildRepository _childRepository;

        public NewChildDialog(
            ISessionService sessionService,
            IParentService parentService,
            IChildRepository childRepository)
        {
            InitializeComponent();
            _sessionService = sessionService;
            _parentService = parentService;
            _childRepository = childRepository;
        }
        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(FirstNameTextBox.Text) ||
                string.IsNullOrWhiteSpace(LastNameTextBox.Text) ||
                string.IsNullOrWhiteSpace(ParentNameTextBox.Text) ||
                string.IsNullOrWhiteSpace(ParentPhoneTextBox.Text))
            {
                MessageBox.Show("Sva polja su obavezna!", "Greška", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                // 1. Provjeri da li roditelj već postoji
                var parent = await _parentService.GetParentByPhoneAsync(ParentPhoneTextBox.Text);
                if (parent == null)
                {
                    parent = await _parentService.CreateParentAsync(
                        ParentNameTextBox.Text,
                        ParentPhoneTextBox.Text
                    );
                }

                // 2. Kreiraj dijete
                var child = new Child
                {
                    FirstName = FirstNameTextBox.Text,
                    LastName = LastNameTextBox.Text,
                    ParentId = parent.Id,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                await _childRepository.AddAsync(child);
                await _childRepository.SaveChangesAsync();

                // 3. Pokreni sesiju
                await _sessionService.StartSessionAsync(child.Id);

                // 4. Zatvori dialog

                DialogHost.CloseDialogCommand.Execute(null, null);

            }
            catch (Exception ex)
            {
                var innerMessage = ex.InnerException?.Message ?? ex.Message;
                MessageBox.Show($"Greška prilikom spremanja: {innerMessage}", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);

                // Za debugging
                Debug.WriteLine($"Error details: {ex}");
            }
        }



    }
}