using System.Collections.Generic;
using System.Linq;
using Contracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

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
        
        [TestMethod]
        public void TestParseAndListNodes()
        {
            var readObs = new DictionaryObservations();

            var readCat = new ParseCategory();
            var rootLevel = readCat.root;

            var allNodes = rootLevel.ListAll();

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
        public void TestUpdateWeights()
        {
            var readObs = new DictionaryObservations();

            var readCat = new ParseCategory();
            var rootLevel = readCat.root;

            var listall = rootLevel.ListAll();
            foreach (var branch in listall)
            {

                branch.Value.SetObservation(readObs.series[branch.Value._serieCodeId]);

            }

            
            rootLevel.Children[0].Children[0].UpdateWeightUpAndDown(30, true);
            rootLevel.Children[0].Children[1].UpdateWeightUpAndDown(5, true);

            var totalWeightAfterUpdate = rootLevel.GetTotalWeight();
            rootLevel.NormalizeWeights();
            totalWeightAfterUpdate = rootLevel.GetTotalWeight();
            rootLevel.UpdateWeightUpAndDown(100);
            totalWeightAfterUpdate = rootLevel.GetTotalWeight();

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

            rootLevel.Children[0].Children[0].UpdateWeightUpAndDown(30, true);
            rootLevel.Children[0].Children[1].UpdateWeightUpAndDown(5, true);

            var totalWeightAfterUpdate = rootLevel.GetTotalWeight();
            
            var result = rootLevel.CalculateInflationRecursivelyFromProvidedLevels2();
        }

        [TestMethod]
        public void TestCaculateYearlyInflationAllItems()
        {
            var readObs = new DictionaryObservations();

            var readCat = new ParseCategory();
            var rootLevel = readCat.root;

            foreach (var branch in rootLevel.Flatten())
            {

                branch.SetObservation(readObs.series[branch._serieCodeId]);

            }

            var result = rootLevel.CalculateInflationRecursivelyFromProvidedLevels2();
        }

        [TestMethod]

        public void TestCaculateInflationAfterNormalize()
        {
            var readObs = new DictionaryObservations();

            var readCat = new ParseCategory();
            var rootLevel = readCat.root;

            foreach (var branch in rootLevel.Flatten())
            {

                branch.SetObservation(readObs.series[branch._serieCodeId]);

            }


            rootLevel.BackPopulateNodesRecursive();

            rootLevel.Children[0].Children[0].UpdateWeightUpAndDown(30, true);
            rootLevel.Children[0].Children[1].UpdateWeightUpAndDown(5, true);

            var totalWeightAfterUpdate = rootLevel.GetTotalWeight();
            rootLevel.NormalizeWeights();
            rootLevel.UpdateWeightUpAndDown(100);
            totalWeightAfterUpdate = rootLevel.GetTotalWeight();

            var result = rootLevel.CalculateInflationRecursivelyFromProvidedLevels2();

            //celcius referral code: 18983601a8
        }

        [TestMethod]
        public void UpdateData()
        {
            var readObs = new DictionaryObservations();

            var readCat = new ParseCategory();
            var rootLevel = readCat.root;

            foreach (var branch in rootLevel.Flatten())
            {

                branch.SetObservation(readObs.series[branch._serieCodeId]);

            }


            //rootLevel.BackPopulateNodesRecursive();
            rootLevel.NormalizeWeights();
            rootLevel.UpdateWeightUpAndDown(100);

            

            SaveLoad.SaveBackfilledData(rootLevel.Flatten().ToList());
            var res = SaveLoad.LoadBackfilledData();

        }
    }
}
