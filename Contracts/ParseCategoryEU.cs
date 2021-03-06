using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Contracts
{
    public class ParseCategoryEU
    {
        private char[] delimiter = new char[] { ',' };

        //The top-level Node
        public TreeNode<Category> root;

        //Temp value for processing in parseTextIntoTree
        private TreeNode<Category> ParentNode;

        //Temp value for processing in parseTextIntoTree
        private int actualdepth = -1;

        //The depth step between two layers
        private int depthdifference = 1;

        public ParseCategoryEU()
        {
            buildDirectoryFromFile("C:\\Users\\rchapas\\source\\repos\\InflationCalculator\\prc_hicp_inw_1_Data.csv");
        }

        public ParseCategoryEU(List<Category> cats)
        {
            buildDirectoryFromListOfCat(cats);
        }

        public void buildDirectoryFromFile(string fileLocation)
        {
            foreach (var line in File.ReadLines(fileLocation))
            {
                var data = line.Replace("\"", "").Split(delimiter).Select(x => x.Trim()).ToArray();
                if (data.Length < 5)
                {
                    throw new NotImplementedException();
                }
                if (data[4].Equals("0"))
                {
                    continue;
                }
                //country, serie code, name, year, value, footnote
                if (data[1].Contains("_") || data[1].Contains("-"))
                {
                    continue;
                }
                 
                if ("CP00".Equals(data[1]))
                {
                    var newCat = new Category(data[2], data[4], data[1].Length - 4, data[1]);
                    parseTextIntoTree(newCat, newCat._depth);
                }
                else if(data[1].Equals("CP0820") || data[1].Equals("CP0830"))
                {
                    var newCat = new Category(data[2], data[4], 2, data[1]);
                    parseTextIntoTree(newCat, newCat._depth);
                }
                else
                {
                    if (data[1][data[1].Length - 2] == '0' && !(data[1][data[1].Length - 3] == 'P' || data[1][data[1].Length - 3] == '1' && data[1][data[1].Length - 4] == 'P'))
                    {
                         var newCat = new Category(data[2], data[4], data[1].Length - 4, data[1]);
                        parseTextIntoTree(newCat, newCat._depth);
                    }
                    else
                    {
                        var newCat = new Category(data[2], data[4], data[1].Length - 3, data[1]);
                        parseTextIntoTree(newCat, newCat._depth);
                    }
                }
            }
        }

        public void buildDirectoryFromListOfCat(List<Category> categories)
        {
            foreach (var cat in categories)
            {
                parseTextIntoTree(cat, cat._depth);
            }
        }

        public void parseTextIntoTree(Category cat, int depth)
        {
            //At the beginning define the root node
            if (root == null)
            {
                root = new TreeNode<Category>(cat);
                ParentNode = root;
                actualdepth = depth;
            }
            else
            {
                if (cat._serieCodeId.Equals("CP03"))
                {
                    var a = "";
                }
                //Search the parent node for the current depth
                while (depth <= actualdepth)
                {
                    //One step down
                    ParentNode = ParentNode.Parent;
                    actualdepth -= depthdifference;
                }

                ParentNode = ParentNode.AddChild(cat);
                actualdepth = depth;
            }
        }
    }
}