namespace Tools.NumberToWordsConversion.Application.NumberToWordsConverter;

public interface INumberToWordsConverterService
{
    string GetAmountToWords(decimal amount, string currencyCode);
}
