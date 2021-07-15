using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;

namespace Contracts
{
    public class TreeNode<TCategory> where TCategory : Category
    {
        private readonly TCategory _value;
        private readonly List<TreeNode<TCategory>> _children = new List<TreeNode<TCategory>>();

        public TreeNode(TCategory value)
        {
            _value = value;
        }

        public TreeNode<TCategory> this[int i]
        {
            get { return _children[i]; }
        }

        public string TabbedView(int depth, string text)
        {
            var tabbedText = "";
            for (int i = 0; i < depth; i++)
            {
                tabbedText += "  ";
            }

            return tabbedText + text;
        }

        public string Name  => TabbedView(Value._depth , Value._name);
        
        public decimal Weight
        {
            get { return Value.GetWeight;}
            set { this.UpdateWeightUpAndDown(value,true); }
        } 

        public TreeNode<TCategory> Parent { get; private set; }

        public TCategory Value { get { return _value; } }

        public ReadOnlyCollection<TreeNode<TCategory>> Children
        {
            get { return _children.AsReadOnly(); }
        }

        public double AverageInflation
        {
            get
            {
                return (double)this.Value.observations.AnnualObservations
                    .Select(x => x.Value._percentChange)
                    .Average();
            }
        }

        public double ValueOfADollar
        {
            get
            {
                return 1/Math.Pow(1+(double) this.Value.observations.AnnualObservations
                    .Select(x => x.Value._percentChange).Average()/100, this.Value.observations.AnnualObservations.Count);
            }
        }

        public TreeNode<TCategory> AddChild(TCategory value)
        {
            var node = new TreeNode<TCategory>(value) { Parent = this };
            _children.Add(node);
            return node;
        }

        public TreeNode<TCategory>[] AddChildren(params TCategory[] values)
        {
            return values.Select(AddChild).ToArray();
        }

        public bool RemoveChild(TreeNode<TCategory> node)
        {
            return _children.Remove(node);
        }


        public static void PrintNodesRecursive(TreeNode<TCategory> oParentNode)
        {
            var getWeight = oParentNode.GetTotalWeight();

            if (getWeight != oParentNode.Value.GetWeight)
            {
                var lineTOprint = "Diff: "  + (oParentNode.Value.GetWeight - getWeight) + " " +oParentNode.Value._name + " SumChildCatIs " + getWeight + " Announced " + oParentNode.Value.GetWeight;
                Debug.WriteLine(lineTOprint);
            }
            
            // Start recursion on all subnodes.
            foreach (TreeNode<TCategory> oSubNode in oParentNode._children)
            {
                PrintNodesRecursive(oSubNode);
            }

        }

        public void BackPopulateNodesRecursive()
        {
            this.BackFillMissingYearlyData();
            this.BackFillMissingMonthlyData();

            // Start recursion on all subnodes.
            foreach (TreeNode<TCategory> oSubNode in this._children)
            {
                
                    oSubNode.BackPopulateNodesRecursive();
            }
        }

        public SerieObservations CalculateInflationRecursivelyFromProvidedLevels()
        {
            // If no adjustement of weight was provided at a lower level, use the parent level to calculate inflation (ie if nothing is provided, the "all item" would be used
            var result = new SerieObservations(); //after backfill all data should be populated and equal
            decimal totalWeights = 0;
            if (this.HasAChildWithUserSetWeight())
            {
                // Start recursion on all subnodes.
                foreach (TreeNode<TCategory> oSubNode in this._children)
                {
                    //TODO: if weight is 0 we can skip
                    result = SerieObservations.SumObservations(result, oSubNode.CalculateInflationRecursivelyFromProvidedLevels());
                    totalWeights += oSubNode.Value.GetWeight;
                }
            }
            else
            {
                // return the serie of observation * the ponderation
                Debug.WriteLine($"WeightUsed: {Value.GetWeight} for {Value._name} depth: {Value._depth}");
                return SerieObservations.WeightedObservationsPercentages(Value.observations, Value.GetWeight);
            }
            Debug.WriteLine($"TotalWeights: {totalWeights}");

            return result;
        }

