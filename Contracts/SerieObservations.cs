using System;
using System.Collections.Generic;
using System.Linq;

namespace Contracts
{
    public class SerieObservations
    {
        public Dictionary<string, SerieObservation> MensualObservations;
        public Dictionary<string, SerieObservation> AnnualObservations;

        public SerieObservations()
        {
            MensualObservations = new Dictionary<string, SerieObservation>();
            AnnualObservations = new Dictionary<string, SerieObservation>();
        }

        public void Add(SerieObservation so)
        {
            if (so.IsAnnual)
            {
                AnnualObservations.Add(so.GetDateKey ,so);
            }
            else
            {
                MensualObservations.Add(so.GetDateKey, so);
            }
        }

        public void SetPercents()
        {
            CalculatePercentChange(MensualObservations.Values.ToList());
            CalculatePercentChange(AnnualObservations.Values.ToList());
        }

        private void CalculatePercentChange(List<SerieObservation> observations)
        {
            if (observations.Count > 0)
            {
                observations[0]._percentChange = 0;
                for (int i = 1; i < observations.Count; i++)
                {
                    observations[i]._percentChange =
                        100 * (observations[i]._value / observations[i - 1]._value - 1);
                }
            }
        }

        void Order()
        {
            //data is already ordered
        }

        public static SerieObservations SumObservations(
            SerieObservations serie1,
            SerieObservations serie2)
        {
            SerieObservations result = new SerieObservations();
            // if serie1 is null or empty return the serie 2;
            if(serie1.AnnualObservations.Count() == 0 || serie1.MensualObservations.Count() == 0)
            {
                return serie2;
            }
            if (serie1.AnnualObservations.Count != serie2.AnnualObservations.Count || serie1.MensualObservations.Count != serie2.MensualObservations.Count)
            {
                throw new NotImplementedException("BackfillData");
            }

            foreach (var so in serie1.AnnualObservations)
            {
                result.Add(new SerieObservation("CalculatedSum",so.Value._year, so.Value._month, so.Value._percentChange + serie2.AnnualObservations[so.Key]._percentChange));
            }
            foreach (var so in serie1.MensualObservations)
            {
                result.Add(new SerieObservation("CalculatedSum", so.Value._year, so.Value._month, so.Value._percentChange + serie2.MensualObservations[so.Key]._percentChange));
            }

            return result;
        }

        public static SerieObservations WeightedObservationsPercentages(
            SerieObservations serie1, decimal weight)
        {
            SerieObservations result = new SerieObservations();

            foreach (var so in serie1.AnnualObservations)
            {
                result.Add(new SerieObservation("WeightedObservations", so.Value._year, so.Value._month, so.Value._percentChange * weight/100));
            }

            foreach (var so in serie1.MensualObservations)
            {
                result.Add(new SerieObservation("WeightedObservations", so.Value._year, so.Value._month, so.Value._percentChange * weight/100));
            }

            return result;
        }
    }
}
