using System.Text.RegularExpressions;
using VolvoHackathon.Models;

namespace VolvoHackathon.Services;

/// <summary>
/// Runs for the lifetime of the app: fetches a <see cref="FactoryResponse"/> from the factory API
/// in a loop, appends each summary to the agent chat and updates the agents' running status.
/// Configure the URL and wait time under "FactoryApi" in appsettings.json.
/// </summary>
public partial class FactoryPollingService(
    IHttpClientFactory httpClientFactory,
    AgentChatService chat,
    AgentStatusService agentStatus,
    IConfiguration config,
    ILogger<FactoryPollingService> logger) : BackgroundService
{
    // Wait this long after a failed request so an unreachable API doesn't flood the log
    private static readonly TimeSpan ErrorDelay = TimeSpan.FromSeconds(2);

    private readonly HttpClient _http = httpClientFactory.CreateClient();

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        string? url = config["FactoryApi:Url"];
        var interval = TimeSpan.FromMilliseconds(config.GetValue("FactoryApi:PollIntervalMs", 20));

        if (string.IsNullOrWhiteSpace(url))
        {
            logger.LogWarning("FactoryApi:Url is not set in appsettings.json, so the factory API is not being polled.");
            return;
        }

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                FactoryResponse? factoryResponse = await Get(url, stoppingToken);

                if (factoryResponse is not null)
                {
                    if (!string.IsNullOrWhiteSpace(factoryResponse.Summary))
                    {
                        var (sender, text) = SplitSender(factoryResponse.Summary);
                        chat.Add(sender, text);
                    }

                    // true = green check, false = red cross on the Factory Monitoring page
                    agentStatus.Update(factoryResponse.AgentsRunning ?? []);
                }

                await Task.Delay(interval, stoppingToken);
            }
            catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
            {
                break; // app is shutting down
            }
            catch (Exception ex)
            {
                logger.LogWarning("Factory API request to {Url} failed: {Message}", url, ex.Message);
                try { await Task.Delay(ErrorDelay, stoppingToken); }
                catch (OperationCanceledException) { break; }
            }
        }
    }

    private Task<FactoryResponse?> Get(string url, CancellationToken cancellationToken) =>
        _http.GetFromJsonAsync<FactoryResponse>(url, cancellationToken);

    // "Agent 2: Bolt supply low" -> ("Agent 2", "Bolt supply low"); anything else comes from "Factory"
    private static (string Sender, string Text) SplitSender(string summary)
    {
        var match = SenderPrefix().Match(summary);
        return match.Success
            ? (match.Groups[1].Value, match.Groups[2].Value)
            : ("Factory", summary);
    }

    [GeneratedRegex(@"^\s*(Agent \d+)\s*:\s*(.+)$", RegexOptions.Singleline)]
    private static partial Regex SenderPrefix();
}