        public (decimal,SerieObservations) CalculateInflationRecursivelyFromProvidedLevels2()
        {
            // If no adjustement of weight was provided at a lower level, use the parent level to calculate inflation (ie if nothing is provided, the "all item" would be used
            var result = new SerieObservations(); //after backfill all data should be populated and equal
            decimal totalWeights = 0;
            if (this.HasAChildWithUserSetWeight())
            {
                // Start recursion on all subnodes.
                foreach (TreeNode<TCategory> oSubNode in this._children)
                {
                    var bla = oSubNode.CalculateInflationRecursivelyFromProvidedLevels2();
                    result = SerieObservations.SumObservations(result, bla.Item2);
                    totalWeights += bla.Item1;
                }
            }
            else
            {
                // return the serie of observation * the ponderation
                Debug.WriteLine($"WeightUsed: {Value.GetWeight} for {Value._name} depth: {Value._depth}");
                return (Value.GetWeight,SerieObservations.WeightedObservationsPercentages(Value.observations, Value.GetWeight));
            }
            Debug.WriteLine($"TotalWeights: {totalWeights}");

            return (totalWeights, result);
        }


        public bool HasAChildWithUserSetWeight()
        {
            bool hasChildWitUserSetWeight = false;

            if (!this.HasWeightedChild())
            {
                return false;
            }
            else
            {
                foreach (TreeNode<TCategory> oSubNode in this._children)
                {
                    hasChildWitUserSetWeight |= oSubNode.Value.HasUserSetWeight || oSubNode.HasAChildWithUserSetWeight();
                }
            }
            // Optimisation would be to remember the results of each parent category to not have to iterate the tree. this is N2 but small set.
            return hasChildWitUserSetWeight;
        }

        public void Traverse(Action<TCategory> action)
        {
            action(Value);
            foreach (var child in _children)
                child.Traverse(action);
        }

        public IEnumerable<TCategory> Flatten()
        {
            return new[] { Value }.Concat(_children.SelectMany(x => x.Flatten()));
        }

        public IEnumerable<TreeNode<TCategory>> ListAll()
        {
            List<TreeNode<TCategory>> listAll = new List<TreeNode<TCategory>>();
            listAll.Add(this);
            foreach (var child in Children)
            {
                listAll.AddRange(child.ListAll());
            }

            return listAll;
        }

        public bool HasChild()
        {
            return Children != null && Children.Count != 0;
        }

        public bool HasWeightedChild()
        {
            return HasChild() && Children.Any(c => c.Value.HasWeight());
        }

        public bool UpdateWeightUpAndDown(decimal newWeight, bool userSet = false)
        {
            var currentWeightOnStartingNode = Value.GetWeight;
            UpdateWeightUp(newWeight);
            Value.UpdateWeight(currentWeightOnStartingNode); // reset the weight change on starting node to populate down too.
            UpdateWeightDown(newWeight, userSet);
            return true;
        }

        public void UpdateWeightUpAndDown(string newWeight, bool userSet = false)
        {
            if (decimal.TryParse(newWeight, out var parsedWeight))
                UpdateWeightUpAndDown(parsedWeight, userSet);
            else
                throw new Exception("Provided: " + newWeight);
        }

        private void UpdateWeightDown(decimal newWeight, bool userSet = false)
        {
            // the childs will have updated weights but should be user set ?
            if (HasWeightedChild())
            {
                foreach (var child in Children)
                {
                    child.UpdateWeightDown(newWeight / Value.GetWeight * child.Value.GetWeight, false);
                }
            }

            // new weight should be equal to sum of child weights ( but only leaf level series should be used in caclulations)
            Value.UpdateWeight(newWeight, userSet);
        }
        private void UpdateWeightUp(decimal newWeight)
        {
            if (this.Parent != null)
            {
                Parent.UpdateWeightUp(Parent.Value.GetWeight + (newWeight - Value.GetWeight));
            }

            Value.UpdateWeight(this.Value.GetWeight + (newWeight - Value.GetWeight));

        }

        public void NormalizeWeights()
        {
            //make childs equal to parent if child not 0
            var nodeWeight = Value.GetWeight;
            decimal childTotalWeight = 0;
            foreach (var c in Children)
            {
                childTotalWeight += c.Value.GetWeight;
            }
            foreach (var c in Children)
            {
                if (c.Value.GetWeight != 0)
                {
                    c.Value.UpdateWeight(c.Value.GetWeight * nodeWeight / childTotalWeight);
                    c.NormalizeWeights();
                }
            }
        }

