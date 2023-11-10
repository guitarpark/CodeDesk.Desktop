using Newtonsoft.Json;

namespace CodeDesk.Desktop.Extensions
{

    public static class JsonExtensions
    {
        static JsonSerializerSettings defaultJsonSetting = new JsonSerializerSettings()
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            ContractResolver = new Newtonsoft.Json.Serialization.DefaultContractResolver()
        };
        public static T FromJson<T>(this string jsonString)
        {
            return JsonConvert.DeserializeObject<T>(jsonString, defaultJsonSetting);

        }
        public static string FromJson(this string jsonString)
        {
            return JsonConvert.DeserializeObject<string>(jsonString, defaultJsonSetting);

        }
        public static string ToJson(this object jsonObject)
        {
            return JsonConvert.SerializeObject(jsonObject, defaultJsonSetting);
        }
        public static string ToJson(this object jsonObject, JsonSerializerSettings settings)
        {
            return JsonConvert.SerializeObject(jsonObject, settings);
        }

        public static T DeepClone<T>(this T entity) where T : class
        {

            var setting = new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                ContractResolver = new Newtonsoft.Json.Serialization.DefaultContractResolver(),
                ObjectCreationHandling = ObjectCreationHandling.Replace,
                NullValueHandling = NullValueHandling.Include,
                TypeNameHandling = TypeNameHandling.Auto
            };
            return JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(entity, setting), setting);
        }
    }
}
