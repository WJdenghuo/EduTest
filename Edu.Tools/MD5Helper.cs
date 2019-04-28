using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Edu.Tools
{
    public static class MD5Helper
    {
        /// <summary>
        /// 通过字符串获取MD5值，返回32位字符串。
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string GetMD5String(string str)
        {
            MD5 md5 = MD5.Create();
            byte[] data = Encoding.UTF8.GetBytes(str);
            byte[] data2 = md5.ComputeHash(data);

            return GetbyteToString(data2);
            //return BitConverter.ToString(data2).Replace("-", "").ToLower();
        }
        /// <summary>
        /// 获取MD5值。HashAlgorithm.Create("MD5") 或 MD5.Create() HashAlgorithm.Create("SHA256") 或 SHA256.Create()
        /// </summary>
        /// <param name="str"></param>
        /// <param name="hash"></param>
        /// <returns></returns>
        public static string GetMD5String(string str, HashAlgorithm hash)
        {
            byte[] data = Encoding.UTF8.GetBytes(str);
            byte[] data2 = hash.ComputeHash(data);
            return GetbyteToString(data2);
            //return BitConverter.ToString(data2).Replace("-", "").ToLower();
        }

        public static string GetMD5FromFile(string path)
        {
            MD5 md5 = MD5.Create();
            if (!File.Exists(path))
            {
                return "";
            }
            FileStream stream = File.OpenRead(path);
            byte[] data2 = md5.ComputeHash(stream);

            return GetbyteToString(data2);
            //return BitConverter.ToString(data2).Replace("-", "").ToLower();
        }

        private static string GetbyteToString(byte[] data)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                sb.Append(data[i].ToString("x2"));
            }
            return sb.ToString();
        }
    }
}
