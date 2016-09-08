using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeSeriesAnalyze
{
    class Translator
    {
        public static decimal[][] _base = null;

        public static void FetchBase()
        {
            var reader = new StreamReader("centers.csv");
            List<decimal[]> centers = new List<decimal[]>();

            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                var values = line.Split(',').Select(i => Decimal.Parse(i, NumberStyles.Any, CultureInfo.InvariantCulture)).ToArray();
                centers.Add(values);
            }
            _base = centers.ToArray();
        }

        public static void LearnBase(string filename, int k, int steps)
        {
            var reader = new StreamReader(File.OpenRead(filename));
            List<decimal[]> series = new List<decimal[]>();

            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                var values = line.Split(',').Select(i => Decimal.Parse(i, NumberStyles.Any, CultureInfo.InvariantCulture)).ToArray();
                series.Add(values);
            }

            var input = series.ToArray();
            var res = Centers.Kmeans(input, k, steps);

            using (var writerCenters = new StreamWriter("centers.csv", true))
            {
                for (int i = 0; i < k; i++)
                {
                    using (var writerMembers = new StreamWriter(@"out\center" + i.ToString() + ".csv", true))
                    {
                        writerMembers.WriteLine(string.Join(",", res[i].Item1.Select(a => Convert.ToString(a, CultureInfo.InvariantCulture))));
                        writerCenters.WriteLine(string.Join(",", res[i].Item1.Select(a => Convert.ToString(a, CultureInfo.InvariantCulture))));

                        foreach (var seq in res[i].Item2)
                        {
                            writerMembers.WriteLine(string.Join(",", seq.Select(a => Convert.ToString(a, CultureInfo.InvariantCulture))));
                        }
                    }
                }
            }
        }
        
        public static int Classify(decimal[] serie)
        {
            decimal min = decimal.MaxValue;
            int ret = -1;
            for (int i=0; i<_base.Length; i++)
            {
                decimal val = DtwAverage.Dtw2(serie, _base[i]);
                if (val < min)
                {
                    ret = i;
                    min = val;
                }
            }
            return ret;
        }

        public static int[] ToReal(decimal[] serie, int dimension = -1)
        {
            if (dimension == -1)
            {
                dimension = serie.Length / _base.First().Length;
            }

            List<int> ret = new List<int>();
            int step = serie.Length / dimension;
            for (int i=0; i<serie.Length; i+=step)
            {
                decimal[] window = DtwAverage.Normalize(serie.Skip(i).Take(step).ToArray());
                ret.Add(Classify(window));
            }
            return ret.ToArray();
        }

        public static void TranslateSeries(string fileRead, string fileWrite)
        {
            using (var writer = new StreamWriter(fileWrite, true))
            using (var reader = new StreamReader(fileRead))
            {
                int num = 1;
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(',').Select(i => Decimal.Parse(i, NumberStyles.Any, CultureInfo.InvariantCulture)).ToArray();

                    string val = string.Join(",", ToReal(values).Select(a => a.ToString()));
                    writer.WriteLine(val);
                    Console.WriteLine(val);
                    Console.WriteLine("{0} translated", num);
                    num++;
                }
            }
        }
    }
}
