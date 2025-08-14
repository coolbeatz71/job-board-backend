namespace Authentication.Application.Authorization;

public class AuthorizationPolicies
{
    public const string AdminOnly = "AdminOnly";
    public const string EmployerOnly = "EmployerOnly";
    public const string JobSeekerOnly = "JobSeekerOnly";
    public const string AdminOrEmployer = "AdminOrEmployer";
    public const string AllRoles = "AllRoles";
}