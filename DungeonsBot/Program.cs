using System;
using System.Xml;
using System.Net;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonsBot
{
    class Program
    {
        static void Main()
        {
            string data = "";
            string uid = "fb:924660480936953";
            string auth = "ab3ea4154fc2a9313aa3e16f98d244ad";

            //получаем sid
            data = sendRequest("/command/get_game_info", string.Format(@"<get_game_info uid=""{0}"" auth_key=""{1}""/>", uid, auth));
            
            XmlDocument userScheme = new XmlDocument();
            userScheme.LoadXml(data);
            string sid = userScheme.SelectSingleNode(".//@sid").Value;
            //Console.WriteLine(sid); 

            ResourceLimits limits = new ResourceLimits(userScheme);
            Console.WriteLine(limits.Gold); 

            // NOTE: нужен ли тут апдейт, или для "производства" ресурсов достаточно запустить гетгейминфо
            //data = sendRequest("/command/update", string.Format(@"<update uid=""{0}"" auth_key=""{1}"" sid=""{2}""/>", uid, auth, sid));
            //Console.WriteLine(data);

            //словарь зданий добывающих ресурсы
            Dictionary <string, string[]> resourceBuildings = new Dictionary<string, string[]>();
            resourceBuildings.Add("lumberjack", new string[] { "materials", "lumberjack_s01", "lumberjack_s02", "lumberjack_s03", "lumberjack_s04" });
            resourceBuildings.Add("quarry", new string[] { "materials", "lumberjack_s01", "lumberjack_s02", "lumberjack_s03", "lumberjack_s04" });
            resourceBuildings.Add("mine", new string[] { "materials", "lumberjack_s01", "lumberjack_s02", "lumberjack_s03", "lumberjack_s04" });

            resourceBuildings.Add("field", new string[] { "food", "field_s01", "field_s02", "field_s03", "field_s04" });
            resourceBuildings.Add("fish", new string[] { "food", "field_s01", "field_s02", "field_s03", "field_s04" });
            resourceBuildings.Add("animal_farm", new string[] { "food", "field_s01", "field_s02", "field_s03", "field_s04" });

            resourceBuildings.Add("grove", new string[] { "dust", "grove_s01", "grove_s02", "grove_s03", "grove_s04" });
            resourceBuildings.Add("grotto", new string[] { "dust", "grove_s01", "grove_s02", "grove_s03", "grove_s04" });

            resourceBuildings.Add("tavern", new string[] { "gold" });
            resourceBuildings.Add("brothel", new string[] { "gold" });
            resourceBuildings.Add("gambling", new string[] { "gold" });
            resourceBuildings.Add("cook_shop", new string[] { "gold" });
            resourceBuildings.Add("bath", new string[] { "gold" });
            resourceBuildings.Add("fortune_teller", new string[] { "gold" }); //TODO: уточнить название

            resourceBuildings.Add("golden_fountain", new string[] { "gold" });
            resourceBuildings.Add("magic_generator", new string[] { "dust" });
            resourceBuildings.Add("rotten_belly_pit", new string[] { "materials" });
            resourceBuildings.Add("food_anturajka", new string[] { "food" }); //TODO: уточнить название

            resourceBuildings.Add("arkentarium", new string[] { "crystal" });
            resourceBuildings.Add("magic_crystal", new string[] { "snowstorm" });




            Console.ReadKey();
        }

        static string sendRequest(string command, string requestBody)
        {
            string host = "https://game-r06ww.rjgplay.com";
            string url = host + command;

            MyWebRequest request = new MyWebRequest(url, "POST", requestBody);
            return request.GetResponse();
        }        
    }

    public class ResourceLimits
    {
        //начальные лимиты
        int gold = 1000;
        int materials = 1000;
        int dust = 1000;
        int food = 1000;

        public ResourceLimits(XmlDocument userScheme)
        {
            int goldStorageLimit = 1000;
            int foodStorageLimit = 1000;
            int dustStorageLimit = 1000;
            int materialsStorageLimit = 1000;

            int goldStorageCount = userScheme.SelectNodes(".//building/type[text()='treasury']").Count;
            int foodStorageCount = userScheme.SelectNodes(".//building/type[text()='granary']").Count;
            int dustStorageCount = userScheme.SelectNodes(".//building/type[text()='dustStorageCount']").Count; //TODO: поменять потом на актуальное
            int materialsStorageCount = userScheme.SelectNodes(".//building/type[text()='storehouse']").Count;

            gold = gold + goldStorageLimit * goldStorageCount;
            food = food + foodStorageLimit * foodStorageCount;
            dust = dust + dustStorageLimit * dustStorageCount;
            materials = materials + materialsStorageLimit * materialsStorageCount;
        }

        // TODO: помоему есть какая то более простая запись. Глянуть
        public int Gold { get { return gold; } }
        public int Materials { get { return materials; } }
        public int Dust { get { return dust; } }
        public int Food { get { return food; } }
    }
}
