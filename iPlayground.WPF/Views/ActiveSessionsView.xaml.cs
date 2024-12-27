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
using Microsoft.Extensions.Configuration;
using System.ComponentModel;
using System.Linq;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Model;
using iPlayground.Data.Repositories;

namespace iPlayground.WPF.Views
{
    public partial class ActiveSessionsView : UserControl
    {
        private Session _currentScanningSession;

        private readonly ISessionService _sessionService;
        private readonly IParentService _parentService;
        private readonly IChildRepository _childRepository;
        private readonly IPrinterService _printerService;
        private readonly IReceiptService _receiptService;
        private readonly IVoucherService _voucherService;
        private readonly USBScannerService _scannerService;
        private readonly ISessionVoucherRepository _voucherRepository;


        private readonly DispatcherTimer _refreshTimer;
private readonly IConfiguration _configuration;

        // Prazan konstruktor za designer
        public ActiveSessionsView()
        {
            InitializeComponent();
        }

        // Glavni konstruktor sa DI
        public ActiveSessionsView(
            ISessionService sessionService,
            IParentService parentService,
            IChildRepository childRepository,
            IReceiptService receiptService,
            IConfiguration configuration,
            IPrinterService printerService,
             IVoucherService voucherService,
    USBScannerService scannerService,
    ISessionVoucherRepository voucherRepository) 
        {
            InitializeComponent();
            _sessionService = sessionService;
            _parentService = parentService;
            _childRepository = childRepository;
            _receiptService = receiptService;
            _configuration = configuration;
            _printerService = printerService;

            _voucherService = voucherService;
            _scannerService = scannerService;
            _voucherRepository = voucherRepository;

            // Inicijalizacija skenera
            _scannerService.DataScanned += OnDataScanned;
            _scannerService.StartListening();

            _refreshTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(5)
            };
            _refreshTimer.Tick += (s, e) => LoadActiveSessions();

