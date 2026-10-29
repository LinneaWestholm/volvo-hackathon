namespace VolvoHackathon.Components.Shared;

/// <summary>
/// One colour per agent, used for both its card and its chat bubbles so they always match.
/// Tint = light background, Accent = stronger shade for borders and the agent's name.
/// The hues are spread around the colour wheel so no two agents look alike.
/// </summary>
public static class AgentColors
{
    public record Palette(string Tint, string Accent);

    private static readonly Palette[] Agents =
    [
        // Accents are dark enough to stay readable as text on their own tint
        new("#A9CCF7", "#154E9E"), // Agent 1: blue
        new("#A6E2B7", "#17652F"), // Agent 2: green
        new("#F7B2AA", "#9E2A22"), // Agent 3: coral red
        new("#F9D48C", "#7A4B00"), // Agent 4: amber
        new("#DDB2EC", "#6F2789"), // Agent 5: purple
    ];

    // Anything that isn't "Agent 1" … "Agent 5", e.g. "Factory"
    private static readonly Palette Default = new("#FFFFFF", "#667781");

    public static Palette For(string agent) =>
        agent.StartsWith("Agent ") && int.TryParse(agent.AsSpan(6), out var n) && n >= 1 && n <= Agents.Length
            ? Agents[n - 1]
            : Default;
}
