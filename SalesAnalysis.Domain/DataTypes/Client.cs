namespace SalesAnalysis.Domain.DataTypes
{
    public class Client
    {
        public Client(string[] client)
        {
            CNPJ = client[1];
            Name = MountName(client);
            BusinessArea = client[^1];
        }
        public string CNPJ { get; set; }
        public string Name { get; set; }
        public string BusinessArea { get; set; }
        private string MountName(string[] client) => DataHelper.MountName(client, DataType.Client);
    }
}
