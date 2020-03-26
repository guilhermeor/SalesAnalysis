using System.Globalization;

namespace SalesAnalysis.Domain.DataTypes
{
    public class Seller
    {
        public Seller(string[] seller)
        {
            CPF = seller[1];
            Name = MountName(seller);
            Salary = decimal.Parse(seller[^1], new CultureInfo("en-US"));
        }
        public string CPF { get; set; }
        public string Name { get; set; }
        public decimal Salary { get; set; }
        private string MountName(string[] seller) => DataHelper.MountName(seller, DataType.Seller);
    }
}
