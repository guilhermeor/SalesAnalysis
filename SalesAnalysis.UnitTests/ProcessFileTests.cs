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
            var reportFake = new Report(2, 3, "10", "Juçara Nunes Nunçapo");

            var builder = new StringBuilder();
            builder.AppendLine("001ç1234567891234çLuisç50000");
            builder.AppendLine("001ç1234567891234çPedroç50000");
            builder.AppendLine("001ç3245678865434çJuçara Nunes Nunçapoç40000");
            builder.AppendLine("002ç2345675434544345çJose da SilvaçRural");
            builder.AppendLine("002ç2345675433444345çJuçara asdwdwefwe NunçaçRural");
            builder.AppendLine("003ç10ç[1 - 10 - 100, 2 - 30 - 2.50, 3 - 40 - 3.10]çPedro");
            builder.AppendLine("003ç08ç[1 - 34 - 10, 2 - 33 - 1.50, 3 - 40 - 0.10]çJuçara Nunes Nunçapo");

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
            var reportFake = new Report(2, 3, "10", "Juçara Nunes Nunçapo");

            await _processFile.Write(reportFake, fullName);

            Assert.True(File.Exists(fullName));
            File.Delete(fullName);
        }
    }
}
//var builder = new StringBuilder();
//builder.AppendLine("001ç1234567891234çLuisç50000");
//builder.AppendLine("001ç1234567891234çPedroç50000");
//builder.AppendLine("001ç3245678865434çJJuçara Nunes Nunçapo40000");
//builder.AppendLine("002ç2345675434544345çJose da SilvaçRural");
//builder.AppendLine("002ç2345675433444345çJuçara asdwdwefwe NunçaçRural");
//builder.AppendLine("003ç10ç[1 - 10 - 100, 2 - 30 - 2.50, 3 - 40 - 3.10] çPedro");
//builder.AppendLine("003ç08ç[1 - 34 - 10, 2 - 33 - 1.50, 3 - 40 - 0.10] çJuçara Nunes Nunçapo");

//var fullName = $"{ _options.Value.PathSettings.Default }{_options.Value.PathSettings.In}/fake123fake.txt";

//string[] fakeData = new string[] { "001ç1234567891234çLuisç50000" , "001ç1234567891234çPedroç50000", "001ç3245678865434çJJuçara Nunes Nunçapo40000" ,
//"002ç2345675434544345çJose da SilvaçRural","002ç2345675433444345çJuçara asdwdwefwe NunçaçRural","003ç10ç[1 - 10 - 100, 2 - 30 - 2.50, 3 - 40 - 3.10] çPedro",
//"003ç08ç[1 - 34 - 10, 2 - 33 - 1.50, 3 - 40 - 0.10] çJuçara Nunes Nunçapo"};


//_mockProcessFile.Setup(m => m.GetReport(It.IsAny<string>())).ReturnsAsync(reportFake);