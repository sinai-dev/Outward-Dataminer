using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SideLoader;

namespace Dataminer
{
    public class DM_Building : DM_Item
    {
        public Building.BuildingTypes BuildingType;

        public readonly List<DM_ConstructionPhase> ConstructionPhases = new List<DM_ConstructionPhase>();
        public readonly List<DM_ConstructionPhase> UpgradePhases = new List<DM_ConstructionPhase>();

        public override void SerializeItem(Item item, DM_Item holder)
        {
            base.SerializeItem(item, holder);

            var building = item as Building;

            var constructionPhases = At.GetField(building, "m_constructionPhases") as Building.ConstructionPhase[];
            foreach (var phase in constructionPhases)
                ConstructionPhases.Add(DM_ConstructionPhase.SerializePhase(phase));

            var upgradePhases = At.GetField(building, "m_upgradePhases") as Building.ConstructionPhase[];
            foreach (var phase in upgradePhases)
                UpgradePhases.Add(DM_ConstructionPhase.SerializePhase(phase));
        }

        [DM_Serialized]
        public class DM_ConstructionPhase
        {   
            public Building.ConstructionPhase.Type PhaseType;

            public BuildingResourceValues ConstructionCosts;
            public int ConstructionTime;

            public string RareMaterial;

            public List<DM_BuildingRequirement> BuildingRequirements;
            
            public int HouseCountRequirement;
            public int HousingValue;
            public bool MultiplyProductionPerHouse;

            public BuildingResourceValues CapacityBonus;
            public BuildingResourceValues UpkeepCosts;
            public BuildingResourceValues UpkeepProductions;

            public static DM_ConstructionPhase SerializePhase(Building.ConstructionPhase phase)
            {
                return new DM_ConstructionPhase
                {
                    PhaseType = phase.ConstructionType,
                    HouseCountRequirement = phase.HouseCountRequirements,
                    ConstructionTime = phase.GetConstructionTime(),
                    ConstructionCosts = (BuildingResourceValues)At.GetField(phase, "m_constructionCosts"),
                    UpkeepCosts = phase.UpkeepCosts,
                    UpkeepProductions = phase.UpkeepProductions,
                    CapacityBonus = phase.CapacityBonus,
                    MultiplyProductionPerHouse = phase.MultiplyProductionPerHouse,
                    HousingValue = phase.HousingValue,
                    RareMaterial = phase.RareMaterial?.Name,
                    BuildingRequirements = (from req in phase.BuildingRequirements
                                            select new DM_BuildingRequirement()
                                            {
                                                ReqBuildingName = req.ReqBuilding?.Name,
                                                ReqUpgradeIndex = req.UpgradeIndex
                                            }).ToList(),
                };
            }
        }

        [DM_Serialized]
        public class DM_BuildingRequirement
        {
            public string ReqBuildingName;
            public int ReqUpgradeIndex;
        }

        //[DM_Serialized]
        //public struct DM_BuildingResourceValues
        //{
        //    public int Food, Funds, Stone, Timber;
        //}
    }
}
