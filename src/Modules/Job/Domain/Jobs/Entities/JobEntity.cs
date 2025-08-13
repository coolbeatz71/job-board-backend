using Core.Domain;
using Job.Domain.Jobs.Enums;

namespace Job.Domain.Jobs.Entities;

public class JobEntity: Aggregate<Guid>
{
    /// <summary>
    /// Title of the job position.
    /// </summary>
    public string Title { get; private set; } = null!;
    
    /// <summary>
    /// Description of the job.
    /// </summary>
    public string Description { get; private set; } = null!;

    /// <summary>
    /// Requirements for the job.
    /// </summary>
    public string? Requirements { get; private set; } = null;

    /// <summary>
    /// Name of the company offering the job.
    /// </summary>
    public string CompanyName { get; private set; } = null!;

    /// <summary>
    /// Company website URL.
    /// </summary>
    public string CompanyWebsite { get; private set; } = null!;

    /// <summary>
    /// Location of the job.
    /// </summary>
    public string Location { get; private set; } = null!;

    /// <summary>
    /// Work arrangement type.
    /// </summary>
    public WorkMode WorkMode { get; private set; }
    
    /// <summary>
    /// Current status of the job.
    /// </summary>
    public JobStatus Status { get; private set; }

    /// <summary>
    /// Type of employment.
    /// </summary>
    public JobType JobType { get; private set; }
    
    /// <summary>
    /// Deadline for applications.
    /// </summary>
    public DateTime? ApplicationDeadline { get; private set; }

    public static JobEntity Create(
        Guid id,
        string title,
        string description,
        string? requirements,
        string companyName,
        string companyWebsite,
        string location,
        WorkMode workMode,
        JobStatus status,
        JobType jobType,
        DateTime? applicationDeadline
    )
    {
        ArgumentException.ThrowIfNullOrEmpty(title);
        ArgumentException.ThrowIfNullOrEmpty(description);
        ArgumentException.ThrowIfNullOrEmpty(companyName);
        ArgumentException.ThrowIfNullOrEmpty(companyWebsite);
        ArgumentException.ThrowIfNullOrEmpty(location);
        
        if (applicationDeadline.HasValue && applicationDeadline.Value < DateTime.UtcNow)
        {
            throw new ArgumentException("Application deadline cannot be in the past.", nameof(applicationDeadline));
        }

        var job = new JobEntity
        {
            Id = id,
            Title = title,
            Description = description,
            Requirements = requirements,
            CompanyName = companyName,
            CompanyWebsite = companyWebsite,
            Location = location,
            WorkMode = workMode,
            Status = status,
            JobType = jobType,
            ApplicationDeadline = applicationDeadline
        };
    
        job.AddDomainEvent(new JobCreatedEvent(job));
        return job;
    }

    public JobEntity Update(
        string title,
        string description,
        string? requirements,
        string companyName,
        string companyWebsite,
        string location,
        WorkMode workMode,
        JobStatus status,
        JobType jobType,
        DateTime? applicationDeadline
    )
    {
        ArgumentException.ThrowIfNullOrEmpty(title);
        ArgumentException.ThrowIfNullOrEmpty(description);
        ArgumentException.ThrowIfNullOrEmpty(companyName);
        ArgumentException.ThrowIfNullOrEmpty(companyWebsite);
        ArgumentException.ThrowIfNullOrEmpty(location);
        
        if (applicationDeadline.HasValue && applicationDeadline.Value < DateTime.UtcNow)
        {
            throw new ArgumentException("Application deadline cannot be in the past.", nameof(applicationDeadline));
        }
        
        Title = title;
        Description = description;
        Requirements = requirements;
        CompanyName = companyName;
        CompanyWebsite = companyWebsite;
        Location = location;
        WorkMode = workMode;
        Status = status;    
        JobType = jobType;
        ApplicationDeadline = applicationDeadline;
        
        AddDomainEvent(new JobUpdatedEvent(this));
        return this;
    }
}