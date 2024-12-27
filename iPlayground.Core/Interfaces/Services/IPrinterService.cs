using iPlayground.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iPlayground.Core.Interfaces.Services
{
    public interface IPrinterService
    {
        Task PrintReceiptAsync(Session session, decimal amount, string pn);
        Task PrintFiscalReceiptAsync(Session session, decimal amount, string pn);
        Task PrintNonFiscalReceiptAsync(Session session, decimal amount, string pn);
    }
}
