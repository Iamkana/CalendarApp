# CalendarApp

Ứng dụng quản lý thời gian được xây dựng bằng **.NET MAUI Blazor Hybrid** (.NET 10).
Ứng dụng cho phép quản lý thời gian, tạo cuộc hẹn, họp nhóm, và xem lịch theo ngày/tuần/tháng.

## Cài đặt và Chạy ứng dụng

### Yêu cầu hệ thống
- Bắt buộc: **[.NET 10 SDK](https://dotnet.microsoft.com/download)**.
- Môi trường tương ứng với nền tảng đích: Windows (chạy target Windows) hoặc macOS (chạy target Mac Catalyst).

### Cách chạy ứng dụng

1. Clone hoặc tải mã nguồn về máy.
2. Mở Command Prompt, Terminal hoặc PowerShell tại thư mục gốc của dự án (nơi chứa file `CalendarApp.csproj`).
3. Cài đặt các công cụ MAUI (Chỉ cần chạy lệnh này 1 lần duy nhất trên máy của bạn):

**Trên Windows:**
```bash
dotnet workload install maui
```

**Trên macOS:**
```bash
sudo dotnet workload install maui
```

4. Chạy lệnh tương ứng với hệ điều hành của bạn:

**Trên Windows:**
```bash
dotnet run -f net10.0-windows10.0.19041.0
```

**Trên macOS:**
```bash
dotnet run -f net10.0-maccatalyst
```

## Cấu trúc thư mục chính

- `Components/`: Chứa các component giao diện Blazor (`.razor`).
  - `Calendar/`: Chứa các thành phần của Lịch (DayView, WeekView, MonthView, AddAppointment, EventPopover, Sidebar, Topbar).
  - `Pages/`: Trang Đăng nhập (Login) và Đăng ký (Register).
  - `Layout/`: App shell và các layout bọc ngoài.
- `Models/`: Các lớp dữ liệu (`Appointment`, `AppUser`).
- `Services/`: Các service xử lý logic và dữ liệu (`AuthService`, `AppointmentService`).
- `wwwroot/`: Chứa `index.html` và file CSS toàn cục (`app.css`).
