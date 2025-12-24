using FluentValidation;
using WareHouseManagement.Application.Features.Products.Commands;
using WareHouseManagement.Domain.Interfaces;

namespace WareHouseManagement.Application.Validators;

public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public CreateProductCommandValidator(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("პროდუქტის სახელი აუცილებელია")
            .MaximumLength(200).WithMessage("პროდუქტის სახელი არ უნდა აღემატებოდეს 200 სიმბოლოს");

        RuleFor(x => x.Description)
            .MaximumLength(1000).WithMessage("აღწერა არ უნდა აღემატებოდეს 1000 სიმბოლოს")
            .When(x => !string.IsNullOrEmpty(x.Description));

        RuleFor(x => x.Barcode)
            .MaximumLength(50).WithMessage("ბარკოდი არ უნდა აღემატებოდეს 50 სიმბოლოს")
            .When(x => !string.IsNullOrEmpty(x.Barcode));

        RuleFor(x => x.Price)
            .GreaterThan(0).WithMessage("ფასი უნდა იყოს დადებითი");

        // ალკოჰოლური პროდუქტის ვალიდაცია
        RuleFor(x => x.AlcoholPercentage)
            .NotNull().WithMessage("ალკოჰოლის პროცენტი აუცილებელია ალკოჰოლური პროდუქტისთვის")
            .GreaterThanOrEqualTo(0).WithMessage("ალკოჰოლის პროცენტი უნდა იყოს დადებითი")
            .LessThanOrEqualTo(100).WithMessage("ალკოჰოლის პროცენტი არ უნდა აღემატებოდეს 100-ს")
            .When(x => x.IsAlcoholic);

        RuleFor(x => x.UnitTypeRuleId)
            .MustAsync(async (unitTypeRuleId, cancellation) =>
            {
                var rule = await _unitOfWork.UnitTypeRules.GetByIdAsync(unitTypeRuleId);
                return rule != null && rule.IsActive;
            })
            .WithMessage("არჩეული საზომი ერთეული არ არის ხელმისაწვდომი");
    }
}

