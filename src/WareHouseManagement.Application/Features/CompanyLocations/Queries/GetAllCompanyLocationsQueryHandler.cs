using MediatR;
using WareHouseManagement.Application.Common.Models;
using WareHouseManagement.Application.DTOs;
using WareHouseManagement.Domain.Interfaces;

namespace WareHouseManagement.Application.Features.CompanyLocations.Queries;

public class GetAllCompanyLocationsQueryHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<GetAllCompanyLocationsQuery, Result<List<CompanyLocationDto>>>
{
    public async Task<Result<List<CompanyLocationDto>>> Handle(GetAllCompanyLocationsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            List<CompanyLocationDto> locationDtos;

            if (request.CompanyId.HasValue)
            {
                // მხოლოდ კონკრეტული კომპანიის ლოკაციები
                var company = await unitOfWork.Companies.GetCompanyWithLocationsAsync(request.CompanyId.Value);
                if (company == null)
                {
                    return Result<List<CompanyLocationDto>>.Failure("კომპანია ვერ მოიძებნა");
                }

                locationDtos = company.CompanyLocations
                    .Select(cl => new CompanyLocationDto
                    {
                        Id = cl.Id,
                        CompanyId = cl.CompanyId,
                        CompanyName = company.Name,
                        LocationName = cl.LocationName,
                        Address = cl.Address,
                        City = cl.City,
                        Phone = cl.Phone,
                        ContactPerson = cl.ContactPerson,
                        CreatedAt = cl.CreatedAt
                    })
                    .ToList();
            }
            else
            {
                // ყველა ლოკაცია ყველა კომპანიისგან
                var companies = await unitOfWork.Companies.GetAllCompaniesWithLocationsAsync();
                
                locationDtos = companies
                    .SelectMany(c => c.CompanyLocations
                        .Select(cl => new CompanyLocationDto
                        {
                            Id = cl.Id,
                            CompanyId = cl.CompanyId,
                            CompanyName = c.Name,
                            LocationName = cl.LocationName,
                            Address = cl.Address,
                            City = cl.City,
                            Phone = cl.Phone,
                            ContactPerson = cl.ContactPerson,
                            CreatedAt = cl.CreatedAt
                        }))
                    .ToList();
            }

            return Result<List<CompanyLocationDto>>.Success(locationDtos);
        }
        catch (Exception ex)
        {
            return Result<List<CompanyLocationDto>>.Failure($"შეცდომა ლოკაციების მიღებისას: {ex.Message}");
        }
    }
}

