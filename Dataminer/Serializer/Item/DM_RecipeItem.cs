using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dataminer
{
    public class DM_RecipeItem : DM_Item
    {
        public string RecipeUID;

        public override void SerializeItem(Item item, DM_Item holder)
        {
            base.SerializeItem(item, holder);

            var template = holder as DM_RecipeItem;
            var recipeItem = item as RecipeItem;

            template.RecipeUID = recipeItem.Recipe.UID;
        }
    }
}
