 
// 
 
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace LSL_PRO.Utilities
{
    /// <summary>
    /// 字符串处理
    /// </summary>
    public class StringHelper
    {
        /// <summary>
        /// 格式化TextArea输入内容为html显示
        /// </summary>
        /// <param name="s">要格式化内容</param>
        /// <returns>完成内容</returns>
        public static string FormatTextArea(string s)
        {
            s = s.Replace("\n", "<br>");
            s = s.Replace("\x20", "&nbsp;");
            return s;
        }

        #region 得到字符串的长度，一个汉字算2个字符
        /// <summary>   
        /// 得到字符串的长度，一个汉字算2个字符   
        /// </summary>   
        /// <param name="str">字符串</param>   
        /// <returns>返回字符串长度</returns>   
        public static int GetLength(string str)
        {
            if (str.Length == 0) return 0;

            ASCIIEncoding ascii = new ASCIIEncoding();
            int tempLen = 0;
            byte[] s = ascii.GetBytes(str);
            for (int i = 0; i < s.Length; i++)
            {
                if ((int)s[i] == 63)
                {
                    tempLen += 2;
                }
                else
                {
                    tempLen += 1;
                }
            }
            return tempLen;
        }

        public static string GetCenterShow(string p, int size, int font)
        {
            // p = p.Trim();
            if (font == 2)
            {
                int s = 0;
                int len = p.Length;
                if (len >= 11) return p;
                if (len == 9) s = 2;
                if (len == 8) s = 2;
                if (len == 7) s = 3;
                if (len == 6) s = 4;
                if (len == 5) s = 4;
                if (len == 4) s = 5;
                if (len == 3) s = 6;
                if (len == 2) s = 7;
                string ttt = "";
                ttt = ttt.PadLeft(s, ' ');
                return ttt + p;
            }
            if (font == 1)
            {
                int len = GetLength(p);
                int tmp = 34 - len;
                if (tmp < 0) return p;
                double index = ((double)tmp / 2);  // 5
                string ttt = "";
                int s = (int)index;
                ttt = ttt.PadLeft(s, ' ');
                return ttt + p;
            }
            return p;
        }
        #endregion

        #region 省略字符串
        /// <summary>
        /// 省略字符串
        /// </summary>
        /// <param name="RawString">字符</param>
        /// <param name="Length">字节</param>
        /// <param name="status">是否开启省略字符串 0：否，1：是</param>
        /// <returns></returns>
        public static string GetOmitString(string str, int length, string status)
        {
            string temp = str;
            if (status == "1")
            {
                int j = 0;
                int k = 0;
                for (int i = 0; i < temp.Length; i++)
                {
                    if (Regex.IsMatch(temp.Substring(i, 1), @"[\u4e00-\u9fa5]+"))
                    {
                        j += 2;
                    }
                    else
                    {
                        j += 1;
                    }
                    if (j <= length)
                    {
                        k += 1;
                    }
                    if (j >= length)
                    {
                        return temp.Substring(0, k) + "...";
                    }
                }
            }
            return temp;
        }
        #endregion

        #region 分割字符串
        /// <summary>
        /// 分割字符串
        /// </summary>
        public static string[] SplitString(string strContent, string strSplit)
        {
            if (!string.IsNullOrEmpty(strContent))
            {
                if (strContent.IndexOf(strSplit) < 0)
                    return new string[] { strContent };

                return Regex.Split(strContent, Regex.Escape(strSplit), RegexOptions.IgnoreCase);
            }
            else
                return new string[0] { };
        }

        /// <summary>
        /// 分割字符串
        /// </summary>
        /// <returns></returns>
        public static string[] SplitString(string strContent, string strSplit, int count)
        {
            string[] result = new string[count];
            string[] splited = SplitString(strContent, strSplit);

            for (int i = 0; i < count; i++)
            {
                if (i < splited.Length)
                    result[i] = splited[i];
                else
                    result[i] = string.Empty;
            }

            return result;
        }
        #endregion

        #region 删除最后结尾的指定字符后的字符
        /// <summary>
        /// 删除最后结尾的指定字符后的字符
        /// </summary>
        public static string DelLastChar(string str, string strchar)
        {
            if (string.IsNullOrEmpty(str))
                return "";
            if (str.LastIndexOf(strchar) >= 0 && str.LastIndexOf(strchar) == str.Length - 1)
            {
                return str.Substring(0, str.LastIndexOf(strchar));
            }
            return str;
        }
        /// <summary>
        /// 删除最后字符
        /// </summary>
        /// <param name="str"></param>
        /// <param name="Length"></param>
        /// <returns></returns>
        public static string DeleteLastLength(string str, int Length)
        {
            if (string.IsNullOrEmpty(str))
                return "";
            str = str.Substring(0, str.Length - Length);//
            return str;
        }
        #endregion

        /// <summary>
        /// 获得字符串中开始和结束字符串中间得值
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="s">开始</param>
        /// <param name="e">结束</param>
        /// <returns></returns> 
        public static string GetStringValue(string str, string s, string e)
        {
            Regex rg = new Regex("(?<=(" + s + "))[.\\s\\S]*?(?=(" + e + "))", RegexOptions.Multiline | RegexOptions.Singleline);
            return rg.Match(str).Value;
        }


        /// <summary>   
        /// 获得一个16位时间随机数   
        /// </summary>   
        /// <returns>返回随机数</returns>   
        public static string GetDataRandom()
        {
            string strData = DateTime.Now.ToString();
            strData = strData.Replace(":", "");
            strData = strData.Replace("-", "");
            strData = strData.Replace(" ", "");
            Random r = new Random();
            strData = strData + r.Next(100000);
            return strData;
        }

        /// <summary>   
        ///  获得某个字符串在另个字符串中出现的次数   
        /// </summary>   
        /// <param name="strOriginal">要处理的字符</param>   
        /// <param name="strSymbol">符号</param>   
        /// <returns>返回值</returns>   
        public static int GetStrCount(string strOriginal, string strSymbol)
        {
            int count = 0;
            for (int i = 0; i < (strOriginal.Length - strSymbol.Length + 1); i++)
            {
                if (strOriginal.Substring(i, strSymbol.Length) == strSymbol)
                {
                    count = count + 1;
                }
            }
            return count;
        }

        /// <summary>   
        /// 获得某个字符串在另个字符串第一次出现时前面所有字符   
        /// </summary>   
        /// <param name="strOriginal">要处理的字符</param>   
        /// <param name="strSymbol">符号</param>   
        /// <returns>返回值</returns>   
        public static string GetFirstStr(string strOriginal, string strSymbol)
        {
            int strPlace = strOriginal.IndexOf(strSymbol);
            if (strPlace != -1)
                strOriginal = strOriginal.Substring(0, strPlace);
            return strOriginal;
        }

        /// <summary>   
        /// 获得某个字符串在另个字符串最后一次出现时后面所有字符   
        /// </summary>   
        /// <param name="strOriginal">要处理的字符</param>   
        /// <param name="strSymbol">符号</param>   
        /// <returns>返回值</returns>   
        public static string GetLastStr(string strOriginal, string strSymbol)
        {
            int strPlace = strOriginal.LastIndexOf(strSymbol) + strSymbol.Length;
            strOriginal = strOriginal.Substring(strPlace);
            return strOriginal;
        }

        /// <summary>   
        /// 获得两个字符之间第一次出现时前面所有字符   
        /// </summary>   
        /// <param name="strOriginal">要处理的字符</param>   
        /// <param name="strFirst">最前哪个字符</param>   
        /// <param name="strLast">最后哪个字符</param>   
        /// <returns>返回值</returns>   
        public static string GetTwoMiddleFirstStr(string strOriginal, string strFirst, string strLast)
        {
            strOriginal = GetFirstStr(strOriginal, strLast);
            strOriginal = GetLastStr(strOriginal, strFirst);
            return strOriginal;
        }

        /// <summary>   
        ///  获得两个字符之间最后一次出现时的所有字符   
        /// </summary>   
        /// <param name="strOriginal">要处理的字符</param>   
        /// <param name="strFirst">最前哪个字符</param>   
        /// <param name="strLast">最后哪个字符</param>   
        /// <returns>返回值</returns>   
        public static string GetTwoMiddleLastStr(string strOriginal, string strFirst, string strLast)
        {
            strOriginal = GetLastStr(strOriginal, strFirst);
            strOriginal = GetFirstStr(strOriginal, strLast);
            return strOriginal;
        }

        /// <summary>   
        /// 从数据库表读记录时,能正常显示   
        /// </summary>   
        /// <param name="strContent">要处理的字符</param>   
        /// <returns>返回正常值</returns>   
        public static string GetHtmlFormat(string strContent)
        {
            strContent = strContent.Trim();

            if (strContent == null)
            {
                return "";
            }
            strContent = strContent.Replace("<", "<");
            strContent = strContent.Replace(">", ">");
            strContent = strContent.Replace("\n", "<br />");
            return (strContent);
        }

        /// <summary>   
        /// 检查相等之后，获得字符串   
        /// </summary>   
        /// <param name="str">字符串1</param>   
        /// <param name="checkStr">字符串2</param>   
        /// <param name="reStr">相等之后要返回的字符串</param>   
        /// <returns>返回字符串</returns>   
        public static string GetCheckStr(string str, string checkStr, string reStr)
        {
            if (str == checkStr)
            {
                return reStr;
            }
            return "";
        }

        /// <summary>   
        /// 检查相等之后，获得字符串   
        /// </summary>   
        /// <param name="str">数值1</param>   
        /// <param name="checkStr">数值2</param>   
        /// <param name="reStr">相等之后要返回的字符串</param>   
        /// <returns>返回字符串</returns>   
        public static string GetCheckStr(int str, int checkStr, string reStr)
        {
            if (str == checkStr)
            {
                return reStr;
            }
            return "";
        }
        /// <summary>   
        /// 检查相等之后，获得字符串   
        /// </summary>   
        /// <param name="str"></param>   
        /// <param name="checkStr"></param>   
        /// <param name="reStr"></param>   
        /// <returns></returns>   
        public static string GetCheckStr(bool str, bool checkStr, string reStr)
        {
            if (str == checkStr)
            {
                return reStr;
            }
            return "";
        }
        /// <summary>   
        /// 检查相等之后，获得字符串   
        /// </summary>   
        /// <param name="str"></param>   
        /// <param name="checkStr"></param>   
        /// <param name="reStr"></param>   
        /// <returns></returns>   
        public static string GetCheckStr(object str, object checkStr, string reStr)
        {
            if (str == checkStr)
            {
                return reStr;
            }
            return "";
        }
        /// <summary>   
        /// 截取左边规定字数字符串,超过字数用endStr结束   
        /// </summary>   
        /// <param name="str">需截取字符串</param>   
        /// <param name="length">截取字数</param>   
        /// <param name="endStr">超过字数，结束字符串，如"..."</param>   
        /// <returns>返回截取字符串</returns>   
        public static string GetLeftStr(string str, int length, string endStr)
        {
            string reStr;
            if (length < GetStrLength(str))
            {
                reStr = str.Substring(0, length) + endStr;
            }
            else
            {
                reStr = str;
            }
            return reStr;
        }

        /// <summary>   
        /// 截取左边规定字数字符串,超过字数用...结束   
        /// </summary>   
        /// <param name="str">需截取字符串</param>   
        /// <param name="length">截取字数</param>   
        /// <returns>返回截取字符串</returns>   
        public static string GetLeftStr(string str, int length)
        {
            string reStr;
            if (length < str.Length)
            {
                reStr = str.Substring(0, length) + "...";
            }
            else
            {
                reStr = str;
            }
            return reStr;
        }

        /// <summary>   
        /// 截取左边规定字数字符串,超过字数用...结束   
        /// </summary>   
        /// <param name="str">需截取字符串</param>   
        /// <param name="length">截取字数</param>   
        /// <param name="subcount">若超过字数右边减少的字符长度</param>   
        /// <returns>返回截取字符串</returns>   
        public static string GetLeftStr(string str, int length, int subcount)
        {
            string reStr;
            if (length < str.Length)
            {
                reStr = str.Substring(0, length - subcount) + "...";
            }
            else
            {
                reStr = str;
            }
            return reStr;
        }

        /// <summary>   
        /// 获得双字节字符串的字节数   
        /// </summary>   
        /// <param name="str">要检测的字符串</param>   
        /// <returns>返回字节数</returns>   
        public static int GetStrLength(string str)
        {
            ASCIIEncoding n = new ASCIIEncoding();
            byte[] b = n.GetBytes(str);
            int l = 0;  // l 为字符串之实际长度   
            for (int i = 0; i < b.Length; i++)
            {
                if (b[i] == 63)  //判断是否为汉字或全脚符号   
                {
                    l++;
                }
                l++;
            }
            return l;
        }

        /// <summary>   
        /// 剥去HTML标签   
        /// </summary>   
        /// <param name="text">带有HTML格式的字符串</param>   
        /// <returns>string</returns>   
        public static string RegStripHtml(string text)
        {
            string reStr;
            string RePattern = @"<\s*(\S+)(\s[^>]*)?>";
            reStr = Regex.Replace(text, RePattern, string.Empty, RegexOptions.Compiled);
            reStr = Regex.Replace(reStr, @"\s+", string.Empty, RegexOptions.Compiled);
            return reStr;
        }

        /// <summary>   
        /// 使Html失效,以文本显示   
        /// </summary>   
        /// <param name="str">原字符串</param>   
        /// <returns>失效后字符串</returns>   
        public static string ReplaceHtml(string str)
        {
            str = str.Replace("<", "<");
            return str;
        }
        //------//搜索字符串(参数1：目标字符串，参数2：之前字符串，参数3：之后字符串)----(获取两个字符串中间的字符串)

        public static string Search_string(string s, string s1, string s2)  //获取搜索到的数目
        {

            int n1, n2;

            n1 = s.IndexOf(s1, 0) + s1.Length;   //开始位置

            n2 = s.IndexOf(s2, n1);               //结束位置

            return s.Substring(n1, n2 - n1);   //取搜索的条数，用结束的位置-开始的位置,并返回

        }

        /// <summary>   
        /// 获得随机数字   
        /// </summary>   
        /// <param name="Length">随机数字的长度</param>   
        /// <returns>返回长度为 Length 的　<see cref="System.Int32"/> 类型的随机数</returns>   
        /// <example>   
        /// Length 不能大于9,以下为示例演示了如何调用 GetRandomNext：<br />   
        /// <code>   
        ///  int le = GetRandomNext(8);   
        /// </code>   
        /// </example>   
        public static int GetRandomNext(int Length)
        {
            if (Length > 9)
                throw new System.IndexOutOfRangeException("Length的长度不能大于10");
            Guid gu = Guid.NewGuid();
            string str = "";
            for (int i = 0; i < gu.ToString().Length; i++)
            {
                if (isNumber(gu.ToString()[i]))
                {
                    str += ((gu.ToString()[i]));
                }
            }
            int guid = int.Parse(str.Replace("-", "").Substring(0, Length));
            if (!guid.ToString().Length.Equals(Length))
                guid = GetRandomNext(Length);
            return guid;
        }

        /// <summary>   
        /// 返回一个 bool 值，指明提供的值是不是整数   
        /// </summary>   
        /// <param name="obj">要判断的值</param>   
        /// <returns>true[是整数]false[不是整数]</returns>   
        /// <remarks>   
        ///  isNumber　只能判断正(负)整数，如果 obj 为小数则返回 false;   
        /// </remarks>   
        /// <example>   
        /// 下面的示例演示了判断 obj 是不是整数：<br />   
        /// <code>   
        ///  bool flag;   
        ///  flag = isNumber("200");   
        /// </code>   
        /// </example>   
        public static bool isNumber(object obj)
        {
            //为指定的正则表达式初始化并编译 Regex 类的实例   
            System.Text.RegularExpressions.Regex rg = new System.Text.RegularExpressions.Regex(@"^-?(\d*)$");
            //在指定的输入字符串中搜索 Regex 构造函数中指定的正则表达式匹配项   
            System.Text.RegularExpressions.Match mc = rg.Match(obj.ToString());
            //指示匹配是否成功   
            return (mc.Success);
        }

        /// <summary>   
        /// 高亮显示   
        /// </summary>   
        /// <param name="str">原字符串</param>   
        /// <param name="findstr">查找字符串</param>   
        /// <param name="cssclass">Style</param>   
        /// <returns>string</returns>   
        public static string OutHighlightText(string str, string findstr, string cssclass)
        {
            if (findstr != "")
            {
                string text1 = "<span class=\"" + cssclass + "\">%s</span>";
                str = str.Replace(findstr, text1.Replace("%s", findstr));
            }
            return str;
        }

        /// <summary>   
        /// 移除字符串首尾某些字符   
        /// </summary>   
        /// <param name="strOriginal">要操作的字符串</param>   
        /// <param name="startStr">要在字符串首部移除的字符串</param>   
        /// <param name="endStr">要在字符串尾部移除的字符串</param>   
        /// <returns>string</returns>   
        public static string RemoveStartOrEndStr(string strOriginal, string startStr, string endStr)
        {
            char[] start = startStr.ToCharArray();
            char[] end = endStr.ToCharArray();
            return strOriginal.TrimStart(start).TrimEnd(end);
        }

        /// <summary>   
        /// 删除指定位置指定长度字符串   
        /// </summary>   
        /// <param name="strOriginal">要操作的字符串</param>   
        /// <param name="startIndex">开始删除字符的位置</param>   
        /// <param name="count">要删除的字符数</param>   
        /// <returns>string</returns>   
        public static string RemoveStr(string strOriginal, int startIndex, int count)
        {
            return strOriginal.Remove(startIndex, count);
        }

        /// <summary>   
        /// 从左边填充字符串   
        /// </summary>   
        /// <param name="strOriginal">要操作的字符串</param>   
        /// <param name="totalWidth">结果字符串中的字符数</param>   
        /// <param name="paddingChar">填充的字符</param>   
        /// <returns>string</returns>   
        public static string LeftPadStr(string strOriginal, int totalWidth, char paddingChar)
        {
            if (strOriginal.Length < totalWidth)
                return strOriginal.PadLeft(totalWidth, paddingChar);
            return strOriginal;
        }

        /// <summary>   
        /// 从右边填充字符串   
        /// </summary>   
        /// <param name="strOriginal">要操作的字符串</param>   
        /// <param name="totalWidth">结果字符串中的字符数</param>   
        /// <param name="paddingChar">填充的字符</param>   
        /// <returns>string</returns>   
        public static string RightPadStr(string strOriginal, int totalWidth, char paddingChar)
        {
            if (strOriginal.Length < totalWidth)
                return strOriginal.PadRight(totalWidth, paddingChar);
            return strOriginal;
        }   

    }
}
