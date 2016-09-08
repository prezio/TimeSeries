using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeSeriesAnalyze
{
    class DtwAverage
    {
        public static decimal Dist(decimal a, decimal b)
        {
            return Math.Abs(a - b);
        }

        public static Tuple<decimal, Tuple<int, int>>[,] Dtw(decimal[] a, decimal[] b)
        {
            Tuple<decimal, Tuple<int, int>>[,] m = new Tuple<decimal, Tuple<int, int>>[a.Length + 1, b.Length + 1];

            m[1, 1] = new Tuple<decimal, Tuple<int, int>>(Dist(a[0], b[0]), new Tuple<int, int>(0, 0));
            for (int i = 2; i <= a.Length; i++)
            {
                m[i, 1] = new Tuple<decimal, Tuple<int, int>>(m[i - 1, 1].Item1 + Dist(a[i - 1], b[0]), new Tuple<int, int>(i - 1, 1));
            }
            for (int i = 2; i <= b.Length; i++)
            {
                m[1, i] = new Tuple<decimal, Tuple<int, int>>(m[1, i - 1].Item1 + Dist(a[0], b[i - 1]), new Tuple<int, int>(1, i - 1));
            }
            for (int i = 2; i <= a.Length; i++)
            {
                for (int j = 2; j <= b.Length; j++)
                {
                    var prec = new List<Tuple<decimal, Tuple<int, int>>>() { m[i - 1, j], m[i, j - 1], m[i - 1, j - 1] };
                    var mini = prec.Min(a1 => a1.Item1);
                    int index = prec.IndexOf(prec.Where(k => k.Item1 == mini).First());

                    m[i, j] = new Tuple<decimal, Tuple<int, int>>(mini + Dist(a[i - 1], b[j - 1]),
                        index == 0 ? new Tuple<int, int>(i - 1, j) :
                        index == 1 ? new Tuple<int, int>(i, j - 1) :
                        new Tuple<int, int>(i - 1, j - 1));
                }
            }
            return m;
        }

        public static decimal[] Normalize(decimal[] a)
        {
            decimal avg = a.Average();
            return a.Select(x => x - avg).ToArray();
        }

        public static decimal Dtw2(decimal[] a, decimal[] b)
        {
            return Dtw(a, b)[a.Length, b.Length].Item1;
        }

        public static decimal[] Average(decimal[][] s)
        {
            int number_series = s.Count();
            int win_size = s[0].Count();
            var C = new decimal[win_size];
            s[0].CopyTo(C, 0);
            var assocTab = new List<decimal>[win_size];
            for (int i = 0; i < win_size; i++)
            {
                assocTab[i] = new List<decimal>();
            }
            for (int i = 0; i < number_series; i++)
            {
                var m = DtwAverage.Dtw(C, s[i]);

                int k = C.Count();
                int l = win_size;

                while (l >= 1 && k >= 1)
                {
                    assocTab[k - 1].Add(s[i][l - 1]);
                    var last = m[k, l];
                    k = last.Item2.Item1;
                    l = last.Item2.Item2;
                }
            }

            return assocTab.Select(a => a.Average()).ToArray();
        }
    }
}
