
using Authentication.Domain.Users.Entities;
using Job.Domain.Jobs.Entities;
using JobApplication.Domain.JobApplications.Entities;
using JobApplication.Domain.JobApplications.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JobApplication.Infrastructure.Configurations;

/// <summary>
/// Configures the entity properties, relationships, and constraints for <see cref="JobApplicationEntity"/>.
/// </summary>
public class JobApplicationConfiguration : IEntityTypeConfiguration<JobApplicationEntity>
{
    /// <summary>
    /// Configures the schema for the <see cref="JobApplicationEntity"/> table.
    /// </summary>
    /// <param name="builder">The builder used to configure the entity type.</param>
    /// <remarks>
    /// Defines primary key, required fields, column types, enum conversions, and unique constraints
    /// to ensure data integrity for job applications.
    /// </remarks>
    public void Configure(EntityTypeBuilder<JobApplicationEntity> builder)
    {
        builder.HasKey(ja => ja.Id);
        // JobId references jobs in a different schema, so no FK constraint
        builder.Property(ja => ja.JobId).IsRequired();
        // ApplicantId references users in a different schema, so no FK constraint
        builder.Property(ja => ja.ApplicantId).IsRequired();
        builder.Property(ja => ja.Status)
               .HasConversion<string>()
               .HasDefaultValue(ApplicationStatus.Submitted)
               .HasSentinel(ApplicationStatus.Submitted)
               .IsRequired();
        builder.Property(ja => ja.CoverLetter).HasMaxLength(2000); 
        builder.Property(ja => ja.ResumeUrl).HasMaxLength(500).IsRequired();
        builder.Property(ja => ja.Notes).HasMaxLength(1000);
        builder.Property(ja => ja.ApplicationDate).HasColumnType("timestamptz");

        // Constraint: Application date cannot be in the future
        builder.ToTable(t => t.HasCheckConstraint(
            "CK_JobApplication_ApplicationDate_NotInFuture",
            "(application_date IS NULL OR application_date <= NOW())"
        ));

        // Constraint: An applicant can only apply once per job
        builder.HasIndex(ja => new { ja.JobId, ja.ApplicantId })
               .IsUnique()
               .HasDatabaseName("UX_JobApplication_JobId_ApplicantId");
    }
}
