using Core.Infrastructure.Seed;
using Microsoft.EntityFrameworkCore;

namespace Job.Infrastructure.Seed;

public class JobDataSeeder(JobDbContext dbContext): IDataSeeder
{
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