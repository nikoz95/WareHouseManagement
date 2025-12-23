using AutoMapper;
using WareHouseManagement.Application.DTOs;
using WareHouseManagement.Domain.Entities;

namespace WareHouseManagement.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Company mappings
        CreateMap<Company, CompanyDto>()
            .ForMember(dest => dest.CompanyTypeDescription, opt => opt.MapFrom(src => src.CompanyType.ToString()));
        CreateMap<CreateCompanyDto, Company>();
        CreateMap<UpdateCompanyDto, Company>();

        // Product mappings
        CreateMap<Product, ProductDto>();
        CreateMap<CreateProductDto, Product>();
        CreateMap<UpdateProductDto, Product>();

        // Order mappings
        CreateMap<Order, OrderDto>()
            .ForMember(dest => dest.CompanyName, opt => opt.MapFrom(src => src.Company != null ? src.Company.Name : null))
            .ForMember(dest => dest.StatusDescription, opt => opt.MapFrom(src => src.Status.ToString()))
            .ForMember(dest => dest.PaymentStatusDescription, opt => opt.MapFrom(src => src.PaymentStatus.ToString()));
        
        CreateMap<OrderItem, OrderItemDto>()
            .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.Name));

        // WarehouseStock mappings
        CreateMap<WarehouseStock, WarehouseStockDto>()
            .ForMember(dest => dest.WarehouseLocationName, opt => opt.MapFrom(src => src.WarehouseLocation.LocationName))
            .ForMember(dest => dest.WarehouseName, opt => opt.MapFrom(src => src.WarehouseLocation.Warehouse.Name))
            .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.Name))
            .ForMember(dest => dest.ManufacturerName, opt => opt.MapFrom(src => src.Manufacturer.Name));

        // Debtor mappings
        CreateMap<Debtor, DebtorDto>()
            .ForMember(dest => dest.CompanyName, opt => opt.MapFrom(src => src.Company != null ? src.Company.Name : null))
            .ForMember(dest => dest.IsPartnerCompany, opt => opt.MapFrom(src => src.Company != null && src.Company.IsPartner));
    }
}

