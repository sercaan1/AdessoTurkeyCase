using AdessoTurkey.Domain.Common;

namespace AdessoTurkey.Domain.Entities
{
    public class DrawGroup : BaseEntity
    {
        public int DrawId { get; set; }
        public string GroupName { get; set; } = string.Empty;
        public virtual Draw Draw { get; set; } = null!;
        public virtual ICollection<DrawTeam> Teams { get; set; } = new List<DrawTeam>();
    }
}
