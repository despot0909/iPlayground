using iPlayground.Core.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iPlayground.Core.Interfaces.Repositories
{
    // iPlayground.Core/Interfaces/Repositories/ISessionVoucherRepository.cs
    public interface ISessionVoucherRepository : IBaseRepository<SessionVoucher>
    {
        Task<List<SessionVoucher>> GetVouchersBySessionIdAsync(int sessionId);
        Task<decimal> GetTotalDiscountForSessionAsync(int sessionId);

    }

    
}
