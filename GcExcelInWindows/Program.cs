using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using GrapeCity.Documents.Excel;
using Newtonsoft.Json;

namespace GcExcelInWindows
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Generating Stocks Report using GcExcel Templates");
            // Initialize Workbook
            Workbook workbook = new Workbook();
            // Load Stocks_Template.xlsx Template in workbook from Resource
            var templateFile = "Stocks_Evolution_Template.xlsx";
            workbook.Open(templateFile);

            // We can have mutiple types of DataSources like Custom Object/ DataSet/ DataTable/ Json/ Variable.
            // Here ds is a DataTable
            var ds = new DataTable();

            // Adding DataColumns in DataTable according to the Template fields
            ds.Columns.Add(new DataColumn("Name", typeof(string)));
            ds.Columns.Add(new DataColumn("Symbol", typeof(string)));
            ds.Columns.Add(new DataColumn("LastPrice", typeof(decimal)));
            ds.Columns.Add(new DataColumn("Change", typeof(decimal)));
            ds.Columns.Add(new DataColumn("Shares", typeof(int)));
            ds.Columns.Add(new DataColumn("Price", typeof(decimal)));
            ds.Columns.Add(new DataColumn("Cost", typeof(decimal)));
            ds.Columns.Add(new DataColumn("Value", typeof(decimal)));
            ds.Columns.Add(new DataColumn("Gain", typeof(decimal)));

            var dsHistory = new DataTable();

            // Adding DataColumns in DataTable according to the Template fields
            dsHistory.Columns.Add(new DataColumn("PriceDate", typeof(DateTime)));
            dsHistory.Columns.Add(new DataColumn("PriceMonth", typeof(string)));
            dsHistory.Columns.Add(new DataColumn("Symbol", typeof(string)));
            dsHistory.Columns.Add(new DataColumn("Price", typeof(double)));
            dsHistory.Columns.Add(new DataColumn("PriceGrowth", typeof(double)));

            // Adding Data in DataTable
            string stocksText = File.ReadAllText("portfolios-2021.json");
            var portfolios = (List<Portfolio>)JsonConvert.DeserializeObject(stocksText, typeof(List<Portfolio>));

            portfolios = (from p in portfolios orderby p.Symbol select p).ToList();

            var history = new List<SymbolPriceHistory>();

            foreach (var portfolio in portfolios)
            {
                ds.Rows.Add(portfolio.Name,
                            portfolio.Symbol,
                            portfolio.LastPrice,
                            portfolio.Change,
                            portfolio.Shares,
                            portfolio.Price,
                            portfolio.Cost,
                            portfolio.Value,
                            portfolio.Gain);


                foreach (var h in portfolio.PriceHistory)
                {
                    history.Add(new SymbolPriceHistory
                    {
                        PriceDate = h.PriceDate,
                        Symbol = portfolio.Symbol,
                        Price = h.Price,
                        PriceGrowth = h.PriceGrowth * 100.0
                    });
                }
            }
                
            history = history.OrderBy(h => h.PriceDate).ThenBy(h => h.Symbol).ToList();

            foreach (var h in history)
            {
                dsHistory.Rows.Add(
                    h.PriceDate,
                    h.PriceDate.ToString("MMM-yyyy"),
                    h.Symbol,
                    h.Price,
                    h.PriceGrowth);
            }

            // Add DataSource
            // Here "ds" and "dsHistory" are the names of dataSources which are used in templates to define fields like {{ds.LastPrice}}
            workbook.AddDataSource("ds", ds);
            workbook.AddDataSource("dsHistory", dsHistory);

            // Invoke to process the template
            workbook.ProcessTemplate();

            // Save to an excel file
            Console.WriteLine("Stocks_Evolution_Template.xlsx Template is now bound to DataTable and generated Stocks_Evolution.xlsx file");
            workbook.Save("Stocks_Evolution.xlsx");
        }
    }

    //Class for Portfolio to View (Grid & Chart)
    public partial class Portfolio
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

    //Class for Price History of Portfolio
    public partial class SymbolPriceHistory
    {
        public DateTime PriceDate { get; set; }
        public string PriceMonth { get; set; }
        public string Symbol { get; set; }
        public double Price { get; set; }
        public double PriceGrowth { get; set; }
    }
}
