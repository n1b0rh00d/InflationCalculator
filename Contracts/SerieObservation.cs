using System;
using System.Collections.Generic;

namespace Contracts
{
    public class SerieObservation
    {
        public string _serieId;
        public string _year;
        public string _month;
        public decimal _value;

        public bool IsAnnual => _month.Equals("M13");

        public decimal _percentChange;

        public bool IsBackfilled = false;

        public SerieObservation()
        {

        }

        public SerieObservation(string serieId, string year, string month, string value)
        {
            _serieId = serieId;
            _year = year;
            _month = month;
            if (value == ":")
            {
                _value = 0;
            }
            else
            {
                _value = Decimal.Parse(value.Replace(" ",""));
            }
        }

        public SerieObservation(string serieId, string year, string month, decimal percentChange)
        {
            _serieId = serieId;
            _year = year;
            _month = month;
            _percentChange = percentChange;
            IsBackfilled = true; //keeping track of averaged data which may be inaccurate
        }

        public string GetDateKey => _year + _month;
    }
}
