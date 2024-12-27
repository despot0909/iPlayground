// ISessionService.cs
using iPlayground.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace iPlayground.Core.Interfaces.Services
{
    public interface ISessionService
    {
        Task<Session> StartNewSessionAsync(Session session);

        Task<Session> StartSessionAsync(int childId);
        Task<Session> EndSessionAsync(int sessionId, decimal hr = 5.00M);
        Task<Session> UpdateSessionAsync(Session session);
        
        Task<IEnumerable<Session>> GetActiveSessionsAsync();
        Task<IEnumerable<Session>> GetInActiveSessionsAsync();
        Task<Session> GetSessionByIdAsync(int sessionId);
        Task<decimal> CalculateSessionAmountAsync(int sessionId, decimal hr=5.00M);
        Task<bool> ConfirmSessionEndAsync(int sessionId);
    }
}