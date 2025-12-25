using MediatR;
using WareHouseManagement.Application.Common.Models;

namespace WareHouseManagement.Application.Features.Companies.Commands;

public class DeleteCompanyCommand : IRequest<Result<bool>>
{
    public Guid Id { get; set; }
}
