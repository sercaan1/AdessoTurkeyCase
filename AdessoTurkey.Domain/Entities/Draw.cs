using AdessoTurkey.Domain.Common;

namespace AdessoTurkey.Domain.Entities
{
    public class Draw : BaseEntity
    {
        public string DrawerFirstName { get; set; } = string.Empty;
        public string DrawerLastName { get; set; } = string.Empty;
        public int NumberOfGroups { get; set; }
        public DateTime DrawDate { get; set; }
        public virtual ICollection<DrawGroup> Groups { get; set; } = new List<DrawGroup>();
    }
}
