namespace VolvoHackathon.Models;

/// <summary>
/// Response body from the factory API (volvo-hackathon-2026-api-server).
/// Keep in sync with VolvoHackathon.Api.Models.FactoryResponse there.
/// </summary>
public class FactoryResponse
{
    public string Summary { get; set; } = "";

    /// <summary>Whether each agent is running. Index 0 = Agent 1 … index 4 = Agent 5.</summary>
    public bool[] AgentsRunning { get; set; } = new bool[5];
}
