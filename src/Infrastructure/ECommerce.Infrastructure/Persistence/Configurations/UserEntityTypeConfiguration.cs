using ECommerce.Domain.Shared.ValueObjects;
using ECommerce.Domain.Users.Entities;
using ECommerce.Domain.Users.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerce.Infrastructure.Persistence.Configurations;

public class UserEntityTypeConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> users)
    {
        users.ToTable("Users", SchemaNames.Users);
        
        users.Property(x => x.Id)
            .HasConversion(x => x.Value, x => new UserId(x));
        users.HasKey(x => x.Id);
        users.Property(x => x.Email).IsRequired()
            .HasConversion(x => x.Value, x => new Email(x));
        users.HasIndex(x => x.Email).IsUnique();
        users.Property(x => x.Role).IsRequired()
            .HasConversion(x => x.Value, x => new Role(x));
        users.Property("_passwordHash").HasColumnName("PasswordHash");
        users.Property("_refreshToken").HasColumnName("RefreshToken");
    }
}