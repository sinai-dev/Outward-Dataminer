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

        public List<DM_ItemLoc> ItemLocalizations = new List<DM_ItemLoc>();
        public List<DM_DialogueLoc> DialogueLocalizations = new List<DM_DialogueLoc>();
        public List<DM_Loc> MenuLocalizations = new List<DM_Loc>();
        public List<DM_Loc> LoadingTipsLocalization = new List<DM_Loc>();

        // save orig XML
        public static void SaveLocalization(LocalizationReference.Localization loc)
        {
            var locHolder = new LocalizationHolder
            {
                Name = loc.Name,
                DefaultName = loc.DefaultName
            };

            locHolder.DialogueLocalizations = LoadDialogue(loc.DialogueLocalizations);
            locHolder.ItemLocalizations = LoadItems(loc.ItemLocalizations);
            locHolder.MenuLocalizations = LoadMenu(loc.MenuLocalizations);
            locHolder.LoadingTipsLocalization = LoadTips(loc.LoadingTipsLocalization);

            // Serialize

            Serializer.SaveToXml(Serializer.Folders.Main, "Localization", locHolder);
        }

        // Parse Menu XML (load from game data)
        public static List<DM_Loc> LoadMenu(TextAsset[] array)
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

            var list = new List<DM_Loc>();

            foreach (var entry in dict)
            {
                list.Add(new DM_Loc
                {
                    Key = entry.Key,
                    Value = entry.Value
                });
            }

            return list;
        }

        // Parse Tips XML (load from game data)
        public static List<DM_Loc> LoadTips(TextAsset asset)
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

            var list = new List<DM_Loc>();

            foreach (var entry in dict)
            {
                list.Add(new DM_Loc
                {
                    Key = entry.Key,
                    Value = entry.Value
                });
            }

            return list;
        }

        // Parse Items XML (load from game data)
        public static List<DM_ItemLoc> LoadItems(TextAsset[] array)
        {
            var dict = new Dictionary<int, DM_ItemLoc>();

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

                        dict.Add(key, new DM_ItemLoc
                        {
                            Name = name,
                            Desc = desc.Replace("\n\n", "\n")
                        });
                    }
                }
            }

            var list = new List<DM_ItemLoc>();

            foreach (var entry in dict)
            {
                list.Add(new DM_ItemLoc
                {
                    KeyID = entry.Key,
                    Name = entry.Value.Name,
                    Desc = entry.Value.Desc
                });
            }

            return list;
        }

        // Parse Dialogue XML (load from game data)
        public static List<DM_DialogueLoc> LoadDialogue(TextAsset[] array)
        {
            var dict = new Dictionary<string, DM_DialogueLoc>();

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
                            string female = (node2["column_3"] == null) ? string.Empty : node2["column_3"].InnerText.Replace("\n\n", "\n");
                            if (!string.IsNullOrEmpty(key) || !string.IsNullOrEmpty(general))
                            {
                                string uniqueAudioName = (node2["column_4"] == null) ? string.Empty : node2["column_4"].InnerText;
                                string emoteData = (node2["column_5"] == null) ? string.Empty : node2["column_5"].InnerText;
                                string animData = (node2["column_6"] == null) ? string.Empty : node2["column_6"].InnerText;

                                if (!string.IsNullOrEmpty(key) && key != "loc_key" && !dict.ContainsKey(key))
                                {
                                    dict.Add(key, new DM_DialogueLoc
                                    {
                                        Key = key,
                                        General = general,
                                        Female = female,
                                        UniqueAudioName = uniqueAudioName,
                                        EmoteTags = emoteData,
                                        AnimTags = animData
                                    });
                                }
                            }
                        }
                    }
                }
            }

            return dict.Values.ToList();
        }
    }

    [DM_Serialized]
    public class DM_ItemLoc
    {
        public int KeyID;
        public string Name;
        public string Desc;
    }

    [DM_Serialized]
    public class DM_DialogueLoc
    {
        public string Key;
        public string General;
        public string Female;
        public string UniqueAudioName;
        public string AnimTags;
        public string EmoteTags;
    }

    [DM_Serialized]
    public class DM_Loc
    {
        public string Key;
        public string Value;
    }
}
