using iPlayground.Core.Models.Base;
using System;
using System.Collections.Generic;

namespace iPlayground.Core.Models
{
    public class Parent: BaseEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string ContactInfo { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // Navigation property
        public ICollection<Child> Children { get; set; } = new List<Child>();
    }
}