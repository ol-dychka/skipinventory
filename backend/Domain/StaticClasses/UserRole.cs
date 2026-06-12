namespace Domain.StaticClasses;

public static class UserRole
{
    public const string Owner = "Owner";
    public const string Manager = "Manager";
    public const string Employee = "Employee";

    public static readonly string[] All = [Owner, Manager, Employee];

    public static bool HasResolveJoinRights(string role)
    {
        return role == Manager || role == Owner;
    }

    public static bool HasPromoteRights(string role)
    {
        return role == Owner;
    }

    public static bool HasMLServiceRights(string role)
    {
        return role == Owner || role == Manager;
    }
}
