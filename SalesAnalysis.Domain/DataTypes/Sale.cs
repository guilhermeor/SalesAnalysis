using System.Threading.Tasks;
using System.Collections.Concurrent;

namespace SalesAnalysis.Domain.DataTypes
{
    public class Sale
    {
        private readonly string _items;
        public Sale(string[] sale)
        {
            Items = new ConcurrentBag<Item>();
            Id = sale[1];
            _items = sale[2];
            Seller = MountName(sale);
        }
        public string Id { get; set; }
        public ConcurrentBag<Item> Items { get; }
        public string Seller { get; set; }

        private string MountName(string[] sale) => DataHelper.MountName(sale, DataType.Sale);
        public ParallelLoopResult ProcessItems()
        {
            return Parallel.ForEach(DataHelper.SplitItemsSale(_items), (item) =>
            {
                Items.Add(new Item(DataHelper.SplitItemsInfo(item)));
            });
        }
    }
}
