using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace Company.WebsiteAdmin.Services;

/// <summary>
/// Website Settings (owner decisions 2026-11-03, no server needed):
/// - where Publish Website sends the website's content: the website's GitHub repository and branch (Netlify
///   rebuilds the live site from it in about a minute) and a GitHub token allowed to change that repository;
/// - where Contact Us gets new inquiries: the website's Google Apps Script address and its read key.
/// Saved per PC in C:\ProgramData\TriStone\OfficeApp\publishsettings.json, like the database settings; the token and
/// the key are encrypted with Windows DPAPI, so the file is useless on another PC.
/// </summary>
public class PublishSettings
{
    public string Repository { get; set; } = "Third09/tristone_website";
    public string Branch { get; set; } = "Frontend";

    /// <summary>The live website (Netlify). Preview Website loads its design from here.</summary>
    public string SiteUrl { get; set; } = "https://tristone.netlify.app";

    /// <summary>DPAPI-encrypted token, Base64. Never the plain token.</summary>
    public string EncryptedToken { get; set; } = "";

    static readonly string FilePath = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),
        "TriStone", "OfficeApp", "publishsettings.json");

    static readonly byte[] Entropy = Encoding.UTF8.GetBytes("TriStone.WebsiteAdmin.PublishSettings");

    [System.Text.Json.Serialization.JsonIgnore]
    public string Token
    {
        get => Unprotect(EncryptedToken);
        set => EncryptedToken = Protect(value);
    }

    /// <summary>True when this PC can publish (repository, branch and a token are set).</summary>
    [System.Text.Json.Serialization.JsonIgnore]
    public bool IsSet => Repository.Contains('/') && Branch != "" && Token != "";

    // ----- Inquiries (owner decision 2026-11-03): the website's Google Apps Script keeps each Contact form message
    // in a Google Sheet; Contact Us reads them with this address and the script's read key (InquiryImport). -----

    /// <summary>The script's web address (the same one the website's contact.js posts to).</summary>
    public string InquiryScriptUrl { get; set; } =
        "https://script.google.com/macros/s/AKfycbyhOM-z6aTlpGJxKrzilQGESp7YLvnFMcEnwnNRKg7H8fxG8EmtEr7FPJtV3Tdxzbce7A/exec";

    /// <summary>DPAPI-encrypted read key, Base64.</summary>
    public string EncryptedInquiryKey { get; set; } = "";

    [System.Text.Json.Serialization.JsonIgnore]
    public string InquiryKey
    {
        get => Unprotect(EncryptedInquiryKey);
        set => EncryptedInquiryKey = Protect(value);
    }

    /// <summary>True when Contact Us can get new inquiries from the script.</summary>
    [System.Text.Json.Serialization.JsonIgnore]
    public bool InquiriesSet => Uri.IsWellFormedUriString(InquiryScriptUrl, UriKind.Absolute) && InquiryKey != "";

    static string Unprotect(string encrypted)
    {
        if (string.IsNullOrEmpty(encrypted)) return "";
        try { return Encoding.UTF8.GetString(ProtectedData.Unprotect(Convert.FromBase64String(encrypted), Entropy, DataProtectionScope.LocalMachine)); }
        catch (CryptographicException) { return ""; }   // file copied from another PC
    }

    static string Protect(string value) =>
        value == "" ? "" : Convert.ToBase64String(ProtectedData.Protect(Encoding.UTF8.GetBytes(value), Entropy, DataProtectionScope.LocalMachine));

    public static PublishSettings Load()
    {
        if (Company.OfficeApp.Data.AppEdition.IsDemo) return new PublishSettings();   // no token, no read key: nothing reaches the real website
        if (!File.Exists(FilePath)) return new PublishSettings();
        try { return JsonSerializer.Deserialize<PublishSettings>(File.ReadAllText(FilePath)) ?? new PublishSettings(); }
        catch (JsonException) { return new PublishSettings(); }
    }

    public void Save()
    {
        if (Company.OfficeApp.Data.AppEdition.IsDemo) return;
        Directory.CreateDirectory(Path.GetDirectoryName(FilePath)!);
        File.WriteAllText(FilePath, JsonSerializer.Serialize(this, new JsonSerializerOptions { WriteIndented = true }));
    }
}
