namespace Booster.StreamReader.API.Features.StreamStatistics.DTO;

/// <summary>
/// Represents the response DTO for stream statistics.
/// </summary>
public record StreamStatisticsResponseDto
{
    /// <summary>
    /// Gets or sets the total number of characters in the stream.
    /// </summary>
    public int TotalCharacters { get; set; }

    /// <summary>
    /// Gets or sets the total number of words in the stream.
    /// </summary>
    public int TotalWords { get; set; }

    /// <summary>
    /// Gets or sets the list of the 5 largest words in the stream.
    /// </summary>
    public List<string> LargestWords { get; set; } = [];

    /// <summary>
    /// Gets or sets the list of the 5 smallest words in the stream.
    /// </summary>
    public List<string> SmallestWords { get; set; } = [];

    /// <summary>
    /// Gets or sets the dictionary of the 10 most frequently appearing words in the stream.
    /// </summary>
    public Dictionary<string, int> WordFrequency { get; set; } = [];

    /// <summary>
    /// Gets or sets the dictionary of all characters appearing in the stream and their frequencies.
    /// </summary>
    public Dictionary<char, int> CharacterFrequency { get; set; } = [];
}
