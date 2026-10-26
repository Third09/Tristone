using Company.Shared.Models;

namespace Company.OfficeApp.Services;

/// <summary>Who is signed in on this PC right now. Pages read it to decide what to show.</summary>
public static class Session
{
    public static User? CurrentUser { get; private set; }
    public static DateTime? SignedInAt { get; private set; }

    public static void Start(User user)
    {
        CurrentUser = user;
        SignedInAt = DateTime.Now;
    }

    public static void End()
    {
        CurrentUser = null;
        SignedInAt = null;
    }
}
