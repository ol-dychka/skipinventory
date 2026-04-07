using System;

namespace Domain.StaticClasses;

public static class JoinRequestStatus
{
    public const string Pending = "Pending";
    public const string Accepted = "Accepted";
    public const string Rejected = "Rejected";

    public static readonly string[] All = [Pending, Accepted, Rejected];
}
