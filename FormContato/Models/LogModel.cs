namespace FormContato.Models;

public class LogModel
{
    public Guid Id { get; set; }
    public DateTime? LogDate { get; set; }
    public string? LogLevel { get; set; } = string.Empty;
    public string? LogType { get; set; } = string.Empty;
    public string? Source { get; set; } = string.Empty;
    public string? StackTrace { get; set; } = string.Empty;
}
