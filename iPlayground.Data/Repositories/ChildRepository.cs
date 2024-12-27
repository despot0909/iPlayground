// ChildRepository.cs
using iPlayground.Core.Interfaces.Repositories;
using iPlayground.Core.Models;
using iPlayground.Data.Context;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace iPlayground.Data.Repositories
{
    public class ChildRepository : BaseRepository<Child>, IChildRepository
    {
        public ChildRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Child>> GetChildrenByParentPhoneAsync(string phone)
        {
            return await _context.Children
                .Include(c => c.Parent)
                .Where(c => c.Parent.Phone == phone)
                .ToListAsync();
        }

        public async Task<Child> GetChildWithParentAsync(int childId)
        {
            return await _context.Children
                .Include(c => c.Parent)
                .FirstOrDefaultAsync(c => c.Id == childId);
        }
    }
}