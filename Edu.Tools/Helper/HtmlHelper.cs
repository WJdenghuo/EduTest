using System.Collections.Generic;
using System.Text.RegularExpressions;


namespace Edu.Tools
{
    public static class HtmlHelper
    {
        public static string NoHTML(string Htmlstring)
        {
            //删除脚本
            Htmlstring = Regex.Replace(Htmlstring, @"<script[^>]*?>.*?</script>", "",
              RegexOptions.IgnoreCase);
            //删除HTML
            Htmlstring = Regex.Replace(Htmlstring, @"<(.[^>]*)>", "",
              RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"([\r\n])[\s]+", "",
              RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"-->", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"<!--.*", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(quot|#34);", "\"",
              RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(amp|#38);", "&",
              RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(lt|#60);", "<",
              RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(gt|#62);", ">",
              RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(nbsp|#160);", "   ",
              RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(iexcl|#161);", "\xa1",
              RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(cent|#162);", "\xa2",
              RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(pound|#163);", "\xa3",
              RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(copy|#169);", "\xa9",
              RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&#(\d+);", "",
              RegexOptions.IgnoreCase);

            Htmlstring.Replace("<", "");
            Htmlstring.Replace(">", "");

            return Htmlstring;
        }




        /// <summary>
        /// 去除HTML标记 不包含图片和表格
        /// </summary>
        /// <param name="Htmlstring">包括HTML的源码 </param>
        /// <returns>已经去除后的文字</returns>
        public static string NoHTMLNoPicTable(string Htmlstring)
        {
            MatchCollection mcImg = Regex.Matches(Htmlstring, @"<(\bimg\b)[^>]*>");
            List<string> imgList = new List<string>();
            for (int i = 0; i < mcImg.Count; i++)
            {
                imgList.Add(mcImg[i].Value);
            }
            Htmlstring = Regex.Replace(Htmlstring, @"<(\bimg\b)[^>]*>", "imgHtmlString", RegexOptions.IgnoreCase);

            MatchCollection mctable = Regex.Matches(Htmlstring, @"<table[\s\S]*?>[\s\S]*?<\/table>");
            List<string> tableList = new List<string>();
            for (int i = 0; i < mctable.Count; i++)
            {
                tableList.Add(mctable[i].Value);
            }
            Htmlstring = Regex.Replace(Htmlstring, @"<table[\s\S]*?>[\s\S]*?<\/table>", "tableHtmlString", RegexOptions.IgnoreCase);

            Htmlstring = Regex.Replace(Htmlstring, @"<title>[\s\S]*</title>|<[^>]*>", "", RegexOptions.IgnoreCase);
            //Htmlstring = Regex.Replace(Htmlstring, @"<(?!\bimg\b)[^>]*>", "", RegexOptions.IgnoreCase);
            //换行替换br
            Htmlstring = Regex.Replace(Htmlstring, @"[\r\n]+", "<br/>", RegexOptions.IgnoreCase);
            //将img标签内的br 去掉
            //Htmlstring = Regex.Replace(Htmlstring, @"\b<*<br/>*>\b", " ", RegexOptions.IgnoreCase);

            //Htmlstring = Regex.Replace(Htmlstring, @"-->", "", RegexOptions.IgnoreCase);
            //Htmlstring = Regex.Replace(Htmlstring, @"<!--.*", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(quot|#34);", "\"", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(amp|#38);", "&", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(lt|#60);", "<", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(gt|#62);", ">", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(nbsp|#160);", " ", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(iexcl|#161);", "\xa1", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(cent|#162);", "\xa2", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(pound|#163);", "\xa3", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(copy|#169);", "\xa9", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&#(\d+);", "", RegexOptions.IgnoreCase);



            Regex r = new Regex("tableHtmlString");
            for (int i = 0; i < tableList.Count; i++)
            {
                Htmlstring = r.Replace(Htmlstring, tableList[i], 1, 0);
            }
            Regex r1 = new Regex("imgHtmlString");
            for (int i = 0; i < imgList.Count; i++)
            {
                Htmlstring = r1.Replace(Htmlstring, imgList[i], 1, 0);
            }
            //双引号换成单引号
            Htmlstring = Regex.Replace(Htmlstring, "\"", "\'", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"src='", "src='/html/", RegexOptions.IgnoreCase);

            Htmlstring = Regex.Replace(Htmlstring, @"<br/>src", "src", RegexOptions.IgnoreCase);

            return Htmlstring;
        }

        /// <summary>
        /// 对字符串进行 HTML 编码操作
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns></returns>
        public static string strEncode(string str)
        {
            str = str.Replace("&", "&amp;");
            str = str.Replace("'", "''");
            str = str.Replace("\"", "&quot;");
            str = str.Replace(" ", "&nbsp;");
            str = str.Replace("<", "&lt;");
            str = str.Replace(">", "&gt;");
            str = str.Replace("\n", "<br>");
            return str;
        }


        /// <summary>
        /// 对 HTML 字符串进行解码操作
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns></returns>
        public static string strDecode(string str)
        {
            str = str.Replace("<br>", "\n");
            str = str.Replace("&gt;", ">");
            str = str.Replace("&lt;", "<");
            str = str.Replace("&nbsp;", " ");
            str = str.Replace("&quot;", "\"");
            return str;
        }


    }
}
