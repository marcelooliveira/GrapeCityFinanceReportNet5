using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StocksReport.Model
{

    public class SymbolPriceHistory
    {
        public DateTime PriceDate { get; set; }
        public string PriceMonth { get { return PriceDate.ToString("MMM-yyyy"); } }
        public string Symbol { get; set; }
        public double Price { get; set; }
        public double PriceGrowth { get; set; }
    }

}
