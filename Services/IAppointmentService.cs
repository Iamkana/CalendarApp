using CalendarApp.Models;

namespace CalendarApp.Services;

public interface IAppointmentService
{
    IEnumerable<Appointment> GetByUser(string ownerId);
    void AddAppointment(Appointment appointment);
    void DeleteAppointment(Guid id, string userId);
    void ReplaceAppointments(IEnumerable<Guid> oldIds, Appointment newAppointment);
    void JoinGroupMeeting(Guid groupMeetingId, string userId);
    List<Appointment> CheckConflicts(Appointment appointment);
    Appointment? CheckGroupMeetingMatch(Appointment appointment);
    event Action? OnAppointmentsChanged;
}
