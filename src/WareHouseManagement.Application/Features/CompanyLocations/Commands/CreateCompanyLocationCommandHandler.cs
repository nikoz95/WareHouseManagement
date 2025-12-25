using MediatR;
using WareHouseManagement.Application.Common.Models;
using WareHouseManagement.Application.DTOs;
using WareHouseManagement.Domain.Entities;
using WareHouseManagement.Domain.Interfaces;

namespace WareHouseManagement.Application.Features.CompanyLocations.Commands;

public class CreateCompanyLocationCommandHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<CreateCompanyLocationCommand, Result<CompanyLocationDto>>
{
    public async Task<Result<CompanyLocationDto>> Handle(CreateCompanyLocationCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // შევამოწმოთ არსებობს თუ არა კომპანია
            var company = await unitOfWork.Companies.GetByIdAsync(request.CompanyId);
            if (company == null)
            {
                return Result<CompanyLocationDto>.Failure("კომპანია ვერ მოიძებნა");
            }

            // შევამოწმოთ არსებობს თუ არა ლოკაცია იმავე სახელით ამ კომპანიაში
            var existingCompanies = await unitOfWork.Companies.FindAsync(c => c.Id == request.CompanyId);
            var existingLocation = existingCompanies
                .SelectMany(c => c.CompanyLocations)
                .FirstOrDefault(cl => cl.LocationName == request.LocationName && !cl.IsDeleted);

            if (existingLocation != null)
            {
                return Result<CompanyLocationDto>.Failure($"ლოკაცია სახელით '{request.LocationName}' უკვე არსებობს ამ კომპანიაში");
            }

            var location = new CompanyLocation
            {
                Id = Guid.NewGuid(),
                CompanyId = request.CompanyId,
                LocationName = request.LocationName,
                Address = request.Address,
                City = request.City,
                Phone = request.Phone,
                ContactPerson = request.ContactPerson,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            company.CompanyLocations.Add(location);
            await unitOfWork.SaveChangesAsync();

            var locationDto = new CompanyLocationDto
            {
                Id = location.Id,
                CompanyId = location.CompanyId,
                CompanyName = company.Name,
                LocationName = location.LocationName,
                Address = location.Address,
                City = location.City,
                Phone = location.Phone,
                ContactPerson = location.ContactPerson,
                CreatedAt = location.CreatedAt
            };

            return Result<CompanyLocationDto>.Success(locationDto, "კომპანიის ლოკაცია წარმატებით შეიქმნა");
        }
        catch (Exception ex)
        {
            return Result<CompanyLocationDto>.Failure($"შეცდომა ლოკაციის შექმნისას: {ex.Message}");
        }
    }
}

