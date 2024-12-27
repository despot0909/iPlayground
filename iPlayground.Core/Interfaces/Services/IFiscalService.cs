// IParentService.cs
using iPlayground.Core.Models;
using System.Threading.Tasks;

namespace iPlayground.Core.Interfaces.Services
{
    public interface IFiscalService
    {
        Task<FiscalResponse> IssueFiscalReceiptAsync(FiscalRequest request);
        Task<FiscalResponse> IssueRefundReceiptAsync(FiscalRequest request, string originalReceiptNumber);
        Task<bool> ValidateReceiptAsync(string fiscalNumber);
    }

    public class FiscalRequest
    {
        public decimal Amount { get; set; }
        public string PaymentType { get; set; }
        public List<FiscalItem> Items { get; set; }
    }

    public class FiscalItem
    {
        public string Name { get; set; }
        public decimal Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TaxRate { get; set; }
    }

    public class FiscalResponse
    {
        public string FiscalNumber { get; set; }
        public DateTime ProcessTime { get; set; }
        public string QRCode { get; set; }
        public string Journal { get; set; }
    }
}