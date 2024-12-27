using iPlayground.Core.Interfaces.Repositories;
using iPlayground.Core.Interfaces.Services;
using iPlayground.Core.Models;
using Microsoft.Extensions.Configuration;
using System.Net.Sockets;

public class ReceiptService : IReceiptService
{
    private readonly IBaseRepository<Receipt> _receiptRepository;
    private readonly IFiscalService _fiscalService;
    private readonly ISessionService _sessionService;
    private readonly IConfiguration _configuration;
    private readonly decimal _hourlyRate = 5.0m; // Default vrijednost
    private readonly decimal _taxRate = 0.17m;   // Default vrijednost

    public ReceiptService(
        IBaseRepository<Receipt> receiptRepository,
        IFiscalService fiscalService,
        ISessionService sessionService,
        IConfiguration configuration)
    {
        _receiptRepository = receiptRepository;
        _fiscalService = fiscalService;
        _sessionService = sessionService;
        _configuration = configuration;
    }

    public async Task<Receipt> GetReceiptBySessionIdAsync(int sessionId)
    {
        var receipts = await _receiptRepository.FindAsync(r => r.SessionId == sessionId);
        return receipts.FirstOrDefault();
    }

    public async Task<Receipt> GetReceiptByFiscalNumberAsync(string fiscalNumber)
    {
        var receipts = await _receiptRepository.FindAsync(r => r.FiscalNumber == fiscalNumber);
        return receipts.FirstOrDefault();
    }

    public async Task<List<Receipt>> GetReceiptsByDateRangeAsync(DateTime start, DateTime end)
    {
        var receipts = await _receiptRepository.FindAsync(r =>
            r.IssueDate >= start && r.IssueDate <= end);
        return receipts.ToList();
    }

    public async Task<byte[]> GenerateReceiptPdfAsync(Receipt receipt)
    {
        // TODO: Implementirati PDF generisanje
        throw new NotImplementedException();
    }

    public async Task EmailReceiptAsync(Receipt receipt, string emailAddress)
    {
        // TODO: Implementirati slanje emaila
        throw new NotImplementedException();
    }

    public async Task<Receipt> CreateReceiptAsync(Session session, bool isFiscal = true)
    {
        var receipt = new Receipt
        {
            SessionId = session.Id,
            Amount = await _sessionService.CalculateSessionAmountAsync(session.Id),
            IssueDate = DateTime.Now,
            IsFiscal = isFiscal,
            CashierName = "SISTEM",
            CustomerName = $"{session.Child.FirstName} {session.Child.LastName}",
            CustomerPhone = session.Child.Parent.Phone
        };

        if (isFiscal && 1==2)
        {
            var duration = (decimal)Math.Ceiling((DateTime.Now - session.StartTime).TotalHours);
            var fiscalResponse = await _fiscalService.IssueFiscalReceiptAsync(new FiscalRequest
            {
                Amount = receipt.Amount,
                PaymentType = "CASH",
                Items = new List<FiscalItem>
                {
                    new FiscalItem
                    {
                        Name = "Usluga čuvanja djece",
                        Quantity = duration,
                        UnitPrice = _hourlyRate,
                        TaxRate = _taxRate
                    }
                }
            });

            receipt.FiscalNumber = fiscalResponse.FiscalNumber;
        }
        else
        {
            receipt.NonFiscalNumber = $"NF-{DateTime.Now:yyyyMMdd}-{session.Id}";
        }

        //await _receiptRepository.AddAsync(receipt);
       // await _receiptRepository.SaveChangesAsync();

        return receipt;
    }

    public async Task<string> GenerateReceiptHtmlAsync(Receipt receipt)
    {
        var template = await File.ReadAllTextAsync("Templates/Receipt.html");

        return template
            .Replace("{{Amount}}", receipt.Amount.ToString("N2"))
            .Replace("{{CustomerName}}", receipt.CustomerName)
            .Replace("{{Date}}", receipt.IssueDate.ToString("dd.MM.yyyy HH:mm"))
            .Replace("{{ReceiptNumber}}", receipt.IsFiscal ? receipt.FiscalNumber : receipt.NonFiscalNumber);
    }

    public async Task PrintReceiptAsync(Receipt receipt, string printerName = null)
    {
        // Load the HTML template
        var htmlTemplate = await File.ReadAllTextAsync("Templates/Receipt.html");

        // Replace placeholders in the template
        var filledTemplate = htmlTemplate
            .Replace("{{Amount}}", receipt.Amount.ToString("N2"))
            .Replace("{{CustomerName}}", receipt.CustomerName)
            .Replace("{{Date}}", receipt.IssueDate.ToString("dd.MM.yyyy HH:mm"))
            .Replace("{{ReceiptNumber}}", receipt.IsFiscal ? receipt.FiscalNumber : receipt.NonFiscalNumber);

        // Convert HTML to plain text for ESC/POS
        var receiptText = ConvertHtmlToPlainText(filledTemplate, receipt);

        // Generate ESC/POS commands
        var escPosData = GenerateEscPosCommands(receiptText);

        // Connect to the printer and send commands
        await SendToPrinterAsync(escPosData, printerName);
    }

    // Converts the HTML template to plain text for the receipt
    private string ConvertHtmlToPlainText(string htmlTemplate, Receipt receipt)
    {
        return $@"
    iPlayground
    Račun za usluge čuvanja djece
    --------------------------------
    Datum: {receipt.IssueDate:dd.MM.yyyy HH:mm}
    Broj računa: {(receipt.IsFiscal ? receipt.FiscalNumber : receipt.NonFiscalNumber)}
    Klijent: {receipt.CustomerName}
    --------------------------------
    Ukupno za naplatu: {receipt.Amount:F2} KM
    --------------------------------
    Hvala na povjerenju!";
    }

    // Generates ESC/POS commands from the plain text receipt
    private byte[] GenerateEscPosCommands(string receiptText)
    {
        var bytes = new List<byte>();

        // Initialize the printer
        bytes.AddRange(new byte[] { 0x1B, 0x40 }); // ESC @ (Initialize)

        // Add the receipt text
        bytes.AddRange(System.Text.Encoding.UTF8.GetBytes(receiptText));

        // Add cut command
        bytes.AddRange(new byte[] { 0x1D, 0x56, 0x41, 0x03 }); // GS V A 3 (Full cut)

        return bytes.ToArray();
    }

    // Sends ESC/POS commands to the printer
    private async Task SendToPrinterAsync(byte[] escPosData, string printerName = "//DESKTOP-LMVITWK/IGRAONICA")
    {
        // Configure the printer connection
        string printerIp = printerName ?? "192.168.0.100"; // Replace with actual printer IP or configuration
        int printerPort = 9100; // Default port for ESC/POS printers

        using (var client = new TcpClient())
        {
            try
            {
                await client.ConnectAsync(printerIp, printerPort);
                using (var stream = client.GetStream())
                {
                    await stream.WriteAsync(escPosData, 0, escPosData.Length);
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Failed to print receipt"+ex.Message, ex);
            }
        }
    }

}