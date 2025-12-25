using MediatR;
using WareHouseManagement.Application.Common.Models;
using WareHouseManagement.Application.DTOs;
using WareHouseManagement.Domain.Entities;
using WareHouseManagement.Domain.Interfaces;

namespace WareHouseManagement.Application.Features.Manufacturers.Commands;

public class CreateManufacturerCommandHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<CreateManufacturerCommand, Result<ManufacturerDto>>
{
    public async Task<Result<ManufacturerDto>> Handle(CreateManufacturerCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // შევამოწმოთ არსებობს თუ არა მწარმოებელი იგივე სახელით
            var existingManufacturers = await unitOfWork.Manufacturers.FindAsync(m => m.Name == request.Name);
            if (existingManufacturers.Any())
            {
                return Result<ManufacturerDto>.Failure($"მწარმოებელი სახელით '{request.Name}' უკვე არსებობს");
            }

            var manufacturer = new Manufacturer
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Country = request.Country,
                ContactInfo = request.ContactInfo,
                Description = request.Description,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await unitOfWork.Manufacturers.AddAsync(manufacturer);
            await unitOfWork.SaveChangesAsync();

            var manufacturerDto = new ManufacturerDto
            {
                Id = manufacturer.Id,
                Name = manufacturer.Name,
                Country = manufacturer.Country,
                ContactInfo = manufacturer.ContactInfo,
                Description = manufacturer.Description,
                CreatedAt = manufacturer.CreatedAt
            };

            return Result<ManufacturerDto>.Success(manufacturerDto, "მწარმოებელი წარმატებით შეიქმნა");
        }
        catch (Exception ex)
        {
            return Result<ManufacturerDto>.Failure($"შეცდომა მწარმოებლის შექმნისას: {ex.Message}");
        }
    }
}

