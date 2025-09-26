# Attendance System – Project Structure & Architecture Guide

> Mục tiêu của tài liệu này là giúp bạn hiểu nhanh toàn bộ cấu trúc thư mục, vai trò của từng tầng (Controller → Service → Repository → Data) và quy trình làm việc code-first. Đọc xong bạn có thể tự tin tiếp tục phát triển hoặc sửa đổi hệ thống theo nhu cầu của mình.

---

## 1. Kiến trúc tổng quan

Hệ thống tuân theo kiến trúc nhiều tầng rõ ràng:

1. **Presentation (ASP.NET MVC 5 Views/Controllers)** – nhận request, hiển thị giao diện Razor và gọi dịch vụ nghiệp vụ.
2. **Application Services** – chứa logic nghiệp vụ (kiểm tra quyền, xử lý dữ liệu, điều phối workflow đơn giản).
3. **Repositories + EF DbContext** – làm việc với Entity Framework 6 ở chế độ Code First, chịu trách nhiệm truy vấn và ghi dữ liệu.
4. **Domain Models (Entities)** – các lớp POCO mô tả dữ liệu cốt lõi.
5. **Hạ tầng & tiện ích** – cấu hình Dependency Injection (Unity), services phụ trợ (Email, Excel, Realtime), upload, scripts tĩnh…

Luồng xử lý tiêu biểu:

```
Request → Controller → Service → Repository → DbContext (SQL Server)
                                         ↓
                                ViewModel → View (Razor)
```

---

## 2. Thư mục & vai trò chi tiết

| Thư mục / File | Mô tả chi tiết | Ghi chú triển khai |
| --- | --- | --- |
| `App_Start/` | Chứa các cấu hình khởi tạo (Route, Bundle, Filter, WebApi, Dependency Injection). | Mở rộng tại đây nếu cần thêm routing, filter hoặc cấu hình Unity. |
| `Global.asax` & `Global.asax.cs` | Điểm khởi động ứng dụng web. Đã cấu hình `Database.SetInitializer` để EF tự migrate. | Khi thêm migration mới, ứng dụng sẽ tự áp dụng khi chạy lần đầu. |
| `Web.config` | Cấu hình ASP.NET, connection string (`DefaultConnection`), binding redirect và appSettings. | Đổi chuỗi kết nối tại đây khi deploy lên SQL Server thật. |
| `Content/` | CSS, fonts, hình ảnh tĩnh của frontend. | Có thể thêm Bootstrap, custom CSS. |
| `Scripts/` | JavaScript, thư viện client-side, file bundle. | Thanh toán cho component UI/SignalR sau này. |
| `Controllers/` | Lớp controller MVC cho web (`EventController`, `ClassController`, …) và Web API cho mobile (`Controllers/API/*`). | Giữ controller mỏng, chỉ điều phối và gọi service. |
| `Views/` | Thư mục Razor view tương ứng với từng controller. `Shared/_Layout.cshtml` chứa layout chung. | Thêm view mới trùng với action name trong controller. |
| `Models/Entities.cs` | Toàn bộ lớp domain (User, Class, Event, Attendance…). | Các attribute DataAnnotations dùng cho validation & mapping. |
| `Models/ApplicationDbContext.cs` | DbContext EF, khai báo `DbSet<>` và quy tắc quan hệ. | Mọi truy vấn EF đều dựa vào context này. |
| `Models/ViewModels/` | DTO phục vụ giao diện/web API. Tách biệt domain entity & dữ liệu hiển thị. | Khi view cần thêm trường, tạo/điều chỉnh ViewModel tương ứng. |
| `Repositories/` | Lớp repository tổng quát (`Repository<T>`) và chuyên biệt (`ClassRepository`, `AttendanceRepository` …). | Đóng gói truy vấn database; services gọi thông qua interface để dễ test/mocking. |
| `Services/` | Nghiệp vụ chính ở dạng interface + implementation (ví dụ `IAttendanceService`/`AttendanceService`). | Chứa logic xử lý, validation chuyên sâu, giao tiếp repo, gửi email… |
| `Filters/` | Chứa custom filter (authorization, logging…) nếu cần. | Hiện để trống, sẵn sàng mở rộng. |
| `Helpers/` | Tiện ích chia sẻ (ví dụ convert, format…). | Dùng cho logic phụ trợ dùng chung. |
| `Uploads/Certificates`, `Uploads/Excel` | Thư mục lưu file người dùng upload hoặc certificate sinh ra. | Bảo đảm quyền ghi/đọc khi deploy thực tế. |
| `Migrations/` | EF Code First migration (`Configuration.cs`, `20250926000100_InitialCreate.cs`). | Khi change model: chạy `Add-Migration`, cập nhật script. |
| `db/AttendanceSystem_Initial.sql` | Script SQL dựng schema đồng bộ với migration đầu tiên. | Dùng cho DBA hoặc deploy trực tiếp trên SQL Server. |
| `Docs/` | Tài liệu kiến trúc (file hiện tại). | Có thể thêm hướng dẫn cài đặt, mô tả API… |

