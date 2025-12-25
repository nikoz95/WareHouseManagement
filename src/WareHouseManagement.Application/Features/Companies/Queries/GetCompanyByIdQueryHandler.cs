using MediatR;
using WareHouseManagement.Application.Common.Models;
using WareHouseManagement.Application.DTOs;
using WareHouseManagement.Application.Mappings;
using WareHouseManagement.Domain.Interfaces;

namespace WareHouseManagement.Application.Features.Companies.Queries;

public class GetCompanyByIdQueryHandler : IRequestHandler<GetCompanyByIdQuery, Result<CompanyDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ApplicationMapper _mapper;

    public GetCompanyByIdQueryHandler(IUnitOfWork unitOfWork, ApplicationMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<CompanyDto>> Handle(GetCompanyByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var company = await _unitOfWork.Companies.GetByIdAsync(request.Id);
            
            if (company == null)
            {
                return Result<CompanyDto>.Failure($"კომპანია ID {request.Id} ვერ მოიძებნა");
            }

            var companyDto = _mapper.MapToCompanyDto(company);
            return Result<CompanyDto>.Success(companyDto);
        }
        catch (Exception ex)
        {
            return Result<CompanyDto>.Failure($"შეცდომა კომპანიის ჩატვირთვისას: {ex.Message}");
        }
    }
}