            Loaded += ActiveSessionsView_Loaded;
            StartAutoRefresh();
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
            LoadActiveSessions();
        }
        private async void ShowVouchers_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.CommandParameter is Session session)
            {
                var dialog = new MaterialDesignThemes.Wpf.DialogHost();
                var content = new StackPanel { Margin = new Thickness(16) };

                var title = new TextBlock
                {
                    Text = "Primijenjeni vaučeri",
                    Style = (Style)FindResource("MaterialDesignHeadline6TextBlock"),
                    Margin = new Thickness(0, 0, 0, 16)
                };
                content.Children.Add(title);

                foreach (var voucher in session.Vouchers.OrderByDescending(v => v.ScanTime))
                {
                    var voucherInfo = new TextBlock
                    {
                        Text = $"Vaučer: {voucher.FiscalNumber}\n" +
                              $"Vrijeme: {voucher.ScanTime:dd.MM.yyyy HH:mm}\n" +
                              $"Umanjenje: {voucher.DiscountAmount:N2} KM\n" +
                              $"Originalni iznos: {voucher.OriginalAmount:N2} KM",
                        Margin = new Thickness(0, 0, 0, 8)
                    };
                    content.Children.Add(voucherInfo);
                }

                var closeButton = new Button
                {
                    Content = "Zatvori",
                    Style = (Style)FindResource("MaterialDesignFlatButton"),
                    Command = MaterialDesignThemes.Wpf.DialogHost.CloseDialogCommand
                };
                content.Children.Add(closeButton);

                await MaterialDesignThemes.Wpf.DialogHost.Show(content, "RootDialog");
            }
        }
        public async void LoadActiveSessions()
        {
            try
            {
                var sessions = await _sessionService.GetActiveSessionsAsync();
                foreach (var session in sessions)
                {
                    session.OnPropertyChanged(nameof(session.Duration));
                    session.OnPropertyChanged(nameof(session.CompletedHours));
                    decimal hr = _configuration.GetValue<decimal>("Settings:HourlyRate", 5.00M);
                    session.TotalAmount = await _sessionService.CalculateSessionAmountAsync(session.Id,hr);
                }
                SessionsGrid.ItemsSource = sessions;
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
        private async void ScanVoucher_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.CommandParameter is Session session)
            {
                try
                {
                    _currentScanningSession = session;
                    if(session.TotalVaucer == 0 || session.TotalVaucer == null)
                    {
                        var voucher = new SessionVoucher
                        {
                            SessionId = _currentScanningSession.Id,
                            FiscalNumber = "123123"+ session.Id,
                            QRCode = "QR:"+ session.Id,
                            OriginalAmount =20.00M,
                            DiscountAmount = _voucherService.HourlyDiscountAmount,
                            ScanTime = DateTime.Now,
                            IsValid = true
                        };

                        await _voucherRepository.AddAsync(voucher);
                        await _voucherRepository.SaveChangesAsync();

                        // Osvježi prikaz
                    }

                    await _sessionService.UpdateSessionAsync(session).ConfigureAwait(false);



                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Greška pri inicijalizaciji skeniranja: {ex.InnerException}",
                        "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                    _currentScanningSession = null;
                }
            }
        }

        private void OnDataScanned(object sender, string scannedData)
        {
            MessageBox.Show(scannedData);
            Application.Current.Dispatcher.Invoke(async () =>
            {
                try
                {
                    if (_currentScanningSession != null)
                    {
                        var validation = await _voucherService.ValidateVoucherAsync(scannedData);

                        if (validation.IsValid)
                        {
                            var voucher = new SessionVoucher
                            {
                                SessionId = _currentScanningSession.Id,
                                FiscalNumber = validation.FiscalNumber,
                                QRCode = scannedData,
                                OriginalAmount = validation.Amount,
                                DiscountAmount = _voucherService.HourlyDiscountAmount,
                                ScanTime = DateTime.Now,
                                IsValid = true
                            };

                            await _voucherRepository.AddAsync(voucher);
                            await _voucherRepository.SaveChangesAsync();

                            // Osvježi prikaz
                            LoadActiveSessions();

                            MessageBox.Show($"Vaučer uspješno primijenjen. Umanjenje: {voucher.DiscountAmount:N2} KM",
                                "Uspjeh", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                        else
                        {
                            MessageBox.Show(validation.ErrorMessage ?? "Vaučer nije validan",
                                "Greška", MessageBoxButton.OK, MessageBoxImage.Warning);
                        }
                    }
                    _currentScanningSession = null;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Greška pri skeniranju: {ex.Message}",
                        "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                    _currentScanningSession = null;
                }
            });
        }

       
        private async void EndSession_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.CommandParameter is Session session)
            {
                try
                {
                    decimal hr = _configuration.GetValue<decimal>("Settings:HourlyRate", 5.00M);
                    string pn =  _configuration.GetValue<string>("PrinterSettings:Name", "iPlayPrinter");

                    var finalAmount = await _sessionService.CalculateSessionAmountAsync(session.Id, hr);


                    var confirmWindow = new SecondScreenEndSessionWindow(session, finalAmount);

                    if (!DisplayHelper.HasSecondaryScreen())
                    {
                        MessageBox.Show("Drugi ekran nije pronađen. Molimo povežite drugi ekran.",
                            "Upozorenje", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }

                    var result = await confirmWindow.ShowAsync();

                    if (result)
                    {
                        await _sessionService.EndSessionAsync(session.Id, hr);

                        try
                        {
                            // Štampanje fiskalnog računa
                            await _printerService.PrintFiscalReceiptAsync(session, finalAmount,pn);

                            // Štampanje kopije za arhivu
                            await _printerService.PrintNonFiscalReceiptAsync(session, finalAmount,pn);

                            LoadActiveSessions();
                            MessageBox.Show("Sesija je uspješno završena!", "Uspjeh",
                                MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                        catch (Exception printEx)
                        {
                            MessageBox.Show($"Greška pri štampanju računa: {printEx.Message}\nSesija je završena.",
                                "Upozorenje", MessageBoxButton.OK, MessageBoxImage.Warning);
                        }
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