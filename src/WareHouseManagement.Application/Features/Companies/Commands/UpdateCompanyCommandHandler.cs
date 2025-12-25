using MediatR;
using WareHouseManagement.Application.Common.Models;
using WareHouseManagement.Application.DTOs;
using WareHouseManagement.Application.Mappings;
using WareHouseManagement.Domain.Interfaces;

namespace WareHouseManagement.Application.Features.Companies.Commands;

public class UpdateCompanyCommandHandler : IRequestHandler<UpdateCompanyCommand, Result<CompanyDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ApplicationMapper _mapper;

    public UpdateCompanyCommandHandler(IUnitOfWork unitOfWork, ApplicationMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<CompanyDto>> Handle(UpdateCompanyCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var company = await _unitOfWork.Companies.GetByIdAsync(request.Id);
            
            if (company == null)
            {
                return Result<CompanyDto>.Failure($"კომპანია ID {request.Id} ვერ მოიძებნა");
            }

            // შევამოწმოთ სახელის უნიკალურობა (გარდა საკუთარი თავისა)
            var existingCompanyByName = await _unitOfWork.Companies.FindAsync(c => c.Name == request.Name && c.Id != request.Id);
            if (existingCompanyByName.Any())
            {
                return Result<CompanyDto>.Failure($"კომპანია სახელით '{request.Name}' უკვე არსებობს");
            }

            // შევამოწმოთ TaxId-ის უნიკალურობა (გარდა საკუთარი თავისა)
            if (!string.IsNullOrEmpty(request.TaxId))
            {
                var existingCompanyByTaxId = await _unitOfWork.Companies.FindAsync(c => c.TaxId == request.TaxId && c.Id != request.Id);
                if (existingCompanyByTaxId.Any())
                {
                    return Result<CompanyDto>.Failure($"კომპანია საიდენტიფიკაციო კოდით '{request.TaxId}' უკვე არსებობს");
                }
            }

            // განახლება
            company.Name = request.Name;
            company.TaxId = request.TaxId;
            company.Address = request.Address;
            company.Phone = request.Phone;
            company.Email = request.Email;
            company.CompanyType = request.CompanyType;
            company.IsPartner = request.IsPartner;
            company.UpdatedAt = DateTime.UtcNow;

            _unitOfWork.Companies.Update(company);
            await _unitOfWork.SaveChangesAsync();

            var companyDto = _mapper.MapToCompanyDto(company);
            return Result<CompanyDto>.Success(companyDto, "კომპანია წარმატებით განახლდა");
        }
        catch (Exception ex)
        {
            return Result<CompanyDto>.Failure($"შეცდომა კომპანიის განახლებისას: {ex.Message}");
        }
    }
}

