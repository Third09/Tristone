using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Npgsql;

namespace Company.OfficeApp.Data;

/// <summary>
/// Where this PC finds the database. Saved per PC in
/// C:\ProgramData\TriStone\OfficeApp\dbsettings.json, so the same installed app works on
/// localhost while building and on the office server (192.168.1.50) later.
/// The password is encrypted with Windows DPAPI: the file is useless on another PC.
/// </summary>
public class DbSettings
{
    public string Host { get; set; } = "localhost";
    public int Port { get; set; } = 5432;
    public string Database { get; set; } = "construction_db";
    public string Username { get; set; } = "postgres";

    /// <summary>DPAPI-encrypted password, Base64. Never the plain password.</summary>
    public string EncryptedPassword { get; set; } = "";

    static readonly string FilePath = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),
        "TriStone", "OfficeApp", "dbsettings.json");

    // Ties the encryption to this app, on top of this PC.
    static readonly byte[] Entropy = Encoding.UTF8.GetBytes("TriStone.OfficeApp.DbSettings");

    [System.Text.Json.Serialization.JsonIgnore]
    public string Password
    {
        get
        {
            if (string.IsNullOrEmpty(EncryptedPassword)) return "";
            try
            {
                var bytes = ProtectedData.Unprotect(Convert.FromBase64String(EncryptedPassword), Entropy, DataProtectionScope.LocalMachine);
                return Encoding.UTF8.GetString(bytes);
            }
            catch (CryptographicException) { return ""; }   // file copied from another PC
        }
        set
        {
            var bytes = ProtectedData.Protect(Encoding.UTF8.GetBytes(value), Entropy, DataProtectionScope.LocalMachine);
            EncryptedPassword = Convert.ToBase64String(bytes);
        }
    }

    public bool Exists => File.Exists(FilePath);

    public static DbSettings Load()
    {
        if (!File.Exists(FilePath)) return new DbSettings();
        try { return JsonSerializer.Deserialize<DbSettings>(File.ReadAllText(FilePath)) ?? new DbSettings(); }
        catch (JsonException) { return new DbSettings(); }
    }

    public void Save()
    {
        Directory.CreateDirectory(Path.GetDirectoryName(FilePath)!);
        File.WriteAllText(FilePath, JsonSerializer.Serialize(this, new JsonSerializerOptions { WriteIndented = true }));
    }

    public string ConnectionString => new NpgsqlConnectionStringBuilder
    {
        Host = Host,
        Port = Port,
        Database = Database,
        Username = Username,
        Password = Password,
        Timeout = 5,               // seconds to wait when the server can't be reached
        ApplicationName = "TriStone OfficeApp",
    }.ConnectionString;

    /// <summary>Tries to open a connection. Returns null when it works, otherwise a readable reason.</summary>
    public async Task<string?> TestConnectionAsync()
    {
        try
        {
            await using var conn = new NpgsqlConnection(ConnectionString);
            await conn.OpenAsync();
            return null;
        }
        catch (PostgresException ex) when (ex.SqlState == "28P01") { return "Wrong database username or password."; }
        catch (PostgresException ex) when (ex.SqlState == "3D000") { return $"Database \"{Database}\" does not exist. Run database\\setup-database.cmd first."; }
        catch (NpgsqlException ex) when (ex.InnerException is System.Net.Sockets.SocketException or TimeoutException)
        {
            return $"Can't reach the database server at {Host}:{Port}. Check that the server is on and the address is right.";
        }
        catch (Exception ex) { return ex.Message; }
    }
}
