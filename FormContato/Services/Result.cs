using FormContato.Models;

namespace FormContato.Services;

public class Result
{
    public bool Success { get; set; }
    public string? Error { get; set; }
    public UserModel? User { get; set; }

}
