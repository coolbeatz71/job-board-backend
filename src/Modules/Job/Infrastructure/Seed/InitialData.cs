using Bogus;
using Job.Domain.Jobs.Entities;
using Job.Domain.Jobs.Enums;

namespace Job.Infrastructure.Seed;

/// <summary>
/// Provides initial seed data for the job.
/// </summary>
public class InitialData
{
    private static readonly Faker Faker = new Faker();
    
    private static readonly Random Random = new();

    /// <summary>
    /// Gets a collection of randomly generated <see cref="JobEntity"/> instances for seeding.
    /// </summary>
    /// <remarks>
    /// Generates 5 sample jobs with random title, descriptions, requirements, etc using Bogus.
    /// </remarks>
    public static IEnumerable<JobEntity> Jobs =>
        Enumerable.Range(1, 5).Select(_ => JobEntity.Create(
            id: Guid.NewGuid(),
            title: Faker.Name.JobTitle(),
            description: Faker.Lorem.Paragraph(5),
            requirements: Faker.Lorem.Paragraph(2),
            companyName: Faker.Company.CompanyName(),
            companyWebsite: Faker.Internet.Url(),
            location: Faker.Address.City(),
            workMode: GetRandomValue<WorkMode>(),
            status: GetRandomValue<JobStatus>(),
            jobType: GetRandomValue<JobType>(),
            applicationDeadline: Faker.Date.Soon(45)
        ));
    
    /// <summary>
    /// Gets a random value from the specified enum type.
    /// </summary>
    /// <typeparam name="T">The enum type.</typeparam>
    /// <returns>A random value from the enum.</returns>
    /// <exception cref="ArgumentException">Thrown if <typeparamref name="T"/> is not an enum type.</exception>
    private static T GetRandomValue<T>() where T : struct, Enum
    {
        var values = Enum.GetValues<T>();
        return values[Random.Next(values.Length)];
    }
}