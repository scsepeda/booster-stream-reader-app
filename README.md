# Sepeda 2025 Booster Development Test - README

## Overview
This application processes text from a provided stream in real-time and outputs various statistics and analyses on the text. It was developed as part of the Booster Development Test to demonstrate technical proficiency and the approach to solving problems using .NET technologies.

## Features
The application reads and processes text from a stream, performing the following tasks:
- Count the total number of characters and words.
- Identify and output the 5 largest and 5 smallest words.
- Display the 10 most frequently appearing words in the stream.
- List all characters appearing in the stream and their frequency in descending order.

## Requirements
1. **.NET Version**: The application is built with .NET 6.x and C#.
2. **Class Library**: The application depends on the provided `Booster.CodingTest.Library.dll` and `NLipsum.Core.dll` libraries.
3. **Unit Tests**: The solution includes unit tests that verify the functionality of the stream processing logic.
4. **Execution**: The solution is designed to be run from Visual Studio or Visual Studio Code without any changes after unzipping the files.

## How to Run the Application
1. **Clone or Download the Repository**:
   - Unzip the provided files to your local machine.
   
2. **Build the Solution**:
   - Open `Booster.StreamReader.sln` in Visual Studio or Visual Studio Code.
   - Build the solution to restore any dependencies.

3. **Run the Application**:
   - The core functionality is hosted in `Booster.StreamReader`. Start the project from the solution explorer to run the application.

4. **Testing**:
   - Unit tests for the stream processing logic are located in the `Booster.StreamReader.Tests` project.
   - Run the tests to verify the correctness of the implementation.

## Code Structure
We are using vertical slice architecture for the code structure.

- **Booster.StreamReader**: This is the main project that processes the text stream.
  - **Services**: The `BoosterStreamReaderService` is the background service which hosts `StreamProcessingService.cs`, which in turn handles the processing of the text stream.
  - **DTOs/Models**: The `StreamStatistics.cs` model holds the output data such as word counts, character frequencies, etc.
  - **Program.cs**: Entry point of the application.
  
- **Booster.StreamReader.Tests**: Contains unit tests that validate the functionality of the stream processing logic, specifically in `StreamProcessingServiceTests.cs`.

- **Booster.StreamReader.sln**: The solution file that ties the projects together.

## Questions or Concerns
If you have any questions or concerns regarding the implementation or setup, feel free to reach out to the developer.

## Conclusion
This application demonstrates the ability to process text in real-time from a stream, providing various statistics and information as required by the challenge. The code is designed to be clean, modular, and production-ready, adhering to industry standards.
