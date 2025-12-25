﻿﻿﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WareHouseManagement.Domain.Interfaces;
using WareHouseManagement.Infrastructure.Data;
using WareHouseManagement.Infrastructure.Repositories;
using WareHouseManagement.Infrastructure.Services;

namespace WareHouseManagement.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // Database
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(
                configuration.GetConnectionString("DefaultConnection"),
                b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));

        // Repositories
        services.AddScoped<ICompanyRepository, CompanyRepository>();
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IWarehouseRepository, WarehouseRepository>();
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<IDebtorRepository, DebtorRepository>();
        services.AddScoped<IManufacturerRepository, ManufacturerRepository>();
        services.AddScoped<IUnitTypeRuleRepository, UnitTypeRuleRepository>();
        services.AddScoped<IProductDetailsRepository, ProductDetailsRepository>();
        services.AddScoped<IAlcoholicProductDetailsRepository, AlcoholicProductDetailsRepository>();
        services.AddScoped<IWarehouseStockHistoryRepository, WarehouseStockHistoryRepository>();
        
        // Unit of Work
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        
        // Services
        services.AddScoped<IExcelImportService, ExcelImportService>();

        return services;
    }
}

