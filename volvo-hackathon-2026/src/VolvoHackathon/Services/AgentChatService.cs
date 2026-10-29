using VolvoHackathon.Models;

namespace VolvoHackathon.Services;

/// <summary>
/// Holds the messages shown in the factory monitoring chat box.
/// Call <see cref="Add"/> from wherever the agents run; every open page updates live.
/// Registered as a singleton, so all users see the same conversation.
/// </summary>
public class AgentChatService
{
    // Oldest messages are dropped past this, so a fast poll loop can't use up memory
    private const int MaxMessages = 500;

    private readonly Lock _lock = new();
    private readonly List<AgentMessage> _messages = [];
    private long _nextId;

    public IReadOnlyList<AgentMessage> Messages
    {
        get { lock (_lock) return _messages.ToList(); }
    }

    /// <summary>Raised after a message is added. May fire on any thread.</summary>
    public event Action? MessageAdded;

    public void Add(string agent, string text)
    {
        lock (_lock)
        {
            _messages.Add(new AgentMessage(_nextId++, agent, text, DateTime.Now));
            if (_messages.Count > MaxMessages)
            {
                _messages.RemoveRange(0, _messages.Count - MaxMessages);
            }
        }
        MessageAdded?.Invoke();
    }
}
