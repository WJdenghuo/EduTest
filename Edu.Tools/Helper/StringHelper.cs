using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Edu.Tools
{
    public class StringHelper
    {
        #region 分割字符串

        /// <summary>
        /// 分割字符串
        /// </summary>
        /// <param name="sourceStr">源字符串</param>
        /// <param name="splitStr">分隔字符串</param>
        /// <returns></returns>
        public static string[] SplitString(string sourceStr, string splitStr)
        {
            if (string.IsNullOrEmpty(sourceStr) || string.IsNullOrEmpty(splitStr))
                return new string[0] { };

            if (sourceStr.IndexOf(splitStr) == -1)
                return new string[] { sourceStr };

            if (splitStr.Length == 1)
                return sourceStr.Split(splitStr[0]);
            else
                return Regex.Split(sourceStr, Regex.Escape(splitStr), RegexOptions.IgnoreCase);

        }

        /// <summary>
        /// 分割字符串
        /// </summary>
        /// <param name="sourceStr">源字符串</param>
        /// <returns></returns>
        public static string[] SplitString(string sourceStr)
        {
            return SplitString(sourceStr, ",");
        }

        #endregion

        #region 截取字符串

        /// <summary>
        /// 截取字符串
        /// </summary>
        /// <param name="sourceStr">源字符串</param>
        /// <param name="startIndex">开始位置的索引</param>
        /// <param name="length">子字符串的长度</param>
        /// <returns></returns>
        public static string SubString(string sourceStr, int startIndex, int length)
        {
            if (!string.IsNullOrEmpty(sourceStr))
            {
                if (sourceStr.Length >= (startIndex + length))
                    return sourceStr.Substring(startIndex, length);
                else
                    return sourceStr.Substring(startIndex);
            }

            return "";
        }

        /// <summary>
        /// 截取字符串
        /// </summary>
        /// <param name="sourceStr">源字符串</param>
        /// <param name="length">子字符串的长度</param>
        /// <returns></returns>
        public static string SubString(string sourceStr, int length)
        {
            return SubString(sourceStr, 0, length);
        }

        /// <summary>
        /// 根据正则规则得到中间部分
        /// </summary>
        /// <param name="stext">文本</param>
        /// <param name="regular">规则</param>
        /// <returns></returns>
        public static Dictionary<string, string> GetMidLine(string stext, string regular, bool isBegin = true)
        {

            Dictionary<string, string> dic = new Dictionary<string, string>(); //存放分割后字符
            Regex r = new Regex(regular, RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.IgnorePatternWhitespace);
            MatchCollection mc = r.Matches(stext);
            if (mc.Count >= 1)
            {
                for (int i = 0; i < mc.Count - 1; i++)
                {
                    string sKey = mc[i].Value;
                    string sValue = stext.Substring(mc[i].Index, mc[i + 1].Index - mc[i].Index);
                    if (isBegin == false)
                    {
                        sValue = GetBegTrimStr(sValue, sKey);
                    }
                    if (!dic.Keys.Contains(sKey))
                    {
                        dic.Add(sKey, sValue);
                    }
                }
                //dic.Add(mc[mc.Count - 1].Value, stext.Substring(mc[mc.Count - 1].Index, stext.Length - mc[mc.Count - 1].Index));
                if (!dic.Keys.Contains(mc[mc.Count - 1].Value))
                {
                    if (isBegin == false)
                    {
                        dic.Add(mc[mc.Count - 1].Value, GetBegTrimStr(stext.Substring(mc[mc.Count - 1].Index), mc[mc.Count - 1].Value));
                    }
                    else
                    {
                        dic.Add(mc[mc.Count - 1].Value, stext.Substring(mc[mc.Count - 1].Index));
                    }
                }
            }
            return dic;
        }
        /// <summary>
        /// 移除前面字符
        /// </summary>
        /// <param name="stext"></param>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string GetBegTrimStr(string stext, string str)
        {
            stext = stext.Trim();
            while (stext.IndexOf(str) == 0)
            {
                stext = stext.Substring(str.Length);
            }
            return stext;
        }


        /// <summary>
        /// 根据规则得到中间部分
        /// </summary>
        /// <param name="stext">文本</param>
        /// <param name="regular1">开始部分</param>
        /// <param name="regular2">结束规则</param>
        /// <param name="svaule">匹配值</param>
        /// <returns></returns>
        public static string GetMidString(string stext, string regular1, string regular2, out string svaule)
        {
            Regex r1 = new Regex(regular1, RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.IgnorePatternWhitespace);
            Match m1 = r1.Match(stext);
            string sReturn = string.Empty;
            if (m1.Success)
            {
                svaule = m1.Value;
                stext = stext.Substring(m1.Index + m1.Value.Length);
                Regex r2 = new Regex(regular2, RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.IgnorePatternWhitespace);
                Match m2 = r2.Match(stext);

                if (m2.Success)
                {
                    sReturn = stext.Substring(0, m2.Index);
                }
                else
                {
                    sReturn = stext.Substring(0);
                }
            }
            else
            {
                svaule = string.Empty;
            }
            return sReturn;
        }


        public static string GetTrimStr(string stext, string str)
        {
            stext = stext.Trim();
            while (stext.IndexOf(str) == 0)
            {
                stext = stext.Substring(str.Length);
            }
            while (stext.LastIndexOf(str) > 0 && stext.LastIndexOf(str) == (stext.Length - str.Length))
            {
                stext = stext.Remove(stext.LastIndexOf(str)).Trim();
            }
            return stext;
        }


        /// <summary>
        /// 根据正则规则得到中间部分
        /// </summary>
        /// <param name="stext">文本</param>
        /// <param name="regular1">开始规则</param>
        /// <param name="regular2">结束规则</param>
        /// <returns></returns>
        public static string GetMidLine(string stext, string regular1, string regular2)
        {
            Regex r1 = new Regex(regular1, RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.IgnorePatternWhitespace);
            Match m1 = r1.Match(stext);
            string sReturn = string.Empty;
            if (m1.Success)
            {
                stext = stext.Substring(m1.Index + m1.Value.Length);
                Regex r2 = new Regex(regular2, RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.IgnorePatternWhitespace);
                Match m2 = r2.Match(stext);

                if (m2.Success)
                {
                    sReturn = stext.Substring(0, m2.Index);
                }
                else
                {
                    sReturn = stext.Substring(0);
                }
            }
            return sReturn;
        }

        public static List<string> GetMidRegular(string stext, string regular)
        {
            List<string> list = new List<string>(); //存放分割后字符
            Regex r = new Regex(regular, RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.IgnorePatternWhitespace);
            MatchCollection mc = r.Matches(stext);
            if (mc.Count > 1)
            {
                foreach (Match m in mc)
                {
                    list.Add(m.Value);
                }
            }
            return list;
        }
        #endregion

        #region 移除前导/后导字符串

        /// <summary>
        /// 移除前导字符串
        /// </summary>
        /// <param name="sourceStr">源字符串</param>
        /// <param name="trimStr">移除字符串</param>
        /// <returns></returns>
        public static string TrimStart(string sourceStr, string trimStr)
        {
            return TrimStart(sourceStr, trimStr, true);
        }

        /// <summary>
        /// 移除前导字符串
        /// </summary>
        /// <param name="sourceStr">源字符串</param>
        /// <param name="trimStr">移除字符串</param>
        /// <param name="ignoreCase">是否忽略大小写</param>
        /// <returns></returns>
        public static string TrimStart(string sourceStr, string trimStr, bool ignoreCase)
        {
            if (string.IsNullOrEmpty(sourceStr))
                return string.Empty;

            if (string.IsNullOrEmpty(trimStr) || !sourceStr.StartsWith(trimStr, ignoreCase, CultureInfo.CurrentCulture))
                return sourceStr;

            return sourceStr.Remove(0, trimStr.Length);
        }

        /// <summary>
        /// 移除后导字符串
        /// </summary>
        /// <param name="sourceStr">源字符串</param>
        /// <param name="trimStr">移除字符串</param>
        /// <returns></returns>
        public static string TrimEnd(string sourceStr, string trimStr)
        {
            return TrimEnd(sourceStr, trimStr, true);
        }

        /// <summary>
        /// 移除后导字符串
        /// </summary>
        /// <param name="sourceStr">源字符串</param>
        /// <param name="trimStr">移除字符串</param>
        /// <param name="ignoreCase">是否忽略大小写</param>
        /// <returns></returns>
        public static string TrimEnd(string sourceStr, string trimStr, bool ignoreCase)
        {
            if (string.IsNullOrEmpty(sourceStr))
                return string.Empty;

            if (string.IsNullOrEmpty(trimStr) || !sourceStr.EndsWith(trimStr, ignoreCase, CultureInfo.CurrentCulture))
                return sourceStr;

            return sourceStr.Substring(0, sourceStr.Length - trimStr.Length);
        }

        /// <summary>
        /// 移除前导和后导字符串
        /// </summary>
        /// <param name="sourceStr">源字符串</param>
        /// <param name="trimStr">移除字符串</param>
        /// <returns></returns>
        public static string Trim(string sourceStr, string trimStr)
        {
            return Trim(sourceStr, trimStr, true);
        }

        /// <summary>
        /// 移除前导和后导字符串
        /// </summary>
        /// <param name="sourceStr">源字符串</param>
        /// <param name="trimStr">移除字符串</param>
        /// <param name="ignoreCase">是否忽略大小写</param>
        /// <returns></returns>
        public static string Trim(string sourceStr, string trimStr, bool ignoreCase)
        {
            if (string.IsNullOrEmpty(sourceStr))
                return string.Empty;

            if (string.IsNullOrEmpty(trimStr))
                return sourceStr;

            if (sourceStr.StartsWith(trimStr, ignoreCase, CultureInfo.CurrentCulture))
                sourceStr = sourceStr.Remove(0, trimStr.Length);

            if (sourceStr.EndsWith(trimStr, ignoreCase, CultureInfo.CurrentCulture))
                sourceStr = sourceStr.Substring(0, sourceStr.Length - trimStr.Length);

            return sourceStr;
        }

        #endregion

        #region 格式转换
        /// <summary>
        /// 将string[]转换成int
        /// </summary>
        /// <param name="str"></param>
        /// <param name="c"></param>
        /// <returns></returns>
        public static List<int> GetListInt(string[] str)
        {
            List<int> lsInt = new List<int>();
            if (str == null || str.Length == 0)
            {
                return lsInt;
            }


            foreach (var item in str)
            {

                if (item != "")
                {
                    try
                    {
                        lsInt.Add(int.Parse(item.Trim()));
                    }
                    catch { }
                }
            }
            return lsInt;
        }
        /// <summary>
        /// 将string转换成int
        /// </summary>
        /// <param name="str"></param>
        /// <param name="c"></param>
        /// <returns></returns>
        public static List<int> GetListInt(string str, char c)
        {
            List<int> lsInt = new List<int>();
            if (str == "" || str == null)
            {
                return lsInt;
            }

            var strList = str.Split(c);
            foreach (var item in strList)
            {
                if (item != "")
                {
                    try
                    {
                        lsInt.Add(int.Parse(item));
                    }
                    catch { }
                }
            }
            return lsInt;
        }
        /// <summary>
        /// 将string转换成double
        /// </summary>
        /// <param name="str"></param>
        /// <param name="c"></param>
        /// <returns></returns>
        public static List<double> GetListDouble(string str, char c)
        {
            List<double> lsInt = new List<double>();
            if (str == "" || str == null)
            {
                return lsInt;
            }

            var strList = str.Split(c);
            foreach (var item in strList)
            {
                if (item != "")
                {
                    try
                    {
                        lsInt.Add(Convert.ToDouble(item));
                    }
                    catch { }
                }
            }
            return lsInt;
        }
        /// <summary>
        /// 得到List<int>
        /// </summary>
        /// <param name="str"></param>
        /// <param name="c"></param>
        /// <returns></returns>
        public static List<int> GetListInt(object obj, char c)
        {
            if (obj == null || string.IsNullOrEmpty(obj.ToString()))
            {
                return null;
            }
            string str = obj.ToString();
            List<int> lsInt = new List<int>();
            var strList = str.Split(c);
            foreach (var item in strList)
            {
                if (item != "")
                {
                    try
                    {
                        lsInt.Add(int.Parse(item));
                    }
                    catch { }
                }
            }
            return lsInt;
        }
        /// <summary>
        /// 得到List<int>
        /// </summary>
        /// <param name="str"></param>
        /// <param name="c"></param>
        /// <returns></returns>
        public static List<string> GetListString(IEnumerable<int?> obj)
        {
            if (obj == null || string.IsNullOrEmpty(obj.ToString()))
            {
                return null;
            }
            List<string> list = new List<string>();
            foreach (var item in obj)
            {
                if (item != null)
                {
                    try
                    {
                        list.Add(item.ToString());
                    }
                    catch { }
                }
            }
            return list;
        }

        /// <summary>
        /// 数组数字转换成字符串
        /// </summary>
        /// <returns></returns>
        public static List<string> ConvertIntListToList(IEnumerable<int> list)
        {
            if (list == null)
            {
                return null;
            }
            List<string> ls = new List<string>();
            foreach (var item in list)
            {
                ls.Add(item.ToString());
            }
            return ls;
        }
        /// <summary>
        /// 将List 转成String
        /// </summary>
        /// <returns></returns>
        public static string ConvertListToStr(IEnumerable<string> list)
        {
            if (list == null || list.Count() == 0)
            {
                return "";
            }
            string BacStr = "";
            foreach (var item in list)
            {
                BacStr += item + ",";
            }
            if (BacStr.Trim() == "无")
            { return ""; }
            if (BacStr.Trim() == "无,")
            { return ""; }
            if (BacStr.Trim() == "无;")
            { return ""; }
            if (BacStr.Trim() == "无，")
            { return ""; }
            return BacStr.TrimEnd(',');
        }
        /// <summary>
        /// 将List 转成String
        /// </summary>
        /// <returns></returns>
        public static string ConvertListToStr(List<string> list)
        {
            if (list == null || list.Count() == 0)
            {
                return "";
            }
            string BacStr = "";
            foreach (var item in list)
            {
                BacStr += item;
            }
            return BacStr;
        }
        /// <summary>
        /// 将List 转成String
        /// </summary>
        /// <returns></returns>
        public static string ConvertListToStr(List<string> list, char c)
        {
            if (list == null || list.Count() == 0)
            {
                return "";
            }
            string BacStr = "";
            foreach (var item in list)
            {
                BacStr += item + c;
            }
            return BacStr;
        }


        public static List<string> ConvertStrsToList(string[] strs)
        {
            if (strs == null || strs.Count() == 0)
            {
                return null;
            }
            List<string> ls = new List<string>();
            foreach (var item in strs)
            {
                ls.Add(item);
            }
            return ls;

        }
        #endregion

        //public static string GetIP()
        //{
        //    string userIP = "NULL";

        //    try
        //    {
        //        if (HttpContext.Current == null
        //         || HttpContext.Current.Request == null
        //         || HttpContext.Current.Request.ServerVariables == null)
        //        {
        //            return "";
        //        }

        //        string CustomerIP = "";

        //        //CDN加速后取到的IP simone 090805
        //        CustomerIP = HttpContext.Current.Request.Headers["Cdn-Src-Ip"];
        //        if (!string.IsNullOrEmpty(CustomerIP))
        //        {
        //            return CustomerIP;
        //        }

        //        CustomerIP = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

        //        if (!String.IsNullOrEmpty(CustomerIP))
        //        {
        //            return CustomerIP;
        //        }

        //        if (HttpContext.Current.Request.ServerVariables["HTTP_VIA"] != null)
        //        {
        //            CustomerIP = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

        //            if (CustomerIP == null)
        //            {
        //                CustomerIP = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
        //            }
        //        }
        //        else
        //        {
        //            CustomerIP = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
        //        }

        //        if (string.Compare(CustomerIP, "unknown", true) == 0 || String.IsNullOrEmpty(CustomerIP))
        //        {
        //            return HttpContext.Current.Request.UserHostAddress;
        //        }

        //        if (CustomerIP == "::1") { CustomerIP = "127.0.0.1"; }
        //        return CustomerIP;
        //    }
        //    catch { }

        //    return userIP;

        //}

        #region 字节转换
        /// <summary>
        /// 计算文件大小函数(保留两位小数),Size为字节大小
        /// </summary>
        /// <param name="Size">初始文件大小</param>
        /// <returns></returns>
        public static string CountStringSize(string Size)
        {
            if (Size == null || Size == "")
            {
                return "";
            }
            string m_strSize = "";
            long FactSize = 0;
            try
            {
                FactSize = Convert.ToInt64(Size);
            }
            catch
            {
                return "";
            }
            if (FactSize < 1048576)
                m_strSize = (FactSize / 1024.00).ToString("F2") + " K";
            else if (FactSize >= 1048576 && FactSize < 1073741824)
                m_strSize = (FactSize / 1024.00 / 1024.00).ToString("F2") + " M";
            else if (FactSize >= 1073741824)
                m_strSize = (FactSize / 1024.00 / 1024.00 / 1024.00).ToString("F2") + " G";
            return m_strSize;
        }
        public static string CountSize(double? Size)
        {
            if (Size == null)
            {
                return "";
            }
            string m_strSize = "";
            double FactSize = 0;
            FactSize = Size.Value;
            if (FactSize < 1024.00)
                m_strSize = FactSize.ToString("F2") + " Byte";
            else if (FactSize >= 1024.00 && FactSize < 1048576)
                m_strSize = (FactSize / 1024.00).ToString("F2") + " K";
            else if (FactSize >= 1048576 && FactSize < 1073741824)
                m_strSize = (FactSize / 1024.00 / 1024.00).ToString("F2") + " M";
            else if (FactSize >= 1073741824)
                m_strSize = (FactSize / 1024.00 / 1024.00 / 1024.00).ToString("F2") + " G";
            return m_strSize;
        }
        /// <summary>
        /// 计算文件大小函数(保留两位小数),Size为字节大小
        /// </summary>
        /// <param name="Size">初始文件大小</param>
        /// <returns></returns>
        public static string CountSize(long Size)
        {
            string m_strSize = "";
            long FactSize = 0;
            FactSize = Size;
            if (FactSize < 1024.00)
                m_strSize = FactSize.ToString("F2") + " Byte";
            else if (FactSize >= 1024.00 && FactSize < 1048576)
                m_strSize = (FactSize / 1024.00).ToString("F2") + " K";
            else if (FactSize >= 1048576 && FactSize < 1073741824)
                m_strSize = (FactSize / 1024.00 / 1024.00).ToString("F2") + " M";
            else if (FactSize >= 1073741824)
                m_strSize = (FactSize / 1024.00 / 1024.00 / 1024.00).ToString("F2") + " G";
            return m_strSize;
        }

        #endregion


        #region 字典操作 得到字典中value
        public static string GetDicValueByKey(Dictionary<string, string> dic, string sKey)
        {
            if (dic.Keys.Contains(sKey))
            {
                return dic[sKey];
            }
            else
            {
                return string.Empty;
            }
        }
        public static string GetDicValueByKey(Dictionary<int, string> dic, int sKey)
        {
            if (dic.Keys.Contains(sKey))
            {
                return dic[sKey];
            }
            else
            {
                return string.Empty;
            }
        }
        public static string GetDicValueByKeys(Dictionary<int, string> dic, string sKey, char c)
        {
            var list = GetListInt(sKey, c);
            StringBuilder sb = new StringBuilder();

            foreach (var item in list)
            {
                sb.Append(dic[item] + ",");
            }
            return sb.ToString().TrimEnd(c);
        }
        #endregion

    }
}
