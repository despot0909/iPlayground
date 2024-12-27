using iPlayground.Core.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iPlayground.Core.Models
{
    public class Receipt : BaseEntity
    {
        public int Id { get; set; }
        public int SessionId { get; set; }
        public string FiscalNumber { get; set; }
        public string NonFiscalNumber { get; set; }
        public decimal Amount { get; set; }
        public decimal VoucherDiscount { get; set; }
        public string PaymentType { get; set; }
        public DateTime IssueDate { get; set; }
        public bool IsFiscal { get; set; }
        public bool IsSynced { get; set; }
        public string CashierName { get; set; }
        public string CustomerName { get; set; }
        public string CustomerPhone { get; set; }
        public string Notes { get; set; }

        // Navigation properties
        public Session Session { get; set; }
    }
}
