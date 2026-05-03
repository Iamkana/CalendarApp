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

    // ── Queries ──────────────────────────────────────────────────────────────
    public IEnumerable<Appointment> GetAll() => _db.Appointments.AsNoTracking().ToList();

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

    public void DeleteAppointment(Guid id)
    {
        var appt = _db.Appointments.FirstOrDefault(a => a.Id == id);
        if (appt is null) return;
        
        _db.Appointments.Remove(appt);
        _db.SaveChanges();
        OnAppointmentsChanged?.Invoke();
    }

    public void ReplaceAppointment(Guid oldId, Appointment newAppointment)
    {
        var old = _db.Appointments.FirstOrDefault(a => a.Id == oldId);
        if (old is null) return;
        
        _db.Entry(old).CurrentValues.SetValues(newAppointment);
        old.Attendees = newAppointment.Attendees; // Update the list
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
    public Appointment? CheckConflict(Appointment appointment)
    {
        var appointments = _db.Appointments.AsNoTracking().ToList();
        return appointments.FirstOrDefault(a =>
            a.Id != appointment.Id &&
            (a.OwnerId == appointment.OwnerId || a.Attendees.Contains(appointment.OwnerId)) &&
            !a.IsAllDay &&
            a.StartTime < appointment.EndTime &&
            a.EndTime   > appointment.StartTime);
    }

    public Appointment? CheckGroupMeetingMatch(Appointment appointment)
    {
        var appointments = _db.Appointments.AsNoTracking().ToList();
        return appointments.FirstOrDefault(a =>
            a.IsGroupMeeting &&
            a.Name == appointment.Name &&
            (a.EndTime - a.StartTime) == appointment.Duration);
    }
}
