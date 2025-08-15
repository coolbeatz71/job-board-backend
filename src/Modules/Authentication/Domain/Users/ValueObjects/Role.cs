using Authentication.Domain.Users.Enums;

namespace Authentication.Domain.Users.ValueObjects;


public record Role
{
    public UserRole Value { get; init; }

    public Role(UserRole value)
    {
        if (!Enum.IsDefined(value)) throw new ArgumentException($"Invalid user role: {value}");
        
        Value = value;
    }

    public Role(string value)
    {
        if (!Enum.TryParse<UserRole>(value, true, out var parsed) || !Enum.IsDefined(parsed))
        {
            throw new ArgumentException($"Invalid user role: {value}");
        }

        Value = parsed;
    }

    public static implicit operator UserRole(Role role) => role.Value;
    public static implicit operator string(Role role) => role.Value.ToString().ToLowerInvariant();
    public static implicit operator Role(UserRole role) => new(role);
    public static implicit operator Role(string role) => new(role);
}