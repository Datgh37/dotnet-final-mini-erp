/****** Object:  Database [MiniERP_Generic] ******/
CREATE DATABASE [MiniERP_Generic]
GO

USE [MiniERP_Generic]
GO

/****** 1. Phân quyền & Người dùng ******/
CREATE TABLE [dbo].[Roles](
	[Id] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,
	[Name] [nvarchar](100) NOT NULL,
	[CreatedAt] [datetimeoffset](7) NOT NULL DEFAULT (sysutcdatetime()),
	[UpdatedAt] [datetimeoffset](7) NULL,
	[IsDeleted] [bit] NOT NULL DEFAULT (0)
)
GO
CREATE UNIQUE INDEX [UX_Roles_Name] ON [dbo].[Roles]([Name]) WHERE [IsDeleted] = 0;
GO

CREATE TABLE [dbo].[Users](
	[Id] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,
	[UserName] [nvarchar](100) NOT NULL,
	[Email] [nvarchar](256) NULL,
	[PasswordHash] [nvarchar](max) NULL,
	[FullName] [nvarchar](200) NULL,
	[CreatedAt] [datetimeoffset](7) NOT NULL DEFAULT (sysutcdatetime()),
	[UpdatedAt] [datetimeoffset](7) NULL,
	[IsDeleted] [bit] NOT NULL DEFAULT (0)
)
GO
CREATE UNIQUE INDEX [UX_Users_UserName] ON [dbo].[Users]([UserName]) WHERE [IsDeleted] = 0;
GO

CREATE TABLE [dbo].[UserRoles](
	[UserId] [int] NOT NULL,
	[RoleId] [int] NOT NULL,
	PRIMARY KEY ([UserId], [RoleId]),
	CONSTRAINT [FK_UserRoles_User] FOREIGN KEY([UserId]) REFERENCES [dbo].[Users] ([Id]) ON DELETE CASCADE,
	CONSTRAINT [FK_UserRoles_Role] FOREIGN KEY([RoleId]) REFERENCES [dbo].[Roles] ([Id]) ON DELETE CASCADE
)
GO

/****** 2. Danh mục & Đối tác ******/
CREATE TABLE [dbo].[Brands](
	[Id] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,
	[Name] [nvarchar](200) NOT NULL,
	[Description] [nvarchar](500) NULL,
	[CreatedAt] [datetimeoffset](7) NOT NULL DEFAULT (sysutcdatetime()),
	[UpdatedAt] [datetimeoffset](7) NULL,
	[IsDeleted] [bit] NOT NULL DEFAULT (0)
)
GO
CREATE UNIQUE INDEX [UX_Brands_Name] ON [dbo].[Brands]([Name]) WHERE [IsDeleted] = 0;
GO

CREATE TABLE [dbo].[ProductCategories](
	[Id] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,
	[Name] [nvarchar](200) NOT NULL,
	[ParentCategoryId] [int] NULL,
	[CreatedAt] [datetimeoffset](7) NOT NULL DEFAULT (sysutcdatetime()),
	[UpdatedAt] [datetimeoffset](7) NULL,
	[IsDeleted] [bit] NOT NULL DEFAULT (0),
	CONSTRAINT [FK_PC_Parent] FOREIGN KEY([ParentCategoryId]) REFERENCES [dbo].[ProductCategories] ([Id])
)
GO

CREATE TABLE [dbo].[Suppliers](
	[Id] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,
	[Name] [nvarchar](200) NOT NULL,
	[ContactPerson] [nvarchar](200) NULL,
	[Phone] [nvarchar](50) NULL,
	[Email] [nvarchar](256) NULL,
	[Address] [nvarchar](500) NULL,
	[CreatedAt] [datetimeoffset](7) NOT NULL DEFAULT (sysutcdatetime()),
	[UpdatedAt] [datetimeoffset](7) NULL,
	[IsDeleted] [bit] NOT NULL DEFAULT (0)
)
GO

