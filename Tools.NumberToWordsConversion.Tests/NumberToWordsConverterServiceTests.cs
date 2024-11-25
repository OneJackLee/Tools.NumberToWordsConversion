using FluentAssertions;
using Tools.NumberToWordsConversion.Application.NumberToWordsConverter;

namespace Tools.NumberToWordsConversion.Tests;

public class NumberToWordsConverterServiceTests
{
    private readonly INumberToWordsConverterService _service = new NumberToWordsConverterService();
    
    [Fact]
    public void GetAmountToWords_ShouldReturnZero_WhenAmountIsZero()
    {
        // Arrange
        var amount = 0m;
        var currencyCode = "USD"; // Using USD for this example

        // Act
        var result = _service.GetAmountToWords(amount, currencyCode);

        // Assert
        result.Should().Be("ZERO DOLLAR");
    }
    
    [Fact]
    public void GetAmountToWords_ShouldReturnAmountInWords_WhenAmountIsWholeAndPositive()
    {
        // Arrange
        var amount = 1234m;
        var currencyCode = "USD";

        // Act
        var result = _service.GetAmountToWords(amount, currencyCode);

        // Assert
        result.Should().Be("ONE THOUSAND TWO HUNDRED AND THIRTY-FOUR DOLLARS");
    }
    
    [Fact]
    public void GetAmountToWords_ShouldReturnAmountInWords_WhenAmountHasDecimalPart()
    {
        // Arrange
        var amount = 12.34m;
        var currencyCode = "USD";

        // Act
        var result = _service.GetAmountToWords(amount, currencyCode);

        // Assert
        result.Should().Be("TWELVE DOLLARS AND THIRTY-FOUR CENTS");
    }

    [Fact]
    public void GetAmountToWords_ShouldReturnNegativeAmount_WhenAmountIsNegative()
    {
        // Arrange
        var amount = -45.67m;
        var currencyCode = "USD";

        // Act
        var result = _service.GetAmountToWords(amount, currencyCode);

        // Assert
        result.Should().Be("NEGATIVE FORTY-FIVE DOLLARS AND SIXTY-SEVEN CENTS");
    }

    [Fact]
    public void GetAmountToWords_ShouldThrowException_WhenCurrencyIsNotSupported()
    {
        // Arrange
        var amount = 10m;
        var currencyCode = "XYZ"; // Unsupported currency code

        // Act
        Action act = () => _service.GetAmountToWords(amount, currencyCode);

        // Assert
        act.Should().Throw<InvalidOperationException>()
            .WithMessage("The currency code XYZ is not supported.");
    }

    [Fact]
    public void GetAmountToWords_ShouldRoundDecimals_WhenCurrencyHasNoSubunitAndDecimalWithinOneToFour()
    {
        // Arrange
        var amount = 123.456m;
        var currencyCode = "JPY"; // Example with no subunit (cent)

        // Act
        var result = _service.GetAmountToWords(amount, currencyCode);

        // Assert
        result.Should().Be("ONE HUNDRED AND TWENTY-THREE YEN"); // 0.456 round up will become 0.
    }

    [Fact]
    public void GetAmountToWords_ShouldRoundDecimals_WhenCurrencyHasNoSubunitAndDecimalNotWithinOneToFour()
    {
        // Arrange
        var amount = 123.56m;
        var currencyCode = "JPY"; // Example with no subunit (cent)

        // Act
        var result = _service.GetAmountToWords(amount, currencyCode);

        // Assert
        result.Should().Be("ONE HUNDRED AND TWENTY-FOUR YEN"); // 0.56 round up will become 1.
    }
    
    [Fact]
    public void GetAmountToWords_ShouldUseSingularSubunit_WhenAmountIsOne()
    {
        // Arrange
        var amount = 1.1m;
        var currencyCode = "USD"; // Singular subunit case (1 cent)

        // Act
        var result = _service.GetAmountToWords(amount, currencyCode);

        // Assert
        result.Should().Be("ONE DOLLAR AND ONE CENT");
    }

    [Fact]
    public void GetAmountToWords_ShouldAlwaysSetToSingularUnit_WhenCurrencyNotHavingPluralUnit()
    {
        // Arrange
        var amount = 12.34m;
        var currencyCode = "MYR"; // Singular subunit case (1 cent)

        // Act
        var result = _service.GetAmountToWords(amount, currencyCode);

        // Assert
        result.Should().Be("TWELVE RINGGIT AND THIRTY-FOUR SEN");
    }

    [Fact]
    public void GetAmountToWords_ShouldReturnAmountInWords_WhenAmountIsMax()
    {
        // Arrange
        var amount = decimal.MaxValue;
        var currencyCode = "AUD";

        // Act
        var result = _service.GetAmountToWords(amount, currencyCode);

        // Assert
        result.Should().Be("SEVENTY-NINE OCTILLION TWO HUNDRED AND TWENTY-EIGHT SEPTILLION ONE HUNDRED AND SIXTY-TWO SEXTILLION FIVE HUNDRED AND FOURTEEN QUINTILLION TWO HUNDRED AND SIXTY-FOUR QUADRILLION THREE HUNDRED AND THIRTY-SEVEN TRILLION FIVE HUNDRED AND NINETY-THREE BILLION FIVE HUNDRED AND FORTY-THREE MILLION NINE HUNDRED AND FIFTY THOUSAND THREE HUNDRED AND THIRTY-FIVE DOLLARS");
    }
}
