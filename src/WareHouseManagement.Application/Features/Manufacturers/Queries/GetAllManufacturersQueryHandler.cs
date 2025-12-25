using MediatR;
using WareHouseManagement.Application.Common.Models;
using WareHouseManagement.Application.DTOs;
using WareHouseManagement.Domain.Interfaces;

namespace WareHouseManagement.Application.Features.Manufacturers.Queries;

public class GetAllManufacturersQueryHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<GetAllManufacturersQuery, Result<List<ManufacturerDto>>>
{
    public async Task<Result<List<ManufacturerDto>>> Handle(GetAllManufacturersQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var manufacturers = await unitOfWork.Manufacturers.GetAllAsync();

            var manufacturerDtos = manufacturers.Select(m => new ManufacturerDto
            {
                Id = m.Id,
                Name = m.Name,
                Country = m.Country,
                ContactInfo = m.ContactInfo,
                Description = m.Description,
                CreatedAt = m.CreatedAt
            }).ToList();

            return Result<List<ManufacturerDto>>.Success(manufacturerDtos);
        }
        catch (Exception ex)
        {
            return Result<List<ManufacturerDto>>.Failure($"შეცდომა მწარმოებლების მიღებისას: {ex.Message}");
        }
    }
}

