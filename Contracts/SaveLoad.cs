using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;
using Newtonsoft.Json;

namespace Contracts
{
    public class SaveLoad
    {
        public static string FileName(bool isEu)
        {
            return isEu ? "inflationDataEU.txt" : "inflationData.txt";
        }

        public static string WeightFileName(bool isEu)
        {
            return isEu ? "customWeightsEU.txt" : "customWeights.txt";
        }

        public static void SaveBackfilledData(List<Category> categories, string path = "inflationData.txt")
        {
            var serializedData = JsonConvert.SerializeObject(categories);
            File.WriteAllText(path, serializedData);
        }

        public static void SaveBackfilledData(List<Category> categories, bool isEU)
        {
            var serializedData = JsonConvert.SerializeObject(categories);
            File.WriteAllText(FileName(isEU), serializedData);
        }

        public static TreeNode<Category> LoadBackfilledData(bool isEU = false)
        {
            var cats =  JsonConvert.DeserializeObject<List<Category>>(
                File.ReadAllText(FileName(isEU)));
            return new ParseCategoryUS(cats).root;
        }


        public static List<Category>  DownloadOriginalData(bool isEU = false)
        {
            WebClient w = new WebClient();
            var originalData = w.DownloadString(
                "https://raw.githubusercontent.com/n1b0rh00d/InflationCalculator/master/"+ FileName(isEU));
            var cats = JsonConvert.DeserializeObject<List<Category>>(originalData);
            SaveBackfilledData(cats, isEU);
            return cats;
        }

        public static TreeNode<Category> LoadOrDownload(bool isEU = false)
        {
            if (File.Exists(FileName(isEU)))
            {
                return LoadBackfilledData(isEU);
            }
            else
            {
                return new ParseCategoryUS(DownloadOriginalData(isEU)).root;
            }
        }

        public static void DeleteData(bool isEU = false)
        {
            File.Delete(FileName(isEU));
            File.Delete(WeightFileName(isEU));
        }

        // need something to get latest data
        //https://download.bls.gov/pub/time.series/cu/cu.data.0.Current

        public static TreeNode<Category> OverrideWeights(TreeNode<Category> currentData, bool IsEu)
        {
            var serializedData = File.ReadAllText(WeightFileName(IsEu));
            var data = JsonConvert.DeserializeObject<List<SlimCategory>>(serializedData);
            var fazstLookup = new Dictionary<string, SlimCategory>();
            foreach (var c in data)
            {
                fazstLookup.Add(c._serieCodeId, c);
            }
            foreach (var cat in currentData.Flatten())
            {
                cat.OverrideWithSlimCategories(fazstLookup[cat._serieCodeId]);
            }
            return currentData;
        }

        public static List<SlimCategory> ExtractWeights(TreeNode<Category> modifiedData)
        {
            var l = new List<SlimCategory>();
            foreach (var cat in modifiedData.Flatten())
            {
                l.Add(new SlimCategory(cat));
            }
            return l;
        }

        public static void SaveWeights(TreeNode<Category> modifiedData, bool IsEu)
        {
            var weights = ExtractWeights(modifiedData); 
            var serializedData = JsonConvert.SerializeObject(weights);
            File.WriteAllText(WeightFileName(IsEu), serializedData);
        }

        public void DeleteWeights()
        {

        }

    }
}
