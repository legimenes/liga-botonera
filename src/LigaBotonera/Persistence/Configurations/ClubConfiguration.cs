using LigaBotonera.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LigaBotonera.Persistence.Configurations;
public class ClubConfiguration : IEntityTypeConfiguration<Club>
{
    public void Configure(EntityTypeBuilder<Club> builder)
    {
        builder.ToTable("clubs");
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Id).HasColumnName("id");
        builder.Property(p => p.Name).HasColumnName("name");
        builder.Property(p => p.FullName).HasColumnName("fullname");
        builder.Property(p => p.City).HasColumnName("city");
        builder.Property(p => p.State).HasColumnName("state");
    }
}