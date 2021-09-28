using System;
using System.Collections.Generic;
using System.Linq;

namespace Contracts
{
    public class SerieObservationsMonthly
    {
        public Dictionary<string, SerieObservation> Observations;


        public SerieObservationsMonthly()
        {
            Observations = new Dictionary<string, SerieObservation>();
        }

        public void Add(SerieObservation so)
        {
            Observations.Add(so.GetDateKey, so);

        }

        public void SetPercents()
        {
            CalculatePercentChange(Observations.Values.ToList());
        }

        private void CalculatePercentChange(List<SerieObservation> observations)
        {
            if (observations.Count > 0)
            {
                observations[0]._percentChange = 0;
                for (int i = 1; i < observations.Count; i++)
                {
                    if (observations[i - 1]._value == 0)
                    {
                        observations[i]._percentChange = 0;
                    }
                    else
                    {
                        observations[i]._percentChange =
                            100 * (observations[i]._value / observations[i - 1]._value - 1);
                    }
                }
            }
        }

        void Order()
        {
            //data is already ordered
        }

        public static SerieObservationsMonthly SumObservations(
            SerieObservationsMonthly serie1,
            SerieObservationsMonthly serie2)
        {
            SerieObservationsMonthly result = new SerieObservationsMonthly();
            // if serie1 is null or empty return the serie 2;
            if(serie1.Observations.Count() == 0)
            {
                return serie2;
            }
            if (serie1.Observations.Count != serie2.Observations.Count)
            {
                throw new NotImplementedException("BackfillData");
            }

            if (serie1.Observations.Count() != 0)
            {
                foreach (var so in serie1.Observations)
                {
                    result.Add(
                        new SerieObservation(
                            "CalculatedSum",
                            so.Value._year,
                            so.Value._month,
                            so.Value._percentChange +
                            serie2.Observations[so.Key]._percentChange));
                }
            }

            return result;
        }

        public static SerieObservationsMonthly WeightedObservationsPercentages(
            SerieObservationsMonthly serie1, decimal weight)
        {
            SerieObservationsMonthly result = new SerieObservationsMonthly();
            
            foreach (var so in serie1.Observations)
            {
                result.Add(new SerieObservation("WeightedObservations", so.Value._year, so.Value._month, so.Value._percentChange * weight/100));
            }

            return result;
        }
    }
}
