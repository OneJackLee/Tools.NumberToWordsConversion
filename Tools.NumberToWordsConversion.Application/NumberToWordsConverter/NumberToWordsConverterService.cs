using System.Text;
using Tools.NumberToWordsConversion.Application.Currencies;
using Tools.NumberToWordsConversion.Application.Kernel;
using Tools.NumberToWordsConversion.Application.Magnitudes;

namespace Tools.NumberToWordsConversion.Application.NumberToWordsConverter;


public class NumberToWordsConverterService : INumberToWordsConverterService, ISingletonService
{
    private static readonly string[] Units =
    [
        "Zero", "One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine", "Ten", "Eleven", "Twelve", "Thirteen", 
        "Fourteen", "Fifteen", "Sixteen", "Seventeen", "Eighteen", "Nineteen"
    ];
    
    private static readonly string[] Tens = 
    [
        "Zero", "Ten", "Twenty", "Thirty", "Forty", "Fifty", "Sixty", "Seventy", "Eighty", "Ninety"
    ];
        
    public string GetAmountToWords(decimal amount, string currencyCode)
    {
        var currency = Currency.FromCode(currencyCode);
    
        if (currency is null)
            throw new InvalidOperationException($"The currency code {currencyCode} is not supported.");

        var isNegative = amount < 0;
        
        amount = Math.Abs(amount);
        amount = Math.Round(amount, !currency.HasSubunit ? 0 : 2);

        var wholeAmount = Math.Truncate(amount);
        var decimalAmount = GetDecimalDigits(amount - wholeAmount);
        
        var result = new StringBuilder();
        
        if (isNegative)
            result.Append("Negative ");
        
        result.Append($"{GetUnitAmountToWords(wholeAmount, currency.Unit)} ");

        if (decimalAmount == 0)
        {
            return result.ToString().Trim().ToUpperInvariant();
        }
        
        result.Append("and ");
        result.Append(GetSubunitAmountToWords(decimalAmount, currency.Subunit));

        return result.ToString().Trim().ToUpperInvariant();
    }

    private static int GetDecimalDigits(decimal amount)
    {
        var power = (int)(Math.Pow(10, amount.Scale));
        return (int)(amount * power);
    }

    private static string GetUnitAmountToWords(decimal amount, CurrencyUnit currencyUnit)
    {
        var result = new StringBuilder();

        var currency = amount > 1 ? currencyUnit.Plural : currencyUnit.Singular;
        
        foreach (var magnitude in Magnitude.All.OrderByDescending(x => x.PowerOfTen))
        {
            var magnitudeUnit = ((int)Math.Pow(10, magnitude.PowerOfTen));
            var magnitudeAmount = (int) Math.Truncate(amount / magnitudeUnit);

            if (magnitudeAmount > 0)
            {
                result.Append($"{Convert3DigitToWords(magnitudeAmount)} ");
                
                if (!string.IsNullOrWhiteSpace(magnitude.Name))
                    result.Append($"{magnitudeUnit} ");
            }

            amount %= magnitudeUnit;
        }

        result.Append($"{currency} ");

        return result.ToString().Trim();
    }

    private static string GetSubunitAmountToWords(int amount, CurrencyUnit? currencyUnit)
    {
        var result = new StringBuilder();

        result.Append($"{Convert3DigitToWords(amount)} ");
        
        if (amount > 1)
            result.Append($"{currencyUnit?.Plural} ");
        else
            result.Append($"{currencyUnit?.Singular} ");
        
        return result.ToString().Trim();
    }

    private static string Convert3DigitToWords(int number)
    {
        var result = new StringBuilder();
        
        switch (number)
        {
            case > 999 or < 0:
                throw new ArgumentOutOfRangeException(nameof(number));
            case >= 100:
                {
                    result.Append($"{Units[number / 100]} Hundred");
                    number %= 100;
            
                    if (number > 0)
                        result.Append(" and "); // Add and when there are tens/units
                    break;
                }
        }

        switch (number)
        {
            case >= 20:
                {
                    result.Append($"{Tens[number / 10]}");
                    number %= 10;
            
                    if (number > 0)
                        result.Append($"-{Units[number]}");
                    break;
                }
            case > 0:
                result.Append($"{Units[number]}");
                break;
        }
        
        return result.ToString().Trim();
    }
}

