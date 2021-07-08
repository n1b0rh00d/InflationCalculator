using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contracts;

namespace WebAppInflation
{
    public class HomeController : Controller
    {
        // GET: HomeController
        public ActionResult Index()
        {
            var readCat = new ParseCategory();
            var rootLevel = readCat.root;
            var flatWeights = rootLevel.Flatten();
            return View(flatWeights);
        }
    }
}
