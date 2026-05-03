using CalendarApp.Models;
using CalendarApp.Data;

namespace CalendarApp.Services;

public class AuthService
{
    private readonly AppDbContext _db;

    public AuthService(AppDbContext db)
    {
        _db = db;
    }

    public AppUser? CurrentUser { get; private set; }
    public bool IsAuthenticated => CurrentUser != null;
    public event Action? OnAuthChanged;

    public bool Login(string email, string password)
    {
        var user = _db.Users.FirstOrDefault(u =>
            u.Email.ToLower() == email.ToLower() &&
            u.Password == password);
        
        if (user is null) return false;
        
        CurrentUser = user;
        OnAuthChanged?.Invoke();
        return true;
    }

    /// <summary>Registers a new user. Returns null on success, or an error message.</summary>
    public string? Register(string fullName, string email, string password)
    {
        if (string.IsNullOrWhiteSpace(fullName)) return "Vui lòng nhập họ tên.";
        if (string.IsNullOrWhiteSpace(email))    return "Vui lòng nhập email.";
        if (string.IsNullOrWhiteSpace(password)) return "Vui lòng nhập mật khẩu.";

        if (_db.Users.Any(u => u.Email.ToLower() == email.ToLower()))
            return "Email này đã được đăng ký.";

        var newUser = new AppUser
        {
            Id       = Guid.NewGuid().ToString(),
            FullName = fullName.Trim(),
            Email    = email.Trim(),
            Password = password,
        };
        
        _db.Users.Add(newUser);
        _db.SaveChanges();

        CurrentUser = newUser;
        OnAuthChanged?.Invoke();
        return null; // success
    }

    public void Logout()
    {
        CurrentUser = null;
        OnAuthChanged?.Invoke();
    }
}
