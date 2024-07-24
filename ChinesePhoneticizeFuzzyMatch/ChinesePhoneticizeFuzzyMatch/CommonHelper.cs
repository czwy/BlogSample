using Microsoft.International.Converters.PinYinConverter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ChinesePhoneticizeFuzzyMatch
{
    public class CommonHelper
    {
        /// <summary>
        /// 获取汉字拼音
        /// </summary>
        /// <param name="str">待处理包含汉字的字符串</param>
        /// <param name="split">拼音分隔符</param>
        /// <returns></returns>
        public static List<string> GetChinesePhoneticize(string str, string split = "")
        {
            List<string> result = new List<string>();
            char[] chs = str.ToCharArray();
            Dictionary<int, List<string>> totalPhoneticizes = new Dictionary<int, List<string>>();
            for (int i = 0; i < chs.Length; i++)
            {
                var phoneticizes = new List<string>();
                if (ChineseChar.IsValidChar(chs[i]))
                {
                    ChineseChar cc = new ChineseChar(chs[i]);
                    phoneticizes.AddRange(cc.Pinyins.Where(r => !string.IsNullOrWhiteSpace(r)).ToList<string>().ConvertAll(p => Regex.Replace(p, @"\d", "").ToLower()).Distinct());
                }
                else
                {
                    phoneticizes.Add(chs[i].ToString());
                }
                if (phoneticizes.Any())
                    totalPhoneticizes[i] = phoneticizes;
            }

            foreach (var phoneticizes in totalPhoneticizes)
            {
                var items = phoneticizes.Value;
                if (result.Count <= 0)
                {
                    result = items;
                }
                else
                {

                    var newtotalPhoneticizes = new List<string>();
                    foreach (var totalPingYin in result)
                    {
                        newtotalPhoneticizes.AddRange(items.Select(item => totalPingYin + split + item));
                    }
                    newtotalPhoneticizes = newtotalPhoneticizes.Distinct().ToList();
                    result = newtotalPhoneticizes;
                }
            }

            return result;
        }

        public static bool PhoneticizeMatch(string search, string[] pinyin, out int matchStart, out int matchCount)
        {
            int wordIndex = 0;
            int wordStart = 0;
            int searchIndex = 0;
            matchStart = 0;
            matchCount = 0;
            int pinyinLen = pinyin.Length;
            while (searchIndex < search.Length && wordIndex < pinyinLen)
            {
                //没有匹配到拼音最后一个字符，判断是否匹配
                if (wordStart < pinyin[wordIndex].Length && search[searchIndex] == pinyin[wordIndex][wordStart])
                {
                    searchIndex++;
                    wordStart++;
                }
                //上一个拼音匹配完，或者匹配失败
                else
                {
                    //到最后一个拼音，无法匹配
                    if (wordIndex == pinyinLen - 1)
                    {
                        return false;
                    }
                    wordIndex++;
                    wordStart = 0;
                    //判断是否匹配
                    if (search[searchIndex] == pinyin[wordIndex][wordStart])
                    {
                        searchIndex++;
                        wordStart++;
                        if (searchIndex == 1) matchStart = wordIndex;
                    }
                    //不匹配，回退
                    else
                    {
                        if (searchIndex > 0)
                        {
                            searchIndex--;
                            //判断是否匹配
                            while (searchIndex >= 0 && search[searchIndex] != pinyin[wordIndex][0])
                            {
                                searchIndex--;
                            }
                            if (searchIndex < 0)
                            {
                                searchIndex = 0;
                                wordIndex++;
                                wordStart = 0;
                                matchStart = wordIndex;
                            }
                            else
                            {
                                searchIndex++;
                                wordStart++;
                            }
                        }
                    }
                }
            }
            if (searchIndex == search.Length)
            {
                Console.WriteLine("search:" + search + "--->pinyin:" + string.Join(" ", pinyin) + "---->start:" + matchStart + "end:" + wordIndex);
                matchCount = wordIndex - matchStart + 1;
                return true;
            }
            return false;
        }

        public static bool fuzzyMatchChar(string character, string input, out int matchStart, out int matchCount)
        {
            List<string> regexs = GetChinesePhoneticize(input);
            List<string> targetStr = GetChinesePhoneticize(character, " ");
            matchStart = -1;
            matchCount = 0;
            foreach (string regex in regexs)
            {
                foreach (string target in targetStr)
                {
                    if (PhoneticizeMatch(regex, target.Split(' '), out matchStart, out matchCount))
                        return true;
                }
            }
            return false;
        }
    }
}
