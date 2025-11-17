using AdessoTurkey.Domain.Common;

namespace AdessoTurkey.Domain.Entities
{
    public class Team : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public virtual ICollection<DrawTeam> DrawTeams { get; set; } = new List<DrawTeam>();
    }
}
