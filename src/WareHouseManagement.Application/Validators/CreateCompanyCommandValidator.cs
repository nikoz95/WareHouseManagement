using FluentValidation;
using WareHouseManagement.Application.Features.Companies.Commands;

namespace WareHouseManagement.Application.Validators;

public class CreateCompanyCommandValidator : AbstractValidator<CreateCompanyCommand>
{
    public CreateCompanyCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("კომპანიის სახელი აუცილებელია")
            .MaximumLength(200).WithMessage("კომპანიის სახელი არ უნდა აღემატებოდეს 200 სიმბოლოს");

        RuleFor(x => x.TaxId)
            .NotEmpty().WithMessage("საიდენტიფიკაციო ნომერი აუცილებელია")
            .MaximumLength(50).WithMessage("საიდენტიფიკაციო ნომერი არ უნდა აღემატებოდეს 50 სიმბოლოს");

        RuleFor(x => x.Phone)
            .MaximumLength(50).WithMessage("ტელეფონის ნომერი არ უნდა აღემატებოდეს 50 სიმბოლოს")
            .When(x => !string.IsNullOrEmpty(x.Phone));

        RuleFor(x => x.Email)
            .EmailAddress().WithMessage("არასწორი ელ-ფოსტის ფორმატი")
            .When(x => !string.IsNullOrEmpty(x.Email));

        RuleFor(x => x.CompanyType)
            .IsInEnum().WithMessage("არასწორი კომპანიის ტიპი");
    }
}

