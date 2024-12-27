// ParentService.cs
using iPlayground.Core.Interfaces.Repositories;
using iPlayground.Core.Interfaces.Services;
using iPlayground.Core.Models;
using System;
using System.Threading.Tasks;

namespace iPlayground.Core.Services
{
    public class ParentService : IParentService
    {
        private readonly IChildRepository _childRepository;
        private readonly IBaseRepository<Parent> _parentRepository;

        public ParentService(IChildRepository childRepository, IBaseRepository<Parent> parentRepository)
        {
            _childRepository = childRepository;
            _parentRepository = parentRepository;
        }

        public async Task<Parent> CreateParentAsync(string name, string phone)
        {
            var existingParent = await GetParentByPhoneAsync(phone);
            if (existingParent != null)
                return existingParent;

            var parent = new Parent
            {
                Name = name,
                Phone = phone,
                ContactInfo = phone,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _parentRepository.AddAsync(parent);
            await _parentRepository.SaveChangesAsync();

            return parent;
        }

        public async Task<Parent> GetParentByPhoneAsync(string phone)
        {
            var parents = await _parentRepository.FindAsync(p => p.Phone == phone);
            return parents.FirstOrDefault();
        }

        public async Task<Parent> GetParentWithChildrenAsync(int parentId)
        {
            // Pretpostavljamo da imamo metodu u ChildRepository koja vraća djecu za roditelja
            var children = await _childRepository.GetChildrenByParentPhoneAsync(parentId.ToString());
            var parent = await _parentRepository.GetByIdAsync(parentId);
            if (parent != null)
            {
                parent.Children = children.ToList();
            }
            return parent;
        }
    }
}