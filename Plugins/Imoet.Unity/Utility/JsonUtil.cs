using UnityEngine;
#if USING_NEWTONSOFT_JSON
using Newtonsoft.Json;
#endif

namespace Imoet.Unity.Utility {
    public static class JsonUtil
    {
        public static string ToJson(object obj)
        {
#if USING_NEWTONSOFT_JSON
            return JsonConvert.SerializeObject(obj);
#else
            return JsonUtility.ToJson(obj);
#endif
        }

        public static object FromJson(string json)
        {
#if USING_NEWTONSOFT_JSON
            return JsonConvert.DeserializeObject(json);
#else
            return JsonUtility.FromJson(json, typeof(object));
#endif
        }

        public static T FromJson<T>(string json)
        {
#if USING_NEWTONSOFT_JSON
            return JsonConvert.DeserializeObject<T>(json);
#else
            return JsonUtility.FromJson<T>(json);
#endif
        }
    }
}