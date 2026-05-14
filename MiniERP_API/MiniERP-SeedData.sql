USE [MiniERP_Generic]
GO

-- 1. Roles
INSERT INTO [dbo].[Roles] ([Name]) VALUES ('Admin'), ('Staff');
GO

-- 2. Users
INSERT INTO [dbo].[Users] ([UserName], [Email], [PasswordHash], [FullName])
VALUES ('admin', 'admin@minierp.com', 'admin@123', N'Quản trị viên hệ thống');
GO

-- 3. UserRoles
DECLARE @AdminId INT = (SELECT Id FROM Users WHERE UserName = 'admin');
DECLARE @RoleId INT = (SELECT Id FROM Roles WHERE Name = 'Admin');
INSERT INTO [dbo].[UserRoles] ([UserId], [RoleId]) VALUES (@AdminId, @RoleId);
GO

-- 4. Brands (10 Brands)
INSERT INTO [dbo].[Brands] ([Name], [Description]) VALUES 
(N'Samsung', N'Điện tử Hàn Quốc'),
(N'Apple', N'Công nghệ Mỹ'),
(N'Sony', N'Giải trí Nhật Bản'),
(N'Nike', N'Thời trang thể thao'),
(N'Adidas', N'Thời trang thể thao'),
(N'Uniqlo', N'Thời trang cơ bản'),
(N'Xiaomi', N'Đồ gia dụng thông minh'),
(N'Nestle', N'Thực phẩm & Giải khát'),
(N'Logitech', N'Phụ kiện máy tính'),
(N'Dell', N'Máy tính & Laptop');
GO

-- 5. Product Categories (10 Categories)
INSERT INTO [dbo].[ProductCategories] ([Name]) VALUES 
(N'Điện thoại'), (N'Laptop'), (N'Máy tính bảng'), (N'Tai nghe'), (N'Giày thể thao'), 
(N'Áo thun'), (N'Gia dụng'), (N'Nước giải khát'), (N'Bánh kẹo'), (N'Phụ kiện');
GO

-- 6. Suppliers (5 Suppliers)
INSERT INTO [dbo].[Suppliers] ([Name], [ContactPerson], [Phone], [Email], [Address]) VALUES 
(N'Công ty Thế Giới Số', N'Nguyễn Văn Trỗi', '0912345678', 'contact@digiworld.vn', N'Quận 1, TP.HCM'),
(N'Kho Sỉ Thời Trang HN', N'Lê Thị Mai', '0988776655', 'mai@thoitranghn.com', N'Cầu Giấy, Hà Nội'),
(N'Nhà cung cấp TechHub', N'Trần Đại Nghĩa', '0243334455', 'sales@techhub.vn', N'Quận Hải Châu, Đà Nẵng'),
(N'Đại lý Phụ kiện Pro', N'Phạm Nhật Vượng', '0909090909', 'pro@phukien.com', N'Bình Thạnh, TP.HCM'),
(N'Tổng kho FMCG Miền Nam', N'Bùi Văn Phúc', '0123456789', 'phuc@fmcg.vn', N'Cần Thơ');
GO

-- 7. Customers (10 Customers)
INSERT INTO [dbo].[Customers] ([Name], [Email], [Phone], [Address]) VALUES 
(N'Nguyễn Văn An', 'an.nguyen@gmail.com', '0901112223', N'Hồ Chí Minh'),
(N'Trần Thị Bình', 'binh.tran@yahoo.com', '0904445556', N'Hà Nội'),
(N'Lê Văn Cường', 'cuong.le@hotmail.com', '0907778889', N'Đà Nẵng'),
(N'Phạm Thị Dung', 'dung.pham@gmail.com', '0912223334', N'Cần Thơ'),
(N'Hoàng Văn Em', 'em.hoang@gmail.com', '0915556667', N'Hải Phòng'),
(N'Võ Thị Phượng', 'phuong.vo@gmail.com', '0918889990', N'Huế'),
(N'Đỗ Văn Giang', 'giang.do@gmail.com', '0921112223', N'Nha Trang'),
(N'Bùi Thị Hoa', 'hoa.bui@gmail.com', '0924445556', N'Vũng Tàu'),
(N'Lý Văn Hùng', 'hung.ly@gmail.com', '0927778889', N'Đà Lạt'),
(N'Chu Thị Hương', 'huong.chu@gmail.com', '0931112223', N'Vinh');
GO

