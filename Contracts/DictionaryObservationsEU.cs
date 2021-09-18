using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Contracts
{
    public class DictionaryObservationsEU
    {
        public Dictionary<string, SerieObservations> series;

        char[] delimiter = new char[] { ',' };

        public DictionaryObservationsEU ()
        {
            series = new Dictionary<string, SerieObservations>();
            ParseFile();
            InitPercentChanges();
        }

        void ParseFile()
        {
            foreach (var line in File.ReadLines(
                "C:\\Users\\rchapas\\source\\repos\\InflationCalculator\\prc_hicp_aind_1_Data.csv"))
            {
                // year, code, name, unit , unit label, country, country name, value, extra
                var data = line.Replace("\"","").Split(delimiter, StringSplitOptions.RemoveEmptyEntries).Select(x=>x.Trim()).ToArray();
                if (data.Length < 8)
                {
                    throw new NotImplementedException();
                }

                if (!series.ContainsKey(data[1]))
                    series[data[1]] = new SerieObservations();

                series[data[1]].Add(new SerieObservation(data[1], data[0], "M13", data[7]));
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
