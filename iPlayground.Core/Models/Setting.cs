using iPlayground.Core.Models.Base;

namespace iPlayground.Core.Models
{
    public class Setting : BaseEntity
    {
        public string Key { get; set; }
        public string Value { get; set; }
        public string Description { get; set; }
    }
}