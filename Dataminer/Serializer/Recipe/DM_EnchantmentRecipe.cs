using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Discord;
using UnityEngine;

namespace Dataminer
{
    [DM_Serialized]
    public class DM_EnchantmentRecipe
    {
        public int RecipeID;
        public string Name;

        public EquipmentData CompatibleEquipment;
        public PillarData[] PillarDatas;
        public WeatherCondition[] Weather;
        public Vector2[] TimeOfDay;
        public TemperatureSteps[] Temperatures;
        public bool WindAltarActivated;
        public AreaManager.AreaEnum[] Regions;

        public DM_Enchantment Result;

        public static void ParseAllRecipes()
        {
            var recipes = (Dictionary<int, EnchantmentRecipe>)At.GetValue(typeof(RecipeManager), RecipeManager.Instance, "m_enchantmentRecipes");
            if (recipes == null)
            {
                Debug.Log("null enchantment dict!");
                return;
            }

            Debug.Log("Parsing Enchantments...");

            foreach (var recipe in recipes.Values)
            {
                if (recipe.RecipeID <= 0)
                {
                    recipe.InitID();
                }

                var enchantment = ResourcesPrefabManager.Instance.GetEnchantmentPrefab(recipe.RecipeID);

                if (!enchantment)
                {
                    Debug.Log("null enchantment!");
                    continue;
                }

                var template = new DM_EnchantmentRecipe();
                template.Serialize(recipe, enchantment);

                string dir = Serializer.Folders.Enchantments;
                string saveName = template.Name + " (" + template.RecipeID + ")";

                ListManager.EnchantmentRecipes.Add(template.RecipeID.ToString(), template);
                Serializer.SaveToXml(dir, saveName, template);
            }
        }

        public void Serialize(EnchantmentRecipe recipe, Enchantment enchantment)
        {
            Name = enchantment.Name;
            RecipeID = recipe.RecipeID;

            TimeOfDay = recipe.TimeOfDay;
            Temperatures = recipe.Temperature;
            WindAltarActivated = recipe.WindAltarActivated;
            Regions = recipe.Region;

            CompatibleEquipment = new EquipmentData();

            if (recipe.CompatibleEquipments.EquipmentTag != null && recipe.CompatibleEquipments.EquipmentTag.Tag != Tag.None)
            {
                CompatibleEquipment.EquipmentTag = recipe.CompatibleEquipments.EquipmentTag.Tag.TagName;
            }

            if (recipe.CompatibleEquipments.CompatibleEquipments != null)
            {
                var list = new List<IngredientData>();
                foreach (var equip in recipe.CompatibleEquipments.CompatibleEquipments)
                {
                    var data = new IngredientData
                    {
                        Type = equip.Type
                    };
                    if (equip.Type == EnchantmentRecipe.IngredientData.IngredientType.Generic)
                    {
                        data.IngredientTag = equip.IngredientTag.Tag.TagName;
                    }
                    else
                    {
                        data.SpecificItemID = equip.SpecificIngredient.ItemID;
                    }
                    list.Add(data);
                }
                CompatibleEquipment.CompatibleEquipments = list.ToArray();
            }

            if (recipe.PillarDatas != null)
            {
                var list = new List<PillarData>();

                foreach (var pillarData in recipe.PillarDatas)
                {
                    var pillar = new PillarData
                    {
                        Direction = pillarData.Direction,
                        IsFar = pillarData.IsFar,
                    };
                    var ingList = new List<IngredientData>();
                    foreach (var ingredient in pillarData.CompatibleIngredients)
                    {
                        var ing = new IngredientData
                        {
                            Type = ingredient.Type,
                        };

                        if (ingredient.Type == EnchantmentRecipe.IngredientData.IngredientType.Generic)
                        {
                            ing.IngredientTag = ingredient.IngredientTag.Tag.TagName;
                        }
                        else
                        {
                            ing.SpecificItemID = ingredient.SpecificIngredient.ItemID;
                        }

                        ingList.Add(ing);
                    }
                    pillar.CompatibleIngredients = ingList.ToArray();

                    list.Add(pillar);
                }

                PillarDatas = list.ToArray();
            }

            if (recipe.Weather != null)
            {
                var weatherList = new List<WeatherCondition>();
                foreach (var weather in recipe.Weather)
                {
                    weatherList.Add(new WeatherCondition
                    {
                        Invert = weather.Invert,
                        WeatherType = weather.Weather
                    });
                }
                Weather = weatherList.ToArray();
            }

            Result = DM_Enchantment.ParseEnchantment(enchantment);
        }

        public class PillarData
        {
            public IngredientData[] CompatibleIngredients;
            public UICardinalPoint_v2.CardinalPoint Direction;
            public bool IsFar;
        }

        public class EquipmentData
        {
            public IngredientData[] CompatibleEquipments;
            public string EquipmentTag;
        }

        public class IngredientData
        {
            public string IngredientTag;
            public int SpecificItemID;
            public EnchantmentRecipe.IngredientData.IngredientType Type;
        }

        public class WeatherCondition
        {
            public bool Invert;
            public EnchantmentRecipe.WeaterType WeatherType;
        }
    }
}
