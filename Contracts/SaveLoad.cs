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
            SaveBackfilledData(cats);
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
        }

        // need something to get latest data
        //https://download.bls.gov/pub/time.series/cu/cu.data.0.Current

       

    }
}
