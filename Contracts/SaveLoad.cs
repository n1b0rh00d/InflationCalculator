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
        public static void SaveBackfilledData(List<Category> categories, string path = "inflationData.txt")
        {
            var serializedData = JsonConvert.SerializeObject(categories);
            File.WriteAllText(path, serializedData);
        }
        public static TreeNode<Category> LoadBackfilledData()
        {
            var cats =  JsonConvert.DeserializeObject<List<Category>>(
                File.ReadAllText("inflationData.txt"));
            return new ParseCategoryUS(cats).root;
        }

        public static List<Category>  DownloadOriginalData()
        {
            WebClient w = new WebClient();
            var originalData = w.DownloadString(
                "https://raw.githubusercontent.com/n1b0rh00d/InflationCalculator/master/inflationData.txt");
            var cats = JsonConvert.DeserializeObject<List<Category>>(originalData);
            SaveBackfilledData(cats);
            return cats;
        }

        public static TreeNode<Category> LoadOrDownload()
        {
            if (File.Exists("inflationData.txt"))
            {
                return LoadBackfilledData();
            }
            else
            {
                return new ParseCategoryUS(DownloadOriginalData()).root;
            }
        }

        public static void DeleteData()
        {
            File.Delete("inflationData.txt");
        }

        // need something to get latest data
        //https://download.bls.gov/pub/time.series/cu/cu.data.0.Current

       

    }
}
