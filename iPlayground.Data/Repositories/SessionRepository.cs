// SessionRepository.cs
using iPlayground.Core.Interfaces.Repositories;
using iPlayground.Core.Models;
using iPlayground.Data.Context;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace iPlayground.Data.Repositories
{
    public class SessionRepository : BaseRepository<Session>, ISessionRepository
    {
        public SessionRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Session>> GetActiveSessionsAsync()
        {
            return await _context.Sessions
                .Include(s => s.Child)
                    .ThenInclude(c => c.Parent)
                .Where(s => !s.IsFinished)
                .OrderBy(s => s.StartTime)
                .ToListAsync();
        }
        public async Task<IEnumerable<Session>> GetInActiveSessionsAsync()
        {
            return await _context.Sessions
                .Include(s => s.Child)
                    .ThenInclude(c => c.Parent)
                .Where(s => s.IsFinished)
                .OrderBy(s => s.StartTime)
                .ToListAsync();
        }

        public async Task<Session> GetSessionWithChildAndParentAsync(int sessionId)
        {
            return await _context.Sessions
                .Include(s => s.Child)
                    .ThenInclude(c => c.Parent)
                .FirstOrDefaultAsync(s => s.Id == sessionId);
        }

        public async Task<IEnumerable<Session>> GetUnSyncedSessionsAsync()
        {
            return await _context.Sessions
                .Include(s => s.Child)
                .Where(s => !s.IsSynced && s.IsFinished)
                .OrderBy(s => s.EndTime)
                .ToListAsync();
        }
    }
}