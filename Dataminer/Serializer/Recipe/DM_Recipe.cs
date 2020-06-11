﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.IO;
using System.Xml.Serialization;

namespace Dataminer
{
    [DM_Serialized]
    public class DM_Recipe
    {
        public string Name;
        public int RecipeID;
        public string UID = "";

        public Recipe.CraftingType StationType = Recipe.CraftingType.Survival;

        public List<Ingredient> Ingredients = new List<Ingredient>();
        public List<ItemQty> Results = new List<ItemQty>();

        public static void ParseAllRecipes()
        {
            if (At.GetValue(typeof(RecipeManager), RecipeManager.Instance, "m_recipes") is Dictionary<string, Recipe> recipes)
            {
                foreach (Recipe recipe in recipes.Values)
                {
                    var recipeHolder = ParseRecipe(recipe);

                    string dir = Serializer.Folders.Recipes;
                    string saveName = recipeHolder.Name + " (" + recipeHolder.RecipeID + ")";

                    ListManager.Recipes.Add(recipeHolder.RecipeID.ToString(), recipeHolder);
                    Serializer.SaveToXml(dir, saveName, recipeHolder);
                }
            }
        }

        public static DM_Recipe ParseRecipe(Recipe recipe)
        {
            var recipeHolder = new DM_Recipe
            {
                Name = recipe.Name,
                RecipeID = recipe.RecipeID,
                StationType = recipe.CraftingStationType,
            };

            foreach (RecipeIngredient ingredient in recipe.Ingredients)
            {
                if (ingredient.ActionType == RecipeIngredient.ActionTypes.AddSpecificIngredient)
                {
                    recipeHolder.Ingredients.Add(new Ingredient() 
                    {
                        Type = ingredient.ActionType,
                        Ingredient_ItemID = ingredient.AddedIngredient.ItemID
                    });
                }
                else
                {
                    recipeHolder.Ingredients.Add(new Ingredient() 
                    {
                        Type = ingredient.ActionType,
                        Ingredient_Tag = ingredient.AddedIngredientType.Tag.TagName
                    });
                }
            }

            foreach (ItemQuantity item in recipe.Results)
            {
                recipeHolder.Results.Add(new ItemQty
                {
                    ItemName = item.Item.Name,
                    ItemID = item.Item.ItemID,
                    Quantity = item.Quantity
                });
            }

            return recipeHolder;
        }

        [DM_Serialized]
        public class Ingredient
        {
            public RecipeIngredient.ActionTypes Type;

            public int Ingredient_ItemID;
            public string Ingredient_Tag;
        }

        [DM_Serialized]
        public class ItemQty
        {
            public string ItemName;
            public int ItemID;
            public int Quantity;
        }
    }
}