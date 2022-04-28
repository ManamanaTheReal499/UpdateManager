using Newtonsoft.Json;
using System.Web;

namespace UpdateManager_Core
{
    public static class Extensions
    {
        public static T? Deserialize<T>(this string json) =>
            JsonConvert.DeserializeObject<T>(json);

        public static string Serialize(this object Object) =>
            JsonConvert.SerializeObject(Object);

        public static string UrlEncode(this string url) => 
            HttpUtility.UrlEncode(url != null ? url : "");
    }
}
