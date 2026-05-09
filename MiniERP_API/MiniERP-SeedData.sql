USE [MiniERP_Generic]
GO

-- 1. Seed Roles
INSERT INTO [dbo].[Roles] ([Name]) VALUES ('Admin'), ('Staff'), ('Customer');
GO

-- 2. Seed Users
INSERT INTO [dbo].[Users] ([UserName], [Email], [PasswordHash], [FullName])
VALUES ('admin', 'admin@minierp.com', 'AQAAAAEAACcQAAAAEAD...', 'System Administrator');
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
