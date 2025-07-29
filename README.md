# Hệ Thống Quản Lý Điểm Thi

Phần mềm quản lý điểm thi được xây dựng bằng ASP.NET MVC, hỗ trợ quản lý kết quả thi, thông tin sinh viên, phòng thi và các chức năng quản trị cho các cơ sở giáo dục.

## Tính Năng Chính

### 🎓 Quản Lý Sinh Viên
- Thêm, sửa, xóa thông tin sinh viên
- Tìm kiếm và lọc danh sách sinh viên
- Upload ảnh đại diện cho sinh viên
- Xuất danh sách sinh viên ra Excel

### 📊 Quản Lý Kết Quả Thi
- Nhập và quản lý điểm thi
- Xem kết quả thi chi tiết
- Thống kê kết quả theo nhiều tiêu chí
- Xuất báo cáo kết quả ra PDF/Excel

### 🏫 Quản Lý Phòng Thi & Kỳ Thi
- Quản lý thông tin phòng thi
- Phân công sinh viên vào phòng thi
- Quản lý hội đồng thi
- Lập lịch thi và giám sát

### 👥 Quản Lý Người Dùng
- Hệ thống phân quyền theo vai trò
- Quản lý tài khoản người dùng
- Đăng nhập/đăng xuất an toàn
- Ghi nhớ thông tin đăng nhập

### 📈 Báo Cáo & Thống Kê
- Dashboard tổng quan
- Thống kê theo trường, khoa, kỳ thi
- Xuất báo cáo Excel với template có sẵn
- Tìm kiếm nâng cao với nhiều bộ lọc

## Công Nghệ Sử Dụng

- **Framework**: ASP.NET MVC 5.2.3
- **Database**: SQL Server với Entity Framework 6
- **Frontend**: 
  - AdminLTE Template
  - Bootstrap 3.3.7
  - jQuery
  - Font Awesome
- **Libraries**:
  - EPPlus (Xử lý Excel)
  - Newtonsoft.Json
  - System.Linq.Dynamic

## Yêu Cầu Hệ Thống

- .NET Framework 4.5.2 hoặc cao hơn
- SQL Server 2012 hoặc cao hơn
- Visual Studio 2015 hoặc cao hơn
- IIS Express hoặc IIS

## Cài Đặt & Chạy Ứng Dụng

### 1. Clone Repository
```bash
git clone [repository-url]
cd exam-result-system
```

### 2. Cấu Hình Database
1. Tạo database `QLDT` trên SQL Server
2. Cập nhật connection string trong `Web.config`:
```xml
<connectionStrings>
  <add name="QLDTEntities1" 
       connectionString="data source=[YOUR_SERVER];initial catalog=QLDT;integrated security=True;MultipleActiveResultSets=True;App=EntityFramework" 
       providerName="System.Data.EntityClient" />
</connectionStrings>
```

### 3. Restore Packages
- Mở project trong Visual Studio
- Restore NuGet packages (Visual Studio sẽ tự động thực hiện)

### 4. Build & Run
- Build solution (Ctrl+Shift+B)
- Chạy ứng dụng (F5)

## Cấu Trúc Thư Mục

```
Quản lý điểm thi/
├── Controllers/          # MVC Controllers
├── Models/              # Entity Framework Models
├── Views/               # Razor Views
├── Content/             # CSS, Images, Excel Templates
├── Scripts/             # JavaScript Files
├── Excel/               # Uploaded Excel Files
├── PDF/                 # Generated PDF Reports
├── Image/               # User Profile Images
└── App_Start/           # Application Configuration
```

## Tài Khoản Mặc Định

Sau khi setup database, sử dụng tài khoản admin mặc định:
- **Username**: admin
- **Password**: [Kiểm tra trong database]

## Chức Năng Chi Tiết

### Dashboard
- Thống kê tổng quan số lượng sinh viên, kỳ thi
- Biểu đồ phân tích kết quả
- Truy cập nhanh các chức năng chính

### Quản Lý Sinh Viên
- **Thêm mới**: Form nhập thông tin chi tiết
- **Chỉnh sửa**: Cập nhật thông tin, upload ảnh
- **Tìm kiếm**: Theo tên, SBD, trường, khoa
- **Xuất Excel**: Danh sách với format tùy chỉnh

### Quản Lý Điểm Thi
- **Nhập điểm**: Import từ Excel hoặc nhập thủ công
- **Xem kết quả**: Hiển thị chi tiết điểm từng môn
- **Phân loại**: Tự động xếp loại theo thang điểm
- **Xuất giấy chứng nhận**: Template có sẵn

### Báo Cáo
- **Thống kê theo trường**: Tỷ lệ đậu/rớt
- **Phân tích điểm**: Biểu đồ phân bố điểm
- **Xuất báo cáo**: PDF/Excel theo template

## API Endpoints

Hệ thống sử dụng MVC pattern với các route chính:
- `/Home` - Dashboard và tìm kiếm
- `/Student` - Quản lý sinh viên  
- `/Exam` - Quản lý kỳ thi
- `/KetQua` - Quản lý kết quả
- `/User` - Quản lý người dùng

## Bảo Mật

- Session-based authentication
- Role-based authorization
- Input validation và SQL injection protection
- File upload security với whitelist extension

## Đóng Góp

1. Fork repository
2. Tạo feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to branch (`git push origin feature/AmazingFeature`)
5. Tạo Pull Request

## Liên Hệ & Hỗ Trợ

Để được hỗ trợ kỹ thuật hoặc báo lỗi, vui lòng tạo issue trên GitHub repository.

## License

Dự án này được phát triển cho mục đích giáo dục và quản lý nội bộ.