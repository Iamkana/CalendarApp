using CalendarApp.Models;
using CalendarApp.Data;
using Microsoft.EntityFrameworkCore;

namespace CalendarApp.Services;

public class AppointmentService : IAppointmentService
{
    private readonly AppDbContext _db;
    public event Action? OnAppointmentsChanged;

    public AppointmentService(AppDbContext db)
    {
        _db = db;
    }


    public IEnumerable<Appointment> GetByUser(string ownerId)
    {
        // For SQLite, Attendees is serialized as JSON string. Since we need to check if ownerId is in Attendees,
        // we can do client-side evaluation or basic string contains (not perfect but works for this demo).
        var appointments = _db.Appointments.AsNoTracking().ToList();
        return appointments.Where(a => a.OwnerId == ownerId || a.Attendees.Contains(ownerId));
    }

    // ── Commands ─────────────────────────────────────────────────────────────
    public void AddAppointment(Appointment appointment)
    {
        _db.Appointments.Add(appointment);
        _db.SaveChanges();
        OnAppointmentsChanged?.Invoke();
    }

    public void DeleteAppointment(Guid id, string userId)
    {
        var appt = _db.Appointments.FirstOrDefault(a => a.Id == id);
        if (appt is null) return;

        if (appt.IsGroupMeeting && appt.OwnerId != userId && appt.Attendees.Contains(userId))
        {
            appt.Attendees.Remove(userId);
            _db.Appointments.Update(appt);
        }
        else
        {
            _db.Appointments.Remove(appt);
        }

        _db.SaveChanges();
        OnAppointmentsChanged?.Invoke();
    }

    public void ReplaceAppointments(IEnumerable<Guid> oldIds, Appointment newAppointment)
    {
        var toRemove = _db.Appointments.Where(a => oldIds.Contains(a.Id)).ToList();
        if (toRemove.Any())
        {
            _db.Appointments.RemoveRange(toRemove);
        }
        
        _db.Appointments.Add(newAppointment);
        _db.SaveChanges();
        
        OnAppointmentsChanged?.Invoke();
    }

    public void JoinGroupMeeting(Guid groupMeetingId, string userId)
    {
        var meeting = _db.Appointments.FirstOrDefault(a => a.Id == groupMeetingId);
        if (meeting is null || meeting.Attendees.Contains(userId)) return;
        
        meeting.Attendees.Add(userId);
        _db.Appointments.Update(meeting);
        _db.SaveChanges();
        
        OnAppointmentsChanged?.Invoke();
    }

    // ── Conflict checks ───────────────────────────────────────────────────────
    public List<Appointment> CheckConflicts(Appointment appointment)
    {
        var appointments = _db.Appointments.AsNoTracking().ToList();
        return appointments.Where(a =>
            a.Id != appointment.Id &&
            (a.OwnerId == appointment.OwnerId || a.Attendees.Contains(appointment.OwnerId)) &&
            a.StartTime < appointment.EndTime &&
            a.EndTime   > appointment.StartTime).ToList();
    }

    public Appointment? CheckGroupMeetingMatch(Appointment appointment)
    {
        var appointments = _db.Appointments.AsNoTracking().ToList();
        return appointments.FirstOrDefault(a =>
            a.IsGroupMeeting &&
            a.Name == appointment.Name &&
            (a.EndTime - a.StartTime) == appointment.Duration &&
            a.StartTime == appointment.StartTime);
    }
}
