using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeSeriesAnalyze
{
    class Program
    {
        public static void Main()
        {
            var reader = new StreamReader(File.OpenRead("sample_input.csv"));
            List<decimal[]> series = new List<decimal[]>();

            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                var values = line.Split(',').Select(i => Decimal.Parse(i, NumberStyles.Any, CultureInfo.InvariantCulture)).ToArray();
                series.Add(values);
            }

            var input = series.ToArray();
            using (var writer = new StreamWriter("output.csv", true))
            {
                writer.WriteLine(string.Join(",", DtwAverage.Average(input).Select(a => Convert.ToString(a, CultureInfo.InvariantCulture))));
            }
        }
    }
}
