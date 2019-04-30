using System;
using System.Text.RegularExpressions;

namespace Edu.Tools
{
    /// <summary>
    /// 类型帮助类
    /// </summary>
    public class TypeHelper
    {
        #region 转Int

        /// <summary>
        /// 将string类型转换成int类型
        /// </summary>
        /// <param name="s">目标字符串</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns></returns>
        public static int StringToInt(string s, int defaultValue)
        {
            if (!string.IsNullOrWhiteSpace(s))
            {
                int result;
                if (int.TryParse(s, out result))
                    return result;
            }

            return defaultValue;
        }

        /// <summary>
        /// 将string类型转换成int类型
        /// </summary>
        /// <param name="s">目标字符串</param>
        /// <returns></returns>
        public static int StringToInt(string s)
        {
            return StringToInt(s, 0);
        }

        /// <summary>
        /// 将object类型转换成int类型
        /// </summary>
        /// <param name="s">目标对象</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns></returns>
        public static int ObjectToInt(object o, int defaultValue)
        {
            if (o != null)
                return StringToInt(o.ToString(), defaultValue);

            return defaultValue;
        }

        /// <summary>
        /// 将object类型转换成int类型
        /// </summary>
        /// <param name="s">目标对象</param>
        /// <returns></returns>
        public static int ObjectToInt(object o)
        {
            return ObjectToInt(o, 0);
        }

        #endregion

        #region 转Bool

        /// <summary>
        /// 将string类型转换成bool类型
        /// </summary>
        /// <param name="s">目标字符串</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns></returns>
        public static bool StringToBool(string s, bool defaultValue)
        {
            if (s == "false")
                return false;
            else if (s == "true")
                return true;

            return defaultValue;
        }

        /// <summary>
        /// 将string类型转换成bool类型
        /// </summary>
        /// <param name="s">目标字符串</param>
        /// <returns></returns>
        public static bool ToBool(string s)
        {
            return StringToBool(s, false);
        }

        /// <summary>
        /// 将object类型转换成bool类型
        /// </summary>
        /// <param name="s">目标对象</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns></returns>
        public static bool ObjectToBool(object o, bool defaultValue)
        {
            if (o != null)
                return StringToBool(o.ToString(), defaultValue);

            return defaultValue;
        }

        /// <summary>
        /// 将object类型转换成bool类型
        /// </summary>
        /// <param name="s">目标对象</param>
        /// <returns></returns>
        public static bool ObjectToBool(object o)
        {
            return ObjectToBool(o, false);
        }

        #endregion

        #region 转DateTime

        /// <summary>
        /// 将string类型转换成datetime类型
        /// </summary>
        /// <param name="s">目标字符串</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns></returns>
        public static DateTime StringToDateTime(string s, DateTime defaultValue)
        {
            if (!string.IsNullOrWhiteSpace(s))
            {
                DateTime result;
                if (DateTime.TryParse(s, out result))
                    return result;
            }
            return defaultValue;
        }

        /// <summary>
        /// 将string类型转换成datetime类型
        /// </summary>
        /// <param name="s">目标字符串</param>
        /// <returns></returns>
        public static DateTime StringToDateTime(string s)
        {
            return StringToDateTime(s, DateTime.Now);
        }

        /// <summary>
        /// 将object类型转换成datetime类型
        /// </summary>
        /// <param name="s">目标对象</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns></returns>
        public static DateTime ObjectToDateTime(object o, DateTime defaultValue)
        {
            if (o != null)
                return StringToDateTime(o.ToString(), defaultValue);

            return defaultValue;
        }

        /// <summary>
        /// 将object类型转换成datetime类型
        /// </summary>
        /// <param name="s">目标对象</param>
        /// <returns></returns>
        public static DateTime ObjectToDateTime(object o)
        {
            return ObjectToDateTime(o, DateTime.Now);
        }

        #endregion

        #region 转Decimal

        /// <summary>
        /// 将string类型转换成decimal类型
        /// </summary>
        /// <param name="s">目标字符串</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns></returns>
        public static decimal StringToDecimal(string s, decimal defaultValue)
        {
            if (!string.IsNullOrWhiteSpace(s))
            {
                decimal result;
                if (decimal.TryParse(s, out result))
                    return result;
            }

            return defaultValue;
        }

