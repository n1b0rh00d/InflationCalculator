using System;
using System.Collections.Generic;
using System.Text;

namespace Contracts
{
    public static class AdjustWeightToConsumerType
    {
        public static void AdjustWeight(TreeNode<Category> root, ConsumerType c)
        {
            switch (c)
            {
                case ConsumerType.Regular:
                    return;
                case ConsumerType.EssentialRenter:
                    AdjustWeightForEssentialRenter(root);
                    return;
                case ConsumerType.EssentialHomeOwner:
                    AdjustWeightForEssentialHomeOwner(root);
                    return;
                case ConsumerType.Prodige:
                    AdjustWeightForProdige(root);
                    return;
            }
        }

        public static void AdjustWeightForEssentialRenter(TreeNode<Category> root)
        {
            root.UpdateWeightUpAndDown(0, true);
            root[0].Children[0].Children[0].Children[0].UpdateWeightUpAndDown(5,true); // Cereals and cereal products
            root[0].Children[0].Children[1].UpdateWeightUpAndDown(10,true); //      Meats, poultry, fish, and eggs
            root[0].Children[0].Children[2].UpdateWeightUpAndDown(10,true); //            Dairy and related products
            root[0].Children[0].Children[3].UpdateWeightUpAndDown(10,true); //            Fruits and vegetables
            root[0].Children[0].Children[4].UpdateWeightUpAndDown(0,true); //         Nonalcoholic beverages and beverage materials
            root[0].Children[0].Children[5].UpdateWeightUpAndDown(0,true); //               Other food at home
            root[0].Children[0].Children[5].Children[1].Children[1].UpdateWeightUpAndDown(2,true); //              Butter and margarine

            root[1].Children[0].Children[1].Children[0].UpdateWeightUpAndDown(5, true);//          Gasoline
            root[1].Children[1].Children[0].UpdateWeightUpAndDown(20, true);//                Electricity

            //shelter
            root[2].Children[1].Children[0].Children[0].UpdateWeightUpAndDown(50, true); //Rent of shelter
            root.UpdateWeightUpAndDown(100, true);
            root.NormalizeWeights(); 
        }

        public static void AdjustWeightForEssentialHomeOwner(TreeNode<Category> root)
        {
            root.UpdateWeightUpAndDown(0, true);

            root.UpdateWeightUpAndDown(25, true); //food at home
            root.UpdateWeightUpAndDown(25, true); //food at home
            root.UpdateWeightUpAndDown(100, true);

            root.NormalizeWeights();
        }

        public static void AdjustWeightForProdige(TreeNode<Category> root)
        {
            root.UpdateWeightUpAndDown(0, true);

            root.UpdateWeightUpAndDown(25, true); //Eat outside
            root.UpdateWeightUpAndDown(25, true); //food at home
            root.UpdateWeightUpAndDown(100, true);
            root.NormalizeWeights();
        }

    }
    
    public enum ConsumerType
    {
        Regular  = 0,
        EssentialRenter = 1, // rent + food + gas
        EssentialHomeOwner = 2,
        Prodige = 3 //eat outside, drink
    }

}
