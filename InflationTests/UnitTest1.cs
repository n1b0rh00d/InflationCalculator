using System;
using System.Collections.Generic;
using System.Diagnostics;
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

            rootLevel.BackPopulateNodesRecursive();
            rootLevel.NormalizeWeights();
            rootLevel.UpdateWeightUpAndDown(100);

            

            SaveLoad.SaveBackfilledData(rootLevel.Flatten().ToList(), "../../../../inflationData.txt");

            //upload new data
        }

        [TestMethod]
        public void RoundData()
        {
            var s = @"root.UpdateWeightUpAndDown(2895.5471227254042630742641281,true);
root.Children[0].UpdateWeightUpAndDown(495.61895630137209041483613413,true);
root.Children[0].Children[0].UpdateWeightUpAndDown(395.61895630137209041483613413,true);
root.Children[0].Children[0].Children[0].Children[0].Children[0].UpdateWeightUpAndDown(1.4229067311750201447609504990,true);
root.Children[0].Children[0].Children[0].Children[0].Children[1].UpdateWeightUpAndDown(10,true);
root.Children[0].Children[0].Children[0].Children[0].Children[2].UpdateWeightUpAndDown(6.0842311742906363765615762130,true);
root.Children[0].Children[0].Children[0].Children[0].Children[2].Children[0].UpdateWeightUpAndDown(4.2687201935250604342828514910,true);
root.Children[0].Children[0].Children[0].Children[1].Children[0].UpdateWeightUpAndDown(0,true);
root.Children[0].Children[0].Children[0].Children[1].Children[1].UpdateWeightUpAndDown(0,true);
root.Children[0].Children[0].Children[0].Children[1].Children[2].UpdateWeightUpAndDown(20.0,true);
root.Children[0].Children[0].Children[0].Children[1].Children[2].Children[0].UpdateWeightUpAndDown(10,true);
root.Children[0].Children[0].Children[0].Children[1].Children[2].Children[1].UpdateWeightUpAndDown(0,true);
root.Children[0].Children[0].Children[0].Children[1].Children[3].UpdateWeightUpAndDown(12.887539802391036769046794350,true);
root.Children[0].Children[0].Children[0].Children[1].Children[3].Children[1].UpdateWeightUpAndDown(12.887539802391036769046794350,true);
root.Children[0].Children[0].Children[1].Children[0].Children[0].Children[0].Children[0].UpdateWeightUpAndDown(4.2687201935250604342828514940,true);
root.Children[0].Children[0].Children[1].Children[0].Children[0].Children[0].Children[1].UpdateWeightUpAndDown(8.537440387050120868565702988,true);
root.Children[0].Children[0].Children[1].Children[0].Children[0].Children[0].Children[2].UpdateWeightUpAndDown(0,true);
root.Children[0].Children[0].Children[1].Children[0].Children[0].Children[0].Children[3].UpdateWeightUpAndDown(2.8458134623500402895219009950,true);
root.Children[0].Children[0].Children[1].Children[0].Children[0].Children[1].Children[0].UpdateWeightUpAndDown(8.591693201594024512697862900,true);
root.Children[0].Children[0].Children[1].Children[0].Children[0].Children[1].Children[0].Children[1].UpdateWeightUpAndDown(8.591693201594024512697862900,true);
root.Children[0].Children[0].Children[1].Children[0].Children[0].Children[1].Children[1].UpdateWeightUpAndDown(2.8458134623500402895219009950,true);
root.Children[0].Children[0].Children[1].Children[0].Children[0].Children[1].Children[2].UpdateWeightUpAndDown(5.6916269247000805790438019930,true);
root.Children[0].Children[0].Children[1].Children[0].Children[0].Children[1].Children[3].UpdateWeightUpAndDown(2.8458134623500402895219009950,true);
root.Children[0].Children[0].Children[1].Children[0].Children[0].Children[2].UpdateWeightUpAndDown(20,true);
root.Children[0].Children[0].Children[1].Children[0].Children[1].UpdateWeightUpAndDown(11.471967473970792886902822612,true);
root.Children[0].Children[0].Children[1].Children[0].Children[1].Children[0].UpdateWeightUpAndDown(10.814091156930153100183223784,true);
root.Children[0].Children[0].Children[1].Children[0].Children[1].Children[1].UpdateWeightUpAndDown(0.6578763170406397867195988310,true);
root.Children[0].Children[0].Children[1].Children[0].Children[2].UpdateWeightUpAndDown(17.806160580575181302848554500,true);
root.Children[0].Children[0].Children[1].Children[0].Children[2].Children[0].UpdateWeightUpAndDown(12.806160580575181302848554479,true);
root.Children[0].Children[0].Children[1].Children[0].Children[2].Children[1].UpdateWeightUpAndDown(5,true);
root.Children[0].Children[0].Children[1].Children[0].Children[3].UpdateWeightUpAndDown(3.4149761548200483474262811940,true);
root.Children[0].Children[0].Children[2].Children[0].UpdateWeightUpAndDown(3.4149761548200483474262811940,true);
root.Children[0].Children[0].Children[2].Children[0].Children[0].UpdateWeightUpAndDown(3.2216756177547625919115860340,true);
root.Children[0].Children[0].Children[2].Children[0].Children[1].UpdateWeightUpAndDown(0,true);
root.Children[0].Children[0].Children[2].Children[1].UpdateWeightUpAndDown(14.229067311750201447609504981,true);
root.Children[0].Children[0].Children[2].Children[2].UpdateWeightUpAndDown(10,true);
root.Children[0].Children[0].Children[2].Children[3].UpdateWeightUpAndDown(0,true);
root.Children[0].Children[0].Children[3].UpdateWeightUpAndDown(10,true);
root.Children[0].Children[0].Children[3].Children[0].Children[2].UpdateWeightUpAndDown(10,true);
root.Children[0].Children[0].Children[3].Children[0].Children[2].Children[1].UpdateWeightUpAndDown(10,true);
root.Children[0].Children[0].Children[3].Children[0].Children[2].Children[2].Children[0].UpdateWeightUpAndDown(0,true);
root.Children[0].Children[0].Children[4].UpdateWeightUpAndDown(42.958466007970122563489314500,true);
root.Children[0].Children[0].Children[4].Children[0].UpdateWeightUpAndDown(17.183386403188049025395725800,true);
root.Children[0].Children[0].Children[4].Children[1].Children[0].UpdateWeightUpAndDown(25.775079604782073538093588700,true);
root.Children[0].Children[0].Children[5].Children[0].Children[0].UpdateWeightUpAndDown(10,true);
root.Children[0].Children[0].Children[5].Children[0].Children[1].UpdateWeightUpAndDown(10,true);
root.Children[0].Children[0].Children[5].Children[0].Children[2].UpdateWeightUpAndDown(0,true);
root.Children[0].Children[0].Children[5].Children[1].Children[0].Children[0].UpdateWeightUpAndDown(4.2687201935250604342828514910,true);
root.Children[0].Children[0].Children[5].Children[1].Children[1].UpdateWeightUpAndDown(5,true);
root.Children[0].Children[0].Children[5].Children[1].Children[2].UpdateWeightUpAndDown(3,true);
root.Children[0].Children[0].Children[5].Children[2].UpdateWeightUpAndDown(143,true);
root.Children[0].Children[0].Children[5].Children[2].Children[0].UpdateWeightUpAndDown(1,true);
root.Children[0].Children[0].Children[5].Children[2].Children[1].UpdateWeightUpAndDown(100,true);
root.Children[0].Children[0].Children[5].Children[2].Children[2].UpdateWeightUpAndDown(30,true);
root.Children[0].Children[0].Children[5].Children[2].Children[3].UpdateWeightUpAndDown(1,true);
root.Children[0].Children[0].Children[5].Children[2].Children[4].UpdateWeightUpAndDown(1,true);
root.Children[0].Children[0].Children[5].Children[2].Children[5].UpdateWeightUpAndDown(10,true);
root.Children[0].Children[1].UpdateWeightUpAndDown(100,true);
root.Children[0].Children[1].Children[0].UpdateWeightUpAndDown(95.58823529411764705882352942,true);
root.Children[0].Children[1].Children[3].UpdateWeightUpAndDown(4.4117647058823529411764705884,true);
root.Children[1].Children[0].Children[0].Children[0].UpdateWeightUpAndDown(0,true);
root.Children[1].Children[0].Children[0].Children[1].UpdateWeightUpAndDown(0,true);
root.Children[1].Children[0].Children[1].Children[0].UpdateWeightUpAndDown(59.313809080785595760236257275,true);
root.Children[1].Children[0].Children[1].Children[0].Children[0].UpdateWeightUpAndDown(59.313809080785595760236257275,true);
root.Children[1].Children[0].Children[1].Children[1].UpdateWeightUpAndDown(0,true);
root.Children[1].Children[1].Children[0].UpdateWeightUpAndDown(55.037665864223749076229147077,true);
root.Children[1].Children[1].Children[1].UpdateWeightUpAndDown(0,true);
root.Children[2].UpdateWeightUpAndDown(2285.5766914790228278229625943,true);
root.Children[2].Children[0].Children[0].UpdateWeightUpAndDown(85.91693201594024512697862900,true);
root.Children[2].Children[0].Children[0].Children[1].Children[1].UpdateWeightUpAndDown(17.143870708639100201674455684,true);
root.Children[2].Children[0].Children[0].Children[4].UpdateWeightUpAndDown(8.571935354319550100837227839,true);
root.Children[2].Children[0].Children[0].Children[5].Children[0].UpdateWeightUpAndDown(25.715806062958650302511683523,true);
root.Children[2].Children[0].Children[0].Children[5].Children[1].UpdateWeightUpAndDown(34.287741417278200403348911365,true);
root.Children[2].Children[0].Children[1].UpdateWeightUpAndDown(85.91693201594024512697862900,true);
root.Children[2].Children[0].Children[2].Children[0].UpdateWeightUpAndDown(171.83386403188049025395725799,true);
root.Children[2].Children[0].Children[2].Children[1].UpdateWeightUpAndDown(0,true);
root.Children[2].Children[0].Children[2].Children[2].UpdateWeightUpAndDown(17.183386403188049025395725799,true);
root.Children[2].Children[0].Children[2].Children[2].Children[1].UpdateWeightUpAndDown(17.177837476466742279048881900,true);
root.Children[2].Children[0].Children[3].UpdateWeightUpAndDown(25,true);
root.Children[2].Children[0].Children[3].Children[0].Children[0].UpdateWeightUpAndDown(24.972923741120179980855089754,true);
root.Children[2].Children[0].Children[4].Children[0].Children[0].UpdateWeightUpAndDown(0,true);
root.Children[2].Children[0].Children[4].Children[0].Children[1].UpdateWeightUpAndDown(7.8625236948891070108898781537,true);
root.Children[2].Children[0].Children[4].Children[0].Children[2].UpdateWeightUpAndDown(0,true);
root.Children[2].Children[0].Children[4].Children[0].Children[3].UpdateWeightUpAndDown(0,true);
root.Children[2].Children[0].Children[4].Children[1].UpdateWeightUpAndDown(0,true);
root.Children[2].Children[0].Children[4].Children[2].UpdateWeightUpAndDown(0,true);
root.Children[2].Children[0].Children[4].Children[3].UpdateWeightUpAndDown(0,true);
root.Children[2].Children[0].Children[4].Children[4].UpdateWeightUpAndDown(10,true);
root.Children[2].Children[0].Children[4].Children[5].UpdateWeightUpAndDown(0,true);
root.Children[2].Children[0].Children[4].Children[5].Children[0].UpdateWeightUpAndDown(0,true);
root.Children[2].Children[0].Children[5].UpdateWeightUpAndDown(74.615384615384615384615384616,true);
root.Children[2].Children[0].Children[5].Children[1].Children[0].UpdateWeightUpAndDown(9.230769230769230769230769230,true);
root.Children[2].Children[0].Children[5].Children[1].Children[2].UpdateWeightUpAndDown(50,true);
root.Children[2].Children[0].Children[6].UpdateWeightUpAndDown(199.99999999999999999999999999,true);
root.Children[2].Children[0].Children[6].Children[0].UpdateWeightUpAndDown(150,true);
root.Children[2].Children[0].Children[6].Children[1].UpdateWeightUpAndDown(50,true);
root.Children[2].Children[0].Children[7].UpdateWeightUpAndDown(62.958466007970122563489314499,true);
root.Children[2].Children[0].Children[7].Children[0].UpdateWeightUpAndDown(20,true);
root.Children[2].Children[0].Children[7].Children[1].UpdateWeightUpAndDown(42.958466007970122563489314499,true);
root.Children[2].Children[1].Children[0].Children[0].UpdateWeightUpAndDown(1100,true);
root.Children[2].Children[1].Children[0].Children[0].Children[0].UpdateWeightUpAndDown(1089.1684162608650320229607047,true);
root.Children[2].Children[1].Children[0].Children[0].Children[1].UpdateWeightUpAndDown(10.828354239877209069311098463,true);
root.Children[2].Children[1].Children[0].Children[0].Children[2].UpdateWeightUpAndDown(0,true);
root.Children[2].Children[1].Children[0].Children[0].Children[2].Children[0].UpdateWeightUpAndDown(0,true);
root.Children[2].Children[1].Children[1].UpdateWeightUpAndDown(78.625236948891070108898781537,true);
root.Children[2].Children[1].Children[2].UpdateWeightUpAndDown(28.920089082737716342449539959,true);
root.Children[2].Children[1].Children[2].Children[2].UpdateWeightUpAndDown(3.1450094779556428043559512611,true);
root.Children[2].Children[1].Children[2].Children[3].UpdateWeightUpAndDown(25.775079604782073538093588701,true);
root.Children[2].Children[1].Children[3].UpdateWeightUpAndDown(78.956337627688080915652920270,true);
root.Children[2].Children[1].Children[3].Children[0].Children[0].UpdateWeightUpAndDown(20,true);
root.Children[2].Children[1].Children[3].Children[1].UpdateWeightUpAndDown(10,true);
root.Children[2].Children[1].Children[3].Children[2].UpdateWeightUpAndDown(0,true);
root.Children[2].Children[1].Children[3].Children[3].UpdateWeightUpAndDown(0,true);
root.Children[2].Children[1].Children[3].Children[4].UpdateWeightUpAndDown(39.312618474445535054449390768,true);
root.Children[2].Children[1].Children[4].UpdateWeightUpAndDown(76.881483621561269108877796698,true);
root.Children[2].Children[1].Children[4].Children[3].UpdateWeightUpAndDown(47.175142169334642065339268920,true);
root.Children[2].Children[1].Children[4].Children[4].Children[1].Children[0].UpdateWeightUpAndDown(3.9312618474445535054449390771,true);
root.Children[2].Children[1].Children[4].Children[5].Children[0].UpdateWeightUpAndDown(25.775079604782073538093588701,true);
root.Children[2].Children[1].Children[5].UpdateWeightUpAndDown(89.48535681058919682017719296,true);
root.Children[2].Children[1].Children[5].Children[0].Children[0].UpdateWeightUpAndDown(44.485356810589196820177192960,true);
root.Children[2].Children[1].Children[5].Children[3].Children[0].UpdateWeightUpAndDown(0,true);
root.Children[2].Children[1].Children[5].Children[3].Children[1].Children[0].UpdateWeightUpAndDown(15,true);
root.Children[2].Children[1].Children[5].Children[3].Children[1].Children[1].UpdateWeightUpAndDown(30,true);
root.Children[2].Children[1].Children[6].UpdateWeightUpAndDown(86.70318438542915582806761681,true);
root.Children[2].Children[1].Children[6].Children[1].Children[0].UpdateWeightUpAndDown(0.7862523694889107010889878160,true);
root.Children[2].Children[1].Children[6].Children[2].UpdateWeightUpAndDown(42.958466007970122563489314500,true);
root.Children[2].Children[1].Children[6].Children[3].UpdateWeightUpAndDown(42.958466007970122563489314500,true);
root.Children[2].Children[1].Children[7].UpdateWeightUpAndDown(4.7175142169334642065339268902,true);
root.Children[2].Children[1].Children[7].Children[1].Children[3].UpdateWeightUpAndDown(4.7175142169334642065339268902,true);";
            var lines = s.Split(System.Environment.NewLine);
            var ba0 = "";
            foreach (var l in lines)
            {
                var li = l.Split('(');
                var lin = li[1].Split(',');

                var num = Math.Round(Double.Parse(lin[0]),2);

                if (num == 0)
                {
                    ba0 += li[0] + "(" + num + "m," + lin[1] + System.Environment.NewLine;
                }
                else
                {
                    Debug.WriteLine(li[0] + "(" + num + "m," + lin[1]);
                }

            }
            Debug.WriteLine(ba0);

        }
    }
}
