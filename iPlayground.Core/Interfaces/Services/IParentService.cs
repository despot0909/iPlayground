// IParentService.cs
using iPlayground.Core.Models;
using System.Threading.Tasks;

namespace iPlayground.Core.Interfaces.Services
{
    public interface IParentService
    {
        Task<Parent> CreateParentAsync(string name, string phone);
        Task<Parent> GetParentByPhoneAsync(string phone);
        Task<Parent> GetParentWithChildrenAsync(int parentId);
    }
}