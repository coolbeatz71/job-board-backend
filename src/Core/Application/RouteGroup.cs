namespace Core.Application;

public class RouteGroup
{
    private static string Version => "api/v1";
    public static string Auth => $"{Version}/auth";
    
    public static string User => $"{Version}/users";
    public static string Job => $"{Version}/jobs";
}