using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;

namespace DungeonsBot
{
    class User
    {
        private string uid, auth, sid, userHost;
        private int sleepInterval, nextStart;
        private UserItems userItems;
        private ResourceLimits limits;
        private XmlDocument userScheme;

        public User(string _uid, string _auth, int _sleepInterval)
        {
            uid = _uid;
            auth = _auth;
            sleepInterval = _sleepInterval;

            if (_uid.Substring(0, 2) == "fb")
            {
                userHost = "https://game-r06ww.rjgplay.com";
            } else if (_uid.Substring(0, 2) == "od" || _uid.Substring(0, 2) == "vk")
            {
                userHost = "https://game-r06ru.rjgplay.com";
            }
            //тут подтягиваются настройки игрока. Что именно он хочет делать, а что не хочет.
        }

        public string Uid { get { return uid; } }
        public string Auth { get { return auth; } }
        public int NextStart { get { return nextStart; } }


        public void StartMagic()
        {
            nextStart = UnixTimeNow() + sleepInterval;
            Console.WriteLine("текущее время" + UnixTimeNow());
            Console.WriteLine("следущее начало в : " + nextStart);

            //получаем sid и схему игрока
            string data = sendRequest("/command/get_game_info", string.Format(@"<get_game_info uid=""{0}"" auth_key=""{1}""/>", uid, auth));
            userScheme = new XmlDocument();
            userScheme.LoadXml(data);
            sid = userScheme.SelectSingleNode(".//@sid").Value;
            
            //задаем предметы игрока
            userItems = new UserItems(userScheme);

            //в зависимости от настроек игрока запускаем функции по списку
            CollectResources();

            Console.WriteLine("end of iteration");           
        }


        private void CollectResources()
        {
            //считаем лимиты по ресурсам
            limits = new ResourceLimits(userScheme);

            //словарь зданий, добывающих ресурсы
            ResourceBuildings resourceBuildings = new ResourceBuildings();
            var resourceBuildingsDic = resourceBuildings.getResourceBuildingsDic();

            //ищем все ноды со зданиями
            var buildingNodes = userScheme.SelectNodes(".//init_game/user/entities/building");            

            foreach (XmlNode buildingNode in buildingNodes)
            {
                string buildingID = buildingNode.SelectSingleNode("id").InnerText;
                //Console.WriteLine(buildingID);
                string buildingType = buildingNode.SelectSingleNode("type").InnerText;
                //Console.WriteLine(buildingType);
                var typeNodes = buildingNode.SelectNodes("./int_properties/property/type");
                var valueNodes = buildingNode.SelectNodes("./int_properties/property/value");

                if (resourceBuildingsDic.ContainsKey(buildingType))
                {
                    int i = 0;
                    foreach (XmlNode typeNode in typeNodes)
                    {
                        var resource = typeNode.InnerText;
                        var resourceCount = valueNodes[i].InnerText;

                        if (resourceBuildingsDic[buildingType].Contains(resource))
                        {
                            //Console.WriteLine("количество предмета у игрока: " + userItems.GetItemValue(resource));
                            //Console.WriteLine("лимит на сбор: " + limits.getResourceLimit(resource));
                            //Console.WriteLine("building type: " + buildingType);
                            //Console.WriteLine("resource type: " + typeNode.InnerText);
                            //Console.WriteLine("resource count: " + valueNodes[i].InnerText);

                            if (limits.getResourceLimit(resource) == 0 || userItems.GetItemValue(resource) < limits.getResourceLimit(resource))
                            {
                                Console.WriteLine("Collecting from: " + buildingType + ", id: " + buildingID + ", resource: " + resource + ", " + resourceCount);
                                sendRequest("/command/collect_production", string.Format(@"<collect_production uid=""{0}"" auth_key=""{1}"" sid=""{2}""><id>{3}</id><type>{4}</type><count>{5}</count></collect_production>",
                                    uid, auth, sid, buildingID, resource, resourceCount));
                                userItems.GiveItem(resource, Convert.ToInt32(resourceCount));
                                Console.WriteLine("Новое количество предметов: " + userItems.GetItemValue(resource));
                            }
                        }
                        i++;
                    }
                }
            }

        }

        private string sendRequest(string command, string requestBody)
        {
            if (userHost == null) { return "user host is not set properly"; }

            string host = userHost;
            string url = host + command;

            MyWebRequest request = new MyWebRequest(url, "POST", requestBody);
            return request.GetResponse();
        }

        public static int UnixTimeNow()
        {
            return (int)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
        }


    }
}
