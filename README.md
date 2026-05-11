# Hobby Store Mini ERP

## 📌 Tổng quan dự án (Overview)
# 📊 Mini ERP System - Giải pháp quản trị doanh nghiệp nhỏ

Một hệ thống **Mini ERP** tinh gọn được xây dựng trên nền tảng **.NET Core**, hỗ trợ quản lý quy trình chuỗi cung ứng từ thu mua, kho bãi đến bán hàng và báo cáo doanh thu.

Hệ thống bao gồm:
- **MiniERP_API:** Web API backend xử lý logic nghiệp vụ
- **MiniERP_WebView:** Frontend Razor Pages cho giao diện người dùng

Hệ thống cung cấp các giải pháp quản lý tập trung từ khâu nhập hàng, theo dõi tồn kho, quản lý danh mục sản phẩm đặc thù đến xử lý đơn hàng bán lẻ. Dự án được xây dựng dưới nền tảng **.NET 10**, hướng tới sự linh hoạt, dễ dàng bảo trì và mở rộng.

---

## 🌟 Tính năng chính (Key Features)

- **Quản lý danh mục đa cấp:** Phân loại sản phẩm linh hoạt theo Danh mục và Thương hiệu.
- **Quản lý Thu mua (Procurement):** Quy trình nhập hàng chuyên nghiệp từ Nhà cung cấp.
- **Quản lý Bán hàng (Sales):** Xử lý đơn hàng, tính toán doanh thu và chiết khấu.
- **Quản lý Kho (Inventory):** Theo dõi tồn kho thực tế và nhật ký biến động kho.
- **Phân quyền người dùng (RBAC):** Hệ thống bảo mật dựa trên vai trò (Admin, Nhân viên).
- **Báo cáo thông minh:** Thống kê doanh thu, sản phẩm bán chạy và cảnh báo tồn kho thấp.

---

## 🎯 Mục tiêu dự án (Project Goals)
1. **Quản lý chuyên sâu sản phẩm:** Hỗ trợ các thuộc tính đặc thù như SKU, thương hiệu, phân loại...
2. **Tối ưu quy trình vận hành:** Số hóa quy trình từ lúc đặt hàng nhà cung cấp (PO) đến khi xuất kho bán hàng (SO).
3. **Kiểm soát tồn kho thời gian thực:** Theo dõi biến động kho để tránh thất thoát và tối ưu hóa vòng quay hàng tồn.
4. **Kiến trúc bền vững:** Xây dựng codebase theo Layered Architecture (Repository/Service Pattern) để dễ dàng bảo trì và mở rộng.

---

## 🛠 Cấu trúc dự án (Project Structure)

```text
HobbyStore-MiniERP/
├── MiniERP_API/                        # Backend API (ASP.NET Core Web API)
│   ├── Controllers/                    # Xử lý Request
│   │   ├── AuthController.cs
│   │   ├── ProductsController.cs
│   │   ├── SalesOrdersController.cs
│   │   ├── InventoryController.cs
│   │   └── ... (Các Controller khác)
│   ├── Models/
│   │   ├── Entities/                   # Lớp đại diện Database
│   │   │   ├── Product.cs
│   │   │   ├── SalesOrder.cs
│   │   │   └── ...
│   │   └── DTOs/                       # Dữ liệu truyền tải
│   │       ├── ProductDto.cs
│   │       ├── SalesOrderDto.cs
│   │       └── ...
│   ├── Services/                       # Tầng Logic nghiệp vụ
│   │   ├── Interfaces/                 # IProductService.cs, ...
│   │   └── (Implementations)           # ProductService.cs, ...
│   ├── Repositories/                   # Tầng kết nối Database
│   │   ├── Interfaces/                 # IProductRepository.cs, ...
│   │   └── (Implementations)           # ProductRepository.cs, ...
│   ├── Helpers/                        # PasswordHasher.cs, JwtHelper.cs
│   ├── Program.cs                      # Cấu hình hệ thống
│   ├── MiniERP-API-Schema.sql          # Script tạo Database
│   └── MiniERP-SeedData.sql            # Script dữ liệu mẫu
│
├── MiniERP_WebView/                    # Frontend (ASP.NET Core MVC)
│   ├── Controllers/                    # Home, Product, Order Controllers
│   ├── Models/                         # ViewModels
│   ├── Views/                          # Razor Views (.cshtml)
│   ├── wwwroot/                        # CSS, JS, Images
│   └── Program.cs                      # Cấu hình Frontend
│
├── .gitignore
└── README.md
```



