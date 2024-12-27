using iPlayground.Core.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iPlayground.Core.Models
{
    public class SessionVoucher : BaseEntity
    {
        public int SessionId { get; set; }
        public string FiscalNumber { get; set; }
        public string QRCode { get; set; }
        public decimal OriginalAmount { get; set; }
        public decimal DiscountAmount { get; set; }
        public DateTime ScanTime { get; set; }
        public bool IsValid { get; set; }

        // Navigation property
        public Session Session { get; set; }
    }
}