/****** 3. Sản phẩm (Generic) ******/
CREATE TABLE [dbo].[Products](
	[Id] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,
	[CategoryId] [int] NULL,
	[BrandId] [int] NULL,
	[SKU] [nvarchar](100) NOT NULL,
	[Name] [nvarchar](300) NOT NULL,
	[Description] [nvarchar](max) NULL,
	[Unit] [nvarchar](50) NULL, -- Đơn vị tính (Cái, Thùng, Kg...)
	[CostPrice] [decimal](18, 2) NOT NULL DEFAULT (0),
	[RetailPrice] [decimal](18, 2) NOT NULL DEFAULT (0),
	[StockQuantity] [int] NOT NULL DEFAULT (0),
	[ImageUrl] [nvarchar](500) NULL,
	[CreatedAt] [datetimeoffset](7) NOT NULL DEFAULT (sysutcdatetime()),
	[UpdatedAt] [datetimeoffset](7) NULL,
	[IsDeleted] [bit] NOT NULL DEFAULT (0),
	CONSTRAINT [FK_Product_Category] FOREIGN KEY([CategoryId]) REFERENCES [dbo].[ProductCategories] ([Id]),
	CONSTRAINT [FK_Product_Brand] FOREIGN KEY([BrandId]) REFERENCES [dbo].[Brands] ([Id])
)
GO
CREATE UNIQUE INDEX [UX_Products_SKU] ON [dbo].[Products]([SKU]) WHERE [IsDeleted] = 0;
GO

/****** 4. Khách hàng ******/
CREATE TABLE [dbo].[Customers](
	[Id] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,
	[UserId] [int] NULL,
	[Name] [nvarchar](200) NOT NULL,
	[Email] [nvarchar](256) NULL,
	[Phone] [nvarchar](50) NULL,
	[Address] [nvarchar](500) NULL,
	[CreatedAt] [datetimeoffset](7) NOT NULL DEFAULT (sysutcdatetime()),
	[UpdatedAt] [datetimeoffset](7) NULL,
	[IsDeleted] [bit] NOT NULL DEFAULT (0),
	CONSTRAINT [FK_Cust_User] FOREIGN KEY([UserId]) REFERENCES [dbo].[Users] ([Id])
)
GO

/****** 5. Mua hàng (Purchase Orders) ******/
CREATE TABLE [dbo].[PurchaseOrders](
	[Id] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,
	[PONumber] [nvarchar](50) NOT NULL,
	[SupplierId] [int] NULL,
	[OrderDate] [date] NULL,
	[ExpectedDate] [date] NULL,
	[ReceivedDate] [date] NULL,
	[Status] [nvarchar](50) NULL, -- PENDING, RECEIVED, CANCELLED
	[TotalAmount] [decimal](18, 2) DEFAULT (0),
	[Notes] [nvarchar](max) NULL,
	[CreatedBy] [int] NULL,
	[CreatedAt] [datetimeoffset](7) NOT NULL DEFAULT (sysutcdatetime()),
	[UpdatedAt] [datetimeoffset](7) NULL,
	[IsDeleted] [bit] NOT NULL DEFAULT (0),
	CONSTRAINT [FK_PO_Supplier] FOREIGN KEY([SupplierId]) REFERENCES [dbo].[Suppliers] ([Id]),
	CONSTRAINT [FK_PO_User] FOREIGN KEY([CreatedBy]) REFERENCES [dbo].[Users] ([Id])
)
GO
CREATE UNIQUE INDEX [UX_PurchaseOrders_PONumber] ON [dbo].[PurchaseOrders]([PONumber]) WHERE [IsDeleted] = 0;
GO

CREATE TABLE [dbo].[PurchaseOrderItems](
	[Id] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,
	[PurchaseOrderId] [int] NOT NULL,
	[ProductId] [int] NOT NULL,
	[Quantity] [int] NOT NULL,
	[UnitPrice] [decimal](18, 2) NOT NULL,
	CONSTRAINT [FK_POI_Order] FOREIGN KEY([PurchaseOrderId]) REFERENCES [dbo].[PurchaseOrders] ([Id]) ON DELETE CASCADE,
	CONSTRAINT [FK_POI_Product] FOREIGN KEY([ProductId]) REFERENCES [dbo].[Products] ([Id])
)
GO

