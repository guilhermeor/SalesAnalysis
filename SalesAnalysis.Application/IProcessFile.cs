using SalesAnalysis.Domain;
using System.Threading.Tasks;

namespace SalesAnalysis.Application
{
    public interface IProcessFile
    {
        Task Write(Report report, string fullName);
        Task<Report> GetReport(string fullName);
    }
}
