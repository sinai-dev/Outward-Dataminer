using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using Localizer;

namespace Dataminer
{
    // This class is used to parse the original Localizations to custom XML holders, and these are used for saving the user's custom locs too.

    public class LocalizationHolder
    {
        public string Name;
        public string DefaultName;

        public static List<ItemLoc> Items = new List<ItemLoc>();
        public static List<Dialogue> DialogueOther = new List<Dialogue>();
        public static List<Dialogue> DialogueBC = new List<Dialogue>();
        public static List<Dialogue> DialogueHK = new List<Dialogue>();
        public static List<Dialogue> DialogueHM = new List<Dialogue>();
        public static List<Dialogue> DialogueSO = new List<Dialogue>();
        public static List<Dialogue> DialogueCA = new List<Dialogue>();
        public static List<BasicLoc> GeneralLocalization = new List<BasicLoc>();
        public static List<BasicLoc> LoadingTips = new List<BasicLoc>();

        // save orig XML
        public static void SaveLocalization(LocalizationReference.Localization loc)
        {
            LoadDialogue(loc.DialogueLocalizations);
            LoadItems(loc.ItemLocalizations);
            LoadMenu(loc.MenuLocalizations);
            LoadTips(loc.LoadingTipsLocalization);

            // Serialize

            Serializer.SaveToXml(Serializer.Folders.Main + @"\Localization", "Items", Items);
            Serializer.SaveToXml(Serializer.Folders.Main + @"\Localization", "General", GeneralLocalization);
            Serializer.SaveToXml(Serializer.Folders.Main + @"\Localization", "LoadingTips", LoadingTips);

            Serializer.SaveToXml(Serializer.Folders.Main + @"\Localization", "Dialogue_Other", DialogueOther);
            Serializer.SaveToXml(Serializer.Folders.Main + @"\Localization", "Dialogue_BlueChamber", DialogueBC);
            Serializer.SaveToXml(Serializer.Folders.Main + @"\Localization", "Dialogue_HeroicKingdom", DialogueHK);
            Serializer.SaveToXml(Serializer.Folders.Main + @"\Localization", "Dialogue_HolyMission", DialogueHM);
            Serializer.SaveToXml(Serializer.Folders.Main + @"\Localization", "Dialogue_Sorobor", DialogueSO);
            Serializer.SaveToXml(Serializer.Folders.Main + @"\Localization", "Dialogue_Sirocco", DialogueCA);
        }

        // Parse Menu XML (load from game data)
        public static void LoadMenu(TextAsset[] array)
        {
            var dict = new Dictionary<string, string>();

            XmlDocument xmlDocument = new XmlDocument();

            foreach (var asset in array)
            {
                xmlDocument.LoadXml(asset.text);

                var nodes = xmlDocument.DocumentElement.SelectNodes("/ooo_calc_export/ooo_sheet");

                foreach (XmlNode node in nodes)
                {
                    var nodes2 = node.SelectNodes("ooo_row");

                    foreach (XmlNode node2 in nodes2)
                    {
                        string text = node2["column_1"].InnerText.TrimEnd(new char[0]);

                        if (!dict.ContainsKey(text))
                        {
                            if (node2["column_2"] != null)
                            {
                                dict.Add(text, node2["column_2"].InnerText);
                            }
                            else if (text.Equals("name_unpc_narrator"))
                            {
                                dict.Add(text, string.Empty);
                            }
                        }
                    }
                }
            }

            var list = new List<BasicLoc>();

            foreach (var entry in dict)
            {
                list.Add(new BasicLoc
                {
                    Key = entry.Key,
                    Value = entry.Value
                });
            }

            GeneralLocalization = list;
        }

        // Parse Tips XML (load from game data)
        public static void LoadTips(TextAsset asset)
        {
            var dict = new Dictionary<string, string>();

            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(asset.text);

            var nodes = xmlDocument.DocumentElement.SelectNodes("/ooo_calc_export/ooo_sheet");

            foreach (XmlNode node in nodes)
            {
                var nodes2 = node.SelectNodes("ooo_row");

                foreach (XmlNode node2 in nodes2)
                {
                    string text2 = node2["column_1"].InnerText.TrimEnd(new char[0]);
                    if (!string.IsNullOrEmpty(text2) && text2 != "loc_key")
                    {
                        if (!text2.Equals("Test_Tip37"))
                        {
                            if (!dict.ContainsKey(text2) && node2["column_2"] != null)
                            {
                                dict.Add(text2, node2["column_2"].InnerText.Replace("\n\n", "\n"));
                            }
                        }
                    }
                }
            }

            var list = new List<BasicLoc>();

            foreach (var entry in dict)
            {
                list.Add(new BasicLoc
                {
                    Key = entry.Key,
                    Value = entry.Value
                });
            }

            LoadingTips = list;
        }

