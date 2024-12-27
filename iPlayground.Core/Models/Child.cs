using iPlayground.Core.Models.Base;
using System;

namespace iPlayground.Core.Models
{
    public class Child: BaseEntity
    {
        public int Id { get; set; }
        public int ParentId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // Navigation property
        public Parent Parent { get; set; }

        public string FullName => $"{FirstName} {LastName}";
    }
}