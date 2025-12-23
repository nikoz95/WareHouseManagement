using AutoMapper;
using MediatR;
using WareHouseManagement.Application.Common.Models;
using WareHouseManagement.Application.DTOs;
using WareHouseManagement.Domain.Interfaces;

namespace WareHouseManagement.Application.Features.Companies.Queries;

public class GetAllCompaniesQueryHandler : IRequestHandler<GetAllCompaniesQuery, Result<IEnumerable<CompanyDto>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetAllCompaniesQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<IEnumerable<CompanyDto>>> Handle(GetAllCompaniesQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var companies = await _unitOfWork.Companies.GetAllAsync();
            var companyDtos = _mapper.Map<IEnumerable<CompanyDto>>(companies);
            return Result<IEnumerable<CompanyDto>>.Success(companyDtos);
        }
        catch (Exception ex)
        {
            return Result<IEnumerable<CompanyDto>>.Failure($"შეცდომა კომპანიების ჩატვირთვისას: {ex.Message}");
        }
    }
}

