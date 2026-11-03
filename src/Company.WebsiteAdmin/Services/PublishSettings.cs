using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace Company.WebsiteAdmin.Services;

/// <summary>
/// Where Publish Website sends the website's content: the website's GitHub repository and branch (Netlify
/// rebuilds the live site from it in about a minute) and a GitHub token allowed to change that repository
/// (owner decision 2026-11-03: Publish pushes to GitHub, no server needed).
/// Saved per PC in C:\ProgramData\TriStone\OfficeApp\publishsettings.json, like the database settings; the token is
/// encrypted with Windows DPAPI, so the file is useless on another PC.
/// </summary>
public class PublishSettings
{
    public string Repository { get; set; } = "Third09/tristone_website";
    public string Branch { get; set; } = "Frontend";

    /// <summary>DPAPI-encrypted token, Base64. Never the plain token.</summary>
    public string EncryptedToken { get; set; } = "";

    static readonly string FilePath = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),
        "TriStone", "OfficeApp", "publishsettings.json");

    static readonly byte[] Entropy = Encoding.UTF8.GetBytes("TriStone.WebsiteAdmin.PublishSettings");

    [System.Text.Json.Serialization.JsonIgnore]
    public string Token
    {
        get
        {
            if (string.IsNullOrEmpty(EncryptedToken)) return "";
            try
            {
                var bytes = ProtectedData.Unprotect(Convert.FromBase64String(EncryptedToken), Entropy, DataProtectionScope.LocalMachine);
                return Encoding.UTF8.GetString(bytes);
            }
            catch (CryptographicException) { return ""; }   // file copied from another PC
        }
        set
        {
            var bytes = ProtectedData.Protect(Encoding.UTF8.GetBytes(value), Entropy, DataProtectionScope.LocalMachine);
            EncryptedToken = Convert.ToBase64String(bytes);
        }
    }

    /// <summary>True when this PC can publish (repository, branch and a token are set).</summary>
    [System.Text.Json.Serialization.JsonIgnore]
    public bool IsSet => Repository.Contains('/') && Branch != "" && Token != "";

    public static PublishSettings Load()
    {
        if (!File.Exists(FilePath)) return new PublishSettings();
        try { return JsonSerializer.Deserialize<PublishSettings>(File.ReadAllText(FilePath)) ?? new PublishSettings(); }
        catch (JsonException) { return new PublishSettings(); }
    }

    public void Save()
    {
        Directory.CreateDirectory(Path.GetDirectoryName(FilePath)!);
        File.WriteAllText(FilePath, JsonSerializer.Serialize(this, new JsonSerializerOptions { WriteIndented = true }));
    }
}
