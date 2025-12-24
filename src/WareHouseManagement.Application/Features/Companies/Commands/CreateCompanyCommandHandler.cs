﻿using MediatR;
using WareHouseManagement.Application.Common.Models;
using WareHouseManagement.Application.DTOs;
using WareHouseManagement.Application.Mappings;
using WareHouseManagement.Domain.Entities;
using WareHouseManagement.Domain.Interfaces;

namespace WareHouseManagement.Application.Features.Companies.Commands;

public class CreateCompanyCommandHandler(IUnitOfWork unitOfWork, ApplicationMapper mapper)
    : IRequestHandler<CreateCompanyCommand, Result<CompanyDto>>
{
    public async Task<Result<CompanyDto>> Handle(CreateCompanyCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // შევამოწმოთ არსებობს თუ არა კომპანია იგივე სახელით
            var existingCompanyByName = await unitOfWork.Companies.FindAsync(c => c.Name == request.Name);
            if (existingCompanyByName.Any())
            {
                return Result<CompanyDto>.Failure($"კომპანია სახელით '{request.Name}' უკვე არსებობს");
            }

            // შევამოწმოთ არსებობს თუ არა კომპანია იგივე საიდენტიფიკაციო კოდით
            if (!string.IsNullOrEmpty(request.TaxId))
            {
                var existingCompanyByTaxId = await unitOfWork.Companies.FindAsync(c => c.TaxId == request.TaxId);
                if (existingCompanyByTaxId.Any())
                {
                    return Result<CompanyDto>.Failure($"კომპანია საიდენტიფიკაციო კოდით '{request.TaxId}' უკვე არსებობს");
                }
            }

            var company = new Company
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                TaxId = request.TaxId,
                Address = request.Address,
                Phone = request.Phone,
                Email = request.Email,
                CompanyType = request.CompanyType,
                IsPartner = request.IsPartner,
                CreatedAt = DateTime.UtcNow
            };

            await unitOfWork.Companies.AddAsync(company);
            await unitOfWork.SaveChangesAsync();

            var companyDto = mapper.MapToCompanyDto(company);
            return Result<CompanyDto>.Success(companyDto, "კომპანია წარმატებით შეიქმნა");
        }
        catch (Exception ex)
        {
            return Result<CompanyDto>.Failure($"შეცდომა კომპანიის შექმნისას: {ex.Message}");
        }
    }
}

