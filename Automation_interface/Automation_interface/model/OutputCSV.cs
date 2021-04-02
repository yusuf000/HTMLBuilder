using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.VisualStyles;
using CsvHelper;

namespace Automation_interface.model
{
    class OutputCSV
    {
        private string systemName;
        private string includeInSiteMap;
        private string title;
        private string body;
        private string metaKeyward;
        private string metaDescription;
        private string metaTile;
        private string pageName;
        private static string[] headers = { "System name", "Include in sitemap", "Title","Body", "Meta Keywords", "Meta description", "Meta title", "Search engine friendly page name"};

        public OutputCSV()
        {

        }

        public void setAllAtribute(string sysName, string inSiteMap, string title, string metaKey, string metaDes, string metaTitle, string pageName)
        {
            this.systemName = sysName;
            this.includeInSiteMap = inSiteMap;
            this.title = title;
            this.metaKeyward = metaKey;
            this.metaDescription = metaDes;
            this.pageName = pageName;
            this.metaTile = metaTitle;
        }

        public static void getCsv(List<PageBuilder> pages)
        {
            var mem = new MemoryStream();
            var writer = new StreamWriter(mem);
            var csvWriter = new CsvWriter(writer, CultureInfo.CurrentCulture);
            bool addHeader = true;
            int counter = 1;
            if (Util.continuousOutputCSV && File.Exists(@"output.csv"))
            {
                counter = File.ReadLines(@"output.csv").Count();
                if (counter > 0)
                {
                    addHeader = false;
                }
            }

            if (addHeader)
            {
                for (int i = 0; i < headers.Length; i++)
                {
                    csvWriter.WriteField(headers[i]);
                }
                csvWriter.NextRecord();
            }
            string directory = Directory.GetCurrentDirectory();
            for (int i = 0; i < pages.Count; i++)
            {
                csvWriter.WriteField(pages[i].outputCsv.systemName + " - " + (counter++));
                csvWriter.WriteField(pages[i].outputCsv.includeInSiteMap);
                csvWriter.WriteField(pages[i].outputCsv.title);
                csvWriter.WriteField(directory + "\\" +pages[i].getFileName());
                csvWriter.WriteField(pages[i].outputCsv.metaKeyward);
                csvWriter.WriteField(pages[i].outputCsv.metaDescription);
                csvWriter.WriteField(pages[i].outputCsv.metaTile);
                csvWriter.WriteField(pages[i].getUrlWithoutHost());
                csvWriter.NextRecord();
            }
            writer.Flush();
            var result = Encoding.UTF8.GetString(mem.ToArray());
            if (Util.continuousOutputCSV)
            {
                File.AppendAllText(@"output.csv", result);
            }
            else
            {
                File.WriteAllText(@"output.csv", result);
            }

            
        }
    }
}