        public decimal GetTotalWeight()
        {
            decimal weight = 0;
            if (HasWeightedChild())
            {
                foreach (var child in Children)
                {
                    weight += child.GetTotalWeight();
                }
            }
            else
            {
                return Value.GetWeight;
            }

            return weight;
        }

        public void BackFillMissingYearlyData()
        {
            var needBackfill = false;
            decimal SumExistingPonderationTimesValues = 0;
            decimal SumMissingPonderations = 0;
            foreach (var obs in Value.observations.AnnualObservations)
            {
                if(this.Name.Contains("Water and sewer and trash collection services"))
                {
                    var a = "";
                }
                foreach (var childnode in _children)
                {
                    if (childnode.Value.observations.AnnualObservations.ContainsKey(obs.Key))
                    {
                        SumExistingPonderationTimesValues +=
                            childnode.Value.GetWeight *
                            childnode.Value.observations.AnnualObservations[obs.Key]._percentChange;
                    }
                    else
                    {
                        SumMissingPonderations += childnode.Value.GetWeight;
                        needBackfill = true;
                    }
                }

                if (needBackfill)
                {
                    if(SumMissingPonderations == 0) SumMissingPonderations += 1;
                    var retro = (Value.GetWeight *
                                Value.observations.AnnualObservations[obs.Key]._percentChange - SumExistingPonderationTimesValues) / SumMissingPonderations ;
                    foreach (var childnode in _children)
                    {

                        if (childnode.Value.observations.AnnualObservations.ContainsKey(obs.Key))
                        {
                           
                        }
                        else
                        {
                            // we are averaging the remaining inflation into the childs
                            childnode.Value.observations.AnnualObservations.Add(obs.Key, new
                                SerieObservation(
                                    childnode.Value._serieCodeId,
                                    obs.Value._year,
                                    obs.Value._month,
                                    retro));

                        }
                        Debug.WriteLine($"Backfilling {childnode.Value._name} on {obs.Key} with {retro} while parent was {Value.observations.AnnualObservations[obs.Key]._percentChange}");

                    }

                    needBackfill = false;
                    SumExistingPonderationTimesValues = 0;
                    SumMissingPonderations = 0;
                }
            }

        }

        public void BackFillMissingMonthlyData()
        {
            var needBackfill = false;
            decimal SumExistingPonderationTimesValues = 0;
            decimal SumMissingPonderations = 0;
            foreach (var obs in Value.observations.MensualObservations)
            {
                foreach (var childnode in _children)
                {
                    if (childnode.Value.observations.MensualObservations.ContainsKey(obs.Key))
                    {
                        SumExistingPonderationTimesValues +=
                            childnode.Value.GetWeight *
                            childnode.Value.observations.MensualObservations[obs.Key]._percentChange;
                    }
                    else
                    {
                        SumMissingPonderations += childnode.Value.GetWeight;
                        needBackfill = true;
                    }
                }

                if (needBackfill)
                {
                    if (SumMissingPonderations == 0) SumMissingPonderations += 1;
                    var retro = (Value.GetWeight *
                                Value.observations.MensualObservations[obs.Key]._percentChange - SumExistingPonderationTimesValues) / SumMissingPonderations;
                    foreach (var childnode in _children)
                    {

                        if (childnode.Value.observations.MensualObservations.ContainsKey(obs.Key))
                        {

                        }
                        else
                        {
                            // we are averaging the remaining inflation into the childs
                            childnode.Value.observations.MensualObservations.Add(obs.Key, new
                                SerieObservation(
                                    childnode.Value._serieCodeId,
                                    obs.Value._year,
                                    obs.Value._month,
                                    retro));

                        }
                        Debug.WriteLine($"Backfilling {childnode.Value._name} on {obs.Key} with {retro} while parent was {Value.observations.MensualObservations[obs.Key]._percentChange}");

                    }

                    needBackfill = false;
                    SumExistingPonderationTimesValues = 0;
                    SumMissingPonderations = 0;
                }
            }

        }
    }
}
