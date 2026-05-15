namespace MiniERP_UnitTest
{
    public class PurchaseOrderServiceTests
    {
        private readonly Mock<IPurchaseOrderRepository> _mockRepo;
        private readonly Mock<ISupplierRepository> _mockSupplierRepo;
        private readonly Mock<IProductRepository> _mockProductRepo;
        private readonly Mock<IMapper> _mockMapper;
        private readonly PurchaseOrderService _poService;

        public PurchaseOrderServiceTests()
        {
            _mockRepo = new Mock<IPurchaseOrderRepository>();
            _mockSupplierRepo = new Mock<ISupplierRepository>();
            _mockProductRepo = new Mock<IProductRepository>();
            _mockMapper = new Mock<IMapper>();
            _poService = new PurchaseOrderService(
                _mockRepo.Object, 
                _mockSupplierRepo.Object, 
                _mockProductRepo.Object, 
                _mockMapper.Object);
        }

        [Fact]
        public void CreateOrder_NegativePrice_ThrowsException()
        {
            // Arrange
            var dto = new CreatePurchaseOrderDto
            {
                Items = new List<CreatePurchaseOrderItemDto>
                {
                    new CreatePurchaseOrderItemDto { ProductId = 1, Quantity = 10, UnitPrice = -50 }
                }
            };

            // Act & Assert
            Action act = () => _poService.CreateOrder(dto);
            act.Should().Throw<Exception>().WithMessage("Đơn giá không được âm.");
        }

        [Fact]
        public void ReceiveOrder_AlreadyReceived_ThrowsException()
        {
            // Arrange
            var orderId = 1;
            var order = new PurchaseOrder { Id = orderId, Status = "RECEIVED" };
            _mockRepo.Setup(r => r.GetById(orderId)).Returns(order);

            // Act & Assert
            Action act = () => _poService.ReceiveOrder(orderId, DateTime.Now, 1);
            act.Should().Throw<Exception>().WithMessage("Đơn mua hàng đã được xử lý hoặc hủy.");
        }

        [Fact]
        public void CreateOrder_Success_CalculatesTotal()
        {
            // Arrange
            var dto = new CreatePurchaseOrderDto
            {
                Items = new List<CreatePurchaseOrderItemDto>
                {
                    new CreatePurchaseOrderItemDto { ProductId = 1, Quantity = 2, UnitPrice = 100 },
                    new CreatePurchaseOrderItemDto { ProductId = 2, Quantity = 3, UnitPrice = 200 }
                }
            };
            var entity = new PurchaseOrder();
            _mockMapper.Setup(m => m.Map<PurchaseOrder>(dto)).Returns(entity);
            _mockRepo.Setup(r => r.CreateOrder(entity)).Returns(123);

            // Act
            var result = _poService.CreateOrder(dto);

            // Assert
            result.Should().Be(123);
            entity.TotalAmount.Should().Be(800); // (2*100) + (3*200)
            entity.PONumber.Should().StartWith("PO-");
        }
    }
}
