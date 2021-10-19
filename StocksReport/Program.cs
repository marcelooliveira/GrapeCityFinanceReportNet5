using GrapeCity.Documents.Excel;
using Newtonsoft.Json;
using StocksReport.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;

namespace StocksReport
{
    class Program
    {
        static void Main(string[] args)
        {
            //Add the following code to initialize Workbook
            Workbook workbook = new Workbook();

            //Add the following code to load the Stocks_Template.xlsx Template in workbook from Resource
            var templateFile = "Stocks_Report_Template.xlsx";
            workbook.Open(templateFile);

            //We can have mutiple types of DataSources like Custom Object/ DataSet/ DataTable/ Json/ Variable. In this code we're using a DataTable

            // Adding Data in DataTable
            string stocksText = File.ReadAllText("portfolios.json");
            var portfolios = (List<Portfolio>)JsonConvert.DeserializeObject(stocksText, typeof(List<Portfolio>));

            // Add DataSource
            // Here "ds" is the name of dataSource which is used in templates to define fields like {{ds.Symbol}}
            workbook.AddDataSource("ds", portfolios);

            // Invoke to process the template
            workbook.ProcessTemplate();

            // Save to an excel file
            Console.WriteLine("Stocks_Report_Template.xlsx Template is now bound to DataTable and generated Stocks_Report.xlsx file");
            workbook.Save("Stocks_Report.xlsx");
        }
    }
}
