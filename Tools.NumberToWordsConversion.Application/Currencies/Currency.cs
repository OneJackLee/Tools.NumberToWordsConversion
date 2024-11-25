using System.Collections.ObjectModel;
using System.Text.Json;

namespace Tools.NumberToWordsConversion.Application.Currencies;

public sealed record Currency
{
    private const string ManifestName
        = "Tools.NumberToWordsConversion.Application.Currencies.currencies.json";

    internal static readonly Currency None 
        = new(string.Empty, string.Empty, new CurrencyUnit(string.Empty, string.Empty), null);

    private Currency(
        string code,
        string name, 
        CurrencyUnit unit, 
        CurrencyUnit? subunit)
    {
        Code = code;
        Name = name;
        Unit = unit;
        Subunit = subunit;
    }

    /// <summary>
    /// The currency code.
    /// </summary>
    public string Code { get; init; } 
    
    /// <summary>
    /// The currency display name.
    /// </summary>
    public string Name { get; init; }
    
    /// <summary>
    /// The main unit of the currency.
    /// </summary>
    public CurrencyUnit Unit { get; init; }
    
    /// <summary>
    /// The Subunit of the currency.
    /// This can be null for some currencies such as Japanese Yen.
    /// </summary>
    public CurrencyUnit? Subunit { get; init; }
    
    public bool HasSubunit => Subunit is not null;
    
    #region All options

    private static readonly Lazy<IReadOnlyList<Currency>> AllCurrencies = new(ReflectAllCurrencies);

    /// <summary>
    /// Gets all currencies.
    /// </summary>
    public static IReadOnlyList<Currency> All => AllCurrencies.Value;

    private static ReadOnlyCollection<Currency> ReflectAllCurrencies()
    {
        // Get the assembly of the current type.
        var assembly = typeof(Currency).Assembly;

        // Get the manifest resource stream of the currency file.
        using var stream = assembly.GetManifestResourceStream(ManifestName)!;

        var currencies = JsonSerializer.Deserialize<IList<CurrencyValue>>(stream)!;

        // Deserialize the currency file.
        return currencies.Select(x => new Currency(x.Code, x.Name, x.Unit, x.Subunit)).ToList().AsReadOnly();
    }

    #endregion  All options

    private sealed record CurrencyValue(
        string Code,
        string Name,
        CurrencyUnit Unit,
        CurrencyUnit? Subunit);

    /// <summary>
    /// Converts the currency to a string.
    /// </summary>
    /// <returns>The code of the currency.</returns>
    public override string ToString() => Code;

    /// <summary>
    /// Gets the currency from its code.
    /// </summary>
    /// <param name="code">The code of the currency.</param>
    /// <returns>The currency with the specified code.</returns>
    public static Currency? FromCode(string code) => All.SingleOrDefault(currency => currency.Code == code);
}

public sealed record CurrencyUnit(string Singular, string Plural);