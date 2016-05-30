using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace DungeonsBot
{
    class UserItems
    {
        private Dictionary<string, int> userItems = new Dictionary<string, int>();

        public UserItems(XmlDocument userScheme)
        {
            var itemNodes = userScheme.SelectNodes(".//init_game/user/items/item");

            foreach (XmlNode itemNode in itemNodes)
            {
                string item     = itemNode.SelectSingleNode(".//@type").Value;
                int itemCount   = Convert.ToInt32(itemNode.SelectSingleNode(".//@value").Value);
                userItems.Add(item, itemCount);
            }
        }


        /// <summary>
        /// возвращает количество предметов игрока
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public int GetItemValue(string item)
        {
            int value = 0;
            userItems.TryGetValue(item, out value);
            return value;
        }

        /// <summary>
       /// Добавляет value к количеству предметов
       /// </summary>
       /// <param name="item"></param>
       /// <param name="value"></param>
        public void GiveItem(string item, int value)
        {
            userItems[item] += value;
        }

        /// <summary>
        /// Устанавливает кол-во предметов
        /// </summary>
        /// <param name="item"></param>
        /// <param name="value"></param>
        public void SetItem(string item, int value)
        {
            userItems[item] = value;
        }
    }
}