/****** 6. Bán hàng (Sales Orders) ******/
CREATE TABLE [dbo].[SalesOrders](
	[Id] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,
	[OrderNumber] [nvarchar](50) NOT NULL,
	[CustomerId] [int] NULL,
	[OrderDate] [date] NULL,
	[Status] [nvarchar](50) NULL, -- NEW, SHIPPING, COMPLETED, CANCELLED
	[PaymentMethod] [nvarchar](50) NULL,
	[PaymentStatus] [nvarchar](50) NULL,
	[TotalAmount] [decimal](18, 2) DEFAULT (0),
	[ShippingAddress] [nvarchar](500) NULL,
	[Notes] [nvarchar](max) NULL,
	[CreatedBy] [int] NULL,
	[CreatedAt] [datetimeoffset](7) NOT NULL DEFAULT (sysutcdatetime()),
	[UpdatedAt] [datetimeoffset](7) NULL,
	[IsDeleted] [bit] NOT NULL DEFAULT (0),
	CONSTRAINT [FK_SO_Customer] FOREIGN KEY([CustomerId]) REFERENCES [dbo].[Customers] ([Id]),
	CONSTRAINT [FK_SO_User] FOREIGN KEY([CreatedBy]) REFERENCES [dbo].[Users] ([Id])
)
GO
CREATE UNIQUE INDEX [UX_SalesOrders_OrderNumber] ON [dbo].[SalesOrders]([OrderNumber]) WHERE [IsDeleted] = 0;
GO

CREATE TABLE [dbo].[SalesOrderItems](
	[Id] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,
	[SalesOrderId] [int] NOT NULL,
	[ProductId] [int] NOT NULL,
	[Quantity] [int] NOT NULL,
	[UnitPrice] [decimal](18, 2) NOT NULL,
	[Discount] [decimal](18, 2) DEFAULT (0),
	CONSTRAINT [FK_SOI_Order] FOREIGN KEY([SalesOrderId]) REFERENCES [dbo].[SalesOrders] ([Id]) ON DELETE CASCADE,
	CONSTRAINT [FK_SOI_Product] FOREIGN KEY([ProductId]) REFERENCES [dbo].[Products] ([Id])
)
GO

/****** 7. Nhật ký kho (Stock Movements) ******/
CREATE TABLE [dbo].[StockMovements](
	[Id] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,
	[ProductId] [int] NOT NULL,
	[MovementType] [nvarchar](50) NOT NULL, -- IN, OUT, ADJUSTMENT, RETURN
	[Quantity] [int] NOT NULL,
	[Reference] [nvarchar](200) NULL, -- Mã PO hoặc SO tương ứng
	[CreatedBy] [int] NULL,
	[CreatedAt] [datetimeoffset](7) NOT NULL DEFAULT (sysutcdatetime()),
	[UpdatedAt] [datetimeoffset](7) NULL,
	[IsDeleted] [bit] NOT NULL DEFAULT (0),
	CONSTRAINT [FK_SM_Product] FOREIGN KEY([ProductId]) REFERENCES [dbo].[Products] ([Id]),
	CONSTRAINT [FK_SM_User] FOREIGN KEY([CreatedBy]) REFERENCES [dbo].[Users] ([Id])
)
GO

/****** 8. Stored Procedures - Tối ưu hóa nghiệp vụ ******/

-- 8.1. SP Tạo đơn hàng và trừ kho (Sales Order)
CREATE PROCEDURE [dbo].[sp_CreateSalesOrder]
    @OrderNumber nvarchar(50),
    @CustomerId int,
    @OrderDate date,
    @PaymentMethod nvarchar(50),
    @ShippingAddress nvarchar(500),
    @Notes nvarchar(max),
    @CreatedBy int,
    @OrderItems nvarchar(max) -- Truyền danh sách item dưới dạng JSON
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRANSACTION;
    BEGIN TRY
        -- 1. Chèn đơn hàng chính
        DECLARE @SalesOrderId int;
        INSERT INTO [dbo].[SalesOrders] (OrderNumber, CustomerId, OrderDate, Status, PaymentMethod, PaymentStatus, TotalAmount, ShippingAddress, Notes, CreatedBy)
        VALUES (@OrderNumber, @CustomerId, @OrderDate, 'NEW', @PaymentMethod, 'PENDING', 0, @ShippingAddress, @Notes, @CreatedBy);
        
        SET @SalesOrderId = SCOPE_IDENTITY();

        -- 2. Đọc JSON và chèn vào SalesOrderItems
        INSERT INTO [dbo].[SalesOrderItems] (SalesOrderId, ProductId, Quantity, UnitPrice, Discount)
        SELECT 
            @SalesOrderId,
            ProductId,
            Quantity,
            UnitPrice,
            Discount
        FROM OPENJSON(@OrderItems)
        WITH (
            ProductId int '$.ProductId',
            Quantity int '$.Quantity',
            UnitPrice decimal(18,2) '$.UnitPrice',
            Discount decimal(18,2) '$.Discount'
        );

        -- 3. Cập nhật tồn kho và ghi nhật ký StockMovements
        DECLARE @Pid int, @Qty int;
        DECLARE item_cursor CURSOR FOR 
        SELECT ProductId, Quantity FROM [dbo].[SalesOrderItems] WHERE SalesOrderId = @SalesOrderId;

        OPEN item_cursor;
        FETCH NEXT FROM item_cursor INTO @Pid, @Qty;

        WHILE @@FETCH_STATUS = 0
        BEGIN
            -- Kiểm tra tồn kho
            IF (SELECT StockQuantity FROM Products WHERE Id = @Pid) < @Qty
            BEGIN
                DECLARE @ErrMsg nvarchar(250) = 'San pham ID ' + CAST(@Pid AS nvarchar) + ' khong du ton kho.';
                RAISERROR(@ErrMsg, 16, 1);
            END

            -- Trừ kho
            UPDATE Products SET StockQuantity = StockQuantity - @Qty WHERE Id = @Pid;

            -- Ghi nhật ký kho
            INSERT INTO StockMovements (ProductId, MovementType, Quantity, Reference, CreatedBy)
            VALUES (@Pid, 'OUT', @Qty, @OrderNumber, @CreatedBy);

            FETCH NEXT FROM item_cursor INTO @Pid, @Qty;
        END
        CLOSE item_cursor;
        DEALLOCATE item_cursor;

        -- 4. Tính toán lại tổng tiền đơn hàng
        UPDATE [dbo].[SalesOrders]
        SET TotalAmount = (SELECT SUM((UnitPrice - Discount) * Quantity) FROM SalesOrderItems WHERE SalesOrderId = @SalesOrderId)
        WHERE Id = @SalesOrderId;

        COMMIT TRANSACTION;
        SELECT @SalesOrderId AS SalesOrderId;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END
