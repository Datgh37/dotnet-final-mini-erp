namespace MiniERP_UnitTest
{
    public class SalesOrderServiceTests
    {
        private readonly Mock<ISalesOrderRepository> _mockRepo;
        private readonly Mock<ICustomerRepository> _mockCustomerRepo;
        private readonly Mock<IProductRepository> _mockProductRepo;
        private readonly Mock<IInventoryService> _mockInventory;
        private readonly Mock<IMapper> _mockMapper;
        private readonly SalesOrderService _salesOrderService;

        public SalesOrderServiceTests()
        {
            _mockRepo = new Mock<ISalesOrderRepository>();
            _mockCustomerRepo = new Mock<ICustomerRepository>();
            _mockProductRepo = new Mock<IProductRepository>();
            _mockInventory = new Mock<IInventoryService>();
            _mockMapper = new Mock<IMapper>();
            _salesOrderService = new SalesOrderService(
                _mockRepo.Object, 
                _mockCustomerRepo.Object, 
                _mockProductRepo.Object,
                _mockInventory.Object,
                _mockMapper.Object);
        }

        [Fact]
        public void PlaceOrder_EmptyItems_ThrowsException()
        {
            // Arrange
            var dto = new CreateSalesOrderDto { Items = new List<CreateSalesOrderItemDto>() };

            // Act & Assert
            Action act = () => _salesOrderService.PlaceOrder(dto);
            act.Should().Throw<Exception>().WithMessage("Đơn hàng phải có ít nhất một sản phẩm.");
        }

        [Fact]
        public void PlaceOrder_Success_ReturnsOrderId()
        {
            // Arrange
            var dto = new CreateSalesOrderDto 
            { 
                CustomerId = 1,
                Items = new List<CreateSalesOrderItemDto> 
                { 
                    new CreateSalesOrderItemDto { ProductId = 1, Quantity = 2, UnitPrice = 100 } 
                } 
            };
            var entity = new SalesOrder();
            
            _mockMapper.Setup(m => m.Map<SalesOrder>(dto)).Returns(entity);
            _mockRepo.Setup(r => r.CreateOrder(entity)).Returns(500);

            // Act
            var result = _salesOrderService.PlaceOrder(dto);

            // Assert
            result.Should().Be(500);
            entity.TotalAmount.Should().Be(200);
            entity.OrderNumber.Should().StartWith("SO-");
        }

        [Fact]
        public void UpdateStatus_ToCompleted_DeductsStock()
        {
            // Arrange
            var orderId = 1;
            var order = new SalesOrder 
            { 
                Id = orderId, 
                Status = "NEW", 
                OrderNumber = "SO-TEST",
                Items = new List<SalesOrderItem> 
                { 
                    new SalesOrderItem { ProductId = 10, Quantity = 5 } 
                } 
            };
            _mockRepo.Setup(r => r.GetById(orderId)).Returns(order);

            // Act
            _salesOrderService.UpdateStatus(orderId, "COMPLETED");

            // Assert
            _mockInventory.Verify(i => i.AdjustStock(
                It.Is<StockAdjustDto>(d => d.ProductId == 10 && d.Quantity == -5), 
                null), Times.Once);
            _mockRepo.Verify(r => r.UpdateStatus(orderId, "COMPLETED"), Times.Once);
        }
    }
}
