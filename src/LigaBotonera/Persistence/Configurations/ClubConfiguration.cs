using LigaBotonera.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LigaBotonera.Persistence.Configurations;
public class ClubConfiguration : IEntityTypeConfiguration<Club>
{
    public void Configure(EntityTypeBuilder<Club> builder)
    {
        builder.ToTable("Clubs");
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Id).HasColumnName("Id").ValueGeneratedOnAdd(); ;
        builder.Property(p => p.Name).HasColumnName("Name");
        builder.Property(p => p.FullName).HasColumnName("FullName");
        builder.Property(p => p.CityId).HasColumnName("CityId");

        builder.HasOne(c => c.City)
            .WithMany()
            .HasForeignKey(c => c.CityId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}