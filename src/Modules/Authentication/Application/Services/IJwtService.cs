using Authentication.Domain.Users.Enums;

namespace Authentication.Application.Services;

public interface IJwtService
{
    string GenerateToken(string email, UserRole userRole, Guid userId);
}