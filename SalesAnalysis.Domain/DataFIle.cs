using SalesAnalysis.Domain.DataTypes;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Concurrent;

namespace SalesAnalysis.Domain
{
    public class DataFile
    {
        public DataFile()
        {
            Sellers = new ConcurrentBag<Seller>();
            Clients = new ConcurrentBag<Client>();
            Sales = new ConcurrentBag<Sale>();
        }
        public ConcurrentBag<Seller> Sellers { get; }
        public ConcurrentBag<Client> Clients { get; }
        public ConcurrentBag<Sale> Sales { get; }

        public ParallelLoopResult Process(string[] lines)
        {
            return Parallel.ForEach(lines, (line) =>
            {
                string[] data = DataHelper.SplitData(line);
                if (data.Length >= 4) 
                    switch (data.First())
                    {
                        case DataType.Seller:
                            Sellers.Add(new Seller(data));
                            break;
                        case DataType.Client:
                            Clients.Add(new Client(data));
                            break;
                        case DataType.Sale:
                            Sale sale = new Sale(data);
                            sale.ProcessItems();
                            Sales.Add(sale);
                            break;
                        default:
                            break;
                    }
            });
        }
    }
}
