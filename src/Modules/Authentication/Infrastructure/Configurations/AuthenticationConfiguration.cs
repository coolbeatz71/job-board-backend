using Authentication.Domain.Users.Entities;
using Authentication.Domain.Users.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Authentication.Infrastructure.Configurations;

/// <summary>
/// Configures the entity properties, relationships, and constraints for <see cref="UserEntity"/>.
/// </summary>
public class AuthenticationConfiguration: IEntityTypeConfiguration<UserEntity>
{
    /// <summary>
    /// Configures the schema for the <see cref="UserEntity"/> table.
    /// </summary>
    /// <param name="builder">The builder used to configure the entity type.</param>
    /// <remarks>
    /// Defines primary key, required fields, column types, enum conversions, and unique constraints
    /// to ensure data integrity for job applications.
    /// </remarks>
    public void Configure(EntityTypeBuilder<UserEntity> builder)
    {
        builder.HasKey(u => u.Id);
        
        builder.Property(u => u.Email).HasColumnType("text").HasMaxLength(50).IsRequired();
        builder.HasIndex(u => u.Email).IsUnique();
        
        builder.Property(u => u.PasswordHash).HasColumnType("text").IsRequired();
        builder.Property(u => u.Role).HasConversion<string>()
            .HasDefaultValue(UserRole.JobSeeker)
            .HasSentinel(UserRole.JobSeeker);
    }
}