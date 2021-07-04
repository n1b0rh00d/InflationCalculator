using Contracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace InflationTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestSerieParsing()
        {
            var readFile = new DictionaryObservations();
        } 
        
        [TestMethod]
        public void TestWeightParsing()
        {
            var readFile = new ParseCategory();
            var rootLevel = readFile.root;

        }

        [TestMethod]
        public void TestParseAndPopulate()
        {
            var readObs = new DictionaryObservations();

            var readCat = new ParseCategory();
            var rootLevel = readCat.root;

            foreach (var branch in rootLevel.Flatten())
            {
                branch.SetObservation(readObs.series[branch._serieCodeId]);
            }

            TreeNode<Category>.PrintNodesRecursive(rootLevel);

        }

        // we should backpopulate childrens series that are missing values based on parent inflation and weights of categories.
        // if we have the other children data, in theory we could extrapolate.

        [TestMethod]
        public void TestBackPopulate()
        {
            var readObs = new DictionaryObservations();

            var readCat = new ParseCategory();
            var rootLevel = readCat.root;

            foreach (var branch in rootLevel.Flatten())
            {
                
                    branch.SetObservation(readObs.series[branch._serieCodeId]);

            }

            rootLevel.BackPopulateNodesRecursive();
        } 
        
        [TestMethod]
        public void TestCaculateInflationFromTop()
        {
            var readObs = new DictionaryObservations();

            var readCat = new ParseCategory();
            var rootLevel = readCat.root;

            foreach (var branch in rootLevel.Flatten())
            {
                
                    branch.SetObservation(readObs.series[branch._serieCodeId]);

            }


            rootLevel.BackPopulateNodesRecursive();

            rootLevel.Children[0].Children[0].UpdateWeight(30, true);
            rootLevel.Children[0].Children[1].UpdateWeight(5, true);

            var totalWeightAfterUpdate = rootLevel.GetTotalWeight();

            var result = rootLevel.CalculateInflationRecursivelyFromProvidedLevels2();
        }





    }
}
