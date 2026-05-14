namespace MiniERP_API.Helpers
{
    public static class Queries
    {
        #region Role Queries
        public const string GetAllRoles = "SELECT * FROM Roles WHERE IsDeleted = 0";
        public const string InsertRole = "INSERT INTO Roles (Name) VALUES (@Name); SELECT CAST(SCOPE_IDENTITY() as int);";
        #endregion

        #region User Queries
        public const string GetAllUsers = "SELECT * FROM Users WHERE IsDeleted = 0";
        public const string GetUserById = "SELECT * FROM Users WHERE Id = @Id AND IsDeleted = 0";
        public const string GetUserByUserName = "SELECT * FROM Users WHERE UserName = @Name AND IsDeleted = 0";
        public const string InsertUser = "INSERT INTO Users (UserName, Email, PasswordHash, FullName) VALUES (@UserName, @Email, @Pass, @Name); SELECT CAST(SCOPE_IDENTITY() as int);";
        public const string AssignRole = "INSERT INTO UserRoles (UserId, RoleId) VALUES (@UserId, @RoleId)";
        public const string GetRoleIdByName = "SELECT Id FROM Roles WHERE Name = @Name AND IsDeleted = 0";
        public const string UpdateUser = "UPDATE Users SET Email = @Email, FullName = @Name, UpdatedAt = SYSDATETIMEOFFSET() WHERE Id = @Id";
        public const string ChangePassword = "UPDATE Users SET PasswordHash = @Hash, UpdatedAt = SYSDATETIMEOFFSET() WHERE Id = @Id";
        public const string GetUserRoleName = @"
            SELECT TOP 1 R.Name 
            FROM Roles R 
            JOIN UserRoles UR ON R.Id = UR.RoleId 
            WHERE UR.UserId = @UserId AND R.IsDeleted = 0";
        #endregion

        #region Brand Queries
        public const string GetAllBrands = "SELECT * FROM Brands WHERE IsDeleted = 0";
        public const string GetBrandById = "SELECT * FROM Brands WHERE Id = @Id AND IsDeleted = 0";
        public const string GetBrandByName = "SELECT * FROM Brands WHERE Name = @Name AND IsDeleted = 0";
        public const string InsertBrand = "INSERT INTO Brands (Name, Description) VALUES (@Name, @Desc); SELECT CAST(SCOPE_IDENTITY() as int);";
        public const string UpdateBrand = "UPDATE Brands SET Name = @Name, Description = @Desc, UpdatedAt = SYSDATETIMEOFFSET() WHERE Id = @Id";
        public const string DeleteBrand = "UPDATE Brands SET IsDeleted = 1 WHERE Id = @Id";
        #endregion

        #region Category Queries
        public const string GetAllCategories = "SELECT * FROM ProductCategories WHERE IsDeleted = 0";
        public const string GetCategoryById = "SELECT * FROM ProductCategories WHERE Id = @Id AND IsDeleted = 0";
        public const string GetCategoryByName = "SELECT * FROM ProductCategories WHERE Name = @Name AND IsDeleted = 0";
        public const string InsertCategory = "INSERT INTO ProductCategories (Name, ParentCategoryId) VALUES (@Name, @ParentId); SELECT CAST(SCOPE_IDENTITY() as int);";
        public const string UpdateCategory = "UPDATE ProductCategories SET Name = @Name, ParentCategoryId = @ParentId, UpdatedAt = SYSDATETIMEOFFSET() WHERE Id = @Id";
        public const string DeleteCategory = "UPDATE ProductCategories SET IsDeleted = 1 WHERE Id = @Id";
        #endregion

        #region Supplier Queries
        public const string GetAllSuppliers = "SELECT * FROM Suppliers WHERE IsDeleted = 0";
        public const string GetSupplierById = "SELECT * FROM Suppliers WHERE Id = @Id AND IsDeleted = 0";
        public const string GetSupplierByName = "SELECT * FROM Suppliers WHERE Name = @Name AND IsDeleted = 0";
        public const string InsertSupplier = @"
            INSERT INTO Suppliers (Name, ContactPerson, Phone, Email, Address, CreatedAt, IsDeleted)
            VALUES (@Name, @ContactPerson, @Phone, @Email, @Address, SYSDATETIMEOFFSET(), 0);
            SELECT CAST(SCOPE_IDENTITY() as int);";
        public const string UpdateSupplier = @"
            UPDATE Suppliers SET 
                Name = @Name, ContactPerson = @ContactPerson, 
                Phone = @Phone, Email = @Email, Address = @Address,
                UpdatedAt = SYSDATETIMEOFFSET()
            WHERE Id = @Id";
        public const string DeleteSupplier = "UPDATE Suppliers SET IsDeleted = 1 WHERE Id = @Id";
        #endregion

        #region Product Queries
        public const string GetAllProducts = "SELECT * FROM Products WHERE IsDeleted = 0";
        public const string GetProductById = "SELECT * FROM Products WHERE Id = @Id AND IsDeleted = 0";
        public const string GetProductBySku = "SELECT * FROM Products WHERE SKU = @SKU AND IsDeleted = 0";
        public const string InsertProduct = @"
            INSERT INTO Products (CategoryId, BrandId, SKU, Name, Description, Unit, CostPrice, RetailPrice, StockQuantity, ImageUrl, CreatedAt, IsDeleted)
            VALUES (@CategoryId, @BrandId, @SKU, @Name, @Description, @Unit, @CostPrice, @RetailPrice, @StockQuantity, @ImageUrl, SYSDATETIMEOFFSET(), 0);
            SELECT CAST(SCOPE_IDENTITY() as int);";
        public const string UpdateProduct = @"
            UPDATE Products SET 
                CategoryId = @CategoryId, BrandId = @BrandId, SKU = @SKU, Name = @Name, 
                Description = @Description, Unit = @Unit, 
                CostPrice = @CostPrice, 
                RetailPrice = @RetailPrice, StockQuantity = @StockQuantity, 
                ImageUrl = @ImageUrl, UpdatedAt = SYSDATETIMEOFFSET()
            WHERE Id = @Id";
        public const string DeleteProduct = "UPDATE Products SET IsDeleted = 1 WHERE Id = @Id";
        public const string UpdateStockQuantity = "UPDATE Products SET StockQuantity = StockQuantity + @Qty, UpdatedAt = SYSDATETIMEOFFSET() WHERE Id = @Id";
        #endregion

        #region Customer Queries
        public const string GetAllCustomers = "SELECT * FROM Customers WHERE IsDeleted = 0";
        public const string GetCustomerById = "SELECT * FROM Customers WHERE Id = @Id AND IsDeleted = 0";
        public const string InsertCustomer = @"
            INSERT INTO Customers (UserId, Name, Email, Phone, Address, CreatedAt, IsDeleted) 
            VALUES (@UserId, @Name, @Email, @Phone, @Address, SYSDATETIMEOFFSET(), 0); 
            SELECT CAST(SCOPE_IDENTITY() as int);";
        public const string UpdateCustomer = "UPDATE Customers SET Name = @Name, Email = @Email, Phone = @Phone, Address = @Address, UpdatedAt = SYSDATETIMEOFFSET() WHERE Id = @Id";
        public const string DeleteCustomer = "UPDATE Customers SET IsDeleted = 1 WHERE Id = @Id";
        #endregion

        #region Purchase Order Queries
        public const string GetAllPurchaseOrders = "SELECT * FROM PurchaseOrders WHERE IsDeleted = 0 ORDER BY CreatedAt DESC";
        public const string GetPurchaseOrderById = "SELECT * FROM PurchaseOrders WHERE Id = @Id AND IsDeleted = 0";
        public const string GetPurchaseOrderByNumber = "SELECT * FROM PurchaseOrders WHERE PONumber = @Num AND IsDeleted = 0";
        public const string GetPurchaseOrderItemsByOrderId = "SELECT * FROM PurchaseOrderItems WHERE PurchaseOrderId = @Id";
        public const string InsertPurchaseOrder = @"
            INSERT INTO PurchaseOrders (PONumber, SupplierId, OrderDate, ExpectedDate, Status, TotalAmount, Notes, CreatedBy, CreatedAt, IsDeleted)
            VALUES (@PONumber, @SupplierId, @OrderDate, @ExpectedDate, 'PENDING', @TotalAmount, @Notes, @CreatedBy, SYSDATETIMEOFFSET(), 0);
            SELECT CAST(SCOPE_IDENTITY() as int);";
        public const string InsertPurchaseOrderItem = @"
            INSERT INTO PurchaseOrderItems (PurchaseOrderId, ProductId, Quantity, UnitPrice) 
            VALUES (@POId, @ProductId, @Qty, @Price)";
        public const string CancelPurchaseOrder = "UPDATE PurchaseOrders SET Status = 'CANCELLED', UpdatedAt = SYSDATETIMEOFFSET() WHERE Id = @Id AND Status = 'PENDING'";
        #endregion

        #region Sales Order Queries
        public const string GetAllSalesOrders = "SELECT * FROM SalesOrders WHERE IsDeleted = 0 ORDER BY CreatedAt DESC";
        public const string GetSalesOrderById = "SELECT * FROM SalesOrders WHERE Id = @Id AND IsDeleted = 0";
        public const string GetSalesOrderByNumber = "SELECT * FROM SalesOrders WHERE OrderNumber = @Num AND IsDeleted = 0";
        public const string GetSalesOrderItemsByOrderId = "SELECT * FROM SalesOrderItems WHERE SalesOrderId = @OrderId";
        public const string UpdateSalesOrderStatus = "UPDATE SalesOrders SET Status = @Status, UpdatedAt = SYSDATETIMEOFFSET() WHERE Id = @Id";
        #endregion

        #region Inventory Queries
        public const string GetStockMovementsBase = "SELECT * FROM StockMovements WHERE IsDeleted = 0";
        public const string InsertStockMovement = @"
            INSERT INTO StockMovements (ProductId, MovementType, Quantity, Reference, CreatedBy, CreatedAt, IsDeleted) 
            VALUES (@ProductId, @MovementType, @Qty, @Reference, @CreatedBy, SYSDATETIMEOFFSET(), 0)";
        #endregion

        #region Report Queries
        public const string GetTopSellingProducts = @"
            SELECT TOP (@Top) P.Id, P.Name, P.SKU, 
            SUM(SOI.Quantity) AS TotalSold, SUM((SOI.UnitPrice - SOI.Discount) * SOI.Quantity) AS TotalRevenue
            FROM SalesOrderItems SOI
            JOIN Products P ON SOI.ProductId = P.Id
            JOIN SalesOrders SO ON SOI.SalesOrderId = SO.Id
            WHERE SO.Status = 'COMPLETED' AND SO.IsDeleted = 0
            GROUP BY P.Id, P.Name, P.SKU
            ORDER BY TotalSold DESC";
        public const string GetLowStockProducts = @"
            SELECT P.Id, P.Name, P.SKU, P.StockQuantity, PC.Name AS CategoryName
            FROM Products P LEFT JOIN ProductCategories PC ON P.CategoryId = PC.Id
            WHERE P.StockQuantity < @Threshold AND P.IsDeleted = 0
            ORDER BY P.StockQuantity ASC";
        #endregion
    }
}
