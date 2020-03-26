using System;
using System.Linq;
using System.Text;

namespace SalesAnalysis.Domain
{
    public static class DataHelper
    {
        public const char DataSeparator = 'ç';
        public const char ItemsSaleSeparator = ',';
        public const char ItemsInfoSeparator = '-';
        private static StringBuilder _builder = new StringBuilder();
        public static string MountName(string[] line, string dataType)
        {
            if (line == null) throw new ArgumentNullException($"{line}");
            return dataType switch
            {
                DataType.Seller => string.Join(DataSeparator, line.Skip(2).Take(line.Length - 3)),
                DataType.Client => string.Join(DataSeparator, line.Skip(2).Take(line.Length - 3)),
                DataType.Sale => string.Join(DataSeparator, line.Skip(3).Take(line.Length - 3)),
                _ => throw new ArgumentNullException($"{line}"),
            };
        }
        public static string[] SplitData(string line) => line.Trim().Split(DataSeparator);
        public static string[] SplitItemsSale(string items) => items.Trim('[', ']').Split(ItemsSaleSeparator);
        public static string[] SplitItemsInfo(string items) => items.Trim().Split(ItemsInfoSeparator);
        public static StringBuilder CreateReport(Report report)
        {
            _builder.Clear();
            _builder.AppendLine($"Salesman quantity: {report?.SalesmanQuantity ?? 0}");
            _builder.AppendLine($"Customer quantitiy: {report?.CustomerQuantity ?? 0}");
            _builder.AppendLine($"Sale Id more expensive quantity: {report?.IdSaleMoreExpensive}");
            _builder.AppendLine($"Worst Salesman: {report?.WorstSalesman}");
            return _builder;
        }
    }
}