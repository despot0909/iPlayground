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
using Microsoft.Extensions.Configuration;

namespace iPlayground.WPF.Views
{
    public partial class InActiveSessionsView : UserControl
    {
        private readonly ISessionService _sessionService;
        private readonly IParentService _parentService;
        private readonly IChildRepository _childRepository;
        private readonly IReceiptService _receiptService;
        private readonly IConfiguration _configuration;
        private readonly DispatcherTimer _refreshTimer;
        private DateTime? _startDate;
        private DateTime? _endDate;

        public InActiveSessionsView(
            ISessionService sessionService,
            IParentService parentService,
            IChildRepository childRepository,
            IReceiptService receiptService,
            IConfiguration configuration)
        {
            InitializeComponent();
            _sessionService = sessionService;
            _parentService = parentService;
            _childRepository = childRepository;
            _receiptService = receiptService;
            _configuration = configuration;

            _refreshTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(30)
            };
            _refreshTimer.Tick += (s, e) => LoadInActiveSessions();

            Loaded += ActiveSessionsView_Loaded;
           
        }

        public async void LoadInActiveSessions()
        {
            try
            {
                var sessions = await _sessionService.GetInActiveSessionsAsync();

                // Filter by date range if specified
                if (_startDate.HasValue && _endDate.HasValue)
                {
                    sessions = sessions.Where(s =>
                        s.StartTime.Date >= _startDate.Value.Date &&
                        s.StartTime.Date <= _endDate.Value.Date).ToList();
                }
                else
                {
                    sessions = sessions.Where(s =>
                       s.StartTime.Date >= DateTime.Today).ToList();
                }

                // Calculate totals
                var totalAmount = sessions.Where(s => !s.IsStorno).Sum(s => s.TotalAmount);
                var totalVouchers = sessions.Where(s => !s.IsStorno).Sum(s => s.TotalVaucer);
                var stornoTotal = sessions.Where(s => s.IsStorno).Sum(s => s.TotalAmount);

                // Update UI
                SessionsGrid.ItemsSource = sessions;
                TotalAmountTextBlock.Text = $"{totalAmount:N2} KM";
                TotalVoucherTextBlock.Text = $"{totalVouchers:N2} KM";
                StornoTotalTextBlock.Text = $"{stornoTotal:N2} KM";
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

        public void Dispose()
        {
            StopAutoRefresh();
            _refreshTimer?.Stop();
        }
    }
}