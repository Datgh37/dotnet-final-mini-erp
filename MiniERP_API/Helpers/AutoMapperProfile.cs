using AutoMapper;
using MiniERP_API.Models.Entities;
using MiniERP_API.Models.DTOs;

namespace MiniERP_API.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            // Product
            CreateMap<Product, ProductDto>();
            CreateMap<ProductCreateUpdateDto, Product>();

            // Supplier
            CreateMap<Supplier, SupplierDto>();
            CreateMap<SupplierCreateUpdateDto, Supplier>();

            // Brand
            CreateMap<Brand, BrandDto>();
            CreateMap<BrandCreateUpdateDto, Brand>();

            // Category
            CreateMap<ProductCategory, CategoryDto>();
            CreateMap<CategoryCreateUpdateDto, ProductCategory>();

            // Customer
            CreateMap<Customer, CustomerDto>();
            CreateMap<CustomerCreateUpdateDto, Customer>();

            // Role
            CreateMap<Role, RoleDto>();
            CreateMap<RoleDto, Role>();

            // User
            CreateMap<User, UserDto>();

            // SalesOrder
            CreateMap<SalesOrder, SalesOrderDto>();
            CreateMap<SalesOrderItem, SalesOrderItemDto>();
            CreateMap<CreateSalesOrderDto, SalesOrder>()
                .ForMember(dest => dest.OrderDate, opt => opt.MapFrom(src => src.OrderDate ?? DateTime.Now))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => "NEW"))
                .ForMember(dest => dest.PaymentStatus, opt => opt.MapFrom(src => "PENDING"));
            CreateMap<CreateSalesOrderItemDto, SalesOrderItem>();

            // PurchaseOrder
            CreateMap<PurchaseOrder, PurchaseOrderDto>();
            CreateMap<PurchaseOrderItem, PurchaseOrderItemDto>();
            CreateMap<CreatePurchaseOrderDto, PurchaseOrder>();
            CreateMap<CreatePurchaseOrderItemDto, PurchaseOrderItem>();

            // StockMovement
            CreateMap<StockMovement, StockMovementDto>();
        }
    }
}
