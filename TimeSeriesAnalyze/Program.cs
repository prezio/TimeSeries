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
            Translator.LearnBase("learn_ready.csv", 40, 2);
            Console.WriteLine("Base succesfully learned");
            Translator.FetchBase();
            Console.WriteLine("Base succesfully fetched");
            Translator.TranslateSeries("translate_ready.csv", "translate_final.csv");
            Console.WriteLine("Series succesfully translated");
        }
    }
}
