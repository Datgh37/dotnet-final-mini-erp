USE [MiniERP_Generic]
GO

-- 1. Seed Roles
INSERT INTO [dbo].[Roles] ([Name]) VALUES ('Admin'), ('Staff'), ('Customer');
GO

-- 2. Seed Users
INSERT INTO [dbo].[Users] ([UserName], [Email], [PasswordHash], [FullName])
VALUES ('admin', 'admin@minierp.com', 'admin@123', 'System Administrator');
GO

-- 3. Seed UserRoles
DECLARE @AdminId INT = (SELECT Id FROM Users WHERE UserName = 'admin');
DECLARE @RoleId INT = (SELECT Id FROM Roles WHERE Name = 'Admin');
INSERT INTO [dbo].[UserRoles] ([UserId], [RoleId]) VALUES (@AdminId, @RoleId);
GO

-- 4. Seed Brands
INSERT INTO [dbo].[Brands] ([Name], [Description])
VALUES 
('Samsung', 'Electronics and Home Appliances'),
('Nike', 'Sports and Footwear'),
('Nestle', 'Food and Beverage');
GO

-- 5. Seed Product Categories
INSERT INTO [dbo].[ProductCategories] ([Name], [ParentCategoryId]) VALUES ('Electronics', NULL);
INSERT INTO [dbo].[ProductCategories] ([Name], [ParentCategoryId]) VALUES ('Apparel', NULL);
INSERT INTO [dbo].[ProductCategories] ([Name], [ParentCategoryId]) VALUES ('Groceries', NULL);
GO

-- 6. Seed Suppliers
INSERT INTO [dbo].[Suppliers] ([Name], [ContactPerson], [Phone], [Email], [Address])
VALUES 
('Tech Supply Co.', 'Alice Smith', '0123456789', 'contact@techsupply.com', 'Silicon Valley'),
('Fashion Hub', 'Bob Johnson', '0987654321', 'sales@fashionhub.com', 'New York');
GO

-- 7. Seed Products (Generic)
DECLARE @SamsungId INT = (SELECT Id FROM Brands WHERE Name = 'Samsung');
DECLARE @ElectronicsId INT = (SELECT Id FROM ProductCategories WHERE Name = 'Electronics');

INSERT INTO [dbo].[Products] 
([CategoryId], [BrandId], [SKU], [Name], [Description], [Unit], [CostPrice], [RetailPrice], [StockQuantity], [ImageUrl])
VALUES 
(@ElectronicsId, @SamsungId, 'SS-S24U', 'Samsung Galaxy S24 Ultra', 'Flagship smartphone.', 'Cái', 1000.00, 1300.00, 10, 'https://example.com/s24.jpg'),
(@ElectronicsId, @SamsungId, 'SS-TV65', 'Samsung 4K TV 65 inch', 'Smart TV with OLED display.', 'Cái', 800.00, 1100.00, 5, 'https://example.com/tv65.jpg');
GO

-- 8. Seed Customers
INSERT INTO [dbo].[Customers] ([Name], [Email], [Phone], [Address])
VALUES 
('Nguyen Van A', 'customerA@gmail.com', '0901234567', '123 District 1, Ho Chi Minh'),
('Tran Thi B', 'customerB@yahoo.com', '0907654321', '456 District 3, Ho Chi Minh');
GO

-- 9. Seed Purchase Orders
DECLARE @SupplierId INT = (SELECT TOP 1 Id FROM Suppliers WHERE Name = 'Tech Supply Co.');
DECLARE @AdminId INT = (SELECT TOP 1 Id FROM Users WHERE UserName = 'admin');

INSERT INTO [dbo].[PurchaseOrders] ([PONumber], [SupplierId], [OrderDate], [ExpectedDate], [ReceivedDate], [Status], [TotalAmount], [Notes], [CreatedBy])
VALUES 
('PO-2023001', @SupplierId, '2023-10-01', '2023-10-10', '2023-10-09', 'RECEIVED', 5000.00, 'First batch of smartphones', @AdminId),
('PO-2023002', @SupplierId, '2023-10-15', '2023-10-25', NULL, 'PENDING', 3000.00, 'Restock TVs', @AdminId);
GO

-- 10. Seed Purchase Order Items
DECLARE @PO1 INT = (SELECT TOP 1 Id FROM PurchaseOrders WHERE PONumber = 'PO-2023001');
DECLARE @PO2 INT = (SELECT TOP 1 Id FROM PurchaseOrders WHERE PONumber = 'PO-2023002');
DECLARE @Prod1 INT = (SELECT TOP 1 Id FROM Products WHERE SKU = 'SS-S24U');
DECLARE @Prod2 INT = (SELECT TOP 1 Id FROM Products WHERE SKU = 'SS-TV65');

INSERT INTO [dbo].[PurchaseOrderItems] ([PurchaseOrderId], [ProductId], [Quantity], [UnitPrice])
VALUES 
(@PO1, @Prod1, 5, 1000.00),
(@PO2, @Prod2, 3, 1000.00);
GO

-- 11. Seed Sales Orders
DECLARE @CustomerId INT = (SELECT TOP 1 Id FROM Customers WHERE Email = 'customerA@gmail.com');
DECLARE @AdminId INT = (SELECT TOP 1 Id FROM Users WHERE UserName = 'admin');

INSERT INTO [dbo].[SalesOrders] ([OrderNumber], [CustomerId], [OrderDate], [Status], [PaymentMethod], [PaymentStatus], [TotalAmount], [ShippingAddress], [Notes], [CreatedBy])
VALUES 
('SO-2023001', @CustomerId, '2023-10-10', 'COMPLETED', 'Cash', 'PAID', 1300.00, '123 District 1, Ho Chi Minh', 'Deliver in the morning', @AdminId),
('SO-2023002', @CustomerId, '2023-10-20', 'NEW', 'Credit Card', 'PENDING', 2200.00, '123 District 1, Ho Chi Minh', 'Call before delivery', @AdminId);
GO

-- 12. Seed Sales Order Items
DECLARE @SO1 INT = (SELECT TOP 1 Id FROM SalesOrders WHERE OrderNumber = 'SO-2023001');
DECLARE @SO2 INT = (SELECT TOP 1 Id FROM SalesOrders WHERE OrderNumber = 'SO-2023002');
DECLARE @Prod1 INT = (SELECT TOP 1 Id FROM Products WHERE SKU = 'SS-S24U');
DECLARE @Prod2 INT = (SELECT TOP 1 Id FROM Products WHERE SKU = 'SS-TV65');

INSERT INTO [dbo].[SalesOrderItems] ([SalesOrderId], [ProductId], [Quantity], [UnitPrice], [Discount])
VALUES 
(@SO1, @Prod1, 1, 1300.00, 0),
(@SO2, @Prod2, 2, 1100.00, 0);
GO

-- 13. Seed Stock Movements
DECLARE @Prod1 INT = (SELECT TOP 1 Id FROM Products WHERE SKU = 'SS-S24U');
DECLARE @AdminId INT = (SELECT TOP 1 Id FROM Users WHERE UserName = 'admin');

INSERT INTO [dbo].[StockMovements] ([ProductId], [MovementType], [Quantity], [Reference], [CreatedBy])
VALUES 
(@Prod1, 'IN', 5, 'PO-2023001', @AdminId),
(@Prod1, 'OUT', 1, 'SO-2023001', @AdminId);
GO
