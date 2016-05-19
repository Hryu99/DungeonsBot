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
            data = SendRequest("/command/get_game_info", string.Format(@"<get_game_info uid=""{0}"" auth_key=""{1}""/>", uid, auth));
            
            XmlDocument userScheme = new XmlDocument();
            userScheme.LoadXml(data);
            string sid = userScheme.SelectSingleNode(".//@sid").Value;
            //Console.WriteLine(sid); 

            //нужен ли тут апдейт, или для "производства" ресурсов достаточно запустить гетгейминфо
            data = SendRequest("/command/update", string.Format(@"<update uid=""{0}"" auth_key=""{1}"" sid=""{2}""/>", uid, auth, sid));
            Console.WriteLine(data);        
           

             
            Console.ReadKey();
        }

        static string SendRequest(string command, string requestBody)
        {
            string host = "https://game-r06ww.rjgplay.com";
            string url = host + command;

            MyWebRequest request = new MyWebRequest(url, "POST", requestBody);
            return request.GetResponse();
        }
    }
}