-- 8. Products (20 Products - Đã bao gồm tồn kho trong cột StockQuantity)
INSERT INTO [dbo].[Products] ([CategoryId], [BrandId], [SKU], [Name], [Description], [Unit], [CostPrice], [RetailPrice], [StockQuantity])
VALUES 
(1, 1, 'SAM-S24', N'Samsung Galaxy S24', N'Điện thoại flagship', N'Cái', 15000000, 18000000, 20),
(1, 2, 'APP-IP15', N'iPhone 15 Pro', N'Điện thoại Apple', N'Cái', 22000000, 26000000, 15),
(2, 10, 'DEL-XPS13', N'Dell XPS 13', N'Laptop cao cấp', N'Cái', 25000000, 30000000, 8),
(4, 3, 'SON-WH1000', N'Sony WH-1000XM5', N'Tai nghe chống ồn', N'Cái', 6000000, 8000000, 12),
(5, 4, 'NIK-AJ1', N'Nike Air Jordan 1', N'Giày thể thao', N'Đôi', 2000000, 3500000, 5),
(5, 5, 'ADI-UB', N'Adidas UltraBoost', N'Giày chạy bộ', N'Đôi', 2500000, 4000000, 30),
(7, 7, 'XIA-MI14', N'Xiaomi Mi 14', N'Điện thoại Xiaomi', N'Cái', 12000000, 15000000, 25),
(10, 9, 'LOG-MXM3', N'Logitech MX Master 3', N'Chuột không dây', N'Con', 1500000, 2200000, 50),
(6, 6, 'UNI-TEE', N'Uniqlo Crew Neck', N'Áo thun cotton', N'Cái', 200000, 450000, 100),
(8, 8, 'NES-MILO', N'Thùng sữa Milo', N'Sữa lúa mạch', N'Thùng', 250000, 350000, 200),
(1, 1, 'SAM-A54', N'Samsung Galaxy A54', N'Tầm trung', N'Cái', 7000000, 9000000, 5),
(2, 10, 'DEL-INS', N'Dell Inspiron 15', N'Văn phòng', N'Cái', 12000000, 15000000, 3),
(3, 2, 'APP-IPA', N'iPad Air 5', N'Máy tính bảng', N'Cái', 13000000, 16000000, 7),
(4, 2, 'APP-AIRP', N'AirPods Pro 2', N'Tai nghe TWS', N'Cái', 4500000, 6000000, 40),
(5, 4, 'NIK-AF1', N'Nike Air Force 1', N'Classic', N'Đôi', 1800000, 2800000, 2),
(10, 9, 'LOG-G502', N'Logitech G502 Hero', N'Chuột gaming', N'Con', 800000, 1200000, 18),
(7, 7, 'XIA-ROBOT', N'Xiaomi Robot Vacuum', N'Robot hút bụi', N'Cái', 5000000, 7500000, 9),
(8, 8, 'NES-CAFE', N'Hộp Cafe hòa tan', N'Cafe 3in1', N'Hộp', 45000, 65000, 500),
(9, 8, 'KIT-KAT', N'Bánh KitKat', N'Socola', N'Gói', 15000, 25000, 1000),
(6, 6, 'UNI-JACKET', N'Uniqlo Ultra Light Down', N'Áo phao nhẹ', N'Cái', 1200000, 1800000, 6);
GO

-- 9. Sales Orders (Dữ liệu doanh thu 30 ngày qua)
DECLARE @Cust1 INT = (SELECT MIN(Id) FROM Customers);
DECLARE @AdminId INT = (SELECT Id FROM Users WHERE UserName = 'admin');

