public enum Speaker
{
    Player,
    Neighbor,
    Unknown,
    Corpse,
}


public static class SpeakerExtension
{
    public static string GetName(this Speaker value)
    {
    return value switch {
        Speaker.Player => "You",
        Speaker.Neighbor  => "Jeremy",
        Speaker.Corpse => "Jessica??",
        Speaker.Unknown   => "???",
        _             => "Unknown" // Fallback
    };
    }
}