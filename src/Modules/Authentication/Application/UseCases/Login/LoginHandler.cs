using Authentication.Application.Services;
using Authentication.Domain.Users.Dtos;
using Authentication.Domain.Users.ValueObjects;
using Authentication.Infrastructure;
using Core.Application.CQRS;
using Core.Application.Exceptions;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Authentication.Application.UseCases.Login;

/// <summary>
/// Handles the <see cref="LoginCommand"/> to authenticate a user.
/// </summary>
/// <param name="dbContext">The database context for data access.</param>
/// <param name="passwordService">Service for verifying hashed passwords.</param>
/// <param name="jwtService">Service for generating JWT tokens.</param>
public class LoginHandler(
    AuthenticationDbContext dbContext, 
    IPasswordService passwordService,
    IJwtService jwtService
): ICommandHandler<LoginCommand, LoginResult>
{
    /// <summary>
    /// Handles the login command by authenticating the user and returning authentication details.
    /// </summary>
    /// <param name="command">The login command containing user credentials.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    /// <returns>A <see cref="LoginResult"/> containing authentication information.</returns>
    /// <exception cref="BadRequestException">Thrown when credentials are invalid or the account is inactive.</exception>
    public async Task<LoginResult> Handle(LoginCommand command, CancellationToken cancellationToken)
    {
        // normalize email using value object
        var email = new Email(command.Email);
        
        var user = await dbContext.Users
            .FirstOrDefaultAsync(
                x => EF.Functions.ILike(x.Email, email),
                cancellationToken
            );

        if (user == null) throw new BadRequestException("Invalid email or password.");

        // verify password
        if (!passwordService.Verify(command.Password, user.PasswordHash))
        {
            throw new BadRequestException("Invalid email or password.");
        }
        
        var token = jwtService.GenerateToken(
            userId: user.Id, 
            email: email, 
            userRole: user.Role
        );
        
        var userDto = user.Adapt<UserResponseDto>();
        var authResult = new AuthenticationResult(userDto, token);

        return new LoginResult(authResult);
    }
}
