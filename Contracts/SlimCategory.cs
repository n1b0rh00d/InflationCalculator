using System;
using System.Collections.Generic;
using System.Linq;

namespace Contracts
{
    public class SlimCategory
    {
       
        public string _serieCodeId;
        public decimal _weight;
        public  bool _userAdjustedWeight = false;

        public SlimCategory()
        {
        }

        public SlimCategory(Category c)
        {
             _serieCodeId = c._serieCodeId;
             _weight = c._weight;
             _userAdjustedWeight = c._userAdjustedWeight;

        }

    }
}
