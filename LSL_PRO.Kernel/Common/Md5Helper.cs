using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace LSL_PRO.Kernel
{
    /// <summary>
    /// MD5加密帮助类

    /// <author>
    ///		<name>lsl</name>
    ///		<date>2016.11.20</date>
    /// </author>
    /// </summary>
    public class Md5Helper
    {
        #region "MD5加密"
        public static readonly string Md5Key = "LslPro";//KEY
        public static string MakeMd5(string data)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(data + Md5Key);
            MD5 mD = new MD5CryptoServiceProvider();
            byte[] array = mD.ComputeHash(bytes);
            StringBuilder stringBuilder = new StringBuilder();
            byte[] array2 = array;
            for (int i = 0; i < array2.Length; i++)
            {
                byte b = array2[i];
                stringBuilder.Append(b.ToString("x2").ToUpper());
            }
            return stringBuilder.ToString();
        }
        #endregion

       
    }
}
