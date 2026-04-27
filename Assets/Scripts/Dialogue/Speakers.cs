public enum Speaker
{
    Player,
    Neighbor,
    Unknown,
}


public static class SpeakerExtension
{
    public static string GetName(this Speaker value)
    {
    return value switch {
        Speaker.Player => "You",
        Speaker.Neighbor  => "Jeremy",
        Speaker.Unknown   => "???",
        _             => "Unknown" // Fallback
    };
    }
}