# SalesAnalysis
System that interprets files with sales information in text format (.txt) and performs the processing by exporting output files.

## Techs:
 - Net core 3.1 

## Approach:
- FileSystemWatcher: It is a native component of .NET. It was used to monitor when files with a .txt extension were created in the root directory of the entry. When it detects new files it sends a message to a channel.
- Channel: It is a component present in net core 3.0+. It provides a unique channel for exchanging messages and exposes reading and writing methods. Thinking about performance and scalability, it was chosen.
- The settings for the input and output directories are in the file appsettings.json

## How to build?
- ``` dotnet build ```

## How to test?
- ``` dotnet test SalesAnalysis.UnitTests/SalesAnalysis.UnitTests.csproj ```

## How to run?
- ``` dotnet run --project SalesAnalysis.Workers/SalesAnalysis.Workers.csproj ```

