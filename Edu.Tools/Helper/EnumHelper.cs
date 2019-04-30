using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Edu.Tools
{
    /// <summary>
    /// 处理枚举类
    /// </summary>
    public static class EnumHelper
    {
        /// <summary>
        /// 枚举类型显示描述名称
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="helper"></param>
        /// <param name="_value"></param>
        /// <returns></returns>
        public static string EnumDescriptionText<TEnum>(TEnum t)
        {
            if (t == null)
            {
                return "";
            }
            IEnumerable<TEnum> values = Enum.GetValues(typeof(TEnum))
              .Cast<TEnum>();
            var query = values.FirstOrDefault(e => e.Equals(t));
            if (query == null)
                return "";
            else
            {
                return EnumDescription.GetFieldText(query);
            }
        }
     
        /// <summary>
        /// 枚举类型显示描述名称
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="helper"></param>
        /// <param name="_value"></param>
        /// <returns></returns>
        public static string EnumDescriptionText<TEnum>(int? _value)
        {
            IEnumerable<TEnum> values = Enum.GetValues(typeof(TEnum))
              .Cast<TEnum>();
            var query = values.FirstOrDefault(e => Convert.ToInt32(e) == _value);
            if (query == null)
                return "";
            else
            {
                return EnumDescription.GetFieldText(query);
            }
        }
        /// <summary>
        /// 枚举类型显示描述名称
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="helper"></param>
        /// <param name="_value"></param>
        /// <returns></returns>
        public static string EnumMultipleDescriptionText<TEnum>(string _value, char c = ',')
        {
            StringBuilder sb = new StringBuilder();
            var list = StringHelper.GetListInt(_value, c);
            foreach (var item in list)
            {

                sb.Append(EnumDescriptionText<TEnum>(item) + c);
            }
            return sb.ToString().TrimEnd(c);
        }
        /// <summary>
        /// 枚举类型显示描述名称
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="helper"></param>
        /// <param name="_value"></param>
        /// <returns></returns>
        public static string EnumDescriptionText<TEnum>(string _value)
        {
            IEnumerable<TEnum> values = Enum.GetValues(typeof(TEnum))
              .Cast<TEnum>();
            var query = values.FirstOrDefault(e => Convert.ToInt32(e) == int.Parse(_value));
            if (query == null)
                return "";
            else
            {
                return EnumDescription.GetFieldText(query);
            }
        }
     
    }
}
