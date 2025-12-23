using AutoMapper;
using MediatR;
using WareHouseManagement.Application.Common.Models;
using WareHouseManagement.Application.DTOs;
using WareHouseManagement.Domain.Entities;
using WareHouseManagement.Domain.Interfaces;

namespace WareHouseManagement.Application.Features.Companies.Commands;

public class CreateCompanyCommandHandler : IRequestHandler<CreateCompanyCommand, Result<CompanyDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateCompanyCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<CompanyDto>> Handle(CreateCompanyCommand request, CancellationToken cancellationToken)
    {
        try
        {
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

            await _unitOfWork.Companies.AddAsync(company);
            await _unitOfWork.SaveChangesAsync();

            var companyDto = _mapper.Map<CompanyDto>(company);
            return Result<CompanyDto>.Success(companyDto, "კომპანია წარმატებით შეიქმნა");
        }
        catch (Exception ex)
        {
            return Result<CompanyDto>.Failure($"შეცდომა კომპანიის შექმნისას: {ex.Message}");
        }
    }
}

