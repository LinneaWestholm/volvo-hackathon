namespace VolvoHackathon.Services;

/// <summary>
/// Latest running state of each agent, updated from the factory API by <see cref="FactoryPollingService"/>.
/// Index 0 = Agent 1 … index 4 = Agent 5. All agents show as not running until the API answers.
/// </summary>
public class AgentStatusService
{
    public const int AgentCount = 5;

    private readonly Lock _lock = new();
    private readonly bool[] _running = new bool[AgentCount];

    /// <summary>Raised when any agent's state changes. May fire on any thread.</summary>
    public event Action? Changed;

    public bool IsRunning(int index)
    {
        lock (_lock) return _running[index];
    }

    /// <summary>Copies in the states from the API. Extra values are ignored; missing ones are left as they were.</summary>
    public void Update(bool[] running)
    {
        bool changed = false;
        lock (_lock)
        {
            for (int i = 0; i < Math.Min(running.Length, AgentCount); i++)
            {
                if (_running[i] != running[i])
                {
                    _running[i] = running[i];
                    changed = true;
                }
            }
        }

        if (changed) Changed?.Invoke();
    }
}
