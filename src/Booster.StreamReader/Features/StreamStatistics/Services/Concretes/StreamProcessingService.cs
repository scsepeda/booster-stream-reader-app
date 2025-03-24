using System.Text;
using Booster.StreamReader.Core.Services;
using Booster.StreamReader.Features.StreamStatistics.DTO;

namespace Booster.StreamReader.Features.StreamStatistics.Services.Concretes
{
    /// <summary>
    /// Service for processing streams to gather statistics.
    /// </summary>
    public class StreamProcessingService : IStreamProcessingService
    {
        private readonly ILogger<BoosterStreamReaderService> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="StreamProcessingService"/> class.
        /// </summary>
        /// <param name="logger">The logger instance.</param>
        public StreamProcessingService(ILogger<BoosterStreamReaderService> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Processes the input stream and gathers statistics.
        /// </summary>
        /// <param name="inputString">The input string to process.</param>
        /// <param name="cancellationToken">Cancellation token to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the stream statistics.</returns>
        public async Task<StreamStatisticsResponseDto> ProcessStream(string inputString, CancellationToken cancellationToken)
        {
            var statistics = new StreamStatisticsResponseDto();
            try
            {
                using var stream = new System.IO.StreamReader(GenerateStreamFromString(inputString));
                string? line;
                while ((line = stream.ReadLine()) != null)
                {
                    ProcessLine(line, statistics);
                }

                // Sort and get distinct values
                statistics.LargestWords = statistics.LargestWords.Distinct().OrderByDescending(w => w.Length).Take(5).ToList();
                statistics.SmallestWords = statistics.SmallestWords.Distinct().OrderBy(w => w.Length).Take(5).ToList();
                statistics.MostFrequentWords = statistics.MostFrequentWords.OrderByDescending(w => w.Value).Take(10).ToDictionary(w => w.Key, w => w.Value);
                statistics.CharacterFrequency = statistics.CharacterFrequency.OrderByDescending(c => c.Value).ToDictionary(c => c.Key, c => c.Value);

                return statistics;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return new StreamStatisticsResponseDto();
            }
        }

        /// <summary>
        /// Generates a memory stream from the given string.
        /// </summary>
        /// <param name="input">The input string.</param>
        /// <returns>A memory stream containing the input string.</returns>
        private static MemoryStream GenerateStreamFromString(string input)
        {
            return new MemoryStream(Encoding.UTF8.GetBytes(input));
        }

        /// <summary>
        /// Processes a line of text and updates the statistics.
        /// </summary>
        /// <param name="line">The line of text to process.</param>
        /// <param name="statistics">The statistics to update.</param>
        private static void ProcessLine(string line, StreamStatisticsResponseDto statistics)
        {
            var words = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            foreach (var word in words)
            {
                var cleanedWord = new string(word.Where(char.IsLetterOrDigit).ToArray());
                if (string.IsNullOrEmpty(cleanedWord)) continue;

                statistics.TotalWords++;
                statistics.TotalCharacters += cleanedWord.Length;

                // Update word lists
                UpdateWordLists(cleanedWord, statistics);

                // Update character frequency
                foreach (var character in word)
                {
                    if (statistics.CharacterFrequency.TryGetValue(character, out int value))
                    {
                        statistics.CharacterFrequency[character] = ++value;
                    }
                    else
                    {
                        statistics.CharacterFrequency[character] = 1;
                    }
                }
            }
        }

        /// <summary>
        /// Updates the word lists and word frequency in the statistics.
        /// </summary>
        /// <param name="word">The word to update.</param>
        /// <param name="statistics">The statistics to update.</param>
        private static void UpdateWordLists(string word, StreamStatisticsResponseDto statistics)
        {
            // Update largest words
            if (!statistics.LargestWords.Contains(word))
            {
                if (statistics.LargestWords.Count < 5)
                {
                    statistics.LargestWords.Add(word);
                    statistics.LargestWords.Sort((a, b) => b.Length.CompareTo(a.Length));
                }
                else if (word.Length > statistics.LargestWords[4].Length)
                {
                    statistics.LargestWords[4] = word;
                    statistics.LargestWords.Sort((a, b) => b.Length.CompareTo(a.Length));
                }
            }

            // Update smallest words
            if (!statistics.SmallestWords.Contains(word))
            {
                if (statistics.SmallestWords.Count < 5)
                {
                    statistics.SmallestWords.Add(word);
                    statistics.SmallestWords.Sort((a, b) => a.Length.CompareTo(b.Length));
                }
                else if (word.Length < statistics.SmallestWords[4].Length)
                {
                    statistics.SmallestWords[4] = word;
                    statistics.SmallestWords.Sort((a, b) => a.Length.CompareTo(b.Length));
                }
            }

            // Update word frequency
            if (statistics.MostFrequentWords.TryGetValue(word, out int value))
            {
                statistics.MostFrequentWords[word] = ++value;
            }
            else
            {
                statistics.MostFrequentWords[word] = 1;
            }
        }
    }
}