---

## 3. Cách hoạt động của từng tầng

### 3.1 Controllers (MVC & API)
- Nhận HTTP request từ trình duyệt (hoặc ứng dụng di động với API Controller).
- Gọi service layer bằng dependency injection (Unity cấu hình trong `DependencyConfig`).
- Xử lý ModelState, chuyển dữ liệu sang ViewModels và trả về View hoặc JSON.

### 3.2 Services
- Đảm bảo nghiệp vụ: kiểm tra quyền role, validate input, xử lý quy trình (ví dụ tạo event, assign certificate).
- Gọi repository thông qua interface: thuận tiện unit test và thay thế data layer về sau.
- Nhận/tạo ViewModel/DTO phù hợp để trả lại controller.

### 3.3 Repositories & DbContext
- `Repository.cs` cung cấp CRUD cơ bản (`Get`, `Add`, `Update`, `Delete`).
- Repository chuyên biệt viết truy vấn LINQ phức tạp (phân trang, thống kê…).
- `ApplicationDbContext` dùng EF6, Code First, mapping quan hệ qua Fluent API trong `OnModelCreating`.

### 3.4 ViewModels & Views
- ViewModel gom dữ liệu cần hiển thị (ví dụ `EventDetailViewModel` kết hợp thông tin Event + danh sách người tham gia).
- Razor view (`Views/Event/Details.cshtml`…) render ViewModel ra HTML, sử dụng layout chung.
- Phần mobile API có thể sử dụng cùng ViewModel hoặc DTO riêng.

---

## 4. Quy trình Code First (SQL Server)

1. **Thay đổi domain model** trong `Entities.cs` hoặc thêm entity mới.
2. Chạy lệnh (trong Package Manager Console):
   ```powershell
   Add-Migration <TenMigration>
   Update-Database
   ```
   Lệnh đầu sinh file migration, lệnh sau apply lên database kết nối `DefaultConnection`.
3. Deploy thực tế:
   - Copy nội dung migration vào file `.sql` nếu cần phê duyệt thủ công (dựa vào `db/AttendanceSystem_Initial.sql`).
   - Khi app chạy, `Global.asax` sẽ tự gọi `MigrateDatabaseToLatestVersion` bảo đảm schema cập nhật.

> **Ghi nhớ:** nếu đổi connection string (production/staging), EF sẽ apply migration lên DB mới ngay lần chạy đầu. Đảm bảo backup trước khi đổi.

---

## 5. Mở rộng & gợi ý thực thi

- **Authentication/Identity:** hiện dự án dùng bảng `Users` tùy biến. Nếu muốn tích hợp ASP.NET Identity, cần kế thừa `IdentityUser` hoặc viết adapter.
- **Realtime Attendance:** `IRealtimeService`/`RealtimeService` là chỗ để bạn cài SignalR hoặc WebSocket.
- **Import Excel:** `ExcelService` cùng thư mục `Uploads/Excel` đã sẵn sàng dùng EPPlus.
- **Certificate generation:** `CertificateService` xử lý template, lưu file vào `Uploads/Certificates`.
- **API cho Mobile:** xây dựng trong `Controllers/API/*` trả JSON; tái sử dụng service/repository.
- **Unit Test:** có thể tạo dự án test riêng, mock repository bằng interface sẵn có.

---

## 6. Bước tiếp theo đề xuất

1. **Cấu hình Unity** (`DependencyConfig.cs`) để đăng ký đầy đủ service/repository mới.
2. **Viết seed data** trong `Migrations/Configuration.Seed` (vai trò, tài khoản admin…).
3. **Hoàn thiện view** – kết nối ViewModel với UI thật (Bootstrap, DataTables…).
4. **Tích hợp bảo mật** – xác thực đăng nhập, phân quyền `[Authorize(Roles="...")]` như controller mẫu.
5. **Viết log/audit** vào bảng riêng nếu cần (có thể mở rộng trong `Filters/` hoặc service).

---

Chúc bạn làm chủ dự án dễ dàng hơn! Khi cần, chỉ cần mở lại tài liệu này để nhớ nhanh hệ thống đang hoạt động như thế nào.
