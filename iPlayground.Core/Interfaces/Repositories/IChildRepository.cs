// IChildRepository.cs
using iPlayground.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace iPlayground.Core.Interfaces.Repositories
{
    public interface IChildRepository : IBaseRepository<Child>
    {
        Task<IEnumerable<Child>> GetChildrenByParentPhoneAsync(string phone);
        Task<Child> GetChildWithParentAsync(int childId);
    }
}