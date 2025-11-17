using AdessoTurkey.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AdessoTurkey.Persistence.Configurations
{
    public class DrawTeamConfiguration : IEntityTypeConfiguration<DrawTeam>
    {
        public void Configure(EntityTypeBuilder<DrawTeam> builder)
        {
            builder.ToTable("DrawTeams");

            builder.HasKey(dt => dt.Id);

            builder.Property(dt => dt.DrawGroupId)
                .IsRequired();

            builder.Property(dt => dt.TeamId)
                .IsRequired();

            builder.Property(dt => dt.CreatedAt)
                .IsRequired();

            builder.Property(dt => dt.UpdatedAt)
                .IsRequired(false);

            builder.HasOne(dt => dt.DrawGroup)
                .WithMany(g => g.Teams)
                .HasForeignKey(dt => dt.DrawGroupId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(dt => dt.Team)
                .WithMany(t => t.DrawTeams)
                .HasForeignKey(dt => dt.TeamId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(dt => new { dt.DrawGroupId, dt.TeamId })
                .IsUnique()
                .HasDatabaseName("IX_DrawTeams_DrawGroupId_TeamId");
        }
    }
}