        /// <summary>
        /// 将string类型转换成decimal类型
        /// </summary>
        /// <param name="s">目标字符串</param>
        /// <returns></returns>
        public static decimal StringToDecimal(string s)
        {
            return StringToDecimal(s, 0m);
        }

        /// <summary>
        /// 将object类型转换成decimal类型
        /// </summary>
        /// <param name="s">目标对象</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns></returns>
        public static decimal ObjectToDecimal(object o, decimal defaultValue)
        {
            if (o != null)
                return StringToDecimal(o.ToString(), defaultValue);

            return defaultValue;
        }

        /// <summary>
        /// 将object类型转换成decimal类型
        /// </summary>
        /// <param name="s">目标对象</param>
        /// <returns></returns>
        public static decimal ObjectToDecimal(object o)
        {
            return ObjectToDecimal(o, 0m);
        }

        #endregion

        /// <summary>
        /// 将中文数字替换为阿拉伯数字
        /// </summary>
        /// <param name="word"></param>
        /// <returns></returns>
        public static string WordToNumber(string word)
        {
            string e = "([零一二三四五六七八九十百千万亿])+";
            MatchCollection mc = Regex.Matches(word, e);

            foreach (Match m in mc)
            {
                word = word.Replace(m.Value, Word2Number(m.Value));
            }
            return word;
        }

        private static string Word2Number(string w)
        {
            if (w == "")
                return w;

            string e = "零一二三四五六七八九";
            string[] ew = new string[] { "十", "百", "千" };
            string ewJoin = "十百千";
            string[] ej = new string[] { "万", "亿" };

            string rss = "^([" + e + ewJoin + "]+" + ej[1] + ")?([" + e
                + ewJoin + "]+" + ej[0] + ")?([" + e + ewJoin + "]+)?$";
            string[] mcollect = Regex.Split(w, rss);
            if (mcollect.Length < 4)
                return w;
            return (
                Convert.ToInt64(foh(mcollect[1])) * 100000000 +
                Convert.ToInt64(foh(mcollect[2])) * 10000 +
                Convert.ToInt64(foh(mcollect[3]))
                ).ToString();
        }

        private static int foh(string str)
        {
            string e = "零一二三四五六七八九";
            string[] ew = new string[] { "十", "百", "千" };
            string[] ej = new string[] { "万", "亿" };

            int a = 0;
            if (str.IndexOf(ew[0]) == 0)
                a = 10;
            str = Regex.Replace(str, e[0].ToString(), "");

            if (Regex.IsMatch(str, "([" + e + "])$"))
            {
                a += e.IndexOf(Regex.Match(str, "([" + e + "])$").Value[0]);
            }

            if (Regex.IsMatch(str, "([" + e + "])" + ew[0]))
            {
                a += e.IndexOf(Regex.Match(str, "([" + e + "])" + ew[0]).Value[0]) * 10;
            }

            if (Regex.IsMatch(str, "([" + e + "])" + ew[1]))
            {
                a += e.IndexOf(Regex.Match(str, "([" + e + "])" + ew[1]).Value[0]) * 100;
            }

            if (Regex.IsMatch(str, "([" + e + "])" + ew[2]))
            {
                a += e.IndexOf(Regex.Match(str, "([" + e + "])" + ew[2]).Value[0]) * 1000;
            }
            return a;
        }
        public static string ArabicNumeralsToChinese(string str)
        {
            string temp = string.Empty;
            string strconvert = string.Empty;

            for (int i = 0; i < str.Length; i++)
            {
                switch (int.Parse(str.Substring(i, 1)))
                {

                    case 0: temp = "○"; break;
                    case 1: temp = "一"; break;
                    case 2: temp = "二"; break;
                    case 3: temp = "三"; break;
                    case 4: temp = "四"; break;
                    case 5: temp = "五"; break;
                    case 6: temp = "六"; break;
                    case 7: temp = "七"; break;
                    case 8: temp = "八"; break;
                    case 9: temp = "九"; break;
                    default: temp = ""; break;
                }
                strconvert += temp;
            }
            return strconvert;
        }
    }
}
