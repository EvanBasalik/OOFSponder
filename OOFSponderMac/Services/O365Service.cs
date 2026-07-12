using Microsoft.Identity.Client;
using Microsoft.Identity.Client.Extensions.Msal;
using Microsoft.IdentityModel.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OOFSponderMac.Services;

/// <summary>
/// Cross-platform Microsoft 365 authentication and API service.
///
/// Key differences from the Windows version:
///   - No Windows Account Manager (WAM) broker; uses the system browser
///     for interactive sign-in via <see cref="AcquireTokenInteractive"/>.
///   - Token cache uses <see cref="MsalCacheHelper"/>, which stores to the
///     OS keychain on macOS and an encrypted file on Linux.
///   - Default user UPN is derived from the acquired token rather than
///     <c>WindowsIdentity.GetCurrent()</c>.
/// </summary>
public static class O365Service
{
    // AAD multi-tenant app registration shared with the Windows client
    private const string ClientId = "c0eceb27-8cd3-4bb8-9271-c90596069f74";
    private static readonly string[] Scopes = { "user.read", "MailboxSettings.ReadWrite" };
    private const string GraphEndpoint = "https://graph.microsoft.com/v1.0/me";
    private const string AutoReplySettingsPath = "/mailboxSettings/automaticRepliesSetting";
    private const string MailboxSettingsPath = "/mailboxSettings";

    private static IPublicClientApplication? _pca;
    private static AuthenticationResult? _authResult;
    private static string _defaultUserUPN = string.Empty;
    private static readonly SemaphoreSlim AuthLock = new(1, 1);

    // ─── Public state ─────────────────────────────────────────────────────────

    public static string DefaultUserUPN
    {
        get => _defaultUserUPN;
        set => _defaultUserUPN = value;
    }

    public static async Task<bool> IsSignedInAsync()
    {
        if (_pca == null) return false;
        try
        {
            var accounts = await _pca.GetAccountsAsync().ConfigureAwait(false);
            return accounts.Any(a =>
                string.IsNullOrEmpty(_defaultUserUPN) ||
                a.Username.Equals(_defaultUserUPN, StringComparison.OrdinalIgnoreCase));
        }
        catch
        {
            return false;
        }
    }

    // ─── Authentication ───────────────────────────────────────────────────────

    /// <summary>
    /// Acquires a token silently (from cache) or interactively via the
    /// system browser.  Returns <c>true</c> when a valid token is obtained.
    /// </summary>
    public static async Task<bool> SignInAsync()
    {
        AppLogger.Info("SignInAsync started");
        await AuthLock.WaitAsync();
        try
        {
            await EnsurePCAAsync();
            return await AcquireTokenAsync();
        }
        finally
        {
            AuthLock.Release();
        }
    }

    public static async Task SignOutAsync()
    {
        AppLogger.Info("SignOutAsync started");
        if (_pca == null) return;
        var accounts = await _pca.GetAccountsAsync();
        foreach (var account in accounts)
            await _pca.RemoveAsync(account);
        _authResult = null;
        _defaultUserUPN = string.Empty;
        AppLogger.Info("Signed out");
    }

    private static async Task EnsurePCAAsync()
    {
        if (_pca != null) return;

        var logger = new MsalLogger();
        _pca = PublicClientApplicationBuilder.Create(ClientId)
            .WithAuthority(AadAuthorityAudience.AzureAdMultipleOrgs)
            // http://localhost is the default redirect URI for public clients on
            // desktop apps – works on macOS without a custom URI scheme.
            .WithRedirectUri("http://localhost")
            .WithLogging(logger)
            .Build();

        await RegisterCacheAsync(_pca);
    }

    private static async Task RegisterCacheAsync(IPublicClientApplication pca)
    {
        try
        {
            // MsalCacheHelper selects the best storage per OS:
            //   macOS  → Keychain
            //   Linux  → libsecret / plaintext fallback
            //   Windows → DPAPI-protected file
            var storageProps = new StorageCreationPropertiesBuilder(
                    "msalcache.bin3",
                    Program.AppDataFolder)
                .WithMacKeyChain("OOFSponder", "MsalTokenCache")
                .Build();

            var cacheHelper = await MsalCacheHelper.CreateAsync(storageProps);
            cacheHelper.RegisterCache(pca.UserTokenCache);
            AppLogger.Info("MSAL cache registered");
        }
        catch (Exception ex)
        {
            AppLogger.Error("MSAL cache registration failed (continuing without persistent cache)", ex);
        }
    }

