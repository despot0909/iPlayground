// ISessionRepository.cs
using iPlayground.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace iPlayground.Core.Interfaces.Repositories
{
    public interface ISessionRepository : IBaseRepository<Session>
    {
        Task<IEnumerable<Session>> GetActiveSessionsAsync();

        Task<IEnumerable<Session>> GetInActiveSessionsAsync();
        Task<Session> GetSessionWithChildAndParentAsync(int sessionId);
        Task<IEnumerable<Session>> GetUnSyncedSessionsAsync();
    }
}