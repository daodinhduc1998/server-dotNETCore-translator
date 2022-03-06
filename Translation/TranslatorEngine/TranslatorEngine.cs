using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace Translation.TranslatorEngine
{
    public class TranslatorEngine
    {
        public const int CHINESE_LOOKUP_MAX_LENGTH = 20;

        private static bool dictionaryDirty = true;

        private static Dictionary<string, string> hanVietDictionary = new Dictionary<string, string>();

        private static Dictionary<string, string> vietPhraseDictionary = new Dictionary<string, string>();

        private static Dictionary<string, string> thieuChuuDictionary = new Dictionary<string, string>();

        private static Dictionary<string, string> lacVietDictionary = new Dictionary<string, string>();

        private static Dictionary<string, string> cedictDictionary = new Dictionary<string, string>();

        private static Dictionary<string, string> chinesePhienAmEnglishDictionary = new Dictionary<string, string>();

        private static Dictionary<string, string> vietPhraseOneMeaningDictionary = new Dictionary<string, string>();

        private static Dictionary<string, string> onlyVietPhraseDictionary = new Dictionary<string, string>();

        private static Dictionary<string, string> onlyNameDictionary = new Dictionary<string, string>();

        private static Dictionary<string, string> onlyNameOneMeaningDictionary = new Dictionary<string, string>();

        private static Dictionary<string, string> onlyNameChinhDictionary = new Dictionary<string, string>();

        private static Dictionary<string, string> onlyNamePhuDictionary = new Dictionary<string, string>();

        private static Dictionary<string, string> luatNhanDictionary = new Dictionary<string, string>();

        private static Dictionary<string, string> pronounDictionary = new Dictionary<string, string>();

        private static Dictionary<string, string> pronounOneMeaningDictionary = new Dictionary<string, string>();

        private static Dictionary<string, string> nhanByDictionary = null;

        private static Dictionary<string, string> nhanByOneMeaningDictionary = null;

        private static DataSet onlyVietPhraseDictionaryHistoryDataSet = new DataSet();

        private static DataSet onlyNameDictionaryHistoryDataSet = new DataSet();

        private static DataSet onlyNamePhuDictionaryHistoryDataSet = new DataSet();

        private static DataSet hanVietDictionaryHistoryDataSet = new DataSet();

        private static List<string> ignoredChinesePhraseList = new List<string>();

        private static List<string> ignoredChinesePhraseForBrowserList = new List<string>();

        private static object lockObject = new object();

        private static string NULL_STRING = Convert.ToChar(0).ToString();

        public static string LastTranslatedWord_HanViet = "";

        public static string LastTranslatedWord_VietPhrase = "";

        public static string LastTranslatedWord_VietPhraseOneMeaning = "";

        private static bool NextCharIsCap = false;
        private static char[] hgac = new char[196]
{
      'à',
      'á',
      'ả',
      'ã',
      'ạ',
      'ă',
      'ằ',
      'ắ',
      'ẳ',
      'ẵ',
      'ặ',
      'â',
      'ầ',
      'ấ',
      'ẩ',
      'ẫ',
      'ậ',
      'đ',
      'è',
      'é',
      'ẻ',
      'ẽ',
      'ẹ',
      'ê',
      'ề',
      'ế',
      'ể',
      'ễ',
      'ệ',
      'ì',
      'í',
      'ỉ',
      'ĩ',
      'ị',
      'ò',
      'ó',
      'ỏ',
      'õ',
      'ọ',
      'ô',
      'ồ',
      'ố',
      'ổ',
      'ỗ',
      'ộ',
      'ơ',
      'ờ',
      'ớ',
      'ở',
      'ỡ',
      'ợ',
      'ù',
      'ú',
      'ủ',
      'ũ',
      'ụ',
      'ư',
      'ừ',
      'ứ',
      'ử',
      'ữ',
      'ự',
      'ỳ',
      'ý',
      'ỷ',
      'ỹ',
      'ỵ',
      'À',
      'Á',
      'Ả',
      'Ã',
      'Ạ',
      'Ă',
      'Ằ',
      'Ắ',
      'Ẳ',
      'Ẵ',
      'Ặ',
      'Â',
      'Ầ',
      'Ấ',
      'Ẩ',
      'Ẫ',
      'Ậ',
      'Đ',
      'È',
      'É',
      'Ẻ',
      'Ẽ',
      'Ẹ',
      'Ê',
      'Ề',
      'Ế',
      'Ể',
      'Ễ',
      'Ệ',
      'Ì',
      'Í',
      'Ỉ',
      'Ĩ',
      'Ị',
      'Ò',
      'Ó',
      'Ỏ',
      'Õ',
      'Ọ',
      'Ô',
      'Ồ',
      'Ố',
      'Ổ',
      'Ỗ',
      'Ộ',
      'Ơ',
      'Ờ',
      'Ớ',
      'Ở',
      'Ỡ',
      'Ợ',
      'Ù',
      'Ú',
      'Ủ',
      'Ũ',
      'Ụ',
      'Ư',
      'Ừ',
      'Ứ',
      'Ử',
      'Ữ',
      'Ự',
      'Ỳ',
      'Ý',
      'Ỷ',
      'Ỹ',
      'Ỵ',
      'a',
      'b',
      'c',
      'd',
      'e',
      'f',
      'g',
      'h',
      'i',
      'j',
      'k',
      'l',
      'm',
      'n',
      'o',
      'p',
      'q',
      'r',
      's',
      't',
      'u',
      'v',
      'w',
      'x',
      'y',
      'z',
      'A',
      'B',
      'C',
      'D',
      'E',
      'F',
      'G',
      'H',
      'I',
      'J',
      'K',
      'L',
      'M',
      'N',
      'O',
      'P',
      'Q',
      'R',
      'S',
      'T',
      'U',
      'V',
      'W',
      'X',
      'Y',
      'Z',
      '0',
      '1',
      '2',
      '3',
      '4',
      '5',
      '6',
      '7',
      '8',
      '9'
};
        private static char[] daucau = new char[8]
        {
      '.',
      ':',
      '?',
      '!',
      '(',
      '[',
      '\n',
      '\t'
        };
        private static string[] daucau2 = new string[8]
        {
      ".",
      ":",
      "?",
      "!",
      "(",
      "[",
      "\n",
      "\t"
        };

        private static char[] trimCharsForAnalyzer = new char[]
        {
            ' ',
            '\r',
            '\n',
            '\t'
        };

        public static bool DictionaryDirty
        {
            get
            {
                return dictionaryDirty;
            }
            set
            {
                dictionaryDirty = value;
            }
        }

        public static string GetVietPhraseOrNameValueFromKey(string key)
        {
            if (!vietPhraseDictionary.ContainsKey(key))
            {
                return null;
            }
            return vietPhraseDictionary[key];
        }

        public static string GetVietPhraseValueFromKey(string key)
        {
            if (!onlyVietPhraseDictionary.ContainsKey(key))
            {
                return null;
            }
            return onlyVietPhraseDictionary[key];
        }

        public static string GetNameValueFromKey(string key)
        {
            if (!onlyNameDictionary.ContainsKey(key))
            {
                return null;
            }
            return onlyNameDictionary[key];
        }

        public static string GetNameValueFromKey(string key, bool isNameChinh)
        {
            Dictionary<string, string> dictionary = isNameChinh ? onlyNameChinhDictionary : onlyNamePhuDictionary;
            if (!dictionary.ContainsKey(key))
            {
                return null;
            }
            return dictionary[key];
        }

        public static void DeleteKeyFromVietPhraseDictionary(string key, bool sorting)
        {
            vietPhraseDictionary.Remove(key);
            vietPhraseOneMeaningDictionary.Remove(key);
            onlyVietPhraseDictionary.Remove(key);
            if (sorting)
            {
                SaveDictionaryToFile(ref onlyVietPhraseDictionary, DictionaryConfigurationHelper.GetVietPhraseDictionaryPath());
            }
            else
            {
                SaveDictionaryToFileWithoutSorting(onlyVietPhraseDictionary, DictionaryConfigurationHelper.GetVietPhraseDictionaryPath());
            }
            writeVietPhraseHistoryLog(key, "Deleted");
        }

        public static void DeleteKeyFromNameDictionary(string key, bool sorting, bool isNameChinh)
        {
            vietPhraseDictionary.Remove(key);
            vietPhraseOneMeaningDictionary.Remove(key);
            onlyNameDictionary.Remove(key);
            onlyNameOneMeaningDictionary.Remove(key);
            Dictionary<string, string> dictionary = isNameChinh ? onlyNameChinhDictionary : onlyNamePhuDictionary;
            if (!dictionary.ContainsKey(key))
            {
                return;
            }
            dictionary.Remove(key);
            if (sorting)
            {
                SaveDictionaryToFile(ref dictionary, isNameChinh ? DictionaryConfigurationHelper.GetNamesDictionaryPath() : DictionaryConfigurationHelper.GetNamesPhuDictionaryPath());
            }
            else
            {
                SaveDictionaryToFileWithoutSorting(dictionary, isNameChinh ? DictionaryConfigurationHelper.GetNamesDictionaryPath() : DictionaryConfigurationHelper.GetNamesPhuDictionaryPath());
            }
            writeNamesHistoryLog(key, "Deleted", isNameChinh);
        }

        public static void DeleteKeyFromPhienAmDictionary(string key, bool sorting)
        {
            hanVietDictionary.Remove(key);
            if (sorting)
            {
                SaveDictionaryToFile(ref hanVietDictionary, DictionaryConfigurationHelper.GetChinesePhienAmWordsDictionaryPath());
            }
            else
            {
                SaveDictionaryToFileWithoutSorting(hanVietDictionary, DictionaryConfigurationHelper.GetChinesePhienAmWordsDictionaryPath());
            }
            writePhienAmHistoryLog(key, "Deleted");
        }

        public static void UpdateVietPhraseDictionary(string key, string value, bool sorting)
        {
            if (vietPhraseDictionary.ContainsKey(key))
            {
                vietPhraseDictionary[key] = value;
            }
            else
            {
                vietPhraseDictionary.Add(key, value);
            }
            if (vietPhraseOneMeaningDictionary.ContainsKey(key))
            {
                vietPhraseOneMeaningDictionary[key] = value.Split(new char[]
                {
                    '/',
                    '|'
                })[0];
            }
            else
            {
                vietPhraseOneMeaningDictionary.Add(key, value.Split(new char[]
                {
                    '/',
                    '|'
                })[0]);
            }
            if (onlyVietPhraseDictionary.ContainsKey(key))
            {
                onlyVietPhraseDictionary[key] = value;
                writeVietPhraseHistoryLog(key, "Updated");
            }
            else
            {
                if (sorting)
                {
                    onlyVietPhraseDictionary.Add(key, value);
                }
                else
                {
                    onlyVietPhraseDictionary = AddEntryToDictionaryWithoutSorting(onlyVietPhraseDictionary, key, value);
                }
                writeVietPhraseHistoryLog(key, "Added");
            }
            if (sorting)
            {
                SaveDictionaryToFile(ref onlyVietPhraseDictionary, DictionaryConfigurationHelper.GetVietPhraseDictionaryPath());
                return;
            }
            SaveDictionaryToFileWithoutSorting(onlyVietPhraseDictionary, DictionaryConfigurationHelper.GetVietPhraseDictionaryPath());
        }

        private static Dictionary<string, string> AddEntryToDictionaryWithoutSorting(Dictionary<string, string> dictionary, string key, string value)
        {
            Dictionary<string, string> dictionary2 = new Dictionary<string, string>();
            foreach (KeyValuePair<string, string> current in dictionary)
            {
                dictionary2.Add(current.Key, current.Value);
            }
            dictionary2.Add(key, value);
            return dictionary2;
        }

        public static void UpdateNameDictionary(string key, string value, bool sorting, bool isNameChinh)
        {
            if (vietPhraseDictionary.ContainsKey(key))
            {
                vietPhraseDictionary[key] = value;
            }
            else
            {
                vietPhraseDictionary.Add(key, value);
            }
            if (vietPhraseOneMeaningDictionary.ContainsKey(key))
            {
                vietPhraseOneMeaningDictionary[key] = value.Split(new char[]
                {
                    '/',
                    '|'
                })[0];
            }
            else
            {
                vietPhraseOneMeaningDictionary.Add(key, value.Split(new char[]
                {
                    '/',
                    '|'
                })[0]);
            }
            Dictionary<string, string> dictionary = isNameChinh ? onlyNameChinhDictionary : onlyNamePhuDictionary;
            if (dictionary.ContainsKey(key))
            {
                dictionary[key] = value;
                writeNamesHistoryLog(key, "Updated", isNameChinh);
            }
            else
            {
                if (sorting)
                {
                    dictionary.Add(key, value);
                }
                else if (isNameChinh)
                {
                    onlyNameChinhDictionary = AddEntryToDictionaryWithoutSorting(onlyNameChinhDictionary, key, value);
                    dictionary = onlyNameChinhDictionary;
                }
                else
                {
                    onlyNamePhuDictionary = AddEntryToDictionaryWithoutSorting(onlyNamePhuDictionary, key, value);
                    dictionary = onlyNamePhuDictionary;
                }
                writeNamesHistoryLog(key, "Added", isNameChinh);
            }
            if (onlyNameDictionary.ContainsKey(key))
            {
                onlyNameDictionary[key] = value;
                onlyNameOneMeaningDictionary[key] = value.Split(new char[]
                {
                    '/',
                    '|'
                })[0];
            }
            else if (sorting)
            {
                onlyNameDictionary.Add(key, value);
                onlyNameOneMeaningDictionary.Add(key, value.Split(new char[]
                {
                    '/',
                    '|'
                })[0]);
            }
            else
            {
                onlyNameDictionary = AddEntryToDictionaryWithoutSorting(onlyNameDictionary, key, value);
                onlyNameOneMeaningDictionary = AddEntryToDictionaryWithoutSorting(onlyNameOneMeaningDictionary, key, value.Split(new char[]
                {
                    '/',
                    '|'
                })[0]);
            }
            if (sorting)
            {
                SaveDictionaryToFile(ref dictionary, isNameChinh ? DictionaryConfigurationHelper.GetNamesDictionaryPath() : DictionaryConfigurationHelper.GetNamesPhuDictionaryPath());
                return;
            }
            SaveDictionaryToFileWithoutSorting(dictionary, isNameChinh ? DictionaryConfigurationHelper.GetNamesDictionaryPath() : DictionaryConfigurationHelper.GetNamesPhuDictionaryPath());
        }

        public static void UpdatePhienAmDictionary(string key, string value, bool sorting)
        {
            if (hanVietDictionary.ContainsKey(key))
            {
                hanVietDictionary[key] = value;
                writePhienAmHistoryLog(key, "Updated");
            }
            else
            {
                if (sorting)
                {
                    hanVietDictionary.Add(key, value);
                }
                else
                {
                    hanVietDictionary = AddEntryToDictionaryWithoutSorting(hanVietDictionary, key, value);
                }
                writePhienAmHistoryLog(key, "Added");
            }
            if (sorting)
            {
                SaveDictionaryToFile(ref hanVietDictionary, DictionaryConfigurationHelper.GetChinesePhienAmWordsDictionaryPath());
                return;
            }
            SaveDictionaryToFileWithoutSorting(hanVietDictionary, DictionaryConfigurationHelper.GetChinesePhienAmWordsDictionaryPath());
        }

        public static void SaveDictionaryToFileWithoutSorting(Dictionary<string, string> dictionary, string filePath)
        {
            string text = filePath + "." + DateTime.Now.Ticks;
            if (File.Exists(filePath))
            {
                File.Copy(filePath, text, true);
            }
            StringBuilder stringBuilder = new StringBuilder();
            foreach (KeyValuePair<string, string> current in dictionary)
            {
                stringBuilder.Append(current.Key).Append("=").AppendLine(current.Value);
            }
            try
            {
                File.WriteAllText(filePath, stringBuilder.ToString(), Encoding.UTF8);
            }
            catch (Exception ex)
            {
                try
                {
                    File.Copy(text, filePath, true);
                }
                catch
                {
                }
                throw ex;
            }
            if (File.Exists(filePath))
            {
                File.Delete(text);
            }
        }

        public static void SaveDictionaryToFile(ref Dictionary<string, string> dictionary, string filePath)
        {
            IOrderedEnumerable<KeyValuePair<string, string>> orderedEnumerable = from pair in dictionary
                                                                                 orderby pair.Key.Length descending, pair.Key
                                                                                 select pair;
            Dictionary<string, string> dictionary2 = new Dictionary<string, string>();
            string text = filePath + "." + DateTime.Now.Ticks;
            if (File.Exists(filePath))
            {
                File.Copy(filePath, text, true);
            }
            StringBuilder stringBuilder = new StringBuilder();
            foreach (KeyValuePair<string, string> current in orderedEnumerable)
            {
                stringBuilder.Append(current.Key).Append("=").AppendLine(current.Value);
                dictionary2.Add(current.Key, current.Value);
            }
            dictionary = dictionary2;
            try
            {
                File.WriteAllText(filePath, stringBuilder.ToString(), Encoding.UTF8);
            }
            catch (Exception ex)
            {
                try
                {
                    File.Copy(text, filePath, true);
                }
                catch
                {
                }
                throw ex;
            }
            if (File.Exists(filePath))
            {
                File.Delete(text);
            }
        }

        public static string ChineseToHanViet(string chinese, out CharRange[] chineseHanVietMappingArray)
        {
            LastTranslatedWord_HanViet = "";
            List<CharRange> list = new List<CharRange>();
            StringBuilder stringBuilder = new StringBuilder();
            int length = chinese.Length;
            for (int i = 0; i < length - 1; i++)
            {
                int length2 = stringBuilder.ToString().Length;
                char c = chinese[i];
                char character = chinese[i + 1];
                if (isChinese(c))
                {
                    if (isChinese(character))
                    {
                        appendTranslatedWord(stringBuilder, ChineseToHanViet(c), ref LastTranslatedWord_HanViet, ref length2);
                        stringBuilder.Append(" ");
                        LastTranslatedWord_HanViet += " ";
                        list.Add(new CharRange(length2, ChineseToHanViet(c).Length));
                    }
                    else
                    {
                        appendTranslatedWord(stringBuilder, ChineseToHanViet(c), ref LastTranslatedWord_HanViet, ref length2);
                        list.Add(new CharRange(length2, ChineseToHanViet(c).Length));
                    }
                }
                else
                {
                    stringBuilder.Append(c);
                    LastTranslatedWord_HanViet += c.ToString();
                    list.Add(new CharRange(length2, 1));
                }
            }
            if (isChinese(chinese[length - 1]))
            {
                appendTranslatedWord(stringBuilder, ChineseToHanViet(chinese[length - 1]), ref LastTranslatedWord_HanViet);
                list.Add(new CharRange(stringBuilder.ToString().Length, ChineseToHanViet(chinese[length - 1]).Length));
            }
            else
            {
                stringBuilder.Append(chinese[length - 1]);
                LastTranslatedWord_HanViet += chinese[length - 1].ToString();
                list.Add(new CharRange(stringBuilder.ToString().Length, 1));
            }
            chineseHanVietMappingArray = list.ToArray();
            LastTranslatedWord_HanViet = "";
            return stringBuilder.ToString();
        }

        public static string ChineseToHanVietForBrowser(string chinese)
        {
            if (string.IsNullOrEmpty(chinese))
            {
                return "";
            }
            chinese = StandardizeInputForBrowser(chinese);
            StringBuilder stringBuilder = new StringBuilder();
            string[] array = classifyWordsIntoLatinAndChinese(chinese);
            int num = array.Length;
            for (int i = 0; i < num; i++)
            {
                string text = array[i];
                if (!string.IsNullOrEmpty(text))
                {
                    string text2;
                    if (isChinese(text[0]))
                    {
                        CharRange[] array2;
                        text2 = ChineseToHanViet(text, out array2).TrimStart(new char[0]);
                        if (i == 0 || !array[i - 1].EndsWith(", "))
                        {
                            text2 = toUpperCase(text2);
                        }
                    }
                    else
                    {
                        text2 = text;
                    }
                    stringBuilder.Append(text2);
                }
            }
            return stringBuilder.ToString();
        }

        public static string ChineseToHanVietForBatch(string chinese)
        {
            string str = "";
            StringBuilder stringBuilder = new StringBuilder();
            int length = chinese.Length;
            for (int i = 0; i < length - 1; i++)
            {
                char c = chinese[i];
                char character = chinese[i + 1];
                if (isChinese(c))
                {
                    if (isChinese(character))
                    {
                        appendTranslatedWord(stringBuilder, ChineseToHanViet(c), ref str);
                        stringBuilder.Append(" ");
                        str += " ";
                    }
                    else
                    {
                        appendTranslatedWord(stringBuilder, ChineseToHanViet(c), ref str);
                    }
                }
                else
                {
                    stringBuilder.Append(c);
                    str += c.ToString();
                }
            }
            if (isChinese(chinese[length - 1]))
            {
                appendTranslatedWord(stringBuilder, ChineseToHanViet(chinese[length - 1]), ref str);
            }
            else
            {
                stringBuilder.Append(chinese[length - 1]);
                str += chinese[length - 1].ToString();
            }
            return stringBuilder.ToString();
        }

        public static string ChineseToHanViet(char chinese)
        {
            if (chinese == ' ')
            {
                return "";
            }
            if (!hanVietDictionary.ContainsKey(chinese.ToString()))
            {
                return ToNarrow(chinese.ToString());
            }
            return hanVietDictionary[chinese.ToString()];
        }

        public static string ChineseToVietPhrase(string chinese, int wrapType, int translationAlgorithm, bool prioritizedName, out CharRange[] chinesePhraseRanges, out CharRange[] vietPhraseRanges)
        {
            LastTranslatedWord_VietPhrase = "";
            List<CharRange> list = new List<CharRange>();
            List<CharRange> list2 = new List<CharRange>();
            StringBuilder stringBuilder = new StringBuilder();
            int num = chinese.Length - 1;
            int i = 0;
            int num2 = -1;
            int num3 = -1;
            int num4 = -1;
            loadNhanByDictionary();
            while (i <= num)
            {
                bool flag = false;
                bool flag2 = true;
                for (int j = 20; j > 0; j--)
                {
                    if (chinese.Length >= i + j)
                    {
                        if (vietPhraseDictionary.ContainsKey(chinese.Substring(i, j)))
                        {
                            if ((!prioritizedName || !containsName(chinese, i, j)) && (translationAlgorithm != 0 && translationAlgorithm != 2 || isLongestPhraseInSentence(chinese, i, j, vietPhraseDictionary, translationAlgorithm) || prioritizedName && onlyNameDictionary.ContainsKey(chinese.Substring(i, j))))
                            {
                                list.Add(new CharRange(i, j));
                                if (wrapType == 0)
                                {
                                    appendTranslatedWord(stringBuilder, vietPhraseDictionary[chinese.Substring(i, j)], ref LastTranslatedWord_VietPhrase);
                                    list2.Add(new CharRange(stringBuilder.ToString().Length - vietPhraseDictionary[chinese.Substring(i, j)].Length, vietPhraseDictionary[chinese.Substring(i, j)].Length));
                                }
                                else if (wrapType == 1 || wrapType == 11)
                                {
                                    appendTranslatedWord(stringBuilder, "[" + vietPhraseDictionary[chinese.Substring(i, j)] + "]", ref LastTranslatedWord_VietPhrase);
                                    list2.Add(new CharRange(stringBuilder.ToString().Length - vietPhraseDictionary[chinese.Substring(i, j)].Length - 2, vietPhraseDictionary[chinese.Substring(i, j)].Length + 2));
                                }
                                else if (hasOnlyOneMeaning(vietPhraseDictionary[chinese.Substring(i, j)]))
                                {
                                    appendTranslatedWord(stringBuilder, vietPhraseDictionary[chinese.Substring(i, j)], ref LastTranslatedWord_VietPhrase);
                                    list2.Add(new CharRange(stringBuilder.ToString().Length - vietPhraseDictionary[chinese.Substring(i, j)].Length, vietPhraseDictionary[chinese.Substring(i, j)].Length));
                                }
                                else
                                {
                                    appendTranslatedWord(stringBuilder, "[" + vietPhraseDictionary[chinese.Substring(i, j)] + "]", ref LastTranslatedWord_VietPhrase);
                                    list2.Add(new CharRange(stringBuilder.ToString().Length - vietPhraseDictionary[chinese.Substring(i, j)].Length - 2, vietPhraseDictionary[chinese.Substring(i, j)].Length + 2));
                                }
                                if (nextCharIsChinese(chinese, i + j - 1))
                                {
                                    stringBuilder.Append(" ");
                                    LastTranslatedWord_VietPhrase += " ";
                                }
                                flag = true;
                                i += j;
                                break;
                            }
                        }
                        else if (!chinese.Substring(i, j).Contains("\n") && !chinese.Substring(i, j).Contains("\t") && nhanByDictionary != null && flag2 && 2 < j && num2 < i + j - 1 && IsAllChinese(chinese.Substring(i, j)))
                        {
                            if (i < num3)
                            {
                                if (num3 < i + j && j <= num4 - num3)
                                {
                                    j = num3 - i + 1;
                                }
                            }
                            else
                            {
                                string empty = string.Empty;
                                int num5 = -1;
                                int num6 = containsLuatNhan(chinese.Substring(i, j), nhanByDictionary, out empty, out num5);
                                num3 = i + num6;
                                num4 = num3 + num5;
                                if (num6 == 0)
                                {
                                    if (isLongestPhraseInSentence(chinese, i - 1, num5 - 1, vietPhraseOneMeaningDictionary, translationAlgorithm))
                                    {
                                        j = num5;
                                        list.Add(new CharRange(i, j));
                                        string text = ChineseToLuatNhan(chinese.Substring(i, j), nhanByDictionary);
                                        if (wrapType == 0)
                                        {
                                            appendTranslatedWord(stringBuilder, text, ref LastTranslatedWord_VietPhrase);
                                            list2.Add(new CharRange(stringBuilder.ToString().Length - text.Length, text.Length));
                                        }
                                        else if (wrapType == 1 || wrapType == 11)
                                        {
                                            appendTranslatedWord(stringBuilder, "[" + text + "]", ref LastTranslatedWord_VietPhrase);
                                            list2.Add(new CharRange(stringBuilder.ToString().Length - text.Length - 2, text.Length + 2));
                                        }
                                        else if (hasOnlyOneMeaning(text))
                                        {
                                            appendTranslatedWord(stringBuilder, text, ref LastTranslatedWord_VietPhrase);
                                            list2.Add(new CharRange(stringBuilder.ToString().Length - text.Length, text.Length));
                                        }
                                        else
                                        {
                                            appendTranslatedWord(stringBuilder, "[" + text + "]", ref LastTranslatedWord_VietPhrase);
                                            list2.Add(new CharRange(stringBuilder.ToString().Length - text.Length - 2, text.Length + 2));
                                        }
                                        if (nextCharIsChinese(chinese, i + j - 1))
                                        {
                                            stringBuilder.Append(" ");
                                            LastTranslatedWord_VietPhrase += " ";
                                        }
                                        flag = true;
                                        i += j;
                                        break;
                                    }
                                }
                                else if (0 >= num6)
                                {
                                    num2 = i + j - 1;
                                    flag2 = false;
                                    int num7 = 100;
                                    while (i + num7 < chinese.Length && isChinese(chinese[i + num7 - 1]))
                                    {
                                        num7++;
                                    }
                                    if (i + num7 <= chinese.Length)
                                    {
                                        num6 = containsLuatNhan(chinese.Substring(i, num7), nhanByDictionary, out empty, out num5);
                                        if (num6 < 0)
                                        {
                                            num2 = i + num7 - 1;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                if (!flag)
                {
                    int length = stringBuilder.ToString().Length;
                    int num8 = ChineseToHanViet(chinese[i]).Length;
                    list.Add(new CharRange(i, 1));
                    if (isChinese(chinese[i]))
                    {
                        appendTranslatedWord(stringBuilder, (wrapType != 1 ? "" : "[") + ChineseToHanViet(chinese[i]) + (wrapType != 1 ? "" : "]"), ref LastTranslatedWord_VietPhrase);
                        if (nextCharIsChinese(chinese, i))
                        {
                            stringBuilder.Append(" ");
                            LastTranslatedWord_VietPhrase += " ";
                        }
                        num8 += wrapType != 1 ? 0 : 2;
                    }
                    else if ((chinese[i] == '"' || chinese[i] == '\'') && !LastTranslatedWord_VietPhrase.EndsWith(" ") && !LastTranslatedWord_VietPhrase.EndsWith(".") && !LastTranslatedWord_VietPhrase.EndsWith("?") && !LastTranslatedWord_VietPhrase.EndsWith("!") && !LastTranslatedWord_VietPhrase.EndsWith("\t") && i < chinese.Length - 1 && chinese[i + 1] != ' ' && chinese[i + 1] != ',')
                    {
                        stringBuilder.Append(" ").Append(chinese[i]);
                        LastTranslatedWord_VietPhrase = LastTranslatedWord_VietPhrase + " " + chinese[i].ToString();
                    }
                    else
                    {
                        stringBuilder.Append(chinese[i]);
                        LastTranslatedWord_VietPhrase += chinese[i].ToString();
                        num8 = 1;
                    }
                    list2.Add(new CharRange(length, num8));
                    i++;
                }
            }
            chinesePhraseRanges = list.ToArray();
            vietPhraseRanges = list2.ToArray();
            LastTranslatedWord_VietPhrase = "";
            return stringBuilder.ToString();
        }

        public static string ChineseToVietPhraseForBrowser(string chinese, int wrapType, int translationAlgorithm, bool prioritizedName)
        {
            chinese = StandardizeInputForBrowser(chinese);
            StringBuilder stringBuilder = new StringBuilder();
            string[] array = classifyWordsIntoLatinAndChinese(chinese);
            string[] array2 = array;
            for (int i = 0; i < array2.Length; i++)
            {
                string text = array2[i];
                if (!string.IsNullOrEmpty(text))
                {
                    if (isChinese(text[0]))
                    {
                        CharRange[] array3;
                        CharRange[] array4;
                        stringBuilder.Append(ChineseToVietPhrase(text, wrapType, translationAlgorithm, prioritizedName, out array3, out array4));
                    }
                    else
                    {
                        stringBuilder.Append(text);
                    }
                }
            }
            return stringBuilder.ToString();
        }

        public static string ChineseToVietPhraseForBatch(string chinese, int wrapType, int translationAlgorithm, bool prioritizedName)
        {
            string text = "";
            StringBuilder stringBuilder = new StringBuilder();
            int num = chinese.Length - 1;
            int i = 0;
            int num2 = -1;
            int num3 = -1;
            int num4 = -1;
            while (i <= num)
            {
                bool flag = false;
                bool flag2 = true;
                for (int j = 20; j > 0; j--)
                {
                    if (chinese.Length >= i + j)
                    {
                        if (vietPhraseDictionary.ContainsKey(chinese.Substring(i, j)))
                        {
                            if ((!prioritizedName || !containsName(chinese, i, j)) && (translationAlgorithm != 0 && translationAlgorithm != 2 || isLongestPhraseInSentence(chinese, i, j, vietPhraseDictionary, translationAlgorithm) || prioritizedName && onlyNameDictionary.ContainsKey(chinese.Substring(i, j))))
                            {
                                if (!string.IsNullOrEmpty(vietPhraseDictionary[chinese.Substring(i, j)]))
                                {
                                    if (wrapType == 0)
                                    {
                                        appendTranslatedWord(stringBuilder, vietPhraseDictionary[chinese.Substring(i, j)], ref text);
                                    }
                                    else if (wrapType == 1 || wrapType == 11)
                                    {
                                        appendTranslatedWord(stringBuilder, "[" + vietPhraseDictionary[chinese.Substring(i, j)] + "]", ref text);
                                    }
                                    else if (hasOnlyOneMeaning(vietPhraseDictionary[chinese.Substring(i, j)]))
                                    {
                                        appendTranslatedWord(stringBuilder, vietPhraseDictionary[chinese.Substring(i, j)], ref text);
                                    }
                                    else
                                    {
                                        appendTranslatedWord(stringBuilder, "[" + vietPhraseDictionary[chinese.Substring(i, j)] + "]", ref text);
                                    }
                                    if (nextCharIsChinese(chinese, i + j - 1))
                                    {
                                        stringBuilder.Append(" ");
                                        text += " ";
                                    }
                                }
                                flag = true;
                                i += j;
                                break;
                            }
                        }
                        else if (!chinese.Substring(i, j).Contains("\n") && !chinese.Substring(i, j).Contains("\t") && nhanByDictionary != null && flag2 && 2 < j && num2 < i + j - 1 && IsAllChinese(chinese.Substring(i, j)))
                        {
                            if (i < num3)
                            {
                                if (num3 < i + j && j <= num4 - num3)
                                {
                                    j = num3 - i + 1;
                                }
                            }
                            else
                            {
                                string empty = string.Empty;
                                int num5 = -1;
                                int num6 = containsLuatNhan(chinese.Substring(i, j), nhanByDictionary, out empty, out num5);
                                num3 = i + num6;
                                num4 = num3 + num5;
                                if (num6 == 0)
                                {
                                    if (isLongestPhraseInSentence(chinese, i - 1, num5 - 1, vietPhraseOneMeaningDictionary, translationAlgorithm))
                                    {
                                        j = num5;
                                        string text2 = ChineseToLuatNhan(chinese.Substring(i, j), nhanByDictionary);
                                        if (wrapType == 0)
                                        {
                                            appendTranslatedWord(stringBuilder, text2, ref text);
                                        }
                                        else if (wrapType == 1 || wrapType == 11)
                                        {
                                            appendTranslatedWord(stringBuilder, "[" + text2 + "]", ref text);
                                        }
                                        else if (hasOnlyOneMeaning(text2))
                                        {
                                            appendTranslatedWord(stringBuilder, text2, ref text);
                                        }
                                        else
                                        {
                                            appendTranslatedWord(stringBuilder, "[" + text2 + "]", ref text);
                                        }
                                        if (nextCharIsChinese(chinese, i + j - 1))
                                        {
                                            stringBuilder.Append(" ");
                                            text += " ";
                                        }
                                        flag = true;
                                        i += j;
                                        break;
                                    }
                                }
                                else if (0 >= num6)
                                {
                                    num2 = i + j - 1;
                                    flag2 = false;
                                    int num7 = 100;
                                    while (i + num7 < chinese.Length && isChinese(chinese[i + num7 - 1]))
                                    {
                                        num7++;
                                    }
                                    if (i + num7 <= chinese.Length)
                                    {
                                        num6 = containsLuatNhan(chinese.Substring(i, num7), nhanByDictionary, out empty, out num5);
                                        if (num6 < 0)
                                        {
                                            num2 = i + num7 - 1;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                if (!flag)
                {
                    if (isChinese(chinese[i]))
                    {
                        appendTranslatedWord(stringBuilder, (wrapType != 1 ? "" : "[") + ChineseToHanViet(chinese[i]) + (wrapType != 1 ? "" : "]"), ref text);
                        if (nextCharIsChinese(chinese, i))
                        {
                            stringBuilder.Append(" ");
                            text += " ";
                        }
                    }
                    else if ((chinese[i] == '"' || chinese[i] == '\'') && !text.EndsWith(" ") && !text.EndsWith(".") && !text.EndsWith("?") && !text.EndsWith("!") && !text.EndsWith("\t") && i < chinese.Length - 1 && chinese[i + 1] != ' ' && chinese[i + 1] != ',')
                    {
                        stringBuilder.Append(" ").Append(chinese[i]);
                        text = text + " " + chinese[i].ToString();
                    }
                    else
                    {
                        stringBuilder.Append(chinese[i]);
                        text += chinese[i].ToString();
                    }
                    i++;
                }
            }

            string result = stringBuilder.ToString().Replace("  ", " ");
            //for (int k = 0; k < arr.Length; k++)
            //{
            //    result = result.Replace("#img" + k + "#",arr[k]);
            //}
            return result;
        }

        public static string ChineseToVietPhraseOneMeaning(string chinese, int wrapType, int translationAlgorithm, bool prioritizedName, out CharRange[] chinesePhraseRanges, out CharRange[] vietPhraseRanges)
        {
            //translationAlgorithm:
            /*	0:ưu tiên cụm vp dài
			 *	1: dịch từ tría sang phải
			 *	2: ưu tiên cum vp dài >=4
			 *	
			 *	prioritizedName
			 *  true/false: ưu tiên cụm Names hơn VP

			*/

            Console.WriteLine(wrapType.ToString() + '\n' + translationAlgorithm.ToString() + "\n" + prioritizedName.ToString() + "\n");
            LastTranslatedWord_VietPhraseOneMeaning = "";
            List<CharRange> list = new List<CharRange>();
            List<CharRange> list2 = new List<CharRange>();
            StringBuilder stringBuilder = new StringBuilder();
            int num = chinese.Length - 1;
            int i = 0;
            int num2 = -1;
            int num3 = -1;
            int num4 = -1;
            loadNhanByOneMeaningDictionary();
            while (i <= num)
            {
                bool flag = false;
                bool flag2 = true;
                for (int j = 20; j > 0; j--)
                {
                    if (chinese.Length >= i + j)
                    {
                        if (vietPhraseOneMeaningDictionary.ContainsKey(chinese.Substring(i, j)))
                        {
                            if ((!prioritizedName || !containsName(chinese, i, j)) && (translationAlgorithm != 0 && translationAlgorithm != 2 || isLongestPhraseInSentence(chinese, i, j, vietPhraseOneMeaningDictionary, translationAlgorithm) || prioritizedName && onlyNameDictionary.ContainsKey(chinese.Substring(i, j))))
                            {
                                list.Add(new CharRange(i, j));
                                if (wrapType == 0)
                                {
                                    appendTranslatedWord(stringBuilder, vietPhraseOneMeaningDictionary[chinese.Substring(i, j)], ref LastTranslatedWord_VietPhraseOneMeaning);
                                    list2.Add(new CharRange(stringBuilder.ToString().Length - vietPhraseOneMeaningDictionary[chinese.Substring(i, j)].Length, vietPhraseOneMeaningDictionary[chinese.Substring(i, j)].Length));
                                }
                                else
                                {
                                    appendTranslatedWord(stringBuilder, "[" + vietPhraseOneMeaningDictionary[chinese.Substring(i, j)] + "]", ref LastTranslatedWord_VietPhraseOneMeaning);
                                    list2.Add(new CharRange(stringBuilder.ToString().Length - vietPhraseOneMeaningDictionary[chinese.Substring(i, j)].Length - 2, vietPhraseOneMeaningDictionary[chinese.Substring(i, j)].Length + 2));
                                }
                                if (nextCharIsChinese(chinese, i + j - 1))
                                {
                                    stringBuilder.Append(" ");
                                    LastTranslatedWord_VietPhraseOneMeaning += " ";
                                }
                                flag = true;
                                i += j;
                                break;
                            }
                        }
                        else if (!chinese.Substring(i, j).Contains("\n") && !chinese.Substring(i, j).Contains("\t") && nhanByOneMeaningDictionary != null && flag2 && 2 < j && num2 < i + j - 1 && IsAllChinese(chinese.Substring(i, j)))
                        {
                            if (i < num3)
                            {
                                if (num3 < i + j && j <= num4 - num3)
                                {
                                    j = num3 - i + 1;
                                }
                            }
                            else
                            {
                                string empty = string.Empty;
                                int num5 = -1;
                                int num6 = containsLuatNhan(chinese.Substring(i, j), nhanByOneMeaningDictionary, out empty, out num5);
                                num3 = i + num6;
                                num4 = num3 + num5;
                                if (num6 == 0)
                                {
                                    if (isLongestPhraseInSentence(chinese, i - 1, num5 - 1, vietPhraseOneMeaningDictionary, translationAlgorithm))
                                    {
                                        j = num5;
                                        list.Add(new CharRange(i, j));
                                        string text = ChineseToLuatNhan(chinese.Substring(i, j), nhanByOneMeaningDictionary);
                                        if (wrapType == 0)
                                        {
                                            appendTranslatedWord(stringBuilder, text, ref LastTranslatedWord_VietPhraseOneMeaning);
                                            list2.Add(new CharRange(stringBuilder.ToString().Length - text.Length, text.Length));
                                        }
                                        else
                                        {
                                            appendTranslatedWord(stringBuilder, "[" + text + "]", ref LastTranslatedWord_VietPhraseOneMeaning);
                                            list2.Add(new CharRange(stringBuilder.ToString().Length - text.Length - 2, text.Length + 2));
                                        }
                                        if (nextCharIsChinese(chinese, i + j - 1))
                                        {
                                            stringBuilder.Append(" ");
                                            LastTranslatedWord_VietPhraseOneMeaning += " ";
                                        }
                                        flag = true;
                                        i += j;
                                        break;
                                    }
                                }
                                else if (0 >= num6)
                                {
                                    num2 = i + j - 1;
                                    flag2 = false;
                                    int num7 = 100;
                                    while (i + num7 < chinese.Length && isChinese(chinese[i + num7 - 1]))
                                    {
                                        num7++;
                                    }
                                    if (i + num7 <= chinese.Length)
                                    {
                                        num6 = containsLuatNhan(chinese.Substring(i, num7), nhanByOneMeaningDictionary, out empty, out num5);
                                        if (num6 < 0)
                                        {
                                            num2 = i + num7 - 1;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                if (!flag)
                {
                    int length = stringBuilder.ToString().Length;
                    int num8 = ChineseToHanViet(chinese[i]).Length;
                    list.Add(new CharRange(i, 1));
                    if (isChinese(chinese[i]))
                    {
                        appendTranslatedWord(stringBuilder, (wrapType != 1 ? "" : "[") + ChineseToHanViet(chinese[i]) + (wrapType != 1 ? "" : "]"), ref LastTranslatedWord_VietPhraseOneMeaning);
                        if (nextCharIsChinese(chinese, i))
                        {
                            stringBuilder.Append(" ");
                            LastTranslatedWord_VietPhraseOneMeaning += " ";
                        }
                        num8 += wrapType != 1 ? 0 : 2;
                    }
                    else if ((chinese[i] == '"' || chinese[i] == '\'') && !LastTranslatedWord_VietPhraseOneMeaning.EndsWith(" ") && !LastTranslatedWord_VietPhraseOneMeaning.EndsWith(".") && !LastTranslatedWord_VietPhraseOneMeaning.EndsWith("?") && !LastTranslatedWord_VietPhraseOneMeaning.EndsWith("!") && !LastTranslatedWord_VietPhraseOneMeaning.EndsWith("\t") && i < chinese.Length - 1 && chinese[i + 1] != ' ' && chinese[i + 1] != ',')
                    {
                        stringBuilder.Append(" ").Append(chinese[i]);
                        LastTranslatedWord_VietPhraseOneMeaning = LastTranslatedWord_VietPhraseOneMeaning + " " + chinese[i].ToString();
                    }
                    else
                    {
                        stringBuilder.Append(chinese[i]);
                        LastTranslatedWord_VietPhraseOneMeaning += chinese[i].ToString();
                        num8 = 1;
                    }
                    list2.Add(new CharRange(length, num8));
                    i++;
                }
            }
            chinesePhraseRanges = list.ToArray();
            vietPhraseRanges = list2.ToArray();
            LastTranslatedWord_VietPhraseOneMeaning = "";
            return stringBuilder.ToString();
        }

        public static string ChineseToVietPhraseOneMeaningForBrowser(string chinese, int wrapType, int translationAlgorithm, bool prioritizedName)
        {
            chinese = StandardizeInputForBrowser(chinese);
            StringBuilder stringBuilder = new StringBuilder();
            string[] array = classifyWordsIntoLatinAndChinese(chinese);
            int num = array.Length;
            for (int i = 0; i < num; i++)
            {
                string text = array[i];
                if (!string.IsNullOrEmpty(text))
                {
                    string text2;
                    if (isChinese(text[0]))
                    {
                        CharRange[] array2;
                        CharRange[] array3;
                        text2 = ChineseToVietPhraseOneMeaning(text, wrapType, translationAlgorithm, prioritizedName, out array2, out array3).TrimStart(new char[0]);
                        if (i == 0 || !array[i - 1].EndsWith(", "))
                        {
                            text2 = toUpperCase(text2);
                        }
                    }
                    else
                    {
                        text2 = text;
                    }
                    stringBuilder.Append(text2);
                }
            }
            return stringBuilder.ToString();
        }

        public static string ChineseToVietPhraseOneMeaningForProxy(string chinese, int wrapType, int translationAlgorithm, bool prioritizedName)
        {
            chinese = StandardizeInputForProxy(chinese);
            StringBuilder stringBuilder = new StringBuilder();
            string[] array = classifyWordsIntoLatinAndChineseForProxy(chinese);
            string[] array2 = array;
            for (int i = 0; i < array2.Length; i++)
            {
                string text = array2[i];
                if (!string.IsNullOrEmpty(text))
                {
                    if (isChinese(text[0]))
                    {
                        CharRange[] array3;
                        CharRange[] array4;
                        stringBuilder.Append(ChineseToVietPhraseOneMeaning(text, wrapType, translationAlgorithm, prioritizedName, out array3, out array4));
                    }
                    else
                    {
                        stringBuilder.Append(text);
                    }
                }
            }
            return stringBuilder.ToString();
        }

        public static string ChineseToVietPhraseOneMeaningForBatch(string chinese, int wrapType, int translationAlgorithm, bool prioritizedName)
        {
            DateTime arg_05_0 = DateTime.Now;
            string text = "";
            StringBuilder stringBuilder = new StringBuilder();

            int num = chinese.Length - 1;
            int i = 0;
            int num2 = -1;
            int num3 = -1;
            int num4 = -1;
            while (i <= num)
            {
                bool flag = false;
                bool flag2 = true;
                if (chinese[i] != '\n' && chinese[i] != '\t')
                {
                    for (int j = 20; j > 0; j--)
                    {
                        if (chinese.Length >= i + j)
                        {
                            if (vietPhraseOneMeaningDictionary.ContainsKey(chinese.Substring(i, j)))
                            {
                                if ((!prioritizedName || !containsName(chinese, i, j)) && (translationAlgorithm != 0 && translationAlgorithm != 2 || isLongestPhraseInSentence(chinese, i, j, vietPhraseOneMeaningDictionary, translationAlgorithm) || prioritizedName && onlyNameDictionary.ContainsKey(chinese.Substring(i, j))))
                                {
                                    if (!string.IsNullOrEmpty(vietPhraseOneMeaningDictionary[chinese.Substring(i, j)]))
                                    {
                                        if (wrapType == 0)
                                        {
                                            appendTranslatedWord(stringBuilder, vietPhraseOneMeaningDictionary[chinese.Substring(i, j)], ref text);
                                        }
                                        else
                                        {
                                            if (wrapType == 2)
                                            {
                                                appendTranslatedWord(stringBuilder, "<i h=\"" + ChineseToHanVietForAnalyzer(chinese.Substring(i, j)) + "\" t=\"" + chinese.Substring(i, j) + "\" v=\"" + vietPhraseDictionary[chinese.Substring(i, j)] + "\" >" + vietPhraseOneMeaningDictionary[chinese.Substring(i, j)] + "</i>", ref text);

                                            }
                                            else
                                            {
                                                appendTranslatedWord(stringBuilder, "[" + vietPhraseOneMeaningDictionary[chinese.Substring(i, j)] + "]", ref text);
                                            }
                                        }
                                        if (nextCharIsChinese(chinese, i + j - 1))
                                        {
                                            stringBuilder.Append(" ");
                                            text += " ";
                                        }
                                    }
                                    flag = true;
                                    i += j;
                                    break;
                                }
                            }
                            else if (!chinese.Substring(i, j).Contains("\n") && !chinese.Substring(i, j).Contains("\t") && nhanByOneMeaningDictionary != null && flag2 && 2 < j && num2 < i + j - 1 && IsAllChinese(chinese.Substring(i, j)))
                            {
                                if (i < num3)
                                {
                                    if (num3 < i + j && j <= num4 - num3)
                                    {
                                        j = num3 - i + 1;
                                    }
                                }
                                else
                                {
                                    string empty = string.Empty;
                                    int num5 = -1;
                                    int num6 = containsLuatNhan(chinese.Substring(i, j), nhanByOneMeaningDictionary, out empty, out num5);
                                    num3 = i + num6;
                                    num4 = num3 + num5;
                                    if (num6 == 0)
                                    {
                                        if (isLongestPhraseInSentence(chinese, i - 1, num5 - 1, vietPhraseOneMeaningDictionary, translationAlgorithm))
                                        {
                                            j = num5;
                                            string text2 = ChineseToLuatNhan(chinese.Substring(i, j), nhanByOneMeaningDictionary);
                                            if (wrapType == 0)
                                            {
                                                appendTranslatedWord(stringBuilder, text2, ref text);
                                            }
                                            else
                                            {
                                                if (wrapType == 2)
                                                {
                                                    appendTranslatedWord(stringBuilder, "<i h=\"" + ChineseToHanVietForAnalyzer(chinese.Substring(i, j)) + "\" t=\"" + chinese.Substring(i, j)+"\" v=\""+ text2 + "\"  >" + text2 + "</i>", ref text);
                                                }
                                                else
                                                {
                                                    appendTranslatedWord(stringBuilder, "[" + text2 + "]", ref text);
                                                }

                                            }
                                            if (nextCharIsChinese(chinese, i + j - 1))
                                            {
                                                stringBuilder.Append(" ");
                                                text += " ";
                                            }
                                            flag = true;
                                            i += j;
                                            break;
                                        }
                                    }
                                    else if (0 >= num6)
                                    {
                                        num2 = i + j - 1;
                                        flag2 = false;
                                        int num7 = 100;
                                        while (i + num7 < chinese.Length && isChinese(chinese[i + num7 - 1]))
                                        {
                                            num7++;
                                        }
                                        if (i + num7 <= chinese.Length)
                                        {
                                            num6 = containsLuatNhan(chinese.Substring(i, num7), nhanByOneMeaningDictionary, out empty, out num5);
                                            if (num6 < 0)
                                            {
                                                num2 = i + num7 - 1;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                if (!flag)
                {
                    if (isChinese(chinese[i]))
                    {
                        if (wrapType == 2)
                        {
                            appendTranslatedWord(stringBuilder, "<i h=\"" + ChineseToHanViet(chinese[i]) + "\" t=\"" + chinese[i] + "\"  >" + ChineseToHanViet(chinese[i]) + "</i>", ref text);
                        }
                        else
                        {
                            appendTranslatedWord(stringBuilder, (wrapType != 1 ? "" : "[") + ChineseToHanViet(chinese[i]) + (wrapType != 1 ? "" : "]"), ref text);
                        }

                        if (nextCharIsChinese(chinese, i))
                        {
                            stringBuilder.Append(" ");
                            text += " ";
                        }
                    }
                    else if ((chinese[i] == '"' || chinese[i] == '\'') && !text.EndsWith(" ") && !text.EndsWith(".") && !text.EndsWith("?") && !text.EndsWith("!") && !text.EndsWith("\t") && i < chinese.Length - 1 && chinese[i + 1] != ' ' && chinese[i + 1] != ',')
                    {
                        stringBuilder.Append(" ").Append(chinese[i]);
                        text = text + " " + chinese[i].ToString();
                    }
                    else
                    {
                        stringBuilder.Append(chinese[i]);
                        text += chinese[i].ToString();
                    }
                    i++;
                }
            }
            return stringBuilder.ToString();
        }

        public static string ChineseToNameForBatch(string chinese)
        {
            StringBuilder stringBuilder = new StringBuilder();
            int num = chinese.Length - 1;
            int i = 0;
            while (i <= num)
            {
                bool flag = false;
                if (isChinese(chinese[i]))
                {
                    for (int j = 20; j > 0; j--)
                    {
                        if (chinese.Length >= i + j && onlyNameDictionary.ContainsKey(chinese.Substring(i, j)))
                        {
                            stringBuilder.Append(onlyNameDictionary[chinese.Substring(i, j)]);
                            flag = true;
                            i += j;
                            break;
                        }
                    }
                }
                if (!flag)
                {
                    stringBuilder.Append(chinese[i]);
                    i++;
                }
            }
            return stringBuilder.ToString();
        }

        public static string ChineseToMeanings(string chinese, out int phraseTranslatedLength)
        {
            string text = "";
            if (chinese.Length == 0)
            {
                phraseTranslatedLength = 0;
                return "";
            }
            int num = 0;
            for (int i = 20; i > 0; i--)
            {
                if (chinese.Length >= i && !chinese.Substring(0, i).Contains("\n") && !chinese.Substring(0, i).Contains("\t"))
                {
                    if (containsLuatNhan(chinese.Substring(0, i), vietPhraseDictionary) != 0)
                    {
                        break;
                    }
                    if (matchesLuatNhan(chinese.Substring(0, i), vietPhraseDictionary))
                    {
                        string empty = string.Empty;
                        ChineseToLuatNhan(chinese.Substring(0, i), vietPhraseDictionary, out empty);
                        string text2 = text;
                        text = string.Concat(new string[]
                        {
                            text2,
                            empty,
                            " <<Luật Nhân>> ",
                            luatNhanDictionary[empty].Replace("/", "; "),
                            "\n-----------------\n"
                        });
                        if (num == 0)
                        {
                            num = i;
                        }
                    }
                }
            }
            for (int j = 20; j > 0; j--)
            {
                if (chinese.Length >= j)
                {
                    string text3 = chinese.Substring(0, j);
                    if (vietPhraseDictionary.ContainsKey(text3))
                    {
                        string text4 = text;
                        text = string.Concat(new string[]
                        {
                            text4,
                            text3,
                            " <<VietPhrase>> ",
                            vietPhraseDictionary[text3].Replace("/", "; "),
                            "\n-----------------\n"
                        });
                        if (num == 0)
                        {
                            num = text3.Length;
                        }
                    }
                }
            }
            for (int k = 20; k > 0; k--)
            {
                if (chinese.Length >= k)
                {
                    string text3 = chinese.Substring(0, k);
                    if (lacVietDictionary.ContainsKey(text3))
                    {
                        string text5 = text;
                        text = string.Concat(new string[]
                        {
                            text5,
                            text3,
                            " <<Lạc Việt>>\n",
                            lacVietDictionary[text3],
                            "\n-----------------\n"
                        });
                        if (num == 0)
                        {
                            num = 1;
                        }
                    }
                }
            }
            for (int l = 20; l > 0; l--)
            {
                if (chinese.Length >= l)
                {
                    string text3 = chinese.Substring(0, l);
                    if (cedictDictionary.ContainsKey(text3))
                    {
                        string text6 = text;
                        text = string.Concat(new string[]
                        {
                            text6,
                            text3,
                            " <<Cedict or Babylon>> ",
                            cedictDictionary[text3].Replace("] /", "] ").Replace("/", "; "),
                            "\n-----------------\n"
                        });
                        if (num == 0)
                        {
                            num = 1;
                        }
                    }
                }
            }
            if (thieuChuuDictionary.ContainsKey(chinese[0].ToString()))
            {
                num = num == 0 ? 1 : num;
                object obj = text;
                text = string.Concat(new object[]
                {
                    obj,
                    chinese[0],
                    " <<Thiều Chửu>> ",
                    thieuChuuDictionary[chinese[0].ToString()],
                    "\n-----------------\n"
                });
            }
            int num2 = chinese.Length < 10 ? chinese.Length : 10;
            text = text + chinese.Substring(0, num2).Trim("\n\t ".ToCharArray()) + " <<Phiên Âm English>> ";
            for (int m = 0; m < num2; m++)
            {
                if (chinesePhienAmEnglishDictionary.ContainsKey(chinese[m].ToString()))
                {
                    text = text + "[" + chinesePhienAmEnglishDictionary[chinese[m].ToString()] + "] ";
                }
                else
                {
                    text = text + ChineseToHanViet(chinese[m]) + " ";
                }
            }
            if (num == 0)
            {
                num = 1;
                text = chinese[0] + "\n-----------------\nNot Found";
            }
            phraseTranslatedLength = num;
            return text;
        }

        public static void LoadDictionaries()
        {
            lock (lockObject)
            {
                if (dictionaryDirty)
                {
                    ThreadPool.QueueUserWorkItem(new WaitCallback(loadHanVietDictionaryWithNewThread));
                    ThreadPool.QueueUserWorkItem(new WaitCallback(loadThieuChuuDictionaryWithNewThread));
                    ThreadPool.QueueUserWorkItem(new WaitCallback(loadLacVietDictionaryWithNewThread));
                    ThreadPool.QueueUserWorkItem(new WaitCallback(loadCedictDictionaryWithNewThread));
                    ThreadPool.QueueUserWorkItem(new WaitCallback(loadChinesePhienAmEnglishDictionaryWithNewThread));
                    ThreadPool.QueueUserWorkItem(new WaitCallback(loadIgnoredChinesePhraseListsWithNewThread));
                    ThreadPool.QueueUserWorkItem(new WaitCallback(loadOnlyNameDictionaryHistoryWithNewThread));
                    ThreadPool.QueueUserWorkItem(new WaitCallback(loadOnlyNamePhuDictionaryHistoryWithNewThread));
                    ThreadPool.QueueUserWorkItem(new WaitCallback(loadOnlyVietPhraseDictionaryHistoryWithNewThread));
                    ThreadPool.QueueUserWorkItem(new WaitCallback(loadHanVietDictionaryHistoryWithNewThread));
                    ManualResetEvent[] array = new ManualResetEvent[]
                    {
                        new ManualResetEvent(false)
                    };
                    ThreadPool.QueueUserWorkItem(new WaitCallback(loadLuatNhanDictionaryWithNewThread), array[0]);
                    ManualResetEvent[] array2 = new ManualResetEvent[]
                    {
                        new ManualResetEvent(false)
                    };
                    ThreadPool.QueueUserWorkItem(new WaitCallback(loadPronounDictionaryWithNewThread), array2[0]);
                    ManualResetEvent[] array3 = new ManualResetEvent[]
                    {
                        new ManualResetEvent(false)
                    };
                    ThreadPool.QueueUserWorkItem(new WaitCallback(loadOnlyVietPhraseDictionaryWithNewThread), array3[0]);
                    ManualResetEvent[] array4 = new ManualResetEvent[]
                    {
                        new ManualResetEvent(false)
                    };
                    ThreadPool.QueueUserWorkItem(new WaitCallback(loadOnlyNameDictionaryWithNewThread), array4[0]);
                    WaitHandle.WaitAll(array2);
                    WaitHandle.WaitAll(array3);
                    WaitHandle.WaitAll(array4);
                    loadVietPhraseDictionary();
                    vietPhraseDictionaryToVietPhraseOneMeaningDictionary();
                    pronounDictionaryToPronounOneMeaningDictionary();
                    loadNhanByDictionary();
                    loadNhanByOneMeaningDictionary();
                    WaitHandle.WaitAll(array);
                    dictionaryDirty = false;
                }
            }
        }

        private static void loadLuatNhanDictionaryWithNewThread(object stateInfo)
        {
            loadLuatNhanDictionary();
            ((ManualResetEvent)stateInfo).Set();
        }

        private static void loadLuatNhanDictionary()
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            using (TextReader textReader = new StreamReader(DictionaryConfigurationHelper.GetLuatNhanDictionaryPath(), true))
            {
                string text;
                while ((text = textReader.ReadLine()) != null)
                {
                    if (!text.StartsWith("#"))
                    {
                        string[] array = text.Split(new char[]
                        {
                            '='
                        });
                        if (array.Length == 2 && !dictionary.ContainsKey(array[0]))
                        {
                            dictionary.Add(array[0], array[1]);
                        }
                    }
                }
            }
            IOrderedEnumerable<KeyValuePair<string, string>> orderedEnumerable = from pair in dictionary
                                                                                 orderby pair.Key.Length descending, pair.Key
                                                                                 select pair;
            luatNhanDictionary.Clear();
            foreach (KeyValuePair<string, string> current in orderedEnumerable)
            {
                luatNhanDictionary.Add(current.Key, current.Value);
            }
        }

        private static int compareLuatNhan(KeyValuePair<string, string> x, KeyValuePair<string, string> y)
        {
            if (x.Key.StartsWith("{0}") || x.Key.EndsWith("{0}"))
            {
                if (!y.Key.StartsWith("{0}") && !y.Key.EndsWith("{0}"))
                {
                    return 1;
                }
            }
            else if (y.Key.StartsWith("{0}") || y.Key.EndsWith("{0}"))
            {
                return -1;
            }
            return y.Key.Length - x.Key.Length;
        }

        private static void loadHanVietDictionaryWithNewThread(object stateInfo)
        {
            loadHanVietDictionary();
        }

        private static void loadHanVietDictionary()
        {
            hanVietDictionary.Clear();
            using (TextReader textReader = new StreamReader(DictionaryConfigurationHelper.GetChinesePhienAmWordsDictionaryPath(), true))
            {
                string text;
                while ((text = textReader.ReadLine()) != null)
                {
                    string[] array = text.Split(new char[]
                    {
                        '='
                    });
                    if (array.Length == 2 && !hanVietDictionary.ContainsKey(array[0]))
                    {
                        hanVietDictionary.Add(array[0], array[1]);
                    }
                }
            }
        }

        private static void loadVietPhraseDictionary()
        {
            vietPhraseDictionary.Clear();
            foreach (KeyValuePair<string, string> current in onlyNameDictionary)
            {
                if (!vietPhraseDictionary.ContainsKey(current.Key))
                {
                    vietPhraseDictionary.Add(current.Key, current.Value);
                }
            }
            foreach (KeyValuePair<string, string> current2 in onlyVietPhraseDictionary)
            {
                if (!vietPhraseDictionary.ContainsKey(current2.Key))
                {
                    vietPhraseDictionary.Add(current2.Key, current2.Value);
                }
            }
        }

        private static void loadOnlyVietPhraseDictionaryWithNewThread(object stateInfo)
        {
            loadOnlyVietPhraseDictionary();
            ((ManualResetEvent)stateInfo).Set();
        }

        private static void loadOnlyVietPhraseDictionary()
        {
            onlyVietPhraseDictionary.Clear();
            using (TextReader textReader = new StreamReader(DictionaryConfigurationHelper.GetVietPhraseDictionaryPath(), true))
            {
                string text;
                while ((text = textReader.ReadLine()) != null)
                {
                    string[] array = text.Split(new char[]
                    {
                        '='
                    });
                    if (array.Length == 2 && !onlyVietPhraseDictionary.ContainsKey(array[0]))
                    {
                        onlyVietPhraseDictionary.Add(array[0], array[1]);
                    }
                }
            }
        }

        private static void loadOnlyNameDictionaryWithNewThread(object stateInfo)
        {
            loadOnlyNameDictionary();
            ((ManualResetEvent)stateInfo).Set();
        }

        private static void loadOnlyNameDictionary()
        {
            onlyNameDictionary.Clear();
            onlyNameOneMeaningDictionary.Clear();
            onlyNameChinhDictionary.Clear();
            onlyNamePhuDictionary.Clear();
            char[] separator = "/|".ToCharArray();
            using (TextReader textReader = new StreamReader(DictionaryConfigurationHelper.GetNamesDictionaryPath(), true))
            {
                string text;
                while ((text = textReader.ReadLine()) != null)
                {
                    string[] array = text.Split(new char[]
                    {
                        '='
                    });
                    if (array.Length == 2 && !onlyNameDictionary.ContainsKey(array[0]))
                    {
                        onlyNameDictionary.Add(array[0], array[1]);
                        onlyNameOneMeaningDictionary.Add(array[0], array[1].Split(separator)[0]);
                        onlyNameChinhDictionary.Add(array[0], array[1]);
                    }
                }
            }
            using (TextReader textReader2 = new StreamReader(DictionaryConfigurationHelper.GetNamesPhuDictionaryPath(), true))
            {
                string text2;
                while ((text2 = textReader2.ReadLine()) != null)
                {
                    string[] array2 = text2.Split(new char[]
                    {
                        '='
                    });
                    if (array2.Length == 2 && !onlyNamePhuDictionary.ContainsKey(array2[0]))
                    {
                        if (onlyNameDictionary.ContainsKey(array2[0]))
                        {
                            onlyNameDictionary[array2[0]] = array2[1];
                            onlyNameOneMeaningDictionary[array2[0]] = array2[1].Split(separator)[0];
                        }
                        else
                        {
                            onlyNameDictionary.Add(array2[0], array2[1]);
                            onlyNameOneMeaningDictionary.Add(array2[0], array2[1].Split(separator)[0]);
                        }
                        onlyNamePhuDictionary.Add(array2[0], array2[1]);
                    }
                }
            }
        }

        private static void vietPhraseDictionaryToVietPhraseOneMeaningDictionary()
        {
            vietPhraseOneMeaningDictionary.Clear();
            foreach (KeyValuePair<string, string> current in vietPhraseDictionary)
            {
                vietPhraseOneMeaningDictionary.Add(current.Key, current.Value.Contains("/") || current.Value.Contains("|") ? current.Value.Split(new char[]
                {
                    '/',
                    '|'
                })[0] : current.Value);
            }
        }

        private static void pronounDictionaryToPronounOneMeaningDictionary()
        {
            pronounOneMeaningDictionary.Clear();
            foreach (KeyValuePair<string, string> current in pronounDictionary)
            {
                pronounOneMeaningDictionary.Add(current.Key, current.Value.Contains("/") || current.Value.Contains("|") ? current.Value.Split(new char[]
                {
                    '/',
                    '|'
                })[0] : current.Value);
            }
        }

        private static void loadNhanByDictionary()
        {
            if (DictionaryConfigurationHelper.IsNhanByPronouns)
            {
                nhanByDictionary = pronounDictionary;
                return;
            }
            if (DictionaryConfigurationHelper.IsNhanByPronounsAndNames)
            {
                nhanByDictionary = new Dictionary<string, string>(pronounDictionary);
                using (Dictionary<string, string>.Enumerator enumerator = onlyNameDictionary.GetEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        KeyValuePair<string, string> current = enumerator.Current;
                        if (!nhanByDictionary.ContainsKey(current.Key))
                        {
                            nhanByDictionary.Add(current.Key, current.Value);
                        }
                    }
                    return;
                }
            }
            if (DictionaryConfigurationHelper.IsNhanByPronounsAndNamesAndVietPhrase)
            {
                nhanByDictionary = new Dictionary<string, string>(pronounDictionary);
                using (Dictionary<string, string>.Enumerator enumerator2 = vietPhraseDictionary.GetEnumerator())
                {
                    while (enumerator2.MoveNext())
                    {
                        KeyValuePair<string, string> current2 = enumerator2.Current;
                        if (!nhanByDictionary.ContainsKey(current2.Key))
                        {
                            nhanByDictionary.Add(current2.Key, current2.Value);
                        }
                    }
                    return;
                }
            }
            nhanByDictionary = null;
        }

        private static void loadNhanByOneMeaningDictionary()
        {
            if (DictionaryConfigurationHelper.IsNhanByPronouns)
            {
                nhanByOneMeaningDictionary = pronounOneMeaningDictionary;
                return;
            }
            if (DictionaryConfigurationHelper.IsNhanByPronounsAndNames)
            {
                nhanByOneMeaningDictionary = new Dictionary<string, string>(pronounOneMeaningDictionary);
                using (Dictionary<string, string>.Enumerator enumerator = onlyNameOneMeaningDictionary.GetEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        KeyValuePair<string, string> current = enumerator.Current;
                        if (!nhanByOneMeaningDictionary.ContainsKey(current.Key))
                        {
                            nhanByOneMeaningDictionary.Add(current.Key, current.Value);
                        }
                    }
                    return;
                }
            }
            if (DictionaryConfigurationHelper.IsNhanByPronounsAndNamesAndVietPhrase)
            {
                nhanByOneMeaningDictionary = new Dictionary<string, string>(pronounOneMeaningDictionary);
                using (Dictionary<string, string>.Enumerator enumerator2 = vietPhraseOneMeaningDictionary.GetEnumerator())
                {
                    while (enumerator2.MoveNext())
                    {
                        KeyValuePair<string, string> current2 = enumerator2.Current;
                        if (!nhanByOneMeaningDictionary.ContainsKey(current2.Key))
                        {
                            nhanByOneMeaningDictionary.Add(current2.Key, current2.Value);
                        }
                    }
                    return;
                }
            }
            nhanByOneMeaningDictionary = null;
        }

        private static void loadThieuChuuDictionaryWithNewThread(object stateInfo)
        {
            loadThieuChuuDictionary();
        }

        private static void loadThieuChuuDictionary()
        {
            thieuChuuDictionary.Clear();
            using (TextReader textReader = new StreamReader(DictionaryConfigurationHelper.GetThieuChuuDictionaryPath(), true))
            {
                string text;
                while ((text = textReader.ReadLine()) != null)
                {
                    string[] array = text.Split(new char[]
                    {
                        '='
                    });
                    if (array.Length == 2 && !thieuChuuDictionary.ContainsKey(array[0]))
                    {
                        thieuChuuDictionary.Add(array[0], array[1]);
                    }
                }
            }
        }

        private static void loadLacVietDictionaryWithNewThread(object stateInfo)
        {
            loadLacVietDictionary();
        }

        private static void loadLacVietDictionary()
        {
            lacVietDictionary.Clear();
            using (TextReader textReader = new StreamReader(DictionaryConfigurationHelper.GetLacVietDictionaryPath(), true))
            {
                string text;
                while ((text = textReader.ReadLine()) != null)
                {
                    string[] array = text.Split(new char[]
                    {
                        '='
                    });
                    if (array.Length == 2 && !lacVietDictionary.ContainsKey(array[0]))
                    {
                        lacVietDictionary.Add(array[0], array[1]);
                    }
                }
            }
        }

        private static void loadCedictDictionaryWithNewThread(object stateInfo)
        {
            loadCedictDictionary();
        }

        private static void loadChinesePhienAmEnglishDictionaryWithNewThread(object stateInfo)
        {
            loadChinesePhienAmEnglishDictionary();
        }

        private static void loadPronounDictionaryWithNewThread(object stateInfo)
        {
            loadPronounDictionary();
            ((ManualResetEvent)stateInfo).Set();
        }

        private static void loadIgnoredChinesePhraseListsWithNewThread(object stateInfo)
        {
            loadIgnoredChinesePhraseLists();
        }

        private static void loadOnlyVietPhraseDictionaryHistoryWithNewThread(object stateInfo)
        {
            loadOnlyVietPhraseDictionaryHistory();
        }

        private static void loadOnlyNameDictionaryHistoryWithNewThread(object stateInfo)
        {
            loadOnlyNameDictionaryHistory();
        }

        private static void loadOnlyNamePhuDictionaryHistoryWithNewThread(object stateInfo)
        {
            loadOnlyNamePhuDictionaryHistory();
        }

        private static void loadHanVietDictionaryHistoryWithNewThread(object stateInfo)
        {
            loadHanVietDictionaryHistory();
        }

        private static void loadOnlyVietPhraseDictionaryHistory()
        {
            LoadDictionaryHistory(DictionaryConfigurationHelper.GetVietPhraseDictionaryHistoryPath(), ref onlyVietPhraseDictionaryHistoryDataSet);
        }

        private static void loadOnlyNameDictionaryHistory()
        {
            LoadDictionaryHistory(DictionaryConfigurationHelper.GetNamesDictionaryHistoryPath(), ref onlyNameDictionaryHistoryDataSet);
        }

        private static void loadOnlyNamePhuDictionaryHistory()
        {
            LoadDictionaryHistory(DictionaryConfigurationHelper.GetNamesPhuDictionaryHistoryPath(), ref onlyNamePhuDictionaryHistoryDataSet);
        }

        private static void loadHanVietDictionaryHistory()
        {
            LoadDictionaryHistory(DictionaryConfigurationHelper.GetChinesePhienAmWordsDictionaryHistoryPath(), ref hanVietDictionaryHistoryDataSet);
        }

        public static void LoadDictionaryHistory(string dictionaryHistoryPath, ref DataSet dictionaryHistoryDataSet)
        {
            dictionaryHistoryDataSet.Clear();
            string name = "DictionaryHistory";
            if (!dictionaryHistoryDataSet.Tables.Contains(name))
            {
                dictionaryHistoryDataSet.Tables.Add(name);
                dictionaryHistoryDataSet.Tables[name].Columns.Add("Entry", Type.GetType("System.String"));
                dictionaryHistoryDataSet.Tables[name].Columns.Add("Action", Type.GetType("System.String"));
                dictionaryHistoryDataSet.Tables[name].Columns.Add("User Name", Type.GetType("System.String"));
                dictionaryHistoryDataSet.Tables[name].Columns.Add("Updated Date", Type.GetType("System.DateTime"));
                dictionaryHistoryDataSet.Tables[name].PrimaryKey = new DataColumn[]
                {
                    dictionaryHistoryDataSet.Tables[name].Columns["Entry"]
                };
            }
            if (!File.Exists(dictionaryHistoryPath))
            {
                return;
            }
            string name2 = CharsetDetector.DetectChineseCharset(dictionaryHistoryPath);
            using (TextReader textReader = new StreamReader(dictionaryHistoryPath, Encoding.GetEncoding(name2)))
            {
                textReader.ReadLine();
                string text;
                while ((text = textReader.ReadLine()) != null)
                {
                    string[] array = text.Split(new char[]
                    {
                        '\t'
                    });
                    if (array.Length == 4)
                    {
                        DataRow dataRow = dictionaryHistoryDataSet.Tables[name].Rows.Find(array[0]);
                        if (dataRow == null)
                        {
                            dictionaryHistoryDataSet.Tables[name].Rows.Add(new object[]
                            {
                                array[0],
                                array[1],
                                array[2],
                                DateTime.ParseExact(array[3], "yyyy-MM-dd HH:mm:ss.fffzzz", null)
                            });
                        }
                        else
                        {
                            dataRow[1] = array[1];
                            dataRow[2] = array[2];
                            dataRow[3] = DateTime.ParseExact(array[3], "yyyy-MM-dd HH:mm:ss.fffzzz", null);
                        }
                    }
                }
            }
        }

        private static void loadCedictDictionary()
        {
            cedictDictionary.Clear();
            using (TextReader textReader = new StreamReader(DictionaryConfigurationHelper.GetCEDictDictionaryPath(), Encoding.UTF8))
            {
                string text;
                while ((text = textReader.ReadLine()) != null)
                {
                    if (!text.StartsWith("#"))
                    {
                        string text2 = text.Substring(0, text.IndexOf(" ["));
                        string[] array = text2.Split(new char[]
                        {
                            ' '
                        });
                        for (int i = 0; i < array.Length; i++)
                        {
                            string key = array[i];
                            if (!cedictDictionary.ContainsKey(key))
                            {
                                cedictDictionary.Add(key, text.Substring(text.IndexOf(" [")));
                            }
                        }
                    }
                }
            }
            using (TextReader textReader2 = new StreamReader(DictionaryConfigurationHelper.GetBabylonDictionaryPath(), Encoding.UTF8))
            {
                string text3;
                while ((text3 = textReader2.ReadLine()) != null)
                {
                    string[] array2 = text3.Split(new char[]
                    {
                        '='
                    });
                    if (!cedictDictionary.ContainsKey(array2[0]))
                    {
                        cedictDictionary.Add(array2[0], array2[1]);
                    }
                }
            }
        }

        private static void loadChinesePhienAmEnglishDictionary()
        {
            chinesePhienAmEnglishDictionary.Clear();
            using (TextReader textReader = new StreamReader(DictionaryConfigurationHelper.GetChinesePhienAmEnglishWordsDictionaryPath(), true))
            {
                string text;
                while ((text = textReader.ReadLine()) != null)
                {
                    string[] array = text.Split(new char[]
                    {
                        '='
                    });
                    if (array.Length == 2 && !chinesePhienAmEnglishDictionary.ContainsKey(array[0]))
                    {
                        chinesePhienAmEnglishDictionary.Add(array[0], array[1]);
                    }
                }
            }
        }

        private static void loadPronounDictionary()
        {
            pronounDictionary.Clear();
            using (TextReader textReader = new StreamReader(DictionaryConfigurationHelper.GetPronounsDictionaryPath(), true))
            {
                string text;
                while ((text = textReader.ReadLine()) != null)
                {
                    string[] array = text.Split(new char[]
                    {
                        '='
                    });
                    if (array.Length == 2 && !pronounDictionary.ContainsKey(array[0]))
                    {
                        pronounDictionary.Add(array[0], array[1]);
                    }
                }
            }
        }

        public static void AddIgnoredChinesePhrase(string ignoredChinesePhrase)
        {
            if (ignoredChinesePhraseList.Contains(ignoredChinesePhrase))
            {
                return;
            }
            ignoredChinesePhraseList.Add(ignoredChinesePhrase);
            try
            {
                File.WriteAllLines(DictionaryConfigurationHelper.GetIgnoredChinesePhraseListPath(), ignoredChinesePhraseList.ToArray(), Encoding.UTF8);
            }
            catch
            {
            }
            loadIgnoredChinesePhraseLists();
        }

        private static void loadIgnoredChinesePhraseLists()
        {
            ignoredChinesePhraseList.Clear();
            ignoredChinesePhraseForBrowserList.Clear();
            char[] trimChars = "\t\n".ToCharArray();
            using (TextReader textReader = new StreamReader(DictionaryConfigurationHelper.GetIgnoredChinesePhraseListPath(), true))
            {
                string text;
                while ((text = textReader.ReadLine()) != null)
                {
                    if (!string.IsNullOrEmpty(text))
                    {
                        string text2 = standardizeInputWithoutRemovingIgnoredChinesePhrases(text).Trim(trimChars);
                        if (!string.IsNullOrEmpty(text2) && !ignoredChinesePhraseList.Contains(text2))
                        {
                            ignoredChinesePhraseList.Add(text2);
                        }
                        string text3 = standardizeInputForBrowserWithoutRemovingIgnoredChinesePhrases(text).Trim(trimChars);
                        if (!string.IsNullOrEmpty(text3) && !ignoredChinesePhraseForBrowserList.Contains(text3))
                        {
                            ignoredChinesePhraseForBrowserList.Add(text3);
                        }
                    }
                }
            }
            ignoredChinesePhraseList.Sort(new Comparison<string>(compareStringByDescending));
            ignoredChinesePhraseForBrowserList.Sort(new Comparison<string>(compareStringByDescending));
        }

        private static int compareStringByDescending(string x, string y)
        {
            if (x == null)
            {
                if (y == null)
                {
                    return 0;
                }
                return 1;
            }
            else
            {
                if (y == null)
                {
                    return -1;
                }
                int num = x.Length.CompareTo(y.Length);
                if (num != 0)
                {
                    return num * -1;
                }
                return x.CompareTo(y) * -1;
            }
        }

        public static string StandardizeInput(string original)
        {
            string standardizedChinese = standardizeInputWithoutRemovingIgnoredChinesePhrases(original);
            standardizedChinese = TranslatorEngine.XoaDoubleSpace(standardizedChinese);
            return removeIgnoredChinesePhrases(standardizedChinese);
        }

        public static string standardizeInputWithoutRemovingIgnoredChinesePhrases(string original)
        {
            if (string.IsNullOrEmpty(original) || string.IsNullOrEmpty(original))
            {
                return "";
            }
            string text = ToSimplified(original);
            string[] array = new string[]
            {
                "，",
                "。",
                "：",
                "“",
                "”",
                "‘",
                "’",
                "？",
                "！",
                "「",
                "」",
                "．",
                "、",
                "\u3000",
                "…",
                NULL_STRING,
                "'",
                "（",
                "）"
            };
            string[] array2 = new string[]
            {
                ", ",
                ".",
                ": ",
                "\"",
                "\" ",
                "'",
                "' ",
                "?",
                "!",
                "\"",
                "\" ",
                ".",
                ", ",
                " ",
                "...",
                "",
                "\"",
                " (",
                ") "
            };
            for (int i = 0; i < array.Length; i++)
            {
                text = text.Replace(array[i], array2[i]);
            }
            text = text.Replace("  ", " ").Replace(" \r\n", "\n").Replace(" \n", "\n").Replace(" ,", ",");
            text = ToNarrow(text);
            int length = text.Length;
            StringBuilder stringBuilder = new StringBuilder();
            for (int j = 0; j < length - 1; j++)
            {
                char c = text[j];
                char c2 = text[j + 1];
                if (!char.IsControl(c) || c == '\t' || c == '\n' || c == '\r')
                {
                    if (isChinese(c))
                    {
                        if (!isChinese(c2) && c2 != ',' && c2 != '.' && c2 != ':' && c2 != ';' && c2 != '"' && c2 != '\'' && c2 != '?' && c2 != ' ' && c2 != '!' && c2 != ')')
                        {
                            stringBuilder.Append(c).Append(" ");
                        }
                        else
                        {
                            stringBuilder.Append(c);
                        }
                    }
                    else if (c == '\t' || c == ' ' || c == '"' || c == '\'' || c == '\n' || c == '(')
                    {
                        stringBuilder.Append(c);
                    }
                    else if (c == '!' || c == '.' || c == '?')
                    {
                        if (c2 == '"' || c2 == ' ' || c2 == '\'')
                        {
                            stringBuilder.Append(c);
                        }
                        else
                        {
                            stringBuilder.Append(c).Append(" ");
                        }
                    }
                    else if (isChinese(c2))
                    {
                        stringBuilder.Append(c).Append(" ");
                    }
                    else
                    {
                        stringBuilder.Append(c);
                    }
                }
            }
            stringBuilder.Append(text[length - 1]);
            text = indentAllLines(stringBuilder.ToString(), true);
            //return text.Replace(". . . . . .", "...");
            return removeIgnoredChinesePhrases(text).Replace(". . . . . .", "...");
        }

        public static string StandardizeInputForBrowser(string original)
        {
            string standardizedChinese = standardizeInputForBrowserWithoutRemovingIgnoredChinesePhrases(original);
            return removeIgnoredChinesePhrasesForBrowser(standardizedChinese);
        }

        private static string standardizeInputForBrowserWithoutRemovingIgnoredChinesePhrases(string original)
        {
            if (string.IsNullOrEmpty(original) || string.IsNullOrEmpty(original))
            {
                return "";
            }
            string text = ToSimplified(original);
            string[] array = new string[]
            {
                "，",
                "。",
                "：",
                "“",
                "”",
                "‘",
                "’",
                "？",
                "！",
                "「",
                "」",
                "．",
                "、",
                "\u3000",
                "…",
                NULL_STRING
            };
            string[] array2 = new string[]
            {
                ", ",
                ".",
                ": ",
                "\"",
                "\" ",
                "'",
                "' ",
                "?",
                "!",
                "\"",
                "\" ",
                ".",
                ", ",
                " ",
                "...",
                ""
            };
            for (int i = 0; i < array.Length; i++)
            {
                text = text.Replace(array[i], array2[i]);
            }
            text = text.Replace("  ", " ").Replace(" \r\n", "\n").Replace(" \n", "\n");
            text = ToNarrow(text);
            int length = text.Length;
            StringBuilder stringBuilder = new StringBuilder();
            for (int j = 0; j < length - 1; j++)
            {
                char c = text[j];
                char c2 = text[j + 1];
                if (isChinese(c))
                {
                    if (!isChinese(c2) && c2 != ',' && c2 != '.' && c2 != ':' && c2 != ';' && c2 != '"' && c2 != '\'' && c2 != '?' && c2 != ' ' && c2 != '!')
                    {
                        stringBuilder.Append(c).Append(" ");
                    }
                    else
                    {
                        stringBuilder.Append(c);
                    }
                }
                else if (c == '\t' || c == ' ' || c == '"' || c == '\'' || c == '\n')
                {
                    stringBuilder.Append(c);
                }
                else if (isChinese(c2))
                {
                    stringBuilder.Append(c).Append(" ");
                }
                else
                {
                    stringBuilder.Append(c);
                }
            }
            stringBuilder.Append(text[length - 1]);
            return indentAllLines(stringBuilder.ToString());
        }

        public static string StandardizeInputForProxy(string original)
        {
            string standardizedChinese = standardizeInputForProxyWithoutRemovingIgnoredChinesePhrases(original);
            return removeIgnoredChinesePhrasesForBrowser(standardizedChinese);
        }

        private static string standardizeInputForProxyWithoutRemovingIgnoredChinesePhrases(string original)
        {
            if (string.IsNullOrEmpty(original) || string.IsNullOrEmpty(original))
            {
                return "";
            }
            string text = ToSimplified(original);
            string[] array = new string[]
            {
                "，",
                "。",
                "：",
                "“",
                "”",
                "‘",
                "’",
                "？",
                "！",
                "「",
                "」",
                "．",
                "、",
                "\u3000",
                "…",
                NULL_STRING
            };
            string[] array2 = new string[]
            {
                ", ",
                ".",
                ": ",
                "\"",
                "\" ",
                "'",
                "' ",
                "?",
                "!",
                "\"",
                "\" ",
                ".",
                ", ",
                " ",
                "...",
                ""
            };
            for (int i = 0; i < array.Length; i++)
            {
                text = text.Replace(array[i], array2[i]);
            }
            text = text.Replace("  ", " ").Replace(" \r\n", "\n").Replace(" \n", "\n");
            text = ToNarrow(text);
            int length = text.Length;
            StringBuilder stringBuilder = new StringBuilder();
            for (int j = 0; j < length - 1; j++)
            {
                char c = text[j];
                char c2 = text[j + 1];
                if (isChinese(c))
                {
                    if (!isChinese(c2) && c2 != ',' && c2 != '.' && c2 != ':' && c2 != ';' && c2 != '"' && c2 != '\'' && c2 != '?' && c2 != ' ' && c2 != '!')
                    {
                        stringBuilder.Append(c).Append(" ");
                    }
                    else
                    {
                        stringBuilder.Append(c);
                    }
                }
                else if (c == '\t' || c == ' ' || c == '"' || c == '\'' || c == '\n')
                {
                    stringBuilder.Append(c);
                }
                else if (isChinese(c2))
                {
                    stringBuilder.Append(c).Append(" ");
                }
                else
                {
                    stringBuilder.Append(c);
                }
            }
            stringBuilder.Append(text[length - 1]);
            return text;
        }

        private static string indentAllLines(string text, bool insertBlankLine)
        {
            string[] array = text.Split(new char[]
            {
                '\n'
            }, StringSplitOptions.RemoveEmptyEntries);
            StringBuilder stringBuilder = new StringBuilder();
            string[] array2 = array;
            for (int i = 0; i < array2.Length; i++)
            {
                string text2 = array2[i];
                if (!string.IsNullOrEmpty(text2.Trim()))
                {
                    stringBuilder.Append("\t" + text2.Trim()).Append("\n").Append(insertBlankLine ? "\n" : "");
                }
            }
            return stringBuilder.ToString();
        }

        private static string indentAllLines(string text)
        {
            return indentAllLines(text, false);
        }

        private static bool isChinese(char character)
        {
            return hanVietDictionary.ContainsKey(character.ToString());
        }

        public static bool IsChinese(char character)
        {
            return isChinese(character);
        }

        public static bool IsAllChinese(string text)
        {
            for (int i = 0; i < text.Length; i++)
            {
                char character = text[i];
                if (!isChinese(character))
                {
                    return false;
                }
            }
            return true;
        }

        private static bool hasOnlyOneMeaning(string meaning)
        {
            return meaning.Split(new char[]
            {
                '/',
                '|'
            }).Length == 1;
        }

        internal static string ToSimplified(string str)
        {
            return str;
            //			return Strings.StrConv(str, VbStrConv.SimplifiedChinese, 0);
        }

        internal static string ToWide(string str)
        {
            int length = str.Length;
            int i;
            for (i = 0; i < length; i++)
            {
                char c = str[i];
                if (c >= '!' && c <= '~')
                {
                    break;
                }
            }
            if (i >= length)
            {
                return str;
            }
            StringBuilder stringBuilder = new StringBuilder();
            for (i = 0; i < length; i++)
            {
                char c = str[i];
                if (c >= '!' && c <= '~')
                {
                    stringBuilder.Append(c - '!' + '！');
                }
                else
                {
                    stringBuilder.Append(c);
                }
            }
            return stringBuilder.ToString();
        }

        internal static string ToNarrow(string str)
        {
            int length = str.Length;
            int i;
            for (i = 0; i < length; i++)
            {
                char c = str[i];
                if (c >= '！' && c <= '～')
                {
                    break;
                }
            }
            if (i >= length)
            {
                return str;
            }
            StringBuilder stringBuilder = new StringBuilder();
            for (i = 0; i < length; i++)
            {
                char c = str[i];
                if (c >= '！' && c <= '～')
                {
                    stringBuilder.Append(c - '！' + '!');
                }
                else
                {
                    stringBuilder.Append(c);
                }
            }
            return stringBuilder.ToString();
        }

        private static void appendTranslatedWord(StringBuilder result, string translatedText, ref string lastTranslatedWord)
        {
            int num = 0;
            appendTranslatedWord(result, translatedText, ref lastTranslatedWord, ref num);
        }

        private static void appendTranslatedWord(StringBuilder result, string translatedText, ref string lastTranslatedWord, ref int startIndexOfNextTranslatedText)
        {
            if (lastTranslatedWord.EndsWith("\n") ||
                lastTranslatedWord.EndsWith("\t") || 
                lastTranslatedWord.EndsWith(". ") || 
                lastTranslatedWord.EndsWith("\"") || 
                lastTranslatedWord.EndsWith("'") || 
                lastTranslatedWord.EndsWith("? ") || 
                lastTranslatedWord.EndsWith("! ") || 
                lastTranslatedWord.EndsWith(".\" ") || 
                lastTranslatedWord.EndsWith("?\" ") || 
                lastTranslatedWord.EndsWith("!\" ") || 
                lastTranslatedWord.EndsWith(": ") ||
                lastTranslatedWord.EndsWith("<br> ") ||
                lastTranslatedWord.EndsWith("<br>") ||
                lastTranslatedWord.EndsWith("<p> ") ||
                lastTranslatedWord.EndsWith("<p>"))
            {
                lastTranslatedWord = toUpperCase(translatedText);
            }
            else if (lastTranslatedWord.EndsWith(" ") || lastTranslatedWord.EndsWith("("))
            {
                lastTranslatedWord = translatedText;
            }
            else
            {
                lastTranslatedWord = " " + translatedText;
            }
            if ((string.IsNullOrEmpty(translatedText) || translatedText[0] == ',' || translatedText[0] == '.' || translatedText[0] == '?' || translatedText[0] == '!') && 0 < result.Length && result[result.Length - 1] == ' ')
            {
                result = result.Remove(result.Length - 1, 1);
                startIndexOfNextTranslatedText--;
            }
            result.Append(lastTranslatedWord);
        }

        private static string toUpperCase(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return text;
            }
            if (text.StartsWith("[") && 2 <= text.Length)
            {
                return "[" + char.ToUpper(text[1]) + (text.Length <= 2 ? "" : text.Substring(2));
            }
            if(text.StartsWith("<i") && 2 <= text.Length)
            {
                char[] arr = text.ToCharArray();
                for(int i=0; i<arr.Length; i++)
                {
                    if (arr[i] == '>' )
                    {
                        int j = i;
                        while (!TranslatorEngine.hgac.Contains(arr[j]) && j<arr.Length)
                        {
                            j++;
                        }
                        arr[j] = char.ToUpper(arr[j]);
                        break;
                    }
                }
                string result = new string(arr);
                return result;
                 
            }
            return char.ToUpper(text[0]) + (text.Length <= 1 ? "" : text.Substring(1));
        }

        private static bool nextCharIsChinese(string chinese, int currentPhraseEndIndex)
        {
            return chinese.Length - 1 > currentPhraseEndIndex && isChinese(chinese[currentPhraseEndIndex + 1]);
        }

        private static string[] classifyWordsIntoLatinAndChinese(string inputText)
        {
            List<string> list = new List<string>();
            StringBuilder stringBuilder = new StringBuilder();
            bool flag = false;
            for (int i = 0; i < inputText.Length; i++)
            {
                char c = inputText[i];
                if (isChinese(c))
                {
                    if (flag)
                    {
                        stringBuilder.Append(c);
                    }
                    else
                    {
                        list.Add(stringBuilder.ToString());
                        stringBuilder.Length = 0;
                        stringBuilder.Append(c);
                    }
                    flag = true;
                }
                else
                {
                    if (!flag)
                    {
                        stringBuilder.Append(c);
                    }
                    else
                    {
                        list.Add(stringBuilder.ToString());
                        stringBuilder.Length = 0;
                        stringBuilder.Append(c);
                    }
                    flag = false;
                }
            }
            list.Add(stringBuilder.ToString());
            return list.ToArray();
        }

        private static string[] classifyWordsIntoLatinAndChineseForProxy(string inputText)
        {
            List<string> list = new List<string>();
            StringBuilder stringBuilder = new StringBuilder();
            bool flag = false;
            bool flag2 = false;
            for (int i = 0; i < inputText.Length; i++)
            {
                char c = inputText[i];
                if (flag2)
                {
                    stringBuilder.Append(c);
                    flag = false;
                    if (c == '>')
                    {
                        list.Add(stringBuilder.ToString());
                        stringBuilder.Length = 0;
                        flag2 = false;
                    }
                }
                else if (c == '<')
                {
                    list.Add(stringBuilder.ToString());
                    stringBuilder.Length = 0;
                    stringBuilder.Append(c);
                    flag2 = true;
                    flag = false;
                }
                else if (isChinese(c))
                {
                    if (flag)
                    {
                        stringBuilder.Append(c);
                    }
                    else
                    {
                        list.Add(stringBuilder.ToString());
                        stringBuilder.Length = 0;
                        stringBuilder.Append(c);
                    }
                    flag = true;
                }
                else
                {
                    if (!flag)
                    {
                        stringBuilder.Append(c);
                    }
                    else
                    {
                        list.Add(stringBuilder.ToString());
                        stringBuilder.Length = 0;
                        stringBuilder.Append(c);
                    }
                    flag = false;
                }
            }
            list.Add(stringBuilder.ToString());
            return list.ToArray();
        }

        public static bool IsInVietPhrase(string chinese)
        {
            return vietPhraseDictionary.ContainsKey(chinese);
        }

        public static string ChineseToHanVietForAnalyzer(string chinese)
        {
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i < chinese.Length; i++)
            {
                char c = chinese[i];
                if (hanVietDictionary.ContainsKey(c.ToString()))
                {
                    stringBuilder.Append(hanVietDictionary[c.ToString()] + " ");
                }
                else
                {
                    stringBuilder.Append(c + " ");
                }
            }
            return stringBuilder.ToString().Trim();
        }

        public static string ChineseToVietPhraseForAnalyzer(string chinese, int translationAlgorithm, bool prioritizedName)
        {
            return ChineseToVietPhraseForBrowser(chinese, 11, translationAlgorithm, prioritizedName).Trim(trimCharsForAnalyzer);
        }

        private static bool containsName(string chinese, int startIndex, int phraseLength)
        {
            if (phraseLength < 2)
            {
                return false;
            }
            if (onlyNameDictionary.ContainsKey(chinese.Substring(startIndex, phraseLength)))
            {
                return false;
            }
            int num = startIndex + phraseLength - 1;
            int num2 = 2;
            for (int i = startIndex + 1; i <= num; i++)
            {
                for (int j = 20; j >= num2; j--)
                {
                    if (chinese.Length >= i + j && onlyNameDictionary.ContainsKey(chinese.Substring(i, j)))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private static bool isLongestPhraseInSentence(string chinese, int startIndex, int phraseLength, Dictionary<string, string> dictionary, int translationAlgorithm)
        {
            if (phraseLength < 2)
            {
                return true;
            }
            int num = translationAlgorithm == 0 ? phraseLength : phraseLength < 3 ? 3 : phraseLength;
            int num2 = startIndex + phraseLength - 1;
            for (int i = startIndex + 1; i <= num2; i++)
            {
                for (int j = 20; j > num; j--)
                {
                    if (chinese.Length >= i + j && dictionary.ContainsKey(chinese.Substring(i, j)))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public static int GetVietPhraseDictionaryCount()
        {
            return onlyVietPhraseDictionary.Count;
        }

        public static int GetNameDictionaryCount(bool isNameChinh)
        {
            if (!isNameChinh)
            {
                return onlyNamePhuDictionary.Count;
            }
            return onlyNameChinhDictionary.Count;
        }

        public static int GetPhienAmDictionaryCount()
        {
            return hanVietDictionary.Count;
        }

        public static bool ExistInPhienAmDictionary(string chinese)
        {
            return chinese.Length == 1 && hanVietDictionary.ContainsKey(chinese);
        }

        private static void updateHistoryLogInCache(string key, string action, ref DataSet dictionaryHistoryDataSet)
        {
            string name = "DictionaryHistory";
            DataRow dataRow = dictionaryHistoryDataSet.Tables[name].Rows.Find(key);
            if (dataRow == null)
            {
                dictionaryHistoryDataSet.Tables[name].Rows.Add(new object[]
                {
                    key,
                    action,
                    Environment.GetEnvironmentVariable("USERNAME"),
                    DateTime.Now
                });
                return;
            }
            dataRow[1] = action;
            dataRow[2] = Environment.GetEnvironmentVariable("USERNAME");
            dataRow[3] = DateTime.Now;
        }

        private static void writeVietPhraseHistoryLog(string key, string action)
        {
            updateHistoryLogInCache(key, action, ref onlyVietPhraseDictionaryHistoryDataSet);
            WriteHistoryLog(key, action, DictionaryConfigurationHelper.GetVietPhraseDictionaryHistoryPath());
        }

        private static void writeNamesHistoryLog(string key, string action, bool isNameChinh)
        {
            DataSet dataSet = isNameChinh ? onlyNameDictionaryHistoryDataSet : onlyNamePhuDictionaryHistoryDataSet;
            updateHistoryLogInCache(key, action, ref dataSet);
            WriteHistoryLog(key, action, isNameChinh ? DictionaryConfigurationHelper.GetNamesDictionaryHistoryPath() : DictionaryConfigurationHelper.GetNamesPhuDictionaryHistoryPath());
        }

        private static void writePhienAmHistoryLog(string key, string action)
        {
            updateHistoryLogInCache(key, action, ref hanVietDictionaryHistoryDataSet);
            WriteHistoryLog(key, action, DictionaryConfigurationHelper.GetChinesePhienAmWordsDictionaryHistoryPath());
        }

        public static string GetVietPhraseHistoryLogRecord(string key)
        {
            return getDictionaryHistoryLogRecordInCache(key, onlyVietPhraseDictionaryHistoryDataSet);
        }

        public static string GetNameHistoryLogRecord(string key, bool isNameChinh)
        {
            return getDictionaryHistoryLogRecordInCache(key, isNameChinh ? onlyNameDictionaryHistoryDataSet : onlyNamePhuDictionaryHistoryDataSet);
        }

        public static string GetPhienAmHistoryLogRecord(string key)
        {
            return getDictionaryHistoryLogRecordInCache(key, hanVietDictionaryHistoryDataSet);
        }

        private static string getDictionaryHistoryLogRecordInCache(string key, DataSet dictionaryHistoryDataSet)
        {
            string name = "DictionaryHistory";
            DataRow dataRow = dictionaryHistoryDataSet.Tables[name].Rows.Find(key);
            if (dataRow == null)
            {
                return "";
            }
            return string.Format("Entry này đã được <{0}> bởi <{1}> vào <{2}>.", dataRow[1], dataRow[2], ((DateTime)dataRow[3]).ToString("yyyy-MM-dd HH:mm:ss.fffzzz"));
        }

        public static void CompressPhienAmDictionaryHistory()
        {
            CompressDictionaryHistory(hanVietDictionaryHistoryDataSet, DictionaryConfigurationHelper.GetChinesePhienAmWordsDictionaryHistoryPath());
        }

        public static void CompressOnlyVietPhraseDictionaryHistory()
        {
            CompressDictionaryHistory(onlyVietPhraseDictionaryHistoryDataSet, DictionaryConfigurationHelper.GetVietPhraseDictionaryHistoryPath());
        }

        public static void CompressOnlyNameDictionaryHistory(bool isNameChinh)
        {
            CompressDictionaryHistory(isNameChinh ? onlyNameDictionaryHistoryDataSet : onlyNamePhuDictionaryHistoryDataSet, isNameChinh ? DictionaryConfigurationHelper.GetNamesDictionaryHistoryPath() : DictionaryConfigurationHelper.GetNamesPhuDictionaryHistoryPath());
        }

        private static void CompressDictionaryHistory(DataSet dictionaryHistoryDataSet, string dictionaryHistoryFilePath)
        {
            string name = "DictionaryHistory";
            string text = dictionaryHistoryFilePath + "." + DateTime.Now.Ticks;
            if (File.Exists(dictionaryHistoryFilePath))
            {
                File.Copy(dictionaryHistoryFilePath, text, true);
            }
            using (TextWriter textWriter = new StreamWriter(dictionaryHistoryFilePath, false, Encoding.UTF8))
            {
                try
                {
                    textWriter.WriteLine("Entry\tAction\tUser Name\tUpdated Date");
                    DataTable dataTable = dictionaryHistoryDataSet.Tables[name];
                    foreach (DataRow dataRow in dataTable.Rows)
                    {
                        textWriter.WriteLine(string.Format("{0}\t{1}\t{2}\t{3}", new object[]
                        {
                            dataRow[0],
                            dataRow[1],
                            dataRow[2],
                            ((DateTime)dataRow[3]).ToString("yyyy-MM-dd HH:mm:ss.fffzzz")
                        }));
                    }
                }
                catch (Exception ex)
                {
                    try
                    {
                        textWriter.Close();
                    }
                    catch
                    {
                    }
                    if (File.Exists(dictionaryHistoryFilePath))
                    {
                        try
                        {
                            File.Copy(text, dictionaryHistoryFilePath, true);
                        }
                        catch
                        {
                        }
                    }
                    throw ex;
                }
                finally
                {
                    File.Delete(text);
                }
            }
        }

        public static void WriteHistoryLog(string key, string action, string logPath)
        {
            if (!File.Exists(logPath))
            {
                File.AppendAllText(logPath, "Entry\tAction\tUser Name\tUpdated Date\r\n", Encoding.UTF8);
            }
            File.AppendAllText(logPath, string.Concat(new string[]
            {
                key,
                "\t",
                action,
                "\t",
                Environment.GetEnvironmentVariable("USERNAME"),
                "\t",
                DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fffzzz"),
                "\r\n"
            }), Encoding.UTF8);
        }

        public static void CreateHistoryLog(string key, string action, ref StringBuilder historyLogs)
        {
            historyLogs.AppendLine(string.Concat(new string[]
            {
                key,
                "\t",
                action,
                "\t",
                Environment.GetEnvironmentVariable("USERNAME"),
                "\t",
                DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fffzzz")
            }));
        }

        public static void WriteHistoryLog(string historyLogs, string logPath)
        {
            if (!File.Exists(logPath))
            {
                File.AppendAllText(logPath, "Entry\tAction\tUser Name\tUpdated Date\r\n", Encoding.UTF8);
            }
            File.AppendAllText(logPath, historyLogs, Encoding.UTF8);
        }

        private static string removeIgnoredChinesePhrases(string standardizedChinese)
        {
            if (string.IsNullOrEmpty(standardizedChinese))
            {
                return string.Empty;
            }
            string text = standardizedChinese;
            foreach (string current in ignoredChinesePhraseList)
            {
                text = text.Replace(current, string.Empty);
            }
            return text.Replace("\t\n\n", string.Empty);
        }

        private static string removeIgnoredChinesePhrasesForBrowser(string standardizedChinese)
        {
            if (string.IsNullOrEmpty(standardizedChinese))
            {
                return string.Empty;
            }
            string text = standardizedChinese;
            foreach (string current in ignoredChinesePhraseForBrowserList)
            {
                text = text.Replace(current, string.Empty);
            }
            return text.Replace("\t\n\n", string.Empty);
        }

        private static int containsLuatNhan(string chinese, Dictionary<string, string> dictionary)
        {
            string text;
            int num;
            return containsLuatNhan(chinese, dictionary, out text, out num);
        }

        private static int containsLuatNhan(string chinese, Dictionary<string, string> dictionary, out string luatNhan, out int matchedLength)
        {
            int length = chinese.Length;
            foreach (KeyValuePair<string, string> current in luatNhanDictionary)
            {
                if (length >= current.Key.Length - 2)
                {
                    string text = current.Key.Replace("{0}", "([^,\\. ?]{1,10})");
                    Match match = Regex.Match(chinese, text);
                    int num = 0;
                    while (match.Success)
                    {
                        string value = match.Groups[1].Value;
                        if (current.Key.StartsWith("{0}"))
                        {
                            for (int i = 0; i < value.Length; i++)
                            {
                                if (dictionary.ContainsKey(value.Substring(i)))
                                {
                                    luatNhan = text;
                                    matchedLength = match.Length - i;
                                    int result = match.Index + i;
                                    return result;
                                }
                            }
                        }
                        else if (current.Key.EndsWith("{0}"))
                        {
                            int num2 = value.Length;
                            while (0 < num2)
                            {
                                if (dictionary.ContainsKey(value.Substring(0, num2)))
                                {
                                    luatNhan = text;
                                    matchedLength = match.Length - (value.Length - num2);
                                    int result = match.Index;
                                    return result;
                                }
                                num2--;
                            }
                        }
                        else if (dictionary.ContainsKey(value))
                        {
                            luatNhan = text;
                            matchedLength = match.Length;
                            int result = match.Index;
                            return result;
                        }
                        match = match.NextMatch();
                        num++;
                        if (num > 1)
                        {
                            break;
                        }
                    }
                }
            }
            luatNhan = string.Empty;
            matchedLength = -1;
            return -1;
        }

        private static bool matchesLuatNhan(string chinese, Dictionary<string, string> dictionary)
        {
            foreach (KeyValuePair<string, string> current in luatNhanDictionary)
            {
                string str = current.Key.Replace("{0}", "(.+)");
                Match match = Regex.Match(chinese, "^" + str + "$");
                if (match.Success && dictionary.ContainsKey(match.Groups[1].Value))
                {
                    return true;
                }
            }
            return false;
        }

        private static bool matchesLuatNhan(string chinese, Dictionary<string, string> dictionary, string luatNhan)
        {
            Match match = Regex.Match(chinese, "^" + luatNhan + "$");
            return match.Success && dictionary.ContainsKey(match.Groups[1].Value);
        }

        public static string ChineseToLuatNhan(string chinese, Dictionary<string, string> dictionary)
        {
            string empty = string.Empty;
            return ChineseToLuatNhan(chinese, dictionary, out empty);
        }

        public static string ChineseToLuatNhan(string chinese, Dictionary<string, string> dictionary, out string luatNhan)
        {
            int arg_06_0 = chinese.Length;
            foreach (KeyValuePair<string, string> current in luatNhanDictionary)
            {
                string str = current.Key.Replace("{0}", "(.+)");
                Match match = Regex.Match(chinese, "^" + str + "$");
                if (match.Success && dictionary.ContainsKey(match.Groups[1].Value))
                {
                    string[] array = dictionary[match.Groups[1].Value].Split(new char[]
                    {
                        '/',
                        '|'
                    });
                    StringBuilder stringBuilder = new StringBuilder();
                    string[] array2 = array;
                    for (int i = 0; i < array2.Length; i++)
                    {
                        string newValue = array2[i];
                        stringBuilder.Append(current.Value.Replace("{0}", newValue));
                        stringBuilder.Append("/");
                    }
                    luatNhan = current.Key;
                    return stringBuilder.ToString().Trim(new char[]
                    {
                        '/'
                    });
                }
            }
            throw new NotImplementedException("Lỗi xử lý luật nhân cho cụm từ: " + chinese);
        }

 
        public static string FixCap3(string sIn)
        {
            if (string.IsNullOrEmpty(sIn))
                return sIn;
            StringBuilder stringBuilder = new StringBuilder();
            int i = 0;
            TranslatorEngine.NextCharIsCap = true;
            for (; i < sIn.Length; i++)
            {
                if (!TranslatorEngine.NextCharIsCap && Array.Exists<string>(TranslatorEngine.daucau2, (Predicate<string>)(element => element == sIn[i].ToString())))
                    TranslatorEngine.NextCharIsCap = true;
                if (TranslatorEngine.NextCharIsCap && ((IEnumerable<char>)TranslatorEngine.hgac).Contains<char>(sIn[i]))
                {
                    stringBuilder.Append(char.ToUpper(sIn[i]));
                    TranslatorEngine.NextCharIsCap = false;
                }
                else
                    stringBuilder.Append(sIn[i]);
            }
            return stringBuilder.ToString();
        }

        public static string XoaDoubleSpace(string sIn)
        {
            sIn = sIn.Trim();
            StringBuilder stringBuilder = new StringBuilder();
            bool flag1 = false;
            foreach (char c in sIn)
            {
                bool flag2 = char.IsWhiteSpace(c);
                if (!flag1 || !flag2)
                {
                    stringBuilder.Append(c);
                    flag1 = flag2;
                }
            }
            return stringBuilder.ToString();
        }

        public static string[] skipTag(string text)
        {
            string tempText = text;
            StringBuilder stringBuilder = new StringBuilder();
            int index = 0;
            findtag(tempText, stringBuilder, index);
            string[] result = stringBuilder.ToString().Split('\n').ToArray();
            return result;
        }

        public static void findtag(string text, StringBuilder arr, int index)
        {
            if (text.Contains("<img"))
            {
                index = text.IndexOf("<img");
                for (int i = index; i < text.Length; i++)
                {
                    if (text[i] == '>')
                    {
                        arr.Append(text.Substring(index, i - index + 1) + "\n");
                        string a = text.Replace(text.Substring(index, i - index + 1), "");
                        findtag(a, arr, index);
                        break;
                    }
                }
            }
        }

    }
}
