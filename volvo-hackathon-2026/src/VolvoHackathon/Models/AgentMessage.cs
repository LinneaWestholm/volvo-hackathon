namespace VolvoHackathon.Models;

/// <param name="Id">Unique per message, so the chat can tell messages apart even when the text repeats.</param>
public record AgentMessage(long Id, string Agent, string Text, DateTime Timestamp);
