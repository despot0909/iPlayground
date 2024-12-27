using iPlayground.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iPlayground.Core.Interfaces.Services
{
    public interface IVoucherService
    {
        decimal HourlyDiscountAmount { get; }
        Task<VoucherValidation> ValidateVoucherAsync(string qrCode);
        Task<decimal> ApplyVoucherToSessionAsync(int sessionId, VoucherValidation voucher);
        Task<List<VoucherValidation>> GetVouchersForSessionAsync(int sessionId);

    }
}
