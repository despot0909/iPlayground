// iPlayground.Core/Models/Base/BaseEntity.cs
using System;

namespace iPlayground.Core.Models.Base
{
    public abstract class BaseEntity
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}