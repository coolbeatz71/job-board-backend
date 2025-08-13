using Job.Domain.Jobs.Entities;
using Job.Domain.Jobs.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Job.Infrastructure.Configurations;

/// <summary>
/// Configures the entity properties and constraints for <see cref="JobEntity"/>.
/// </summary>
public class JobConfiguration: IEntityTypeConfiguration<JobEntity>
{
    /// <summary>
    /// Configures the schema for the <see cref="JobEntity"/> table.
    /// </summary>
    /// <param name="builder">The builder used to configure the entity type.</param>
    /// <remarks>
    /// Defines primary key, required fields, maximum lengths, and data types for the job entity.
    /// </remarks>
    public void Configure(EntityTypeBuilder<JobEntity> builder)
    {
        builder.HasKey(p => p.Id);
        builder.Property(j => j.Title).HasMaxLength(100).IsRequired();
        builder.Property(j => j.Description).HasMaxLength(300).IsRequired();
        builder.Property(j => j.Requirements).HasMaxLength(300);
        builder.Property(j => j.CompanyName).HasMaxLength(100).IsRequired();
        builder.Property(j => j.CompanyWebsite).HasMaxLength(50).IsRequired();
        builder.Property(j => j.Location).HasMaxLength(50).IsRequired();
        builder.Property(j => j.WorkMode).HasConversion<string>().IsRequired();
        builder.Property(j => j.Status).HasConversion<string>()
            .HasDefaultValue(JobStatus.Active)
            .HasSentinel(JobStatus.Active);
        builder.Property(j => j.JobType).HasConversion<string>().IsRequired();
        
        builder.Property(j => j.ApplicationDeadline).HasColumnType("timestamptz");

        // Add check constraint to ensure ApplicationDeadline is null or in the future
        builder.ToTable(t => t.HasCheckConstraint(
            "CK_Job_ApplicationDeadline_NotInPast",
            "(application_deadline IS NULL OR application_deadline >= NOW())"
        ));
    }
}