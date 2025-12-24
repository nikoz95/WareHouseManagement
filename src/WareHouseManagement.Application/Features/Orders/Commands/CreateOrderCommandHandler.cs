﻿using AutoMapper;
using MediatR;
using WareHouseManagement.Application.Common.Models;
using WareHouseManagement.Application.DTOs;
using WareHouseManagement.Domain.Entities;
using WareHouseManagement.Domain.Enums;
using WareHouseManagement.Domain.Interfaces;

namespace WareHouseManagement.Application.Features.Orders.Commands;

public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, Result<OrderDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateOrderCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<OrderDto>> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await _unitOfWork.BeginTransactionAsync();

            // შეკვეთის ნომრის გენერაცია
            var orderNumber = await _unitOfWork.Orders.GenerateOrderNumberAsync();

            // შეკვეთის შექმნა
            var order = new Order
            {
                Id = Guid.NewGuid(),
                OrderNumber = orderNumber,
                CompanyId = request.CompanyId,
                CustomerName = request.CustomerName,
                CustomerPhone = request.CustomerPhone,
                CustomerEmail = request.CustomerEmail,
                OrderDate = DateTime.UtcNow,
                Status = OrderStatus.Pending,
                PaymentStatus = PaymentStatus.Unpaid,
                Notes = request.Notes,
                CreatedAt = DateTime.UtcNow
            };

            decimal totalAmount = 0;

            // შეკვეთის ელემენტების დამატება
            foreach (var itemDto in request.OrderItems)
            {
                var product = await _unitOfWork.Products.GetByIdAsync(itemDto.ProductId);
                if (product == null)
                {
                    await _unitOfWork.RollbackTransactionAsync();
                    return Result<OrderDto>.Failure($"პროდუქტი ID {itemDto.ProductId} არ მოიძებნა");
                }

                // საწყობიდან პროდუქტის სტოკების მიღება
                var stocks = (await _unitOfWork.Warehouses.GetStockByProductAsync(itemDto.ProductId)).ToList();
                var firstStock = stocks.FirstOrDefault();
                if (firstStock == null)
                {
                    await _unitOfWork.RollbackTransactionAsync();
                    return Result<OrderDto>.Failure($"პროდუქტი არ არის საწყობში: {product.Name}");
                }
                
                var bottlesPerBox = firstStock.BottlesPerBox;
                var totalPrice = (itemDto.QuantityInBottles + itemDto.QuantityInBoxes * bottlesPerBox) * itemDto.UnitPrice;

                var orderItem = new OrderItem
                {
                    Id = Guid.NewGuid(),
                    OrderId = order.Id,
                    ProductId = itemDto.ProductId,
                    BottlesPerBox = bottlesPerBox,
                    QuantityInBottles = itemDto.QuantityInBottles,
                    QuantityInBoxes = itemDto.QuantityInBoxes,
                    UnitPrice = itemDto.UnitPrice,
                    TotalPrice = totalPrice,
                    CreatedAt = DateTime.UtcNow
                };

                order.OrderItems.Add(orderItem);
                totalAmount += totalPrice;

                // საწყობიდან პროდუქტის გამოკლება
                var totalNeededBottles = itemDto.QuantityInBottles + (itemDto.QuantityInBoxes * bottlesPerBox);
                
                int remainingBottles = totalNeededBottles;
                foreach (var stock in stocks.OrderBy(s => s.ExpirationDate))
                {
                    if (remainingBottles <= 0) break;

                    var availableBottles = stock.QuantityInBottles + (stock.QuantityInBoxes * stock.BottlesPerBox);
                    
                    if (availableBottles > 0)
                    {
                        var bottlesToTake = Math.Min(availableBottles, remainingBottles);
                        
                        // გამოკლება ბოთლებიდან
                        stock.QuantityInBottles -= bottlesToTake;
                        
                        // თუ ბოთლები უარყოფითია, გადავიტანოთ ყუთებიდან
                        while (stock.QuantityInBottles < 0 && stock.QuantityInBoxes > 0)
                        {
                            stock.QuantityInBoxes--;
                            stock.QuantityInBottles += stock.BottlesPerBox;
                        }
                        
                        stock.UpdatedAt = DateTime.UtcNow;
                        remainingBottles -= bottlesToTake;
                    }
                }

                if (remainingBottles > 0)
                {
                    await _unitOfWork.RollbackTransactionAsync();
                    return Result<OrderDto>.Failure($"არასაკმარისი მარაგი პროდუქტისთვის: {product.Name}");
                }
            }

            order.TotalAmount = totalAmount;
            order.DebtAmount = totalAmount;

            await _unitOfWork.Orders.AddAsync(order);

            // დებიტორის სიაში დამატება
            var debtor = new Debtor
            {
                Id = Guid.NewGuid(),
                CompanyId = request.CompanyId,
                DebtorName = request.CustomerName,
                Phone = request.CustomerPhone,
                Email = request.CustomerEmail,
                TotalDebt = totalAmount,
                PaidAmount = 0,
                RemainingDebt = totalAmount,
                DebtDate = DateTime.UtcNow,
                CreatedAt = DateTime.UtcNow
            };

            await _unitOfWork.Debtors.AddAsync(debtor);
            await _unitOfWork.SaveChangesAsync();
            await _unitOfWork.CommitTransactionAsync();

            var orderDto = _mapper.Map<OrderDto>(order);
            return Result<OrderDto>.Success(orderDto, "შეკვეთა წარმატებით შეიქმნა");
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackTransactionAsync();
            return Result<OrderDto>.Failure($"შეცდომა შეკვეთის შექმნისას: {ex.Message}");
        }
    }
}

