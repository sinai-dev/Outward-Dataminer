using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Serialization;
using SideLoader;
using UnityEngine;

namespace Dataminer
{
    [DM_Serialized]
    public class DM_StatusEffect
    {
        public int PresetID;
        public string Identifier;

        public string Name;
        public string Description;

        public float Lifespan;
        public string LengthType;
        public float RefreshRate;
        
        public float BuildupRecoverySpeed;
        public bool IgnoreBuildupIfApplied;

        public bool DisplayedInHUD;
        public bool IsHidden;

        public string ComplicationStatusIdentifier;
        public string RequiredStatusIdentifier;
        public bool RemoveRequiredStatus;
        public bool NormalizeDamageDisplay;
        public bool IgnoreBarrier;

        public List<string> Tags;

        public List<DM_EffectTransform> Effects;

        public static DM_StatusEffect ParseStatusEffect(StatusEffect status)
        {
            var preset = status.GetComponent<EffectPreset>();

            At.Invoke(status, "OnAwake");

            var template = (DM_StatusEffect)Activator.CreateInstance(Serializer.GetBestDMType(status.GetType()));

            template.SerializeStatusEffect(status, preset);

            return template;
        }

        public virtual void SerializeStatusEffect(StatusEffect status, EffectPreset preset)
        {
            PresetID = preset?.PresetID ?? -1;
            Identifier = status.IdentifierName;
            IgnoreBuildupIfApplied = status.IgnoreBuildUpIfApplied;
            BuildupRecoverySpeed = status.BuildUpRecoverSpeed;
            DisplayedInHUD = status.DisplayInHud;
            IsHidden = status.IsHidden;
            Lifespan = status.StatusData.LifeSpan;
            RefreshRate = status.RefreshRate;
            LengthType = status.LengthType.ToString();

            ComplicationStatusIdentifier = status.ComplicationStatus?.IdentifierName;
            RequiredStatusIdentifier = status.RequiredStatus?.IdentifierName;
            RemoveRequiredStatus = status.RemoveRequiredStatus;
            NormalizeDamageDisplay = status.NormalizeDamageDisplay;
            IgnoreBarrier = status.IgnoreBarrier;

            GetStatusLocalization(status, out Name, out Description);

            Tags = new List<string>();
            status.InitTags();
            var tags = (TagSource)At.GetField(status, "m_tagSource");
            foreach (var tag in tags.Tags)
            {
                Tags.Add(tag.TagName);

                ListManager.AddTagSource(tag, Name);
            }

            // For existing StatusEffects, the StatusData contains the real values, so we need to SetValue to each Effect.
            var statusData = status.StatusData.EffectsData;
            var components = status.GetComponentsInChildren<Effect>();
            for (int i = 0; i < components.Length; i++)
            {
                var comp = components[i];
                if (comp && comp.Signature.Length > 0)
                {
                    comp.SetValue(statusData[i].Data);
                }
            }

            Effects = new List<DM_EffectTransform>();

            if (status.transform.childCount > 0)
            {
                var signature = status.transform.GetChild(0);
                if (signature)
                {
                    foreach (Transform child in signature.transform)
                    {
                        var effectsChild = DM_EffectTransform.ParseTransform(child);

                        if (effectsChild.ChildEffects.Count > 0 || effectsChild.Effects.Count > 0 || effectsChild.EffectConditions.Count > 0)
                        {
                            Effects.Add(effectsChild);
                        }
                    }
                }
            }
        }

        public static void GetStatusLocalization(StatusEffect effect, out string name, out string desc)
        {
            var namekey = (string)At.GetField(effect, "m_nameLocKey");
            name = LocalizationManager.Instance.GetLoc(namekey);
            var desckey = (string)At.GetField(effect, "m_descriptionLocKey");
            desc = LocalizationManager.Instance.GetLoc(desckey);
        }

        public static void ParseAllEffects()
        {
            var parsedIdentifiers = new List<string>();

            string dir = Serializer.Folders.Effects;

            if (References.RPM_EFFECT_PRESETS is Dictionary<int, EffectPreset> dict)
            {
                SL.Log("Parsing " + dict.Count + " EffectPresets!");

                foreach (EffectPreset preset in dict.Values)
                {
                    if (preset is ImbueEffectPreset imbuePreset)
                    {
                        var name = imbuePreset.Name;
                        var template = DM_ImbueEffect.ParseImbueEffect(imbuePreset);
                        ListManager.ImbueEffects.Add(template.StatusID.ToString(), template);
                        Serializer.SaveToXml(dir, name, template);
                    }
                    else if (preset.GetComponent<StatusEffect>() is StatusEffect status)
                    {
                        if (string.IsNullOrEmpty(status.IdentifierName) || parsedIdentifiers.Contains(status.IdentifierName))
                        {
                            continue;
                        }
                        var name = status.IdentifierName;
                        parsedIdentifiers.Add(status.IdentifierName);

                        var template = ParseStatusEffect(status);
                        ListManager.Effects.Add(template.PresetID.ToString(), template);
                        Serializer.SaveToXml(dir, name, template);
                    }
                }
            }
            else
            {
                SL.LogError("Could not find Effect Prefabs!");
            }

            int manualID = 1000;
            if (References.RPM_STATUS_EFFECTS is Dictionary<string, StatusEffect> statusDict)
            {
                SL.Log("Parsing " + statusDict.Count + " StatusEffect Prefabs! (before dupe check)");

                foreach (var status in statusDict.Values)
                {
                    //SL.Log(status.name);

                    if (string.IsNullOrEmpty(status.IdentifierName) || parsedIdentifiers.Contains(status.IdentifierName))
                    {
                        continue;
                    }

                    var template = ParseStatusEffect(status);

                    if (!string.IsNullOrEmpty(template.Name))
                    {
                        if (template.PresetID == -1)
                        {
                            manualID++;
                            template.PresetID = manualID;
                        }

                        ListManager.Effects.Add(template.PresetID.ToString(), template);
                        Serializer.SaveToXml(dir, status.IdentifierName, template);
                    }
                }
            }
        }

    }
}
