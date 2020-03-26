using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace SalesAnalysis.Workers
{
    public class ConfigureWatcher : BackgroundService
    {
        private FileSystemWatcher _watcher;
        private readonly PathSettings _pathSettings;

        public ConfigureWatcher(IOptions<Settings> settings) => _pathSettings = settings.Value.PathSettings;

        protected override Task ExecuteAsync(CancellationToken cancellationToken)
        {
            _watcher = new FileSystemWatcher
            {
                Path = $"{ Environment.GetEnvironmentVariable(_pathSettings.Default)}{_pathSettings.In}",
                NotifyFilter = NotifyFilters.FileName,
                Filter = "*.txt"
            };

            _watcher.Created += OnCreated;
            _watcher.EnableRaisingEvents = true;
            return Task.CompletedTask;
        }

        public void OnCreated(object sender, FileSystemEventArgs e) => Program.readChannel.Writer.WriteAsync(e.Name);
    }
}
