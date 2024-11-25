using System.Collections.ObjectModel;
using System.Text.Json;

namespace Tools.NumberToWordsConversion.Application.Magnitudes;

public sealed record Magnitude
{
    private const string ManifestName
        = "Tools.NumberToWordsConversion.Application.Magnitudes.magnitudes.json";

    private Magnitude(
        int powerOfTen, 
        string name)
    {
        PowerOfTen = powerOfTen;
        Name = name;
    }

    /// <summary>
    /// The power of ten.
    /// </summary>
    public int PowerOfTen { get; init; }
    
    /// <summary>
    /// The magnitude in words.
    /// </summary>
    public string Name { get; init; }
    
    #region All options

    private static readonly Lazy<IReadOnlyList<Magnitude>> AllMagnitudes = new(ReflectAllMagnitudes);

    /// <summary>
    /// Gets all magnitudes.
    /// </summary>
    public static IReadOnlyList<Magnitude> All => AllMagnitudes.Value;

    private static ReadOnlyCollection<Magnitude> ReflectAllMagnitudes()
    {
        // Get the assembly of the magnitude json reside on.
        var assembly = typeof(Magnitude).Assembly;

        // Get the manifest resource stream of the currency file.
        using var stream = assembly.GetManifestResourceStream(ManifestName)!;

        var currencies = JsonSerializer.Deserialize<IList<MagnitudeValue>>(stream)!;

        // Deserialize the magnitude file.
        return currencies.Select(x => new Magnitude(x.PowerOfTen, x.Name)).ToList().AsReadOnly();
    }

    #endregion  All options
    
    private sealed record MagnitudeValue(
        int PowerOfTen,
        string Name);
}