using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.IO;
using System.Xml.Serialization;
using SideLoader;

namespace Dataminer
{
    [DM_Serialized]
    public class DM_Recipe
    {
        public string Name;
        public int RecipeID;
        public string UID = "";

        public Recipe.CraftingType StationType = Recipe.CraftingType.Survival;

        public PlayerSystem.PlayerTypes RequiredPType;

        public List<string> Ingredients = new List<string>();
        public List<ItemQty> Results = new List<ItemQty>();

        public static void ParseAllRecipes()
        {
            if (References.ALL_RECIPES is Dictionary<string, Recipe> recipes)
            {
                foreach (Recipe recipe in recipes.Values)
                {
                    int id = recipe.RecipeID;
                    if (id == 0)
                    {
                        if (int.TryParse(recipe.name.Substring(0, 3), out int id2))
                            id = id2;
                    }
                    var key = id.ToString();

                    if (ListManager.Recipes.ContainsKey(key))
                    {
                        SL.LogWarning("Recipe list already contains ID " + key + ", skipping this recipe: " + recipe.name);
                        continue;
                    }

                    var recipeHolder = ParseRecipe(recipe, id);

                    string dir = Serializer.Folders.Recipes;
                    string saveName = recipeHolder.Name + " (" + recipeHolder.RecipeID + ")";

                    ListManager.Recipes.Add(key, recipeHolder);
                    Serializer.SaveToXml(dir, saveName, recipeHolder);
                }
            }
        }

        public static DM_Recipe ParseRecipe(Recipe recipe, int id)
        {
            var recipeHolder = new DM_Recipe
            {
                Name = recipe.Name,
                RecipeID = id,
                StationType = recipe.CraftingStationType,
                RequiredPType = recipe.RequiredPType,
            };

            foreach (RecipeIngredient ingredient in recipe.Ingredients)
            {
                if (ingredient.ActionType == RecipeIngredient.ActionTypes.AddSpecificIngredient)
                {
                    recipeHolder.Ingredients.Add(ingredient.AddedIngredient.Name);
                    //recipeHolder.Ingredients.Add(new Ingredient() 
                    //{
                    //    Type = ingredient.ActionType,
                    //    Ingredient_ItemID = ingredient.AddedIngredient.ItemID
                    //});
                }
                else
                {
                    recipeHolder.Ingredients.Add(ingredient.AddedIngredientType.Tag.TagName);
                    //recipeHolder.Ingredients.Add(new Ingredient() 
                    //{
                    //    Type = ingredient.ActionType,
                    //    Ingredient_Tag = ingredient.AddedIngredientType.Tag.TagName
                    //});
                }
            }

            foreach (ItemReferenceQuantity item in recipe.Results)
            {
                recipeHolder.Results.Add(new ItemQty
                {
                    ItemName = Serializer.SafeName(ResourcesPrefabManager.Instance.GetItemPrefab(item.ItemID).Name),
                    ItemID = item.ItemID,
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
