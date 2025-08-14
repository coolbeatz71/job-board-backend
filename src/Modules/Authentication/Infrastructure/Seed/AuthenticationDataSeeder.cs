using Authentication.Application.Services;
using Core.Infrastructure.Seed;
using Microsoft.EntityFrameworkCore;

namespace Authentication.Infrastructure.Seed;

public class AuthenticationDataSeeder(
    AuthenticationDbContext dbContext, 
    IPasswordService passwordService
): IDataSeeder
{
    /// <summary>
    /// Seeds initial user data asynchronously if the database is empty.
    /// </summary>
    /// <returns>A task representing the asynchronous seed operation.</returns>
    /// <remarks>
    /// This method checks whether any user records exist.
    /// If none are found, it inserts initial user and saves changes.
    /// </remarks>
    public async Task SeedAllAsync()
    {
        // only seed if no job records exist in the DB
        if (!await dbContext.Users.AnyAsync())
        {
            await dbContext.Users.AddRangeAsync(InitialData.Users(passwordService));
            await dbContext.SaveChangesAsync();
        }
    }
}