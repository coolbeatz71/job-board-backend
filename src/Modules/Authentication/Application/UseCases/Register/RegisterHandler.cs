using Authentication.Application.Services;
using Authentication.Domain.Users.Dtos;
using Authentication.Domain.Users.Entities;
using Authentication.Domain.Users.ValueObjects;
using Authentication.Infrastructure;
using Core.Application.CQRS;
using Core.Application.Exceptions;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Authentication.Application.UseCases.Register;

public class RegisterHandler(
    AuthenticationDbContext dbContext, 
    IPasswordService passwordService,
    IJwtService jwtService
): ICommandHandler<RegisterCommand, RegisterResult>
{
    /// <summary>
    /// Handles the registration command by creating a new user and returning authentication details.
    /// </summary>
    /// <param name="command">The registration command containing user details.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    /// <returns>A <see cref="RegisterResult"/> containing authentication information.</returns>
    /// <exception cref="BadRequestException">Thrown when email already exists or validation fails.</exception>
    public async Task<RegisterResult> Handle(RegisterCommand command, CancellationToken cancellationToken)
    {
        // normalize email using value object
        var role = new Role(command.Role);
        var email = new Email(command.Email);
        
        var existingUser = await dbContext.Users
            .FirstOrDefaultAsync(x => x.Email == email.Value, cancellationToken);

        if (existingUser != null) throw new BadRequestException("User with this email already exists.");
        
        var passwordHash = passwordService.Hash(command.Password);
        
        var user = UserEntity.Create(
            id: Guid.NewGuid(),
            email: email,
            passwordHash: passwordHash,
            role: role
        );
        
        dbContext.Users.Add(user);
        await dbContext.SaveChangesAsync(cancellationToken);

        var token = jwtService.GenerateToken(
            email: email,
            userId: user.Id,
            userRole: role
        );

        var userDto = user.Adapt<UserResponseDto>();
        var authResult = new AuthenticationResult(userDto, token);

        return new RegisterResult(authResult);
    }
}