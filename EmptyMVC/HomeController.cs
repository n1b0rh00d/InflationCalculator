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
        public IActionResult Index()
        {
            var readObs = new DictionaryObservations();

            var readCat = new ParseCategory();
            var rootLevel = readCat.root;
            
            foreach (var branch in rootLevel.Flatten())
            {
                branch.SetObservation(readObs.series[branch._serieCodeId]);
            }
           
            //rootLevel.BackFillMissingYearlyData();
            return View(rootLevel);
        }
    }
}
