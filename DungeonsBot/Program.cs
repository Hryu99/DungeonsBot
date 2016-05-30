using System;
using System.Xml;
using System.Threading;
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
            //TODO: получаем настройки игрока пока что из файла (id, auth) (игроков может быть несколько). В том числе время выполнения следующих запросов
            //пробегаемся по всем игрокам, учитывая время старта
            List<User> userList = new List<User>();
            userList.Add(new User("fb:924660480936953", "ab3ea4154fc2a9313aa3e16f98d244ad", 40));
            userList.Add(new User("od:563975376967", "2c8a38aaa12749a09178e7e35d1cacce", 20));


            while (true)
            {
                foreach (User user in userList)
                {
                    if (user.NextStart < UnixTimeNow())
                    {
                        Task userTask = Task.Run(() => user.StartMagic());
                    }
                    Thread.Sleep(1000);
                }                
            }
            
            //узнать как ловить эксепшены
            //если поймали ошибку на каком то из запросов кроме гет гейм инфо, прекращаем. Как то логируем данный факт.
            //ставим таймер на следующий запрос на + 10-20 минут. Если ловим ошибку второй раз (не обязательно ту же), отключаем данного игрока от бота на некоторое время
            //в будущем отключаем не все запросы, а только те что ловят ошибки
            //отключаем на глобальном уровне у всех

            //как сделать так чтобы можно было вставлять паузы между запросами, но при этом можно было бы вести все параллельно. Многопотоковость?
            //класс Thread почитать, похоже это то что надо насчет многопоточности
            //класс Task почитать, возможно его использовать

            Console.ReadKey();
        }

        //NOTE: как сделать метод, которым можно вызывать откуда угодно?
        public static int UnixTimeNow()
        {
            return (int)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
        }



    }


}
