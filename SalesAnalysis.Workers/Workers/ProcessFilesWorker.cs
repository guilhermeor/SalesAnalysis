using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System;
using System.Threading;
using System.Threading.Tasks;
using SalesAnalysis.Application;

namespace SalesAnalysis.Workers.Workers
{
    public class ProcessFilesWorker : BackgroundService
    {
        private readonly PathSettings _pathSettings;
        private readonly IProcessFile _processFile;
        private readonly string _fullPathIn;
        private readonly string _fullPathOut;
        private readonly string _pathDefault;

        public ProcessFilesWorker(IOptions<Settings> settings, IProcessFile processFile)
        {
            _pathSettings = settings.Value.PathSettings;
            _pathDefault = $"{Environment.GetEnvironmentVariable(_pathSettings.Default)}";
            _fullPathIn = $"{_pathDefault}{_pathSettings.In}";
            _fullPathOut = $"{_pathDefault}{_pathSettings.Out}";
            _processFile = processFile;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Task.Run(async () =>
            {
                while (await Program.readChannel.Reader.WaitToReadAsync())
                {
                    var key = await Program.readChannel.Reader.ReadAsync();
                    var report = await _processFile.GetReport($"{_fullPathIn}/{key}");
                    await _processFile.Write(report, $"{_fullPathOut}{key}");
                }
            });

        }
    }

}