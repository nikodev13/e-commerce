using ECommerce.ApplicationCore.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerce.Infrastructure.Persistence.Configurations;

public class UserEntityTypeConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> users)
    {
        users.ToTable("Users");
        
        users.HasKey(x => x.Id);
        users.Property(x => x.Email).IsRequired();
        users.HasIndex(x => x.Email).IsUnique();
        users.Property(x => x.Role).IsRequired();
        users.Property(x => x.PasswordHash).IsRequired();
        users.Property(x => x.RefreshToken).IsRequired(false);
    }
}