using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace Company.Shared;

/// <summary>
/// Password and username rules shared by the office app and the Website Admin app, so both always agree.
/// Passwords: at least 8 characters with a letter and a number. Usernames: 3 to 50 letters, numbers, dots,
/// dashes or underscores, no spaces (uniqueness ignoring capital letters is checked against the database).
/// </summary>
public static partial class AccountRules
{
    public const int MinPasswordLength = 8;

    /// <summary>Null when the new password is acceptable, otherwise the reason.</summary>
    public static string? CheckNewPassword(string password, string confirm)
    {
        if (password.Length < MinPasswordLength) return $"The new password must be at least {MinPasswordLength} characters.";
        if (!password.Any(char.IsDigit) || !password.Any(char.IsLetter)) return "The new password must have at least one letter and one number.";
        if (password != confirm) return "The two passwords don't match.";
        return null;
    }

    /// <summary>A random 10-character password without look-alike characters (no 0/O, 1/l/I).</summary>
    public static string GeneratePassword()
    {
        const string letters = "abcdefghjkmnpqrstuvwxyzABCDEFGHJKLMNPQRSTUVWXYZ";
        const string digits = "23456789";
        var chars = new char[10];
        for (int i = 0; i < chars.Length; i++)
            chars[i] = i < 2 ? digits[RandomNumberGenerator.GetInt32(digits.Length)]
                             : letters[RandomNumberGenerator.GetInt32(letters.Length)];
        RandomNumberGenerator.Shuffle(chars.AsSpan());
        return new string(chars);
    }

    /// <summary>Null when the username's characters are acceptable, otherwise the reason.</summary>
    public static string? CheckUsername(string username) => UsernamePattern().IsMatch(username.Trim())
        ? null
        : "Username: 3 to 50 characters, only letters, numbers, dots, dashes and underscores (no spaces).";

    [GeneratedRegex(@"^[A-Za-z0-9._-]{3,50}$")]
    private static partial Regex UsernamePattern();
}
