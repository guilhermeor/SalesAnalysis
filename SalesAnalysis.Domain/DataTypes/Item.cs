
using System.Globalization;

namespace SalesAnalysis.Domain.DataTypes
{
    public class Item
    {
        public Item(string [] item)
        {
            Id = int.Parse(item[0]);
            Quantity = int.Parse(item[1]);
            Price = decimal.Parse(item[2], new CultureInfo("en-US"));
        }
        public int Id { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
