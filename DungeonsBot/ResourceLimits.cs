using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace DungeonsBot
{
    class ResourceLimits
    {
        private Dictionary<string, int> limits = new Dictionary<string, int>();

        public ResourceLimits(XmlDocument userScheme)
        {
            //базовые значения складов
            limits.Add("gold", 1000);
            limits.Add("food", 1000);
            limits.Add("dust", 1000);
            limits.Add("materials", 1000); 

            //каждое здание добавляет к лимиту
            int goldStorageLimit = 2000;
            int foodStorageLimit = 1500;
            int dustStorageLimit = 5000;
            int materialsStorageLimit = 4000;

            int goldStorageCount = userScheme.SelectNodes(".//building/type[text()='treasury']").Count;
            int foodStorageCount = userScheme.SelectNodes(".//building/type[text()='granary']").Count;
            int dustStorageCount = userScheme.SelectNodes(".//building/type[text()='dust_store']").Count;
            int materialsStorageCount = userScheme.SelectNodes(".//building/type[text()='storehouse']").Count;

            limits["gold"]      += goldStorageLimit * goldStorageCount;
            limits["food"]      += foodStorageLimit * foodStorageCount;
            limits["dust"]      += dustStorageLimit * dustStorageCount;
            limits["materials"] += materialsStorageLimit * materialsStorageCount;
        }

        //возвращает актуальный лимит если в словаре есть такой ресурс, иначе 0
        public int getResourceLimit(string resource)
        {
            int value = 0;
            limits.TryGetValue(resource, out value);
            return value;
        }
    }
}
