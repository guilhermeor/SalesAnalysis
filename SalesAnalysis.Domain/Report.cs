namespace SalesAnalysis.Domain
{
    public class Report
    {
        public Report(int customerQuantity, int salesmanQuantity, string idSale, string salesman)
        {
            CustomerQuantity = customerQuantity;
            SalesmanQuantity = salesmanQuantity;
            IdSaleMoreExpensive = idSale;
            WorstSalesman = salesman;
        }

        public int CustomerQuantity { get; set; }
        public int SalesmanQuantity { get; set; }
        public string IdSaleMoreExpensive { get; set; }
        public string WorstSalesman { get; set; }
    }
}
