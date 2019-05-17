using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
namespace Edu.Tools
{

    public class JsonHelper
    {

    
        #region 通用

        /// <summary>检查字符串是否json格式</summary>
        /// <param name="jText"></param>
        /// <returns></returns>
        public static bool IsJson(string jText)
        {
            if (string.IsNullOrEmpty(jText) || jText.Length < 3)
            {
                return false;
            }

            if (jText.Substring(0, 2) == "{\"" || jText.Substring(0, 3) == "[{\"")
            {
                return true;
            }
            return false;
        }

        /// <summary>检查字符串是否json格式数组</summary>
        /// <param name="jText"></param>
        /// <returns></returns>
        public static bool IsJsonRs(string jText)
        {
            if (string.IsNullOrEmpty(jText) || jText.Length < 3)
            {
                return false;
            }

            if (jText.Substring(0, 3) == "[{\"")
            {
                return true;
            }
            return false;
        }

        /// <summary>格式化 json</summary>
        /// <param name="jText"></param>
        /// <returns></returns>
        public static string Fmt_Null(string jText)
        {
            return jText.Replace(":null,", ":\"\",");

        }

        /// <summary>格式化 json ，删除左右二边的[]</summary>
        /// <param name="jText"></param>
        /// <returns></returns>
        public static string Fmt_Rs(string jText)
        {
            jText = jText.Trim();
            jText = jText.Trim('[');
            jText = jText.Trim(']');
            return jText;
        }

        #endregion


        public static string Serialize(object obj)
        {
            return JsonConvert.SerializeObject(obj, new IsoDateTimeConverter { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
        }

        public static string SerializeByConverter(object obj, params JsonConverter[] converters)
        {
            return JsonConvert.SerializeObject(obj, converters);
        }

        public static T Deserialize<T>(string input)
        {
            return JsonConvert.DeserializeObject<T>(input);
        }

        public static T DeserializeByConverter<T>(string input, params JsonConverter[] converter)
        {
            return JsonConvert.DeserializeObject<T>(input, converter);
        }

        public static T DeserializeBySetting<T>(string input, JsonSerializerSettings settings)
        {
            return JsonConvert.DeserializeObject<T>(input, settings);
        }
        public static bool DeserializeJsonToObject<T>(object result, out T t)
        {       
            t=JsonConvert.DeserializeObject<T>(result.ToString());
            return t==null?false:true;
        }
        private object NullToEmpty(object obj)
        {
            return null;
        }
    }
}
