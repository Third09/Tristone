using System.Net;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json.Nodes;

namespace Company.WebsiteAdmin.Services;

/// <summary>
/// Puts the published website files into the website's GitHub repository as ONE commit, through GitHub's REST API
/// (no Git program needed on the office PCs). Netlify sees the new commit and updates the live site.
/// Only changed files are uploaded; files in the folder that are no longer used are removed.
/// </summary>
public static class GitHubPublisher
{
    /// <summary>
    /// Makes <paramref name="folder"/> in the repository hold exactly <paramref name="files"/> (repository path -> bytes).
    /// Returns null when it worked (or nothing changed), otherwise a readable reason.
    /// </summary>
    public static async Task<string?> PushAsync(PublishSettings settings, IReadOnlyDictionary<string, byte[]> files,
                                                string folder, string message)
    {
        return await RunAsync(settings, async http =>
        {
            var repo = $"repos/{settings.Repository}/";
            var head = await SendAsync(http, HttpMethod.Get, repo + $"git/ref/heads/{settings.Branch}", null, settings);
            var headSha = (string)head["object"]!["sha"]!;
            var commit = await SendAsync(http, HttpMethod.Get, repo + $"git/commits/{headSha}", null, settings);
            var treeSha = (string)commit["tree"]!["sha"]!;
            var tree = await SendAsync(http, HttpMethod.Get, repo + $"git/trees/{treeSha}?recursive=1", null, settings);

            var existing = tree["tree"]!.AsArray()
                .Where(e => (string?)e!["type"] == "blob" && ((string)e["path"]!).StartsWith(folder + "/"))
                .ToDictionary(e => (string)e!["path"]!, e => (string)e!["sha"]!);

            var entries = new JsonArray();
            foreach (var (path, bytes) in files)
            {
                if (existing.TryGetValue(path, out var sha) && sha == BlobSha(bytes)) continue;   // unchanged
                var blob = await SendAsync(http, HttpMethod.Post, repo + "git/blobs",
                    new JsonObject { ["content"] = Convert.ToBase64String(bytes), ["encoding"] = "base64" }, settings);
                entries.Add(new JsonObject { ["path"] = path, ["mode"] = "100644", ["type"] = "blob", ["sha"] = (string)blob["sha"]! });
            }
            foreach (var path in existing.Keys.Where(p => !files.ContainsKey(p)))
                entries.Add(new JsonObject { ["path"] = path, ["mode"] = "100644", ["type"] = "blob", ["sha"] = null });   // remove
            if (entries.Count == 0) return null;   // the live site already has exactly this

            var newTree = await SendAsync(http, HttpMethod.Post, repo + "git/trees",
                new JsonObject { ["base_tree"] = treeSha, ["tree"] = entries }, settings);
            var newCommit = await SendAsync(http, HttpMethod.Post, repo + "git/commits",
                new JsonObject { ["message"] = message, ["tree"] = (string)newTree["sha"]!, ["parents"] = new JsonArray(headSha) }, settings);
            await SendAsync(http, HttpMethod.Patch, repo + $"git/refs/heads/{settings.Branch}",
                new JsonObject { ["sha"] = (string)newCommit["sha"]!, ["force"] = false }, settings);
            return null;
        });
    }

    /// <summary>Checks the settings: the repository and branch exist and the token may change them. Null = fine.</summary>
    public static async Task<string?> TestAsync(PublishSettings settings)
    {
        return await RunAsync(settings, async http =>
        {
            var info = await SendAsync(http, HttpMethod.Get, $"repos/{settings.Repository}", null, settings);
            if (info["permissions"]?["push"]?.GetValue<bool>() != true)
                return "The token can see the repository but isn't allowed to change it. It needs Contents: Read and write.";
            await SendAsync(http, HttpMethod.Get, $"repos/{settings.Repository}/git/ref/heads/{settings.Branch}", null, settings);
            return null;
        });
    }

    static async Task<string?> RunAsync(PublishSettings settings, Func<HttpClient, Task<string?>> work)
    {
        if (!settings.IsSet) return "Publishing isn't set up on this PC yet. Click Website Settings first.";
        using var http = new HttpClient { BaseAddress = new Uri("https://api.github.com/"), Timeout = TimeSpan.FromSeconds(60) };
        http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", settings.Token);
        http.DefaultRequestHeaders.UserAgent.ParseAdd("TriStone-WebsiteAdmin");
        http.DefaultRequestHeaders.Accept.ParseAdd("application/vnd.github+json");
        http.DefaultRequestHeaders.Add("X-GitHub-Api-Version", "2022-11-28");
        try { return await work(http); }
        catch (GitHubException ex) { return ex.Message; }
        catch (HttpRequestException) { return "Can't reach GitHub. Check this PC's internet connection and try again."; }
        catch (TaskCanceledException) { return "GitHub took too long to answer. Please try again."; }
    }

    static async Task<JsonNode> SendAsync(HttpClient http, HttpMethod method, string url, JsonObject? body, PublishSettings settings)
    {
        using var request = new HttpRequestMessage(method, url);
        if (body != null) request.Content = new StringContent(body.ToJsonString(), Encoding.UTF8, "application/json");
        using var response = await http.SendAsync(request);
        var text = await response.Content.ReadAsStringAsync();
        if (response.IsSuccessStatusCode) return JsonNode.Parse(text)!;

        throw new GitHubException(response.StatusCode switch
        {
            HttpStatusCode.Unauthorized => "GitHub didn't accept the token (wrong or expired). Enter a new one in Website Settings.",
            HttpStatusCode.Forbidden => "The token isn't allowed to change this repository. It needs Contents: Read and write.",
            HttpStatusCode.NotFound => $"GitHub can't find \"{settings.Repository}\" (branch \"{settings.Branch}\"), " +
                                       "or the token has no access to it. Check Website Settings.",
            HttpStatusCode.Conflict or HttpStatusCode.UnprocessableEntity when method == HttpMethod.Patch =>
                "The website was changed on GitHub at the same moment. Click Publish Website again.",
            _ => $"GitHub refused ({(int)response.StatusCode}): {TryMessage(text)}",
        });
    }

    static string TryMessage(string text)
    {
        try { return (string?)JsonNode.Parse(text)?["message"] ?? text; }
        catch { return text; }
    }

    /// <summary>Git's id for a file's contents, to skip uploading files GitHub already has.</summary>
    static string BlobSha(byte[] bytes)
    {
        var header = Encoding.ASCII.GetBytes($"blob {bytes.Length}\0");
        return Convert.ToHexStringLower(SHA1.HashData([.. header, .. bytes]));
    }

    sealed class GitHubException(string message) : Exception(message);
}
