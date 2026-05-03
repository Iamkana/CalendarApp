using Microsoft.Extensions.Logging;
using CalendarApp.Services;
using CalendarApp.Components.Calendar;
using Microsoft.EntityFrameworkCore;

namespace CalendarApp;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
            });

        builder.Services.AddMauiBlazorWebView();

        // Database
#if DEBUG && WINDOWS
        var dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "calendar.db");
#else
        var dbPath = Path.Combine(FileSystem.AppDataDirectory, "calendar.db");
#endif
        Console.WriteLine($"[Database] File path: {dbPath}");

        builder.Services.AddDbContext<CalendarApp.Data.AppDbContext>(options =>
            options.UseSqlite($"Filename={dbPath}"), ServiceLifetime.Singleton);

        // Calendar services
        builder.Services.AddSingleton<IAppointmentService, AppointmentService>();
        builder.Services.AddSingleton<AuthService>();
        builder.Services.AddSingleton<CalendarState>();
        builder.Services.AddSingleton<INotificationService, NotificationService>();
        builder.Services.AddSingleton<ReminderService>();

#if DEBUG
        builder.Services.AddBlazorWebViewDeveloperTools();
        builder.Logging.AddDebug();
#endif

        var app = builder.Build();

        // Initialize Database & Services
        using (var scope = app.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<CalendarApp.Data.AppDbContext>();
            db.Database.EnsureCreated();
            
            var reminderService = scope.ServiceProvider.GetRequiredService<ReminderService>();
            reminderService.Start();
        }

        return app;
    }
}
