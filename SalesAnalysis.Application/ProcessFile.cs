using SalesAnalysis.Domain;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System;

namespace SalesAnalysis.Application
{
    public class ProcessFile : IProcessFile
    {
        private readonly ILogger<Report> _logger;
        public ProcessFile(ILogger<Report> logger) => _logger = logger;
        public async Task<Report> GetReport(string fullName)
        {
            var dataFile = new DataFile();
            try
            {
                dataFile.Process(await File.ReadAllLinesAsync(fullName));

                var salesOrdered = dataFile.Sales.OrderByDescending(x => x.Items.Sum(d => d.Price * d.Quantity));
                var report = new Report(
                dataFile.Clients.Count(),
                dataFile.Sellers.Count(),
                salesOrdered?.FirstOrDefault()?.Id ?? string.Empty,
                salesOrdered?.LastOrDefault()?.Seller ?? string.Empty
                );
                _logger.LogInformation($"File: {fullName} processed successfully");
                return report;
            }
            catch (ArgumentException ex)
            {
                _logger.LogError($"Error processing file: {fullName}");
                throw new ArgumentException(ex.ToString());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        public async Task Write(Report report, string fullName)
        {
            var builder = DataHelper.CreateReport(report);
            using StreamWriter outputFile = new StreamWriter(fullName);
            await outputFile.WriteAsync(builder);
        }
    }
}