GO

-- 8.2. SP Xác nhận nhập hàng (Purchase Order)
CREATE PROCEDURE [dbo].[sp_ReceivePurchaseOrder]
    @POId int,
    @ReceivedDate date,
    @ReceivedBy int
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRANSACTION;
    BEGIN TRY
        -- 1. Cập nhật trạng thái PO
        UPDATE PurchaseOrders 
        SET Status = 'RECEIVED', ReceivedDate = @ReceivedDate, UpdatedAt = SYSDATETIMEOFFSET()
        WHERE Id = @POId AND Status = 'PENDING';

        IF @@ROWCOUNT = 0
        BEGIN
            RAISERROR('Don mua hang khong ton tai hoac da duoc xu ly truoc do.', 16, 1);
        END

        -- 2. Cộng tồn kho và ghi nhật ký StockMovements
        INSERT INTO StockMovements (ProductId, MovementType, Quantity, Reference, CreatedBy)
        SELECT 
            POI.ProductId, 'IN', POI.Quantity, PO.PONumber, @ReceivedBy
        FROM PurchaseOrderItems POI
        JOIN PurchaseOrders PO ON POI.PurchaseOrderId = PO.Id
        WHERE PO.Id = @POId;

        UPDATE P
        SET P.StockQuantity = P.StockQuantity + POI.Quantity,
            P.UpdatedAt = SYSDATETIMEOFFSET()
        FROM Products P
        JOIN PurchaseOrderItems POI ON P.Id = POI.ProductId
        WHERE POI.PurchaseOrderId = @POId;

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END
GO

-- 8.3. SP Báo cáo doanh thu
CREATE PROCEDURE [dbo].[sp_GetRevenueReport]
    @FromDate date,
    @ToDate date
AS
BEGIN
    SELECT 
        OrderDate,
        COUNT(Id) AS TotalOrders,
        SUM(TotalAmount) AS DailyRevenue
    FROM SalesOrders
    WHERE OrderDate BETWEEN @FromDate AND @ToDate 
      AND Status = 'COMPLETED' 
      AND IsDeleted = 0
    GROUP BY OrderDate
    ORDER BY OrderDate;
END
GO

-- 8.4. SP Xóa mềm tổng quát
CREATE PROCEDURE [dbo].[sp_SoftDelete]
    @TableName nvarchar(100),
    @Id int
AS
BEGIN
    DECLARE @Sql nvarchar(max);
    SET @Sql = 'UPDATE ' + QUOTENAME(@TableName) + 
               ' SET IsDeleted = 1, UpdatedAt = SYSDATETIMEOFFSET() WHERE Id = @Id';
    
    EXEC sp_executesql @Sql, N'@Id int', @Id;
END
GO

