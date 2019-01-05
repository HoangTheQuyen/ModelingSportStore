# ModelingSportStore
# Đồ án Mô Hình Hóa nhóm 4k - stt 20
Thành viên
* Hoàng Thế Quyền	- 1512455 *
* Lê Thị Mơ - 1512327 *
* Đoàn An Nguyên- 1512354 *
* Lê Phú Sang - 1512453 *
* Lữ Đình Quân - 1512439 *
## Hướng dẫn chạy đồ án
* Cài .Net Core từ trang https://dotnet.microsoft.com/download
* Cài npm https://nodejs.org/en/
* Cài CSDL PostgrestSql tại trang https://www.postgresql.org/ và start nó
* Vào file Properties rồi đến file json là launchSettings.json và cài đặt port là 5001 trên local host
* dotnet run để chạy dự án
* Đường dẫn hệ admin: localhost:5001/admin
* Đường dẫn phân hệ guest and user: localhost:5001/products
## Hướng dẫn chạy đồ án khi chuyển thành csdl sql server
* Cài sql server
* vào file appsettings.json trong dự án và đổi đoạn sau của "DefaultConnection" thành "Server=(localdb)\\MSSQLLocalDB;Database=SportStore;Trusted_Connection=True;MultipleActiveResultSets=true"
* Vào file startup.cs trong dự án và đổi đoạn code "services.AddDbContext<EcommerceContext>(options =>
          options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection")));"
 thành  services.AddDbContext<EcommerceContext>(options =>
          options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
 * Lưu và chạy lệnh dotnet build 
 * Chạy lệnh dotnet ef database update
 * sau đó chạy dotnet run để khởi chạy dự án trên local host ở port 5001

## Các chức năng chính
### 1 Phân hệ khách
* [x] **1.1 Trang chủ**
  * [x] Hiển thị sản phẩm theo tên A-Z 
  * [x] Hiển thị sản phẩm theo tên Z-A 
  * [x] Hiển thị sản phẩm tăng theo giá
  * [x] Hiển thị sản phẩm giảm theo giá
* [x] **1.2 Hệ thống Menu Lọc Sản Phầm**
  * [x] Hiển thị danh sách sản phầm theo Brand
  * [x] Hiển thị danh sách sản phầm theo Price
  * [x] Hiển thị danh sách sản phầm theo Size
  * [x] Hiển thị danh sách sản phầm theo Colour
  * [x] Hiển thị danh sách sản phầm theo Features

* [x] **1.4 Xem chi tiết sản phẩm**
  * [x] Hiển thị các thông tin sau
    * [x] Hình ảnh
    * [x] Giá bán
    * [x] Mô tả
    * [x] Xuất xử
    * [x] Loại
    * [x] Nhà sản xuất
    
* [ ] **1.5 Tìm kiếm sản phẩm** 
  * [ ] Cho phép tìm kiếm theo nhiều tiêu chí: tên, giá, loại, nhà sản xuất
* [x] **1.6 Đăng nhập**
* [x] **1.7 Đăng ký**
### 2 Phân hệ người dùng đã đăng nhập
* [x] **2.1 Có đầy đủ chức năng của phân hệ khách**
* [x] **2.3 Chọn mua sản phẩm**
  * [x] Cho phép chọn mua sản phẩm khi xem chi tiết 
* [x] **2.4 Điều chỉnh thông tin sản phẩm đang chọn mua (quản lý giỏ hàng)**
* [x] **2.5 Thanh toán**
  * [x] Lưu thông tin sản phẩm khách hàng chọn mua và cập nhật lại số lượng bán, số lượng tồn tương ứng cho từng sản phẩm
* [x] **2.6 Xem lịch sử mua hàng**
  * [x] Xem danh sách các đơn hàng đã từng đặt theo thứ tự từ mới đến cũ
  * [ x Xem chi tiết từng đơn hàng và trạng thái của các đơn hàng này
### 3 Phân hệ quản trị (Admin)
* [x] **3.1 Dashboard**
  * [x] Hiển thị danh sách các chức năng mà admin có thể sử dụng
* [x] **3.2 Quản lý sản phẩm, loại sản phẩm, nhà sản xuất**
* [x] **3.3 Quản lý đơn hàng**
  * [x] Thể hiện danh sách đơn hàng theo thứ tự giảm dần của ngày lập
  * [x] Cho phép admin thực hiện việc cập nhật trạng thái đơn hàng(chưa giao, đang giao, đã giao). Đơn hàng đã giao sẽ có thể hiện khác với đơn hàng chưa giao (VD: text màu khác, background màu khác,...)
