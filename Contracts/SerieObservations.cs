using System;
using System.Collections.Generic;
using System.Linq;

namespace Contracts
{
    public class SerieObservations
    {
        public SerieObservationsAnnual AnnualObservations;
        public SerieObservationsMonthly MensualObservations;

        public SerieObservations()
        {
            AnnualObservations = new SerieObservationsAnnual();
            //MensualObservations = new SerieObservationsMonthly();
        }

        public void Add(SerieObservation so)
        {
            if (so.IsAnnual)
            {
                AnnualObservations.Add(so);
            }
            else
            {

                //MensualObservations.Add(so);
            }
        }

        public void SetPercents()
        {
            AnnualObservations.SetPercents();
            //MensualObservations.SetPercents();
        }

        public static SerieObservations SumObservations(
            SerieObservations serie1,
            SerieObservations serie2)
        {
            SerieObservations result = new SerieObservations();
            result.AnnualObservations = SerieObservationsAnnual.SumObservations(serie1.AnnualObservations, serie2.AnnualObservations);
            //result.MensualObservations = SerieObservationsMonthly.SumObservations(serie1.MensualObservations, serie2.MensualObservations);
            
            return result;
        }

        public static SerieObservations WeightedObservationsPercentages(
            SerieObservations serie1, decimal weight)
        {
            SerieObservations result = new SerieObservations();

            result.AnnualObservations = SerieObservationsAnnual.WeightedObservationsPercentages(serie1.AnnualObservations, weight);
            //result.MensualObservations = SerieObservationsMonthly.WeightedObservationsPercentages(serie1.MensualObservations, weight);

            return result;
        }
    }
}
