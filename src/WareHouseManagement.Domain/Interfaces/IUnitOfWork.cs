﻿﻿namespace WareHouseManagement.Domain.Interfaces;

public interface IUnitOfWork : IDisposable
{
    ICompanyRepository Companies { get; }
    IProductRepository Products { get; }
    IWarehouseRepository Warehouses { get; }
    IOrderRepository Orders { get; }
    IDebtorRepository Debtors { get; }
    IManufacturerRepository Manufacturers { get; }
    IUnitTypeRuleRepository UnitTypeRules { get; }
    IProductDetailsRepository ProductDetails { get; }
    IAlcoholicProductDetailsRepository AlcoholicProductDetails { get; }
    
    Task<int> SaveChangesAsync();
    Task BeginTransactionAsync();
    Task CommitTransactionAsync();
    Task RollbackTransactionAsync();
}

