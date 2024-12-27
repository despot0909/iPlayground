using iPlayground.Core.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iPlayground.Core.Models
{
    // iPlayground.Core/Models/VoucherValidation.cs
    public class VoucherValidation :  BaseEntity
    {
        public string FiscalNumber { get; set; }
        public decimal Amount { get; set; }
        public string JIB { get; set; }
        public bool IsValid { get; set; }
        public string ErrorMessage { get; set; }
        public DateTime ValidationTime { get; set; }
        public string QRCode { get; set; }
    }
}
