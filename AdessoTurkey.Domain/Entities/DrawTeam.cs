using AdessoTurkey.Domain.Common;

namespace AdessoTurkey.Domain.Entities
{
    public class DrawTeam : BaseEntity
    {
        public int DrawGroupId { get; set; }
        public int TeamId { get; set; }
        public virtual DrawGroup DrawGroup { get; set; } = null!;
        public virtual Team Team { get; set; } = null!;
    }
}
