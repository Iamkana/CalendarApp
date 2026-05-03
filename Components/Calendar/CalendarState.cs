using CalendarApp.Models;

namespace CalendarApp.Components.Calendar;

public enum ViewMode { Day, Week, Month }

public class CalendarState
{
    public DateTime CurrentDate { get; private set; } = DateTime.Today;
    public ViewMode ViewMode { get; private set; } = ViewMode.Week;
    public HashSet<string> VisibleGroups { get; private set; } = new() { "Thịnh Đặng Bá", "Gia đình", "Tasks" };

    // ── Add-form panel ────────────────────────────────────────────────────────
    public bool IsAddFormOpen    { get; private set; }
    public DateTime PreselectedTime    { get; private set; } = DateTime.Now;
    public DateTime PreselectedEndTime { get; private set; } = DateTime.Now.AddHours(1);

    // ── Event detail popup ───────────────────────────────────────────────
    public Appointment? SelectedEvent  { get; private set; }
    public bool IsEventDetailOpen      => SelectedEvent is not null;

    public event Action? OnChange;

    // ── Navigation ────────────────────────────────────────────────────────────
    public void SetDate(DateTime date) { CurrentDate = date; Notify(); }

    public void GoToToday() { CurrentDate = DateTime.Today; Notify(); }

    public void NavigateNext()
    {
        CurrentDate = ViewMode switch
        {
            ViewMode.Day   => CurrentDate.AddDays(1),
            ViewMode.Month => CurrentDate.AddMonths(1),
            _              => CurrentDate.AddDays(7),
        };
        Notify();
    }

    public void NavigatePrev()
    {
        CurrentDate = ViewMode switch
        {
            ViewMode.Day   => CurrentDate.AddDays(-1),
            ViewMode.Month => CurrentDate.AddMonths(-1),
            _              => CurrentDate.AddDays(-7),
        };
        Notify();
    }

    public void SetViewMode(ViewMode mode) { ViewMode = mode; Notify(); }

    public void ToggleGroup(string group)
    {
        if (!VisibleGroups.Remove(group)) VisibleGroups.Add(group);
        Notify();
    }

    public void OpenAddForm(DateTime? preselected = null, DateTime? preselectedEnd = null)
    {
        PreselectedTime    = preselected ?? DateTime.Now;
        PreselectedEndTime = preselectedEnd ?? PreselectedTime.AddHours(1);
        IsAddFormOpen      = true;
        Notify();
    }

    public void CloseAddForm()
    {
        IsAddFormOpen = false;
        Notify();
    }

    public void OpenEventDetail(Appointment appt)
    {
        SelectedEvent = appt;
        Notify();
    }

    public void CloseEventDetail()
    {
        SelectedEvent = null;
        Notify();
    }

    // ── Helpers ───────────────────────────────────────────────────────────────
    /// <summary>Sunday of the currently viewed week.</summary>
    public DateTime GetWeekStart()
    {
        int diff = (7 + (int)CurrentDate.DayOfWeek) % 7;
        return CurrentDate.AddDays(-diff);
    }

    public string GetTopbarTitle() => ViewMode switch
    {
        ViewMode.Day   => CurrentDate.ToString("dddd, d MMMM yyyy"),
        ViewMode.Week  => GetWeekRangeTitle(),
        ViewMode.Month => CurrentDate.ToString("MMMM yyyy"),
        _              => string.Empty,
    };

    private string GetWeekRangeTitle()
    {
        var start = GetWeekStart();
        var end   = start.AddDays(6);
        if (start.Month == end.Month)
            return $"Tháng {start.Month}, {start.Year}";
        if (start.Year == end.Year)
            return $"Tháng {start.Month} – Tháng {end.Month}, {start.Year}";
        return $"Tháng {start.Month}/{start.Year} – Tháng {end.Month}/{end.Year}";
    }

    private void Notify() => OnChange?.Invoke();
}
