using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using SalesAnalysis.Application;
using SalesAnalysis.Domain;
using SalesAnalysis.Workers;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SalesAnalysis.UnitTests
{
    public class ProcessFileTests
    {
        private readonly IOptions<Settings> _options;
        private readonly Mock<ILogger<Report>> _mockLogger;
        private ProcessFile _processFile;
        public ProcessFileTests()
        {
            _mockLogger = new Mock<ILogger<Report>>();
            _processFile = new ProcessFile(_mockLogger.Object);
            Settings settings = new Settings()
            {
                PathSettings = new PathSettings
                {
                    Default = $"{Environment.CurrentDirectory}",
                    In = "\\data\\in",
                    Out = "\\data\\out"
                }
            };
            _options = Options.Create(settings);
            Directory.CreateDirectory($"{ _options.Value.PathSettings.Default }{_options.Value.PathSettings.In}");
            Directory.CreateDirectory($"{ _options.Value.PathSettings.Default }{_options.Value.PathSettings.Out}");
        }
        ~ProcessFileTests()
        {
            Directory.Delete($"{ _options.Value.PathSettings.Default }", true);
        }

        [Fact]
        public async Task ShouldBeLogAndThrownExceptionWhenFileNotFound()
        {
            await Assert.ThrowsAsync<ArgumentException>(() => _processFile.GetReport(""));
            Assert.Equal(1, _mockLogger.Invocations.Count);
        }

        [Fact]
        public async Task ShouldBeReadAndGenerateAReport()
        {
            var reportFake = new Report(2, 3, "10", "Ju�ara Nunes Nun�apo");

            var builder = new StringBuilder();
            builder.AppendLine("001�1234567891234�Luis�50000");
            builder.AppendLine("001�1234567891234�Pedro�50000");
            builder.AppendLine("001�3245678865434�Ju�ara Nunes Nun�apo�40000");
            builder.AppendLine("002�2345675434544345�Jose da Silva�Rural");
            builder.AppendLine("002�2345675433444345�Ju�ara asdwdwefwe Nun�a�Rural");
            builder.AppendLine("003�10�[1 - 10 - 100, 2 - 30 - 2.50, 3 - 40 - 3.10]�Pedro");
            builder.AppendLine("003�08�[1 - 34 - 10, 2 - 33 - 1.50, 3 - 40 - 0.10]�Ju�ara Nunes Nun�apo");

            var fullName = $"{ _options.Value.PathSettings.Default }{_options.Value.PathSettings.In}/fake123fake.txt";

            using (StreamWriter outputFile = new StreamWriter(fullName))
            {
                await outputFile.WriteAsync(builder);
            }

            var report = await _processFile.GetReport(fullName);
            File.Delete(fullName);

            Assert.Equal(1, _mockLogger.Invocations.Count);

            Assert.Equal(reportFake.CustomerQuantity, report.CustomerQuantity);
            Assert.Equal(reportFake.IdSaleMoreExpensive, report.IdSaleMoreExpensive);
            Assert.Equal(reportFake.SalesmanQuantity, report.SalesmanQuantity);
            Assert.Equal(reportFake.WorstSalesman, report.WorstSalesman);
        }
        [Fact]
        public async Task ShouldWriteAReportOnFile()
        {
            var path = $"{ _options.Value.PathSettings.Default }{_options.Value.PathSettings.Out}";
            var fullName = $"{path}/fake123fake.txt";
            var reportFake = new Report(2, 3, "10", "Ju�ara Nunes Nun�apo");

            await _processFile.Write(reportFake, fullName);

            Assert.True(File.Exists(fullName));
            File.Delete(fullName);
        }
    }
}
//var builder = new StringBuilder();
//builder.AppendLine("001�1234567891234�Luis�50000");
//builder.AppendLine("001�1234567891234�Pedro�50000");
//builder.AppendLine("001�3245678865434�JJu�ara Nunes Nun�apo40000");
//builder.AppendLine("002�2345675434544345�Jose da Silva�Rural");
//builder.AppendLine("002�2345675433444345�Ju�ara asdwdwefwe Nun�a�Rural");
//builder.AppendLine("003�10�[1 - 10 - 100, 2 - 30 - 2.50, 3 - 40 - 3.10] �Pedro");
//builder.AppendLine("003�08�[1 - 34 - 10, 2 - 33 - 1.50, 3 - 40 - 0.10] �Ju�ara Nunes Nun�apo");

//var fullName = $"{ _options.Value.PathSettings.Default }{_options.Value.PathSettings.In}/fake123fake.txt";

//string[] fakeData = new string[] { "001�1234567891234�Luis�50000" , "001�1234567891234�Pedro�50000", "001�3245678865434�JJu�ara Nunes Nun�apo40000" ,
//"002�2345675434544345�Jose da Silva�Rural","002�2345675433444345�Ju�ara asdwdwefwe Nun�a�Rural","003�10�[1 - 10 - 100, 2 - 30 - 2.50, 3 - 40 - 3.10] �Pedro",
//"003�08�[1 - 34 - 10, 2 - 33 - 1.50, 3 - 40 - 0.10] �Ju�ara Nunes Nun�apo"};


//_mockProcessFile.Setup(m => m.GetReport(It.IsAny<string>())).ReturnsAsync(reportFake);