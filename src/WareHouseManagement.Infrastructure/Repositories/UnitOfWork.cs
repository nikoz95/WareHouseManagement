﻿﻿using Microsoft.EntityFrameworkCore.Storage;
using WareHouseManagement.Domain.Interfaces;
using WareHouseManagement.Infrastructure.Data;

namespace WareHouseManagement.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;
    private IDbContextTransaction? _transaction;

    public UnitOfWork(
        ApplicationDbContext context,
        ICompanyRepository companies,
        IProductRepository products,
        IWarehouseRepository warehouses,
        IOrderRepository orders,
        IDebtorRepository debtors,
        IManufacturerRepository manufacturers,
        IUnitTypeRuleRepository unitTypeRules,
        IProductDetailsRepository productDetails,
        IAlcoholicProductDetailsRepository alcoholicProductDetails)
    {
        _context = context;
        Companies = companies;
        Products = products;
        Warehouses = warehouses;
        Orders = orders;
        Debtors = debtors;
        Manufacturers = manufacturers;
        UnitTypeRules = unitTypeRules;
        ProductDetails = productDetails;
        AlcoholicProductDetails = alcoholicProductDetails;
    }

    public ICompanyRepository Companies { get; }
    public IProductRepository Products { get; }
    public IWarehouseRepository Warehouses { get; }
    public IOrderRepository Orders { get; }
    public IDebtorRepository Debtors { get; }
    public IManufacturerRepository Manufacturers { get; }
    public IUnitTypeRuleRepository UnitTypeRules { get; }
    public IProductDetailsRepository ProductDetails { get; }
    public IAlcoholicProductDetailsRepository AlcoholicProductDetails { get; }

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public async Task BeginTransactionAsync()
    {
        _transaction = await _context.Database.BeginTransactionAsync();
    }

    public async Task CommitTransactionAsync()
    {
        try
        {
            await _context.SaveChangesAsync();
            if (_transaction != null)
            {
                await _transaction.CommitAsync();
            }
        }
        catch
        {
            await RollbackTransactionAsync();
            throw;
        }
        finally
        {
            if (_transaction != null)
            {
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }
    }

    public async Task RollbackTransactionAsync()
    {
        if (_transaction != null)
        {
            await _transaction.RollbackAsync();
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    public void Dispose()
    {
        _transaction?.Dispose();
        _context.Dispose();
    }
}

