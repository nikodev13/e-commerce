using ECommerce.ApplicationCore.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerce.Infrastructure.Persistence.Configurations;

public class CategoryEntityTypeConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.ToTable("Categories");
    
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id);

        builder.HasIndex(x => x.Name)
            .IsUnique();
    }
}