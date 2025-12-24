﻿using MediatR;
using WareHouseManagement.Application.Common.Models;
using WareHouseManagement.Application.DTOs;
using WareHouseManagement.Application.Mappings;
using WareHouseManagement.Domain.Interfaces;

namespace WareHouseManagement.Application.Features.Companies.Queries;

public class GetAllCompaniesQueryHandler : IRequestHandler<GetAllCompaniesQuery, Result<IEnumerable<CompanyDto>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ApplicationMapper _mapper;

    public GetAllCompaniesQueryHandler(IUnitOfWork unitOfWork, ApplicationMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<IEnumerable<CompanyDto>>> Handle(GetAllCompaniesQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var companies = await _unitOfWork.Companies.GetAllAsync();
            var companyDtos = companies.Select(_mapper.MapToCompanyDto);
            return Result<IEnumerable<CompanyDto>>.Success(companyDtos);
        }
        catch (Exception ex)
        {
            return Result<IEnumerable<CompanyDto>>.Failure($"შეცდომა კომპანიების ჩატვირთვისას: {ex.Message}");
        }
    }
}

