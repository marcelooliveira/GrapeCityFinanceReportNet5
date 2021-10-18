using C1Finance.Models;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using static C1Finance.Models.PortfolioStatic;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            PortfolioStatic.SymbolList = GetStockSymbols();
            var portfolios = PortfolioModel.PortfolioList;

            //File.WriteAllText("portfolios-2016.json", JsonConvert.SerializeObject(portfolios));
            File.WriteAllText("portfolios-2021.json", JsonConvert.SerializeObject(portfolios));

            //string baseURL = "https://www.alphavantage.co/query?function=TIME_SERIES_DAILY&symbol={simbol}&apikey=EQ8R2LTG732VP7HE&datatype=csv&outputsize=full";
            //var client = new RestClient(baseURL);
            //var companies = GetCompanies();
            //var dailyStocks = new List<DailyStock>();
            //foreach (var company in companies)
            //{
            //    string url = $"query?function=TIME_SERIES_DAILY&symbol={company.Key}&apikey=EQ8R2LTG732VP7HE&datatype=csv&outputsize=full";
            //    var request = new RestRequest(url, DataFormat.Json);
            //    var response = client.Get(request);
            //    var content = response.Content;
            //    var lines = content.Split("\r\n");
            //    foreach (var line in lines)
            //    {
            //        var dailyStock = new DailyStock(company.Key, line);
            //        if (dailyStock.timestamp == null)
            //        {
            //            continue;
            //        }
            //        if (DateTime.Today.Subtract(dailyStock.timestamp.Value).TotalDays >= 90)
            //        {
            //            break;
            //        }
            //        dailyStocks.Add(dailyStock);
            //    }
            //}
            //File.WriteAllText("stocks.json", JsonConvert.SerializeObject(dailyStocks));
        }

        private static Dictionary<string, string> GetCompanies()
        {
            return new Dictionary<string, string>
            {
                { "AAPL", "Apple Inc." },
                { "MSFT", "Microsoft Corporation" },
                { "GOOG", "Alphabet Inc." },
                { "AMZN", "Amazon.com, Inc." },
                { "FB", "Facebook, Inc." }
            };
        }

        private static List<StockSymbol> GetStockSymbols()
        {
            return new List<StockSymbol>
            { 
                new StockSymbol("AAPL", "Apple Inc."),
                new StockSymbol("MSFT", "Microsoft Corporation"),
                new StockSymbol("GOOG", "Alphabet Inc."),
                new StockSymbol("AMZN", "Amazon.com, Inc."),
                new StockSymbol("FB", "Facebook, Inc.")
            };
        }

    }

    public class DailyStock
    {
        public DailyStock(string symbol, string line)
        {
            var parts = line.Split(',');
            var i = 0;
            DateTime.TryParse(parts[i++], out DateTime ts);
            if (ts == new DateTime(0))
            {
                timestamp = null;
                return;
            }
            this.symbol = symbol;
            this.timestamp = ts;
            this.open = decimal.Parse(parts[i++]);
            this.high = decimal.Parse(parts[i++]);
            this.low = decimal.Parse(parts[i++]);
            this.close = decimal.Parse(parts[i++]);
            this.volume = decimal.Parse(parts[i++]);
        }

        public string symbol { get; set; }
        public DateTime? timestamp { get; set; }
        public decimal open { get; set; }
        public decimal high { get; set; }
        public decimal low { get; set; }
        public decimal close { get; set; }
        public decimal volume { get; set; }
    }
}
