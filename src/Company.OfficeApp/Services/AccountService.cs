using Company.OfficeApp.Data;
using Company.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace Company.OfficeApp.Services;

/// <summary>
/// My Account: the signed-in Admin changes their own password and security question.
/// Employees and Supervisors can't change either (Project Outline, Part 0); they ask an Admin.
/// Both changes need the current password, so nobody can do them on a PC someone left signed in.
/// </summary>
public static class AccountService
{
    /// <summary>Suggested questions; an Admin can also type their own.</summary>
    public static readonly string[] SecurityQuestions =
    [
        "What is the name of your first pet?",
        "In what city or town were you born?",
        "What is your mother's maiden name?",
        "What was the name of your elementary school?",
        "What was your childhood nickname?",
    ];

    public const int MinAnswerLength = 2;

    /// <summary>Returns an error message, or null when changed.</summary>
    public static async Task<string?> ChangeOwnPasswordAsync(string currentPassword, string newPassword, string confirm)
    {
        if (Session.CurrentUser is not { IsAdmin: true } me) return "Only Admins can change passwords. Please ask an Admin.";
        if (PasswordService.CheckNewPassword(newPassword, confirm) is { } problem) return problem;
        if (newPassword == currentPassword) return "The new password must be different from the current one.";

        using var db = Db.Open();
        var user = await db.Users.FirstAsync(u => u.UserId == me.UserId);
        if (!BCrypt.Net.BCrypt.Verify(currentPassword, user.PasswordHash)) return "The current password is not correct.";

        await PasswordService.ApplyNewPasswordAsync(db, user, newPassword, "Changed own password (My Account)");
        me.PasswordHash = user.PasswordHash;
        return null;
    }

    /// <summary>Saves the Admin's security question and answer. Returns an error message, or null when saved.</summary>
    public static async Task<string?> SetSecurityQuestionAsync(string currentPassword, string question, string answer)
    {
        if (Session.CurrentUser is not { IsAdmin: true } me) return "Only Admins use a security question.";

        question = question.Trim();
        if (question.Length == 0) return "Choose or type a security question.";
        if (question.Length > 200) return "The question is too long (200 characters at most).";
        if (PasswordService.NormalizeAnswer(answer).Length < MinAnswerLength) return "Type an answer.";

        using var db = Db.Open();
        var user = await db.Users.FirstAsync(u => u.UserId == me.UserId);
        if (!BCrypt.Net.BCrypt.Verify(currentPassword, user.PasswordHash)) return "The current password is not correct.";

        var oldQuestion = user.SecurityQuestion;
        user.SecurityQuestion = question;
        user.SecurityAnswerHash = BCrypt.Net.BCrypt.HashPassword(PasswordService.NormalizeAnswer(answer), workFactor: 11);
        user.UpdatedAt = DateTime.Now;

        // Edit history: the question only. The answer is never written anywhere readable.
        AuditService.Add(db, null, "users", user.UserId, user.FullName, AuditActions.Edited,
            new { security_question = oldQuestion },
            new { security_question = question, security_answer = "(changed)" });
        await db.SaveChangesAsync();

        me.SecurityQuestion = user.SecurityQuestion;
        me.SecurityAnswerHash = user.SecurityAnswerHash;
        return null;
    }
}
