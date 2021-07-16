using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Newtonsoft.Json;

namespace Contracts
{
    public class SaveLoad
    {
        public static void SaveBackfilledData(List<Category> categories)
        {
            var serializedData = JsonConvert.SerializeObject(categories);
            File.WriteAllText("inflationData.txt", serializedData);
        }
        public static TreeNode<Category> LoadBackfilledData()
        {
            var cats =  JsonConvert.DeserializeObject<List<Category>>(
                File.ReadAllText("inflationData.txt"));
            return new ParseCategory(cats).root;
        }

        public static List<Category>  DownloadLatestData()
        {
            return JsonConvert.DeserializeObject<List<Category>>(File.ReadAllText("inflationData.txt"));

        }

    }
}