INSERT INTO [dbo].[SalesOrders] ([OrderNumber], [CustomerId], [OrderDate], [Status], [PaymentMethod], [PaymentStatus], [TotalAmount], [CreatedBy]) VALUES 
('SO-001', @Cust1, DATEADD(day, -30, GETDATE()), 'COMPLETED', 'COD', 'PAID', 18000000, @AdminId),
('SO-002', @Cust1 + 1, DATEADD(day, -25, GETDATE()), 'COMPLETED', 'Banking', 'PAID', 26000000, @AdminId),
('SO-003', @Cust1 + 2, DATEADD(day, -20, GETDATE()), 'COMPLETED', 'COD', 'PAID', 30000000, @AdminId),
('SO-004', @Cust1 + 3, DATEADD(day, -15, GETDATE()), 'COMPLETED', 'Banking', 'PAID', 8000000, @AdminId),
('SO-005', @Cust1 + 4, DATEADD(day, -10, GETDATE()), 'COMPLETED', 'COD', 'PAID', 3500000, @AdminId),
('SO-006', @Cust1 + 5, DATEADD(day, -7, GETDATE()), 'COMPLETED', 'Banking', 'PAID', 15000000, @AdminId),
('SO-007', @Cust1 + 6, DATEADD(day, -5, GETDATE()), 'COMPLETED', 'COD', 'PAID', 450000, @AdminId),
('SO-008', @Cust1 + 7, DATEADD(day, -3, GETDATE()), 'COMPLETED', 'Banking', 'PAID', 7500000, @AdminId),
('SO-009', @Cust1 + 8, DATEADD(day, -2, GETDATE()), 'NEW', 'COD', 'PENDING', 2800000, @AdminId),
('SO-010', @Cust1 + 9, DATEADD(day, -1, GETDATE()), 'NEW', 'Banking', 'PENDING', 1200000, @AdminId);
GO

-- 10. Sales Order Items
DECLARE @SO_1 INT = (SELECT Id FROM SalesOrders WHERE OrderNumber = 'SO-001');
DECLARE @SO_2 INT = (SELECT Id FROM SalesOrders WHERE OrderNumber = 'SO-002');
DECLARE @SO_3 INT = (SELECT Id FROM SalesOrders WHERE OrderNumber = 'SO-003');
DECLARE @SO_4 INT = (SELECT Id FROM SalesOrders WHERE OrderNumber = 'SO-004');
DECLARE @SO_5 INT = (SELECT Id FROM SalesOrders WHERE OrderNumber = 'SO-005');
DECLARE @SO_6 INT = (SELECT Id FROM SalesOrders WHERE OrderNumber = 'SO-006');
DECLARE @SO_7 INT = (SELECT Id FROM SalesOrders WHERE OrderNumber = 'SO-007');
DECLARE @SO_8 INT = (SELECT Id FROM SalesOrders WHERE OrderNumber = 'SO-008');
DECLARE @SO_9 INT = (SELECT Id FROM SalesOrders WHERE OrderNumber = 'SO-009');
DECLARE @SO_10 INT = (SELECT Id FROM SalesOrders WHERE OrderNumber = 'SO-010');

DECLARE @P_1 INT = (SELECT Id FROM Products WHERE SKU = 'SAM-S24');
DECLARE @P_2 INT = (SELECT Id FROM Products WHERE SKU = 'APP-IP15');
DECLARE @P_3 INT = (SELECT Id FROM Products WHERE SKU = 'DEL-XPS13');
DECLARE @P_4 INT = (SELECT Id FROM Products WHERE SKU = 'SON-WH1000');
DECLARE @P_5 INT = (SELECT Id FROM Products WHERE SKU = 'NIK-AJ1');
DECLARE @P_17 INT = (SELECT Id FROM Products WHERE SKU = 'XIA-ROBOT');
DECLARE @P_9 INT = (SELECT Id FROM Products WHERE SKU = 'UNI-TEE');
DECLARE @P_15 INT = (SELECT Id FROM Products WHERE SKU = 'NIK-AF1');
DECLARE @P_16 INT = (SELECT Id FROM Products WHERE SKU = 'LOG-G502');

INSERT INTO [dbo].[SalesOrderItems] ([SalesOrderId], [ProductId], [Quantity], [UnitPrice], [Discount]) VALUES 
(@SO_1, @P_1, 1, 18000000, 0),
(@SO_2, @P_2, 1, 26000000, 0),
(@SO_3, @P_3, 1, 30000000, 0),
(@SO_4, @P_4, 1, 8000000, 0),
(@SO_5, @P_5, 1, 3500000, 0),
(@SO_6, @P_1, 1, 15000000, 0),
(@SO_7, @P_9, 1, 450000, 0),
(@SO_8, @P_17, 1, 7500000, 0),
(@SO_9, @P_15, 1, 2800000, 0),
(@SO_10, @P_16, 1, 1200000, 0);
GO

