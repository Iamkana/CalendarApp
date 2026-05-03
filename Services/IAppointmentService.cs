using CalendarApp.Models;

namespace CalendarApp.Services;

public interface IAppointmentService
{
    IEnumerable<Appointment> GetByUser(string ownerId);
    void AddAppointment(Appointment appointment);
    void DeleteAppointment(Guid id);
    void ReplaceAppointment(Guid oldId, Appointment newAppointment);
    void JoinGroupMeeting(Guid groupMeetingId, string userId);
    Appointment? CheckConflict(Appointment appointment);
    Appointment? CheckGroupMeetingMatch(Appointment appointment);
    event Action? OnAppointmentsChanged;
}
