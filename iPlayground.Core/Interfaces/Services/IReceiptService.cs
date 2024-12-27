// IParentService.cs
using iPlayground.Core.Models;
using System.Threading.Tasks;

namespace iPlayground.Core.Interfaces.Services
{
    public interface IReceiptService
    {
        Task<Receipt> CreateReceiptAsync(Session session, bool isFiscal = true);
        Task<Receipt> GetReceiptBySessionIdAsync(int sessionId);
        Task<Receipt> GetReceiptByFiscalNumberAsync(string fiscalNumber);
        Task<List<Receipt>> GetReceiptsByDateRangeAsync(DateTime start, DateTime end);
        Task<byte[]> GenerateReceiptPdfAsync(Receipt receipt);
        Task<string> GenerateReceiptHtmlAsync(Receipt receipt);
        Task PrintReceiptAsync(Receipt receipt, string printerName = null);
        Task EmailReceiptAsync(Receipt receipt, string emailAddress);
    }
}