using System.Collections.Generic;

namespace StocksReport.Model
{
    public class Portfolio
    {
        public string Name { get; set; }
        public string Symbol { get; set; }
        public double LastPrice { get; set; }
        public double Change { get; set; }
        public int Shares { get; set; }
        public double Price { get; set; }
        public double Cost { get; set; }
        public double Value { get; set; }
        public double Gain { get; set; }
        public List<SymbolPriceHistory> PriceHistory { get; set; }
    }

}