-- 11. More Sales Orders (To fill the chart)
DECLARE @CMin INT = (SELECT MIN(Id) FROM Customers);
DECLARE @AMid INT = (SELECT Id FROM Users WHERE UserName = 'admin');

INSERT INTO [dbo].[SalesOrders] ([OrderNumber], [CustomerId], [OrderDate], [Status], [PaymentMethod], [PaymentStatus], [TotalAmount], [CreatedBy]) VALUES 
('SO-011', @CMin + 1, DATEADD(day, -4, GETDATE()), 'COMPLETED', 'COD', 'PAID', 9000000, @AMid),
('SO-012', @CMin + 2, DATEADD(day, -4, GETDATE()), 'COMPLETED', 'Banking', 'PAID', 15000000, @AMid),
('SO-013', @CMin + 3, DATEADD(day, -6, GETDATE()), 'COMPLETED', 'COD', 'PAID', 1200000, @AMid),
('SO-014', @CMin + 4, DATEADD(day, -8, GETDATE()), 'COMPLETED', 'Banking', 'PAID', 2200000, @AMid),
('SO-015', @CMin + 5, DATEADD(day, -12, GETDATE()), 'COMPLETED', 'COD', 'PAID', 1800000, @AMid),
('SO-016', @CMin, DATEADD(day, -18, GETDATE()), 'COMPLETED', 'Banking', 'PAID', 5000000, @AMid),
('SO-017', @CMin + 1, DATEADD(day, -22, GETDATE()), 'COMPLETED', 'COD', 'PAID', 6000000, @AMid),
('SO-018', @CMin + 2, DATEADD(day, -28, GETDATE()), 'COMPLETED', 'Banking', 'PAID', 4000000, @AMid);
GO

-- 12. Purchase Orders (Nhập kho)
DECLARE @Sup1 INT = (SELECT MIN(Id) FROM Suppliers);
DECLARE @Uid INT = (SELECT Id FROM Users WHERE UserName = 'admin');

INSERT INTO [dbo].[PurchaseOrders] ([PONumber], [SupplierId], [OrderDate], [Status], [TotalAmount], [CreatedBy]) VALUES 
('PO-001', @Sup1, DATEADD(day, -10, GETDATE()), 'RECEIVED', 150000000, @Uid),
('PO-002', @Sup1 + 1, DATEADD(day, -5, GETDATE()), 'RECEIVED', 50000000, @Uid),
('PO-003', @Sup1 + 2, DATEADD(day, -2, GETDATE()), 'PENDING', 25000000, @Uid),
('PO-004', @Sup1 + 3, DATEADD(day, -1, GETDATE()), 'PENDING', 12000000, @Uid);
GO

-- 13. Purchase Order Items
DECLARE @PO_1 INT = (SELECT Id FROM PurchaseOrders WHERE PONumber = 'PO-001');
DECLARE @PO_2 INT = (SELECT Id FROM PurchaseOrders WHERE PONumber = 'PO-002');
DECLARE @PO_3 INT = (SELECT Id FROM PurchaseOrders WHERE PONumber = 'PO-003');
DECLARE @PO_4 INT = (SELECT Id FROM PurchaseOrders WHERE PONumber = 'PO-004');

DECLARE @PROD_S24 INT = (SELECT Id FROM Products WHERE SKU = 'SAM-S24');
DECLARE @PROD_IP15 INT = (SELECT Id FROM Products WHERE SKU = 'APP-IP15');
DECLARE @PROD_XPS INT = (SELECT Id FROM Products WHERE SKU = 'DEL-XPS13');
DECLARE @PROD_TEE INT = (SELECT Id FROM Products WHERE SKU = 'UNI-TEE');

INSERT INTO [dbo].[PurchaseOrderItems] ([PurchaseOrderId], [ProductId], [Quantity], [UnitPrice]) VALUES 
(@PO_1, @PROD_S24, 10, 15000000),
(@PO_2, @PROD_IP15, 5, 22000000),
(@PO_3, @PROD_XPS, 2, 25000000),
(@PO_4, @PROD_TEE, 50, 200000);
GO
