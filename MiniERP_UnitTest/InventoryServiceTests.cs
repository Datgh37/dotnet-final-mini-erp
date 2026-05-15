namespace MiniERP_UnitTest
{
    public class InventoryServiceTests
    {
        private readonly Mock<IInventoryRepository> _mockRepo;
        private readonly Mock<IProductRepository> _mockProductRepo;
        private readonly Mock<IMapper> _mockMapper;
        private readonly InventoryService _inventoryService;

        public InventoryServiceTests()
        {
            _mockRepo = new Mock<IInventoryRepository>();
            _mockProductRepo = new Mock<IProductRepository>();
            _mockMapper = new Mock<IMapper>();
            _inventoryService = new InventoryService(_mockRepo.Object, _mockProductRepo.Object, _mockMapper.Object);
        }

        [Fact]
        public void AdjustStock_ZeroQuantity_ThrowsException()
        {
            // Arrange
            var dto = new StockAdjustDto { ProductId = 1, Quantity = 0, Reason = "Test" };

            // Act & Assert
            Action act = () => _inventoryService.AdjustStock(dto, 1);
            act.Should().Throw<Exception>().WithMessage("Số lượng điều chỉnh phải khác 0.");
        }

        [Fact]
        public void AdjustStock_EmptyReason_ThrowsException()
        {
            // Arrange
            var dto = new StockAdjustDto { ProductId = 1, Quantity = 5, Reason = "" };

            // Act & Assert
            Action act = () => _inventoryService.AdjustStock(dto, 1);
            act.Should().Throw<Exception>().WithMessage("Lý do điều chỉnh không được để trống.");
        }

        [Fact]
        public void AdjustStock_Success_CallsRepository()
        {
            // Arrange
            var dto = new StockAdjustDto { ProductId = 1, Quantity = 10, Reason = "Nhập kho thêm" };

            // Act
            _inventoryService.AdjustStock(dto, 1);

            // Assert
            _mockRepo.Verify(r => r.AdjustStock(1, 10, "Nhập kho thêm", 1), Times.Once);
        }
    }
}
