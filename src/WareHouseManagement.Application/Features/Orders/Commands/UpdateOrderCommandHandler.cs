using MediatR;
using WareHouseManagement.Application.Common.Models;
using WareHouseManagement.Application.DTOs;
using WareHouseManagement.Application.Mappings;
using WareHouseManagement.Domain.Enums;
using WareHouseManagement.Domain.Interfaces;

namespace WareHouseManagement.Application.Features.Orders.Commands;

public class UpdateOrderCommandHandler : IRequestHandler<UpdateOrderCommand, Result<OrderDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ApplicationMapper _mapper;

    public UpdateOrderCommandHandler(IUnitOfWork unitOfWork, ApplicationMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<OrderDto>> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // შეკვეთის პოვნა
            var order = await _unitOfWork.Orders.GetByIdAsync(request.Id);
            if (order == null)
            {
                return Result<OrderDto>.Failure($"შეკვეთა ID {request.Id} არ მოიძებნა");
            }

            // Status განახლება
            if (request.Status.HasValue)
            {
                order.Status = request.Status.Value;
            }

            // PaymentStatus განახლება
            if (request.PaymentStatus.HasValue)
            {
                order.PaymentStatus = request.PaymentStatus.Value;
            }

            // PaidAmount განახლება
            if (request.PaidAmount.HasValue)
            {
                order.PaidAmount = request.PaidAmount.Value;
                order.DebtAmount = order.TotalAmount - order.PaidAmount;

                // თუ სრულად გადახდილია
                if (order.PaidAmount >= order.TotalAmount)
                {
                    order.PaymentStatus = PaymentStatus.Paid;
                    order.DebtAmount = 0;
                }
                else if (order.PaidAmount > 0)
                {
                    order.PaymentStatus = PaymentStatus.PartiallyPaid;
                }

                // დებიტორის განახლება
                if (order.CompanyId.HasValue)
                {
                    var debtors = await _unitOfWork.Debtors.GetDebtorsByCompanyAsync(order.CompanyId.Value);
                    var debtor = debtors.FirstOrDefault(d => d.DebtorName == order.CustomerName);
                    
                    if (debtor != null)
                    {
                        debtor.PaidAmount = order.PaidAmount;
                        debtor.RemainingDebt = order.DebtAmount;
                        
                        if (order.PaidAmount > 0)
                        {
                            debtor.LastPaymentDate = DateTime.UtcNow;
                        }
                        
                        debtor.UpdatedAt = DateTime.UtcNow;
                    }
                }
            }

            // Notes განახლება
            if (!string.IsNullOrEmpty(request.Notes))
            {
                order.Notes = request.Notes;
            }

            order.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.SaveChangesAsync();

            var orderDto = _mapper.MapToOrderDto(order);
            return Result<OrderDto>.Success(orderDto, "შეკვეთა წარმატებით განახლდა");
        }
        catch (Exception ex)
        {
            return Result<OrderDto>.Failure($"შეცდომა შეკვეთის განახლებისას: {ex.Message}");
        }
    }
}

