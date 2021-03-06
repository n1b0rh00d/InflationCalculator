using System;
using System.Collections.Generic;
using System.Linq;

namespace Contracts
{
    public class Category
    {
        public string _name; 
        public string _serieCodeId;
        public decimal _weight;
        public int _depth;
        public SerieObservations observations;
        public bool _userAdjustedWeight = false;

        public Category()
        {

        }

        public Category(string name, string weight, string depth, string serieCodeId)
        {
            _name = name;
            _serieCodeId = serieCodeId;
            decimal extratedWeight;
            var isValid = decimal.TryParse(weight, out extratedWeight);
            _weight = isValid? extratedWeight:0;
            _depth = int.Parse(depth);
        }

        public Category(string name, string weight, int depth, string serieCodeId)
        {
            _name = name;
            _serieCodeId = serieCodeId;
            decimal extratedWeight;
            var isValid = decimal.TryParse(weight.Replace(" ",""), out extratedWeight);
            _weight = isValid ? extratedWeight : 0;
            _depth = depth;
        }
        

        //Override the weights
        public void OverrideWithSlimCategories(SlimCategory c)
        {
            _serieCodeId = c._serieCodeId;
            _weight = c._weight;
            _userAdjustedWeight = c._userAdjustedWeight;
        }

        public void SetObservation(SerieObservations obs)
        {
            observations = obs;
        }

        public bool HasWeight()
        {
            return _weight > 0;
        }

        public bool HasUserSetWeight => _userAdjustedWeight;
        public decimal GetWeight => _weight;

        public void UpdateWeight(decimal newWeight, bool IsFromUser = false)
        {
            _weight = newWeight;
            _userAdjustedWeight = _userAdjustedWeight || IsFromUser;
        }
    }
}
