using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeSeriesAnalyze
{
    class Centers
    {
        public static Tuple<decimal[], List<decimal[]>>[] Kmeans(decimal[][] series, int k, int steps = -1)
        {
            Random rnd = new Random();
            var centers = Enumerable.Range(0, series.Length).OrderBy(r => rnd.Next()).Take(k).Select(i => new Tuple<decimal[], List<decimal[]>>(series[i], new List<decimal[]>())).ToArray();

            for (int i = 0; i < steps; i++)
            {
                foreach (var center in centers)
                {
                    center.Item2.Clear();
                }
                for (int j = 0; j < series.Length; j++)
                {
                    decimal min = decimal.MaxValue;
                    int center_index = -1;
                    for (int z = 0; z < k; z++)
                    {
                        decimal val = DtwAverage.Dtw2(series[j], centers[z].Item1);
                        if (val < min)
                        {
                            center_index = z;
                            min = val;
                        }
                    }

                    Console.WriteLine("Step {0}: serie {1} / {2}", i, j, series.Length);
                    centers[center_index].Item2.Add(series[j]);
                }
                for (int j = 0; j < k; j++)
                {
                    centers[j] = new Tuple<decimal[], List<decimal[]>>(DtwAverage.Average(centers[j].Item2.ToArray()), centers[j].Item2);
                }
            }
            return centers;
        }
    }
}
