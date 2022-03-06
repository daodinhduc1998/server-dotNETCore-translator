using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Translation
{
    public class LoadMain
    {

        //translationAlgorithm:
        /*	0:ưu tiên cụm vp dài
         *	1: dịch từ tría sang phải
         *	2: ưu tiên cum vp dài >=4
         *	prioritizedName
         *  true/false: ưu tiên cụm Names hơn VP
        */
        public static string Dich(string sTQ, int a, int b, int isVP)
        { 
                string[] arr = TranslatorEngine.TranslatorEngine.skipTag(sTQ);
                for (int k = 0; k < arr.Length; k++)
                {
                    if (arr[k].Length > 4)
                    {
                        sTQ = sTQ.Replace(arr[k].ToString(), "</br>#img"+k+"#</br>");
                    }
                }
                bool c;
                if (isVP == 1) c = true;
                else c = false;
                //sTQ = TranslatorEngine.TranslatorEngine.StandardizeInput(sTQ);
                sTQ = TranslatorEngine.TranslatorEngine.standardizeInputWithoutRemovingIgnoredChinesePhrases(sTQ);
                sTQ = !c ? TranslatorEngine.TranslatorEngine.ChineseToHanVietForBatch(sTQ) : TranslatorEngine.TranslatorEngine.ChineseToVietPhraseOneMeaningForBatch(sTQ, a, b, c);
                
                for (int k = 0; k < arr.Length; k++)
                {
                    if (arr[k].Length > 4)
                    {
                        sTQ = sTQ.Replace("#img" + k + "#",arr[k].ToString());
                    }
                }

                return ChuanHoaResponse(sTQ,a);
            
        }

        public static void LoadDictionaries() => TranslatorEngine.TranslatorEngine.LoadDictionaries();

        public static string ChuanHoaResponse(string sTQ,int wrapType)
        {         
            if (wrapType == 2)
            {
                //sTQ = "\n\t" + sTQ;
            }
            if (sTQ.Contains("<p>") || sTQ.Contains("<br>")) sTQ = "<p>" + sTQ;

            sTQ = sTQ.Replace("<br><br>", "<p>").Replace("<p><br>", "<p>").Replace("<br>", "<p>");
            return sTQ;

        }
       


    }
}
