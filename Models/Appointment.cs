namespace CalendarApp.Models;

public class Appointment
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public bool IsGroupMeeting { get; set; }
    public bool IsAllDay { get; set; }
    public string OwnerId { get; set; } = string.Empty;
    public List<string> Attendees { get; set; } = new();
    public string ColorGroup { get; set; } = string.Empty;
    public string Color { get; set; } = "#4285f4";

    /// <summary>Minutes before the event to remind. 0 = no reminder.</summary>
    public int ReminderMinutes { get; set; } = 0;

    public TimeSpan Duration => EndTime - StartTime;
}
