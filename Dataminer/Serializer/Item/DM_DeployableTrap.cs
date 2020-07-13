using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SideLoader;

namespace Dataminer
{
    public class DM_DeployableTrap : DM_Item
    {
        public List<DM_TrapRecipe> TrapRecipeEffects = new List<DM_TrapRecipe>();

        public override void SerializeItem(Item item, DM_Item holder)
        {
            base.SerializeItem(item, holder);

            var trap = item as DeployableTrap;

            var recipes = (TrapEffectRecipe[])At.GetValue(typeof(DeployableTrap), trap, "m_trapRecipes");

            foreach (var recipe in recipes)
            {
                var dmRecipe = new DM_TrapRecipe();
                dmRecipe.Serialize(recipe);
                TrapRecipeEffects.Add(dmRecipe);
            }
        }
    }

    public class DM_TrapRecipe
    {
        //public List<int> CompatibleItemIDs = new List<int>();
        public List<string> CompatibleItemIDs = new List<string>();
        public List<string> CompatibleItemTags = new List<string>();

        public List<DM_Effect> StandardEffects;
        public List<DM_Effect> HiddenEffects;

        public void Serialize(TrapEffectRecipe recipe)
        {
            var items = (Item[])At.GetValue(typeof(TrapEffectRecipe), recipe, "m_compatibleItems");
            if (items != null)
            {
                foreach (var item in items)
                {
                    // this.CompatibleItemIDs.Add(item.ItemID);
                    this.CompatibleItemIDs.Add(item.Name);
                }
            }

            var tags = (TagSourceSelector[])At.GetValue(typeof(TrapEffectRecipe), recipe, "m_compatibleTags");
            if (tags != null)
            {
                foreach (var tag in tags)
                {
                    this.CompatibleItemTags.Add(tag.Tag.TagName);
                }
            }

            if (recipe.TrapEffectsPrefab)
            {
                this.StandardEffects = new List<DM_Effect>();
                foreach (var effect in recipe.TrapEffectsPrefab.GetComponents<Effect>())
                {
                    this.StandardEffects.Add(DM_Effect.ParseEffect(effect));
                }
            }
            if (recipe.HiddenTrapEffectsPrefab)
            {
                this.HiddenEffects = new List<DM_Effect>();
                foreach (var effect in recipe.HiddenTrapEffectsPrefab.GetComponents<Effect>())
                {
                    this.HiddenEffects.Add(DM_Effect.ParseEffect(effect));
                }
            }
        }
    }
}
