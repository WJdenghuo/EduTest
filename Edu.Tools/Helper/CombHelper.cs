 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Edu.Tools
{
    /// <summary>
    ///     COMB（有序的Guid，Guid与时间混合型）类型操作类
    /// </summary>
    public static class CombHelper
    {
        /// <summary>
        ///     返回Guid用于数据库操作，特定的时间代码可以提高检索效率
        /// </summary>
        /// <returns>COMB类型 Guid 数据</returns>
        public static Guid NewComb()
        {
            byte[] guidArray = Guid.NewGuid().ToByteArray();
            DateTime dtBase = new DateTime(1900, 1, 1);
            DateTime dtNow = DateTime.Now;
            //获取用于生成byte字符串的天数与毫秒数
            TimeSpan days = new TimeSpan(dtNow.Ticks - dtBase.Ticks);
            TimeSpan msecs = new TimeSpan(dtNow.Ticks - (new DateTime(dtNow.Year, dtNow.Month, dtNow.Day).Ticks));
            //转换成byte数组
            //注意SqlServer的时间计数只能精确到1/300秒
            byte[] daysArray = BitConverter.GetBytes(days.Days);
            byte[] msecsArray = BitConverter.GetBytes((long)(msecs.TotalMilliseconds / 3.333333));

            //反转字节以符合SqlServer的排序
            Array.Reverse(daysArray);
            Array.Reverse(msecsArray);

            //把字节复制到Guid中
            Array.Copy(daysArray, daysArray.Length - 2, guidArray, guidArray.Length - 6, 2);
            Array.Copy(msecsArray, msecsArray.Length - 4, guidArray, guidArray.Length - 4, 4);
            return new Guid(guidArray);
        }

        /// <summary>
        ///     从SQL Server 返回的Guid中生成时间信息
        /// </summary>
        public static DateTime GetDateFromComb(Guid id)
        {
            DateTime baseDate = new DateTime(1900, 1, 1);
            byte[] daysArray = new byte[4];
            byte[] msecsArray = new byte[4];
            byte[] guidArray = id.ToByteArray();

            // Copy the date parts of the guid to the respective byte arrays. 
            Array.Copy(guidArray, guidArray.Length - 6, daysArray, 2, 2);
            Array.Copy(guidArray, guidArray.Length - 4, msecsArray, 0, 4);

            // Reverse the arrays to put them into the appropriate order 
            Array.Reverse(daysArray);
            Array.Reverse(msecsArray);

            // Convert the bytes to ints 
            int days = BitConverter.ToInt32(daysArray, 0);
            int msecs = BitConverter.ToInt32(msecsArray, 0);

            DateTime date = baseDate.AddDays(days);
            date = date.AddMilliseconds(msecs * 3.333333);

            return date;
        }
        /// <summary>
        /// 生成一个长整型，可以转成19字节长的字符串
        /// </summary>
        /// <returns>System.Int64.</returns>
        public static long GenerateLong()
        {
            byte[] buffer = Guid.NewGuid().ToByteArray();
            return BitConverter.ToInt64(buffer, 0);
        }
        /// <summary>
        /// 生成16个字节长度的数据与英文组合串
        /// </summary>
        public static string GenerateStr()
        {
            long i = 1;

            foreach (byte b in Guid.NewGuid().ToByteArray())
            {
                i *= ((int)b + 1);
            }

            return string.Format("{0:x}", i - DateTime.Now.Ticks);
        }
        /// <summary>
        /// 唯一时间命名
        /// </summary>
        /// <returns></returns>
        public static string GenerateNumber()
        {
            return DateTime.Now.ToString("yyyyMMddHHmmssff");
        }
        /// <summary>
        /// 唯一订单号生成
        /// </summary>
        /// <returns></returns>
        public static string GenerateOrderNumber()
        {
            string strDateTimeNumber = DateTime.Now.ToString("yyyyMMddHHmmssffff");
            string strRandomResult = NextRandom(1000, 1).ToString("0000");

            return strDateTimeNumber + strRandomResult;
        }
        /// <summary>
        /// 参考：msdn上的RNGCryptoServiceProvider例子
        /// </summary>
        /// <param name="numSeeds"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        private static int NextRandom(int numSeeds, int length)
        {
            // Create a byte array to hold the random value.
            byte[] randomNumber = new byte[length];
            // Create a new instance of the RNGCryptoServiceProvider.
            System.Security.Cryptography.RNGCryptoServiceProvider rng = new System.Security.Cryptography.RNGCryptoServiceProvider();
            // Fill the array with a random value.
            rng.GetBytes(randomNumber);
            // Convert the byte to an uint value to make the modulus operation easier.
            uint randomResult = 0x0;
            for (int i = 0; i < length; i++)
            {
                randomResult |= ((uint)randomNumber[i] << ((length - 1 - i) * 8));
            }

            return (int)(randomResult % numSeeds) + 1;
        }

        /// <summary>
        /// 创建11位的英文与数字组合
        /// </summary>
        /// <returns>System.String.</returns>
        public static string ShortStr()
        {
            return Convert(GenerateLong());
        }

        static string Seq = "s9LFkgy5RovixI1aOf8UhdY3r4DMplQZJXPqebE0WSjBn7wVzmN2Gc6THCAKut";
        /// <summary>
        /// 10进制转换为62进制
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        private static string Convert(long id)
        {
            if (id < 62)
            {
                return Seq[(int)id].ToString();
            }
            int y = (int)(id % 62);
            long x = (long)(id / 62);

            return Convert(x) + Seq[y];
        }
    }
}