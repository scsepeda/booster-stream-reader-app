using Booster.StreamReader.Core.Services;
using Booster.StreamReader.Features.StreamStatistics.Services.Concretes;
using FluentAssertions;
using Moq;
using Xunit;

namespace Booster.StreamReader.Test.Features.StreamStatistics.Services
{
    public class StreamProcessingServiceTest
    {
        private readonly Mock<ILogger<BoosterStreamReaderService>> _loggerMock;
        private readonly StreamProcessingService _service;

        public StreamProcessingServiceTest()
        {
            _loggerMock = new Mock<ILogger<BoosterStreamReaderService>>();
            _service = new StreamProcessingService(_loggerMock.Object);
        }

        [Fact]
        public async Task ProcessStream_ShouldReturnCorrectStatistics()
        {
            // Arrange
            var inputString = "Hello world! Hello universe.";
            var cancellationToken = new CancellationToken();

            // Act
            var result = await _service.ProcessStream(inputString, cancellationToken);

            // Assert
            result.TotalWords.Should().Be(4);
            result.TotalCharacters.Should().Be(23);
            result.LargestWords.Should().Contain("universe");
            result.SmallestWords.Should().Contain("Hello");
            result.MostFrequentWords.Should().ContainKey("Hello");
            result.CharacterFrequency.Should().ContainKey('H');
        }
    }
}
