using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CalendarApp.Models;

namespace CalendarApp.Services;

public class ReminderService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly HashSet<Guid> _notifiedIds = new();
    private System.Timers.Timer? _timer;

    public ReminderService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public void Start()
    {
        if (_timer != null) return;

        Console.WriteLine("[ReminderService] Starting...");
        _timer = new System.Timers.Timer(30000); // 30 seconds
        _timer.Elapsed += async (s, e) => await CheckReminders();
        _timer.AutoReset = true;
        _timer.Start();

        // Also check once immediately
        Task.Run(CheckReminders);
    }

    private async Task CheckReminders()
    {
        try 
        {
            using var scope = _serviceProvider.CreateScope();
            var appointmentService = scope.ServiceProvider.GetRequiredService<IAppointmentService>();
            var authService        = scope.ServiceProvider.GetRequiredService<AuthService>();
            var notificationService = scope.ServiceProvider.GetRequiredService<INotificationService>();

            var currentUser = authService.CurrentUser;
            if (currentUser == null) return;

            var appointments = appointmentService.GetByUser(currentUser.Id)
                .Where(a => a.StartTime.Date == DateTime.Today && a.ReminderMinutes > 0)
                .ToList();

            var now = DateTime.Now;
            Console.WriteLine($"[ReminderService] Checking {appointments.Count} appointments at {now:HH:mm:ss}...");

            foreach (var appt in appointments)
            {
                if (_notifiedIds.Contains(appt.Id)) continue;

                var reminderTime = appt.StartTime.AddMinutes(-appt.ReminderMinutes);
                
                // Trigger if we are past reminder time but the event hasn't started yet
                if (now >= reminderTime && now < appt.StartTime)
                {
                    Console.WriteLine($"  !!! Triggering notification for {appt.Name}");
                    _notifiedIds.Add(appt.Id);
                    
                    notificationService.ShowNotification(
                        "Nhắc nhở cuộc hẹn", 
                        $"Cuộc hẹn \"{appt.Name}\" sẽ bắt đầu lúc {appt.StartTime:HH:mm}",
                        appt.Color
                    );
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[ReminderService] Error: {ex.Message}");
        }
    }
}
