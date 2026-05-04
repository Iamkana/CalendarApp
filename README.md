# CalendarApp - OOAD Project

Đây là dự án hệ thống Lịch (Calendar System) sử dụng kiến trúc **phân lớp (N-Layer)**, xây dựng trên nền tảng **.NET MAUI Blazor Hybrid** để chạy đa nền tảng (Windows, macOS, Mobile). Ứng dụng sử dụng **Entity Framework Core** với cơ sở dữ liệu **SQLite** được tích hợp trực tiếp (Local Database) để quản lý dữ liệu người dùng và cuộc hẹn.

## Cài đặt và Chạy ứng dụng

### 1. Yêu cầu hệ thống

| Nền tảng | Yêu cầu bắt buộc |
| :--- | :--- |
| **Chung** | [.NET 10 SDK](https://dotnet.microsoft.com/download) |
| **Windows** | Windows 10/11 với [WebView2](https://developer.microsoft.com/vi-vn/microsoft-edge/webview2?form=MA13LH#download) |
| **macOS** | macOS Sonoma/Sequoia với [Xcode](https://developer.microsoft.com/vi-vn/microsoft-edge/webview2?form=MA13LH#download) (Đã đồng ý điều khoản) |

### 2. Chuẩn bị môi trường
Mở Terminal và cài đặt MAUI workload:

**Windows:**
```bash
dotnet workload install maui
```

**macOS:**
```bash
sudo dotnet workload install maui
```

### 3. Khởi chạy
Mở Terminal tại thư mục gốc của project:

**Windows:**
```bash
dotnet run -f net10.0-windows10.0.19041.0
```

**macOS:**
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
