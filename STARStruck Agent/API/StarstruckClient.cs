using System.Net.Http;

namespace STARStruck_Agent.API;

public class StarstruckClient : IDisposable
{
    private readonly HttpClient _httpClient;
    private readonly Config _config;

    public StarstruckClient(Config config)
    {
        _config = config;
        _httpClient = new HttpClient
        {
            BaseAddress = new Uri(config.StarstruckURL),
            Timeout = TimeSpan.FromSeconds(30)
        };

        // Add authentication header if AgentKey is configured
        if (!string.IsNullOrEmpty(config.AgentKey))
        {
            _httpClient.DefaultRequestHeaders.Add("X-Agent-Key", config.AgentKey);
        }

        _httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("STARstruck-Agent/1.0.0+alpha1 (https://github.com/fourteentrees/STARstruck-Agent)");
    }

    /// <summary>
    /// Downloads a file from the STARStruck server.
    /// </summary>
    public async Task<string?> DownloadFileAsync(string relativePath)
    {
        try
        {
            var response = await _httpClient.GetAsync(relativePath);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error downloading {relativePath}: {ex.Message}");
            return null;
        }
    }

    /// <summary>
    /// Checks if the agent is authenticated with the server.
    /// </summary>
    public async Task<bool> CheckConnectionAsync()
    {
        try
        {
            var response = await _httpClient.GetAsync("/api/ping");
            return response.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }

    public void Dispose()
    {
        _httpClient.Dispose();
    }
}