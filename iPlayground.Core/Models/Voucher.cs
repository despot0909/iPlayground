// iPlayground.Core/Models/Voucher.cs
using iPlayground.Core.Models.Base;
using System;

namespace iPlayground.Core.Models
{
    public class Voucher : BaseEntity
    {
        public string FiscalNumber { get; set; }
        public string FiscalQRCode { get; set; }
        public decimal OriginalAmount { get; set; }
        public bool IsUsed { get; set; }
        public DateTime? UsedAt { get; set; }
        public int? SessionId { get; set; }
        public string JIB { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime ValidationDate { get; set; }
        public string ValidationMessage { get; set; }
        public bool IsValid { get; set; }

        // Navigation property
        public Session Session { get; set; }
    }
}