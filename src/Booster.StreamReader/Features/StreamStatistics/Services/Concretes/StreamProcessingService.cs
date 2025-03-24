using System.Text;
using Booster.StreamReader.API.Core.Services;
using Booster.StreamReader.API.Features.StreamStatistics.DTO;

namespace Booster.StreamReader.API.Features.StreamStatistics.Services.Concretes
{
    public class StreamProcessingService(ILogger<BoosterStreamReaderService> logger) : IStreamProcessingService
    {
        public async Task<StreamStatisticsResponseDto> ProcessStream(string inputString, CancellationToken cancellationToken)
        {

            var statistics = new StreamStatisticsResponseDto();
            try
            {
                //var memoryStream = new MemoryStream(new WordStream());

                using var stream = new System.IO.StreamReader(GenerateStreamFromString(inputString));
                string? line;
                while ((line = stream.ReadLine()) != null)
                {
                    ProcessLine(line, statistics);
                }

                // Sort and get distinct values
                statistics.LargestWords = [.. statistics.LargestWords.Distinct().OrderByDescending(w => w.Length).Take(5)];
                statistics.SmallestWords = [.. statistics.SmallestWords.Distinct().OrderBy(w => w.Length).Take(5)];
                statistics.WordFrequency = statistics.WordFrequency.OrderByDescending(w => w.Value).Take(10).ToDictionary(w => w.Key, w => w.Value);
                statistics.CharacterFrequency = statistics.CharacterFrequency.OrderByDescending(c => c.Value).ToDictionary(c => c.Key, c => c.Value);

                return statistics;
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return new StreamStatisticsResponseDto();
            }
        }
        private static MemoryStream GenerateStreamFromString(string s)
        {
            return new MemoryStream(Encoding.UTF8.GetBytes(s));
        }

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
            if (statistics.WordFrequency.TryGetValue(word, out int value))
            {
                statistics.WordFrequency[word] = ++value;
            }
            else
            {
                statistics.WordFrequency[word] = 1;
            }
        }
    }
}
