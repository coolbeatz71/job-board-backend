using Core.Domain;
using Job.Domain.Jobs.Enums;

namespace Job.Domain.Jobs.Entities;

/// <summary>
/// Represents a job listing in the system with support for domain events and state management.
/// </summary>
/// <remarks>
/// This entity is an aggregate root with a unique identifier and raises domain events on creation and updates.
/// Manages job postings including company information, work arrangements, and application deadlines.
/// </remarks>
public class JobEntity: Aggregate<Guid>
{
    /// <summary>
    /// Gets the title of the job position.
    /// </summary>
    /// <value>The job title, such as "Senior Software Developer" or "Marketing Manager".</value>
    public string Title { get; private set; } = null!;
    
    /// <summary>
    /// Gets the detailed description of the job responsibilities and role.
    /// </summary>
    /// <value>A comprehensive description outlining the job duties, expectations, and role overview.</value>
    public string Description { get; private set; } = null!;

    /// <summary>
    /// Gets the requirements and qualifications needed for the job position.
    /// </summary>
    /// <value>
    /// Optional requirements such as education, experience, skills, and certifications.
    /// Can be null if no specific requirements are defined.
    /// </value>
    public string? Requirements { get; private set; } = null;

    /// <summary>
    /// Gets the name of the company offering the job position.
    /// </summary>
    /// <value>The official company name, such as "Microsoft Corporation" or "Acme Inc."</value>
    public string CompanyName { get; private set; } = null!;

    /// <summary>
    /// Gets the company's official website URL.
    /// </summary>
    /// <value>The full URL to the company's website, including the protocol (e.g., "https://www.company.com").</value>
    public string CompanyWebsite { get; private set; } = null!;

    /// <summary>
    /// Gets the geographical location where the job is based.
    /// </summary>
    /// <value>The job location, which can be a city, state/region, country, or "Remote" for remote positions.</value>
    public string Location { get; private set; } = null!;

    /// <summary>
    /// Gets the work arrangement mode for this position.
    /// </summary>
    /// <value>
    /// The work mode such as Remote, OnSite, or Hybrid as defined by the <see cref="WorkMode"/> enumeration.
    /// </value>
    public WorkMode WorkMode { get; private set; }
    
    /// <summary>
    /// Gets the current status of the job listing.
    /// </summary>
    /// <value>
    /// The job status such as Active, Closed, or Draft as defined by the <see cref="JobStatus"/> enumeration.
    /// </value>
    public JobStatus Status { get; private set; }

    /// <summary>
    /// Gets the type of employment offered.
    /// </summary>
    /// <value>The employment type such as FullTime, PartTime, Contract, or Internship
    /// as defined by the <see cref="JobType"/> enumeration.
    /// </value>
    public JobType JobType { get; private set; }
    
    /// <summary>
    /// Gets the deadline for job applications.
    /// </summary>
    /// <value>
    /// The last date and time when applications will be accepted. 
    /// Can be null if there is no specific deadline.
    /// </value>
    public DateTime? ApplicationDeadline { get; private set; }

    /// <summary>
    /// Creates a new <see cref="JobEntity"/> with the specified properties and raises a <see cref="JobCreatedEvent"/>.
    /// </summary>
    /// <param name="id">The unique identifier of the job listing.</param>
    /// <param name="title">The title of the job position. Cannot be null or empty.</param>
    /// <param name="description">The detailed description of the job. Cannot be null or empty.</param>
    /// <param name="requirements">The job requirements and qualifications. Can be null.</param>
    /// <param name="companyName">The name of the hiring company. Cannot be null or empty.</param>
    /// <param name="companyWebsite">The company's website URL. Cannot be null or empty.</param>
    /// <param name="location">The job location. Cannot be null or empty.</param>
    /// <param name="workMode">The work arrangement mode.</param>
    /// <param name="status">The current status of the job listing.</param>
    /// <param name="jobType">The type of employment offered.</param>
    /// <param name="applicationDeadline">The deadline for applications. Cannot be in the past.</param>
    /// <returns>A new instance of <see cref="JobEntity"/>.</returns>
    /// <exception cref="ArgumentException">
    /// Thrown when <paramref name="title"/>, <paramref name="description"/>, 
    /// <paramref name="companyName"/>, <paramref name="companyWebsite"/>, or <paramref name="location"/> 
    /// is null or empty, or when <paramref name="applicationDeadline"/> is in the past.
    /// </exception>
    /// <example>
    /// <code>
    /// <![CDATA[
    /// var job = JobEntity.Create(
    ///     Guid.NewGuid(),
    ///     "Senior Software Developer",
    ///     "We are looking for an experienced developer to join our team...",
    ///     "5+ years of C# experience, Bachelor's degree preferred",
    ///     "Tech Solutions Inc.",
    ///     "https://www.techsolutions.com",
    ///     "New York, NY",
    ///     WorkMode.Hybrid,
    ///     JobStatus.Active,
    ///     JobType.FullTime,
    ///     DateTime.UtcNow.AddDays(30)
    /// );
    /// ]]>
    /// </code>
    /// </example>
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

    /// <summary>
    /// Updates the job listing properties and raises a <see cref="JobUpdatedEvent"/>.
    /// </summary>
    /// <param name="title">The updated job title. Cannot be null or empty.</param>
    /// <param name="description">The updated job description. Cannot be null or empty.</param>
    /// <param name="requirements">The updated job requirements. Can be null.</param>
    /// <param name="companyName">The updated company name. Cannot be null or empty.</param>
    /// <param name="companyWebsite">The updated company website URL. Cannot be null or empty.</param>
    /// <param name="location">The updated job location. Cannot be null or empty.</param>
    /// <param name="workMode">The updated work arrangement mode.</param>
    /// <param name="status">The updated job status.</param>
    /// <param name="jobType">The updated employment type.</param>
    /// <param name="applicationDeadline">The updated application deadline. Cannot be in the past.</param>
    /// <returns>The current <see cref="JobEntity"/> instance with updated properties.</returns>
    /// <exception cref="ArgumentException">
    /// Thrown when <paramref name="title"/>, <paramref name="description"/>, 
    /// <paramref name="companyName"/>, <paramref name="companyWebsite"/>, or <paramref name="location"/> 
    /// is null or empty, or when <paramref name="applicationDeadline"/> is in the past.
    /// </exception>
    /// <example>
    /// <code>
    /// <![CDATA[
    /// job.Update(
    ///     "Lead Software Developer",
    ///     "Updated job description with new responsibilities...",
    ///     "7+ years experience, team leadership skills required",
    ///     "Tech Solutions Inc.",
    ///     "https://www.techsolutions.com",
    ///     "New York, NY (Remote Available)",
    ///     WorkMode.Remote,
    ///     JobStatus.Active,
    ///     JobType.FullTime,
    ///     DateTime.UtcNow.AddDays(45)
    /// );
    /// ]]>
    /// </code>
    /// </example>
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