// iPlayground.Core/Models/MonthlyPass.cs
using iPlayground.Core.Models.Base;
using System;

namespace iPlayground.Core.Models
{
    public class MonthlyPass : BaseEntity
    {
        public int ChildId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal AmountPaid { get; set; }
        public string QrCode { get; set; }
        public bool IsActive { get; set; }

        // Navigation property
        public Child Child { get; set; }
    }
}