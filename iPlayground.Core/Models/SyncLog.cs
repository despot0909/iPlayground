// iPlayground.Core/Models/SyncLog.cs
using iPlayground.Core.Models.Base;
using System;

namespace iPlayground.Core.Models
{
    public class SyncLog : BaseEntity
    {
        public DateTime SyncTime { get; set; }
        public string Status { get; set; }
        public string ErrorMessage { get; set; }
        public string SyncType { get; set; }
    }
}