    private static async Task<bool> AcquireTokenAsync()
    {
        var accounts = await _pca!.GetAccountsAsync();
        IAccount? account = null;

        if (!string.IsNullOrEmpty(_defaultUserUPN))
            account = accounts.FirstOrDefault(a =>
                a.Username.Contains(_defaultUserUPN, StringComparison.OrdinalIgnoreCase));
        else if (accounts.Any())
            account = accounts.First();

        // Try silent first
        if (account != null)
        {
            try
            {
                _authResult = await _pca.AcquireTokenSilent(Scopes, account).ExecuteAsync();
                _defaultUserUPN = _authResult.Account.Username;
                AppLogger.Info("Token acquired silently");
                return true;
            }
            catch (MsalUiRequiredException)
            {
                AppLogger.Info("Silent auth failed – will use interactive");
            }
            catch (Exception ex)
            {
                AppLogger.Error("Silent auth error", ex);
            }
        }

        // Fall back to interactive (opens system browser)
        try
        {
            var builder = _pca.AcquireTokenInteractive(Scopes)
                .WithPrompt(Prompt.SelectAccount);

            if (!string.IsNullOrEmpty(_defaultUserUPN))
                builder = builder.WithLoginHint(_defaultUserUPN);

            _authResult = await builder.ExecuteAsync();
            _defaultUserUPN = _authResult.Account.Username;
            AppLogger.Info("Token acquired interactively");
            return true;
        }
        catch (Exception ex)
        {
            AppLogger.Error("Interactive auth failed", ex);
            return false;
        }
    }

    // ─── Graph API helpers ────────────────────────────────────────────────────

    public static async Task<string> GetHttpContentAsync(string path)
    {
        await SignInAsync();
        if (_authResult == null) return string.Empty;

        using var client = new HttpClient();
        var request = new HttpRequestMessage(HttpMethod.Get, Combine(GraphEndpoint, path));
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _authResult.AccessToken);
        try
        {
            var response = await client.SendAsync(request);
            return await response.Content.ReadAsStringAsync();
        }
        catch (Exception ex)
        {
            AppLogger.Error("GetHttpContentAsync failed", ex);
            return string.Empty;
        }
    }

    public static async Task<HttpResponseMessage?> PatchMailboxSettingsAsync(
        Microsoft.Graph.AutomaticRepliesSetting oof)
    {
        await SignInAsync();
        if (_authResult == null) return null;

        using var client = new HttpClient();
        var method = new HttpMethod("PATCH");
        var request = new HttpRequestMessage(method, Combine(GraphEndpoint, MailboxSettingsPath));
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _authResult.AccessToken);

        var mbox = new Microsoft.Graph.MailboxSettings { AutomaticRepliesSetting = oof };
        var json = Newtonsoft.Json.JsonConvert.SerializeObject(mbox);
        request.Content = new StringContent(json, Encoding.UTF8, "application/json");

        try
        {
            AppLogger.Info("Sending OOF PATCH to Graph");
            var response = await client.SendAsync(request);
            AppLogger.Info($"Graph response: {response.StatusCode}");
            return response;
        }
        catch (Exception ex)
        {
            AppLogger.Error("PatchMailboxSettingsAsync failed", ex);
            throw;
        }
    }

    private static string Combine(params string[] parts) =>
        parts.Aggregate((a, b) =>
            new Uri(new Uri(a.TrimEnd('/') + "/"), b.TrimStart('/')).ToString());
}

// ─── MSAL logging adapter ─────────────────────────────────────────────────────

internal class MsalLogger : IIdentityLogger
{
    private readonly EventLogLevel _minLevel;

    public MsalLogger()
    {
        var env = Environment.GetEnvironmentVariable("MSAL_LOG_LEVEL");
        _minLevel = Enum.TryParse(env ?? string.Empty, out EventLogLevel level)
            ? level
            : EventLogLevel.Error;
    }

    public EventLogLevel MinLogLevel => _minLevel;
    public bool IsEnabled(EventLogLevel level) => level <= _minLevel;
    public void Log(LogEntry entry) => AppLogger.Info(entry.Message);
}
