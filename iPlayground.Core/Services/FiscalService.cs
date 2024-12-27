using iPlayground.Core.Interfaces.Repositories;
using iPlayground.Core.Interfaces.Services;
using iPlayground.Core.Models;
using Microsoft.Extensions.Configuration;
public class FiscalService : IFiscalService
{
    private readonly IBaseRepository<Receipt> _receiptRepository;
     private readonly ISessionService _sessionService;
    private readonly IConfiguration _configuration;
    private readonly decimal _hourlyRate = 5.0m; // Default vrijednost
    private readonly decimal _taxRate = 0.17m;   // Default vrijednost

    public FiscalService(
        IBaseRepository<Receipt> receiptRepository,
         ISessionService sessionService,
        IConfiguration configuration)
    {
        _receiptRepository = receiptRepository;
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
        var html = await GenerateReceiptHtmlAsync(receipt);
        // TODO: Implementirati štampanje
        throw new NotImplementedException();
    }

    public Task<FiscalResponse> IssueFiscalReceiptAsync(FiscalRequest request)
    {
        throw new NotImplementedException();
    }

    public Task<FiscalResponse> IssueRefundReceiptAsync(FiscalRequest request, string originalReceiptNumber)
    {
        throw new NotImplementedException();
    }

    public Task<bool> ValidateReceiptAsync(string fiscalNumber)
    {
        throw new NotImplementedException();
    }
}