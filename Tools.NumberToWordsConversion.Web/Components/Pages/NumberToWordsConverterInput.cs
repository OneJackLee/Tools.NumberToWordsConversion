using FluentValidation;
using Tools.NumberToWordsConversion.Application.Currencies;
using Tools.NumberToWordsConversion.Extensions;

namespace Tools.NumberToWordsConversion.Components.Pages;

public class NumberToWordsConverterInput
{
    public string? CurrencyCode { get; set; }
    
    public decimal? Amount { get; set; }
}

/// <summary>
/// The validator for number to words input.
/// </summary>
public sealed class NumberToWordsConverterInputValidator : AbstractValidator<NumberToWordsConverterInput>
{
    public NumberToWordsConverterInputValidator()
    {
        RuleFor(input => input.CurrencyCode).NotNull()
            .Must(MustBeValidCurrencyCode)
            .WithMessage($"The currency code must be one of the following values: "
                + $"{string.Join(", ", Currency.All.Select(x => x.Code))}");

        RuleFor(input => input.Amount).NotNull();
    }
    
    private static bool MustBeValidCurrencyCode(string? currencyCode) =>
        Currency.All.Any(x => x.Code == currencyCode);

}

/// <summary>
/// Reserved for fluent validation.
/// </summary>
public sealed class NumberToWordsConverterInputValidation :
    FluentValidationBase<NumberToWordsConverterInput, NumberToWordsConverterInputValidator>
{
    // EMPTY
}