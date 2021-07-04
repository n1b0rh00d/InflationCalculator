using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Contracts
{
    public class DictionaryObservations
    {
        public Dictionary<string, SerieObservations> series;

        char[] delimiter = new char[] { '\t' };

        public DictionaryObservations ()
        {
            series = new Dictionary<string, SerieObservations>();
            ParseFile();
            InitPercentChanges();
        }

        void ParseFile()
        {
            foreach (var line in File.ReadLines(
                "C:\\Users\\rchapas\\source\\repos\\InflationCalculator\\CUUR000.txt"))
            {
                var data = line.Split(delimiter, StringSplitOptions.RemoveEmptyEntries).Select(x=>x.Trim()).ToArray();
                if (data.Length != 4)
                {
                    throw new NotImplementedException();
                }

                if (!series.ContainsKey(data[0]))
                    series[data[0]] = new SerieObservations();

                series[data[0]].Add(new SerieObservation(data[0], data[1], data[2], data[3]));
            }
        }

        void InitPercentChanges()
        {
            foreach (var k in series)
            {
                k.Value.SetPercents();
            }
        }

       

    }
}
