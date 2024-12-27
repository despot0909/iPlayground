using iPlayground.Core.Interfaces.Repositories;
using iPlayground.Core.Interfaces.Services;
using iPlayground.Core.Models;
using System.Windows;
using System;
using System.Windows.Controls;
using System.Windows.Threading;
using MaterialDesignThemes.Wpf;
using System.Threading.Tasks;
using iPlayground.Core.Services;
using iPlayground.WPF.Helpers;
using System.Linq;

namespace iPlayground.WPF.Views
{
    public partial class InActiveSessionsView : UserControl
    {
        private readonly ISessionService _sessionService;
        private readonly IParentService _parentService;
        private readonly IChildRepository _childRepository;
        private readonly IReceiptService _receiptService;
        private readonly DispatcherTimer _refreshTimer;
        private DateTime? _startDate;
        private DateTime? _endDate;
        // Prazan konstruktor za designer
        public InActiveSessionsView()
        {
            InitializeComponent();
        }

        // Glavni konstruktor sa DI
        public InActiveSessionsView(
            ISessionService sessionService,
            IParentService parentService,
            IChildRepository childRepository,
            IReceiptService receiptService) // Dodali smo IReceiptService
        {
            InitializeComponent();
            _sessionService = sessionService;
            _parentService = parentService;
            _childRepository = childRepository;
            _receiptService = receiptService;

            _refreshTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(5)
            };
            _refreshTimer.Tick += (s, e) => LoadInActiveSessions();

            Loaded += ActiveSessionsView_Loaded;
            StartAutoRefresh();
        }
        public async void LoadInActiveSessions()
        {
            try
            {
                var sessions = await _sessionService.GetInActiveSessionsAsync();

                // Filter by date range
                if (_startDate.HasValue && _endDate.HasValue)
                {
                    sessions = sessions.Where(s =>
                        s.StartTime.Date >= _startDate.Value.Date &&
                        s.StartTime.Date <= _endDate.Value.Date).ToList();
                }

                // Calculate total amounts
                var totalAmount = sessions.Sum(s => s.TotalAmount);
                var totalVouchers = sessions.Sum(s => s.TotalVaucer);

                // Update UI
                SessionsGrid.ItemsSource = sessions;
                TotalAmountTextBlock.Text = $"Ukupan Iznos: {totalAmount:N2} KM";
                TotalVoucherTextBlock.Text = $"Ukupan Voucher: {totalVouchers:N2} KM";
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Greška pri učitavanju sesija: {ex.Message}",
                    "Greška",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private void ApplyDateFilter_Click(object sender, RoutedEventArgs e)
        {
            if (DatePickerStart.SelectedDate != null && DatePickerEnd.SelectedDate != null)
            {
                _startDate = DatePickerStart.SelectedDate;
                _endDate = DatePickerEnd.SelectedDate;
                LoadInActiveSessions();
            }
            else
            {
                MessageBox.Show("Molimo odaberite oba datuma za filtriranje.", "Upozorenje",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
        private void StartAutoRefresh()
        {
            _refreshTimer.Start();
        }

        private void StopAutoRefresh()
        {
            _refreshTimer?.Stop();
        }

        private void ActiveSessionsView_Loaded(object sender, RoutedEventArgs e)
        {
            LoadInActiveSessions();
        }

   

        private async void EndSession_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.CommandParameter is Session session)
            {
                try
                {
                    var finalAmount = await _sessionService.CalculateSessionAmountAsync(session.Id);
                    var confirmWindow = new SecondScreenEndSessionWindow(session, finalAmount);

                    // Dodajemo blok za provjeru drugog ekrana
                    if (!DisplayHelper.HasSecondaryScreen())
                    {
                        MessageBox.Show("Drugi ekran nije pronađen. Molimo povežite drugi ekran.",
                            "Upozorenje", MessageBoxButton.OK, MessageBoxImage.Warning);
                       // return;
                    }

                    var result = await confirmWindow.ShowAsync();

                    if (result)
                    {
                        await _sessionService.EndSessionAsync(session.Id);

                        try
                        {
                            var receipt = await _receiptService.CreateReceiptAsync(session, true);
                            await _receiptService.PrintReceiptAsync(receipt);
                        }
                        catch (Exception printEx)
                        {
                            MessageBox.Show($"Greška pri štampanju računa: {printEx.Message}\nSesija je završena.",
                                "Upozorenje", MessageBoxButton.OK, MessageBoxImage.Warning);
                        }

                        LoadInActiveSessions();
                        MessageBox.Show("Sesija je uspješno završena!", "Uspjeh",
                            MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Greška: {ex.Message}", "Greška",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void AddNewChild_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = Window.GetWindow(this) as MainWindow;
            mainWindow?.NewChild_Click(sender, e);
        }

        // Cleanup
        public void Dispose()
        {
            StopAutoRefresh();
            _refreshTimer?.Stop();
        }
    }
}