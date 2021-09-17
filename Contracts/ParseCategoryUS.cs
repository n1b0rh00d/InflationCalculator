using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Contracts
{
    public class ParseCategoryUS
    {
        private char[] delimiter = new char[] { '\t' };

        //The top-level Node
        public TreeNode<Category> root;

        //Temp value for processing in parseTextIntoTree
        private TreeNode<Category> ParentNode;

        //Temp value for processing in parseTextIntoTree
        private int actualdepth = -1;

        //The depth step between two layers
        private int depthdifference = 1;

        public ParseCategoryUS()
        {
            buildDirectoryFromFile("C:\\Users\\rchapas\\source\\repos\\InflationCalculator\\ItemWeight.txt");
        }

        public ParseCategoryUS(List<Category> cats)
        {
            buildDirectoryFromListOfCat(cats);
        }

        public void buildDirectoryFromFile(string fileLocation)
        {
            foreach (var line in File.ReadLines(fileLocation))
            {
                var data = line.Split(delimiter).Select(x => x.Trim()).ToArray();
                if (data.Length != 4)
                {
                    throw new NotImplementedException();
                }

                var newCat = new Category(data[0], data[1], data[2], data[3]);
                parseTextIntoTree(newCat, newCat._depth);
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