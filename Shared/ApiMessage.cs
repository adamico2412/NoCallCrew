namespace Shared;

public class ApiMessage
{
    public string Text { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; }
    public string? User { get; set; }
}
