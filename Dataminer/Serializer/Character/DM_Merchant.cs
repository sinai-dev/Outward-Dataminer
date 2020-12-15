﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SideLoader;
using UnityEngine;

namespace Dataminer
{
    [DM_Serialized]
    public class DM_Merchant
    {
        public string Name;
        public string UID;

        public DM_DropTable DropTable;

        public List<float> BuyModifiers = new List<float>();
        public List<float> SellModifiers = new List<float>();

        public static DM_Merchant ParseMerchant(Merchant merchant)
        {
            var merchantHolder = new DM_Merchant
            {
                Name = merchant.ShopName,
                UID = merchant.HolderUID
            };

            if (At.GetField(merchant, "m_dropableInventory") is Dropable dropper)
            {
                merchantHolder.DropTable = DM_DropTable.ParseDropTable(dropper, merchant);
            }

            foreach (var priceMod in merchant.GetComponentsInChildren<PriceModifier>())
            {
                if (priceMod.BuyMultiplierAdded != 0f || priceMod.SellMultiplierAdded != 0f)
                {
                    //SL.Log("Merchant " + merchantHolder.Name + " has buy or sell mods! Buy: " + priceMod.BuyMultiplierAdded + ", Sell: " + priceMod.SellMultiplierAdded);
                    merchantHolder.BuyModifiers.Add(priceMod.BuyMultiplierAdded);
                    merchantHolder.SellModifiers.Add(priceMod.SellMultiplierAdded);
                }
            }

            string dir = Serializer.Folders.Merchants;
            string saveName = SceneManager.Instance.GetCurrentLocation(merchant.transform.position) + " - " + merchantHolder.Name + " (" + merchantHolder.UID + ")";

            Serializer.SaveToXml(dir, saveName, merchantHolder);

            ListManager.Merchants.Add(saveName, merchantHolder);

            return merchantHolder;
        }

        public static void ParseAllMerchants()
        {
            foreach (Merchant m in Resources.FindObjectsOfTypeAll<Merchant>().Where(x => x.gameObject.scene != null && x.ShopName != "Merchant"))
            {
                var merchantHolder = ParseMerchant(m);

                var summary = ListManager.SceneSummaries[ListManager.GetSceneSummaryKey(m.transform.position)];
                summary.Merchants.Add(merchantHolder.Name + " (" + merchantHolder.UID + ")");
            }
        }
    }
}