---

## 🛠 Định hướng các Module (Module Direction)

Hệ thống được chia thành 4 phân hệ chính:

### 1. Phân hệ Quản trị & Danh mục (Core & Master Data)
- **User & RBAC:** Quản lý nhân viên và phân quyền truy cập.
- **Product Catalog:** Quản lý thông tin chi tiết sản phẩm, thương hiệu (Brands) và phân loại (Categories).
- **Partner Management:** Quản lý thông tin Nhà cung cấp (Suppliers) và Khách hàng (Customers).

### 2. Phân hệ Mua hàng (Procurement)
- **Purchase Orders (PO):** Tạo và quản lý các đơn đặt hàng từ nhà cung cấp.
- **Receiving:** Xác nhận nhập kho và cập nhật giá vốn (Cost Price).

### 3. Phân hệ Kho hàng (Inventory)
- **Stock Management:** Theo dõi số lượng tồn kho thực tế.
- **Stock Movements:** Ghi lại nhật ký mọi biến động nhập/xuất/điều chỉnh kho.

### 4. Phân hệ Bán hàng (Sales)
- **Sales Orders (SO):** Xử lý đơn hàng từ khách hàng, quản lý trạng thái thanh toán và giao hàng.
- **Revenue Tracking:** Theo dõi doanh thu trên từng đơn hàng.

---

## 🚀 Công nghệ sử dụng (Tech Stack)

### Backend (MiniERP_API)
- **Runtime:** .NET 10.0
- **Framework:** ASP.NET Core Web API
- **Database:** SQL Server
- **Data Access:** ADO.NET (Microsoft.Data.SqlClient)
- **Mapping:** AutoMapper (for DTO mapping)
- **API Documentation:** NSwag (Swagger UI)
- **Architecture Pattern:** Repository & Service Pattern

### Frontend (MiniERP_WebView)
- **Framework:** ASP.NET Core Razor Pages
- **Runtime:** .NET 10.0
- **Styling:** Bootstrap 5 (or CSS framework as per project)

---

## 📋 Yêu cầu hệ thống (System Requirements)
- **.NET SDK:** 10.0+
- **Visual Studio:** 2022+ Community/Professional
- **Database:** SQL Server 2022+ (hoặc SQL Server 2019 Express)
- **RAM:** 4GB tối thiểu
- **Disk Space:** 2GB

---

## 🔧 Cài đặt và chạy (Installation & Setup)

### 1. Clone repository
```bash
git clone https://github.com/Datgh37/dotnet-final-hobby-mini-erp.git
cd HobbyStore-MiniERP
```

### 2. Restore dependencies
```bash
dotnet restore
```

### 3. Cấu hình Database Connection
- Cập nhật `appsettings.json` hoặc `appsettings.Development.json` với connection string SQL Server của bạn

### 4. Chạy Backend (MiniERP_API)
```bash
cd MiniERP_API
dotnet run
# API sẽ chạy tại: https://localhost:5001
# Swagger UI: https://localhost:5001/swagger
```

### 5. Chạy Frontend (MiniERP_WebView)
```bash
cd MiniERP_WebView
dotnet run
# WebView sẽ chạy tại: https://localhost:5002
```

---

## 📖 API Documentation
API documentation được tạo tự động bằng **NSwag (Swagger)**:
- Truy cập tại: `https://localhost:5001/swagger` khi chạy backend
- Hỗ trợ testing trực tiếp các endpoint

---

## 📝 License
This project is for educational purposes.

---

## 👥 Contributors
- **Author:** Datgh37

---

**Last Updated:** $(date)
