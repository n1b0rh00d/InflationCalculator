using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contracts;

namespace EmptyMVC
{
    public class HomeController : Controller
    {
        public IActionResult Index(int consumerType, int depth=1)
        {
            return View(new IndexWrapper(consumerType,depth));
        }
    }

    public class IndexWrapper 
    {
        public TreeNode<Category> RootLevel;

        public int Depth;

        public double AvgInflation;

        public double ValueOfDollar;

        public IndexWrapper(int consumerType, int depth = 1)
        {
            RootLevel = SaveLoad.LoadOrDownload();

            var c = (ConsumerType)consumerType;

            AdjustWeightToConsumerType.AdjustWeight(RootLevel, c);

            Depth = depth;
            SerieObservations inflationNumbers = RootLevel.CalculateInflationRecursivelyFromProvidedLevels();
            var annualInflation = inflationNumbers.AnnualObservations;
            //var x = annualInflation.Select(x => new DateTime(year: int.Parse(x.Value._year), month: 1, 1).ToOADate()).ToArray();

            var y = annualInflation.Observations.Select(x => (double)x.Value._percentChange).ToArray();

            AvgInflation = y.Average();

            ValueOfDollar =  1 / Math.Pow(1 + y.Average() / 100, y.Length);
        }


    }
}
