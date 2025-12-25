using MediatR;
using WareHouseManagement.Application.Common.Models;
using WareHouseManagement.Application.DTOs;
using WareHouseManagement.Application.Mappings;
using WareHouseManagement.Domain.Interfaces;

namespace WareHouseManagement.Application.Features.Manufacturers.Queries;

public class GetManufacturerByIdQueryHandler : IRequestHandler<GetManufacturerByIdQuery, Result<ManufacturerDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ApplicationMapper _mapper;

    public GetManufacturerByIdQueryHandler(IUnitOfWork unitOfWork, ApplicationMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<ManufacturerDto>> Handle(GetManufacturerByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var manufacturer = await _unitOfWork.Manufacturers.GetByIdAsync(request.Id);

            if (manufacturer == null)
            {
                return Result<ManufacturerDto>.Failure($"მწარმოებელი ID {request.Id} ვერ მოიძებნა");
            }

            var manufacturerDto = _mapper.MapToManufacturerDto(manufacturer);
            return Result<ManufacturerDto>.Success(manufacturerDto);
        }
        catch (Exception ex)
        {
            return Result<ManufacturerDto>.Failure($"შეცდომა მწარმოებლის ჩატვირთვისას: {ex.Message}");
        }
    }
}

