using Core.Infrastructure.Seed;
using Microsoft.EntityFrameworkCore;

namespace Job.Infrastructure.Seed;

/// <summary>
/// Seeds initial jobs data into the database if no job records exist.
/// </summary>
/// <param name="dbContext">The catalog database context used for data access.</param>
public class JobDataSeeder(JobDbContext dbContext): IDataSeeder
{
    /// <summary>
    /// Seeds initial jobs data asynchronously if the database is empty.
    /// </summary>
    /// <returns>A task representing the asynchronous seed operation.</returns>
    /// <remarks>
    /// This method checks whether any job records exist.
    /// If none are found, it inserts initial jobs and saves changes.
    /// </remarks>
    public async Task SeedAllAsync()
    {
        // only seed if no job records exist in the DB
        if (!await dbContext.Jobs.AnyAsync())
        {
            await dbContext.Jobs.AddRangeAsync(InitialData.Jobs);
            await dbContext.SaveChangesAsync();
        }
    }
}