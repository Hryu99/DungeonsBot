using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonsBot
{
    class ResourceBuildings
    {
        private Dictionary<string, string[]> resourceBuildings = new Dictionary<string, string[]>();

        public ResourceBuildings()
        {
            resourceBuildings.Add("lumberjack", new string[] { "materials", "lumberjack_s01", "lumberjack_s02", "lumberjack_s03", "lumberjack_s04" });
            resourceBuildings.Add("quarry", new string[] { "materials", "lumberjack_s01", "lumberjack_s02", "lumberjack_s03", "lumberjack_s04" });
            resourceBuildings.Add("mine", new string[] { "materials", "lumberjack_s01", "lumberjack_s02", "lumberjack_s03", "lumberjack_s04" });

            resourceBuildings.Add("field", new string[] { "food", "field_s01", "field_s02", "field_s03", "field_s04" });
            resourceBuildings.Add("fish", new string[] { "food", "field_s01", "field_s02", "field_s03", "field_s04" });
            resourceBuildings.Add("animal_farm", new string[] { "food", "field_s01", "field_s02", "field_s03", "field_s04" });
            resourceBuildings.Add("chefs_guild", new string[] { "food" });

            resourceBuildings.Add("grove", new string[] { "dust", "grove_s01", "grove_s02", "grove_s03", "grove_s04" });
            resourceBuildings.Add("grotto", new string[] { "dust", "grove_s01", "grove_s02", "grove_s03", "grove_s04" });

            resourceBuildings.Add("tavern", new string[] { "gold" });
            resourceBuildings.Add("brothel", new string[] { "gold" });
            resourceBuildings.Add("gambling", new string[] { "gold" });
            resourceBuildings.Add("cook_shop", new string[] { "gold" });
            resourceBuildings.Add("bath", new string[] { "gold" });
            resourceBuildings.Add("fortune", new string[] { "gold" });

            resourceBuildings.Add("golden_fountain", new string[] { "gold" });
            resourceBuildings.Add("magic_generator", new string[] { "dust" });
            resourceBuildings.Add("rotten_belly_pit", new string[] { "materials" });
            resourceBuildings.Add("coconut_grove", new string[] { "food" });

            resourceBuildings.Add("arkentarium", new string[] { "crystal" });
            resourceBuildings.Add("magic_crystal", new string[] { "snowstorm" });
        }


        /// <summary>
        /// Возвращает коллекцию зданий, производящих ресурсы
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, string[]> getResourceBuildingsDic()
        {
            return resourceBuildings;
        }
    }


    //скорее всего можно будет удалить в связи с ненадобностью
    class ResourceBuilding
    {
        private string id, buildingName, resource, resourceCount;

        public ResourceBuilding(string _id, string _buildingName, string _resource, string _resourceCount)
        {
            id = _id;
            buildingName = _buildingName;
            resource = _resource;
            resourceCount = _resourceCount;
        }
        
        public string Id { get { return id; } }
        public string BuildingName { get { return buildingName; } }
        public string Resource { get { return resource; } }
        public string ResourceCount { get { return resourceCount; } }

    }
}
