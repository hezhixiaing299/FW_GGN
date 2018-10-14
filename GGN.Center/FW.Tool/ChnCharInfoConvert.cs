using Microsoft.International.Converters.PinYinConverter;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace FW.Tool
{
    /// <summary>
    /// 汉字拼音转换操作(官方)
    /// </summary>
    public static class ChnCharInfoConvert
    {
        public static string ConvertToPinyin(string chineseCharacters)
        {
            char[] ch = chineseCharacters.ToArray();
            string pinyinStr = "";
            foreach (char c in ch)
            {
                if (ChineseChar.IsValidChar(c))
                {
                    ChineseChar chineseChar = new ChineseChar(c);
                    ReadOnlyCollection<string> pinyin = chineseChar.Pinyins;
                    pinyinStr += (pinyin[0].Substring(0, pinyin[0].Length - 1));
                }
                else
                {
                    pinyinStr += c.ToString();
                }
            }
            return pinyinStr.ToLower();
        }
        public static string ConvertToShortPinyin(string chineseCharacters)
        {
            char[] ch = chineseCharacters.ToArray();
            string pinyinStr = "";
            foreach (char c in ch)
            {
                if (ChineseChar.IsValidChar(c))
                {
                    ChineseChar chineseChar = new ChineseChar(c);
                    ReadOnlyCollection<string> pinyin = chineseChar.Pinyins;
                    pinyinStr += (pinyin[0].Substring(0, 1));
                }
                else
                {
                    pinyinStr += c.ToString();
                }
            }
            return pinyinStr.ToLower();
        }
    }
    
}