        // Parse Items XML (load from game data)
        public static void LoadItems(TextAsset[] array)
        {
            var dict = new Dictionary<int, ItemLoc>();

            XmlDocument xmlDocument = new XmlDocument();

            foreach (var asset in array)
            {
                xmlDocument.LoadXml(asset.text);

                var nodes = xmlDocument.DocumentElement.SelectNodes("/ooo_calc_export/ooo_sheet[@num='1']/ooo_row");

                int key = -1;
                foreach (XmlNode node in nodes)
                {
                    if (!string.IsNullOrEmpty(node["column_1"].InnerText) && int.TryParse(node["column_1"].InnerText, out key)
                        && !dict.ContainsKey(key))
                    {
                        string name = (node["column_2"] == null) ? string.Empty : node["column_2"].InnerText;
                        string desc = (node["column_3"] == null) ? string.Empty : node["column_3"].InnerText;

                        dict.Add(key, new ItemLoc
                        {
                            Name = name,
                            Desc = desc.Replace("\n\n", "\n")
                        });
                    }
                }
            }

            var list = new List<ItemLoc>();

            foreach (var entry in dict)
            {
                list.Add(new ItemLoc
                {
                    KeyID = entry.Key,
                    Name = entry.Value.Name,
                    Desc = entry.Value.Desc
                });
            }

            Items = list;
        }

        // Parse Dialogue XML (load from game data)
        public static void LoadDialogue(TextAsset[] array)
        {
            var dict = new Dictionary<string, Dialogue>();

            XmlDocument xmlDocument = new XmlDocument();

            foreach (var asset in array)
            {
                xmlDocument.LoadXml(asset.text);
                XmlNodeList nodes = xmlDocument.DocumentElement.SelectNodes("/ooo_calc_export/ooo_sheet");
                foreach (XmlNode node in nodes)
                {
                    XmlNodeList nodes2 = node.SelectNodes("ooo_row");
                    foreach (XmlNode node2 in nodes2)
                    {
                        string key = (node2["column_1"] == null) ? string.Empty : node2["column_1"].InnerText.TrimEnd(new char[0]);
                        if (!string.IsNullOrEmpty(key))
                        {
                            string general = (node2["column_2"] == null) ? string.Empty : node2["column_2"].InnerText.Replace("\n\n", "\n");
                            general = general.Replace(" !", "\u00a0!");
                            general = general.Replace(" ?", "\u00a0?");
                            //string female = (node2["column_3"] == null) ? string.Empty : node2["column_3"].InnerText.Replace("\n\n", "\n");
                            if (!string.IsNullOrEmpty(key) || !string.IsNullOrEmpty(general))
                            {
                                //string uniqueAudioName = (node2["column_4"] == null) ? string.Empty : node2["column_4"].InnerText;
                                //string emoteData = (node2["column_5"] == null) ? string.Empty : node2["column_5"].InnerText;
                                //string animData = (node2["column_6"] == null) ? string.Empty : node2["column_6"].InnerText;

                                if (key != "loc_key" && !dict.ContainsKey(key))
                                {
                                    dict.Add(key, new Dialogue
                                    {
                                        Key = key,
                                        Value = general,
                                        //Female = female,
                                        //UniqueAudioName = uniqueAudioName,
                                        //EmoteTags = emoteData,
                                        //AnimTags = animData
                                    });
                                }
                            }
                        }
                    }
                }
            }

            foreach (var entry in dict.Values)
            {
                if (entry.Key.Contains("_BC_"))
                    DialogueBC.Add(entry);
                else if (entry.Key.Contains("_HK_") || entry.Key.Contains("TendFlame") || entry.Key.Contains("MouthFeed"))
                    DialogueHK.Add(entry);
                else if (entry.Key.Contains("_HM_") || entry.Key.Contains("Permadeath"))
                    DialogueHM.Add(entry);
                else if (entry.Key.Contains("_SO"))
                    DialogueSO.Add(entry);
                else if (entry.Key.Contains("_CA"))
                    DialogueCA.Add(entry);
                else
                    DialogueOther.Add(entry);
            }

            return;
        }
    }

    [DM_Serialized]
    public class ItemLoc
    {
        public int KeyID;
        public string Name;
        public string Desc;
    }

    [DM_Serialized]
    public class Dialogue
    {
        public string Key;
        public string Value;
        //public string Female;
        //public string UniqueAudioName;
        //public string AnimTags;
        //public string EmoteTags;
    }

    [DM_Serialized]
    public class BasicLoc
    {
        public string Key;
        public string Value;
    }
}
