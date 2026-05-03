namespace CalendarApp.Models;

public class AppUser
{
    public string Id { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string AvatarColor { get; set; } = "#4285f4";

    public string AvatarInitials =>
        string.IsNullOrWhiteSpace(FullName)
            ? "?"
            : FullName.Split(' ', StringSplitOptions.RemoveEmptyEntries).Last()[0].ToString().ToUpper();
}
