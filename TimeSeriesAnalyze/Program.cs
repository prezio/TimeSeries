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
        public static void Main(string[] args)
        {
            Translator.LearnBase("input_learn.csv", 200, 2);
            Console.WriteLine("Base succesfully learned");
            Translator.FetchBase();
            Console.WriteLine("Base succesfully fetched");
            Translator.TranslateSeries("input_trans.csv", "output_trans.csv");
            Console.WriteLine("Series succesfully translated");
        }
    }
}
