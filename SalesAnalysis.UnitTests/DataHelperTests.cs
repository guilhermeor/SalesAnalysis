using SalesAnalysis.Domain;
using System;
using System.Text.RegularExpressions;
using Xunit;

namespace SalesAnalysis.UnitTests
{
    public class DataHelperTests
    {
        [Theory]
        [InlineData("001ç1234567891234çPedroç50000", "Pedro")]
        [InlineData("001ç3245678865434çJuçara Franco Nunçapoç40000", "Juçara Franco Nunçapo")]
        public void SellerNameShouldBeWithNoMissingParts(string line, string name)
            => Assert.Equal(name, DataHelper.MountName(line.Split(DataHelper.DataSeparator), DataType.Seller));

        [Theory]
        [InlineData("002ç2345675434544345çJose da SilvaçRural", "Jose da Silva")]
        [InlineData("002ç2345675433444345çÇoriano LorençoçRural", "Çoriano Lorenço")]
        public void ClientNameShouldBeWithNoMissingParts(string line, string name)
            => Assert.Equal(name, DataHelper.MountName(line.Split(DataHelper.DataSeparator), DataType.Client));

        [Theory]
        [InlineData("003ç10ç[1 - 10 - 100, 2 - 30 - 2.50, 3 - 40 - 3.10] çLorenço", "Lorenço")]
        [InlineData("003ç08ç[1 - 34 - 10, 2 - 33 - 1.50, 3 - 40 - 0.10] çSuelen Saçarami Souça", "Suelen Saçarami Souça")]
        public void SellernNameOnSaleShouldBeWithNoMissingParts(string line, string name)
            => Assert.Equal(name, DataHelper.MountName(line.Split(DataHelper.DataSeparator), DataType.Sale));

        [Theory]
        [InlineData(DataType.Sale)]
        [InlineData(DataType.Seller)]
        [InlineData(DataType.Client)]
        public void ShouldBeThrowExceptionWhenLinIsNull(string type)
            => Assert.Throws<ArgumentNullException>(() => DataHelper.MountName(null, type));

        [Theory]
        [InlineData(1, 1, "2", "Carlos")]
        [InlineData(0, 0, "0", null)]
        public void ShouldBeReturnReportInCorrectFormat(int customerQuantity, int salesmanQuantity, string idSale, string worstSeller)
        {
            var fakeReport = new Report(customerQuantity, salesmanQuantity, idSale, worstSeller);
            var lineCounter = Regex.Matches(DataHelper.CreateReport(fakeReport).ToString(), Environment.NewLine).Count;
            Assert.Equal(4, lineCounter);
        }
    }
}