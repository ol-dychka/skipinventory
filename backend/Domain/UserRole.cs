namespace Domain;

public static class UserRole
{
    public const string Owner = "Owner";
    public const string Manager = "Manager";
    public const string Employee = "Employee";

    public static readonly string[] All =
    [
        Owner,
        Manager,
        Employee
    ];
}