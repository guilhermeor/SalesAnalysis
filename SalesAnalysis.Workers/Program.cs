using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SalesAnalysis.Application;
using SalesAnalysis.Workers;
using SalesAnalysis.Workers.Workers;
using System.Threading.Channels;

namespace SalesAnalysis
{
    public class Program
    {
        public static Channel<string> readChannel = Channel.CreateBounded<string>(new BoundedChannelOptions(100000) { FullMode = BoundedChannelFullMode.Wait });
        public static void Main(string[] args) => CreateHostBuilder(args).Build().Run();
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddHostedService<ConfigureWatcher>();
                    services.AddHostedService<ProcessFilesWorker>();
                    services.Configure<Settings>(hostContext.Configuration);
                    services.AddTransient<IProcessFile, ProcessFile>();
                });
    }
}
