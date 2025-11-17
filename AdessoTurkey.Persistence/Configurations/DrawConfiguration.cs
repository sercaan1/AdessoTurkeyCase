using AdessoTurkey.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AdessoTurkey.Persistence.Configurations
{
    public class DrawConfiguration : IEntityTypeConfiguration<Draw>
    {
        public void Configure(EntityTypeBuilder<Draw> builder)
        {
            builder.ToTable("Draws");

            builder.HasKey(d => d.Id);

            builder.Property(d => d.DrawerFirstName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(d => d.DrawerLastName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(d => d.NumberOfGroups)
                .IsRequired();

            builder.Property(d => d.DrawDate)
                .IsRequired();

            builder.Property(d => d.CreatedAt)
                .IsRequired();

            builder.Property(d => d.UpdatedAt)
                .IsRequired(false);

            builder.HasMany(d => d.Groups)
                .WithOne(g => g.Draw)
                .HasForeignKey(g => g.DrawId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(d => d.DrawDate)
                .HasDatabaseName("IX_Draws_DrawDate");
        }
    }
}
