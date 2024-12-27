using iPlayground.Core.Interfaces.Repositories;
using iPlayground.Core.Models;
using iPlayground.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iPlayground.Data.Repositories
{
    // iPlayground.Data/Repositories/SessionVoucherRepository.cs
    public class SessionVoucherRepository : BaseRepository<SessionVoucher>, ISessionVoucherRepository
    {
        public SessionVoucherRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<bool> IsVoucherUsedAsync(string fiscalNumber, string qrCode)
        {
            return await _context.Set<SessionVoucher>()
                .AnyAsync(v => v.FiscalNumber == fiscalNumber || v.QRCode == qrCode);
        }

        public async Task<List<SessionVoucher>> GetVouchersBySessionIdAsync(int sessionId)
        {
            return await _context.Set<SessionVoucher>()
                .Where(v => v.SessionId == sessionId)
                .OrderByDescending(v => v.ScanTime)
                .ToListAsync();
        }

        public async Task<decimal> GetTotalDiscountForSessionAsync(int sessionId)
        {
            return await _context.Set<SessionVoucher>()
                .Where(v => v.SessionId == sessionId && v.IsValid)
                .SumAsync(v => v.DiscountAmount);
        }
    }
}
