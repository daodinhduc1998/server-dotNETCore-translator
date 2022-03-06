using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using Translation;
namespace TranslatorWebAPI.Controllers
{
    [Route("api/translator")]
    [ApiController]
    public class TranslatorController : ControllerBase
    {

        [HttpGet]
        public string Get()
        {
            return "Hello !";
        }

        [HttpPost]
        public string Post([FromBody] Models.TranslatorModel trans)
        {
            TextInfo myTI = new CultureInfo("vi-VN", false).TextInfo;
            string result="";
            switch (trans.mode)
            {
                case 0:
                    //Dịch list novel, bao gồm các trường:
                    /*  nameNovel
                     *  author
                     *  status
                     *  des
                     */
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                    string[] arr0 = trans.chinese.Split("691998");
#pragma warning restore CS8602 // Dereference of a possibly null reference.
                    string a = "";
                    if (arr0.Length > 0)
                    {   
                        for (int i=0;i< arr0.Length; i++)
                        { 
                            if (arr0[i].Contains("~/#/~"))
                            {
                                string[] arr01 = arr0[i].Split("~/#/~");
                                string nameNovel0 = arr01[0].Replace("\t", "").Replace("\n", "").Replace("\r", ""),
                               authorNovel0 = arr01[1].Replace("\t", "").Replace("\n", "").Replace("\r", ""),
                               desNovel0 = arr01[2].Replace("\t", "").Replace("\n", "").Replace("\r", "");
                                a += myTI.ToTitleCase(LoadMain.Dich(nameNovel0, 0, trans.translationAlgorithm, trans.prioritizedName)) + "~/#/~" +
                                        myTI.ToTitleCase(LoadMain.Dich(authorNovel0, 0, trans.translationAlgorithm, 0)) + "~/#/~" +
                                        LoadMain.Dich(desNovel0, 0, trans.translationAlgorithm, trans.prioritizedName) + "691998";
                            }   
                        }
                    }
                    result = a;
                    break;
                case 1:
                    /*  Dịch novel page, bao gồm các trường:
                     *  name novel
                     *  author
                     *  status
                     *  des
                     *  list chapter : name
                     */
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                    string[] arr1 = trans.chinese.Split("~/#/~");
#pragma warning restore CS8602 // Dereference of a possibly null reference.
                              //string nameNovel1 = arr1[0], authorNovel1 = arr1[1], desNovel1 = arr1[2], statusNovel1 = arr1[3];

                    string listTitle = "";
                    if (arr1.Length > 4)
                    {
                        for (int i = 4; i < arr1.Length; i++)
                        {
                            listTitle += myTI.ToTitleCase(LoadMain.Dich(arr1[i], 0, trans.translationAlgorithm, trans.prioritizedName)) + "~/#/~";
                        }
                    }

                    result = myTI.ToTitleCase(LoadMain.Dich(arr1[0], 0, trans.translationAlgorithm, trans.prioritizedName)).Replace("\t", "").Replace("\n", "").Replace("\r", "") + "~/#/~" +
                       myTI.ToTitleCase(LoadMain.Dich(arr1[1], 0, trans.translationAlgorithm, 0)).Replace("\t", "").Replace("\n", "").Replace("\r", "") + "~/#/~" +
                       LoadMain.Dich(arr1[2], 0, trans.translationAlgorithm, trans.prioritizedName).Replace("\t", "").Replace("\n", "").Replace("\r", "") + "~/#/~" +
                       myTI.ToTitleCase(LoadMain.Dich(arr1[3], trans.wrapType, trans.translationAlgorithm, trans.prioritizedName)) + "~/#/~" + listTitle.Replace("\t", "").Replace("\n", "").Replace("\r", "");
                    break;
                case 2:
                    /*  Dịch trang đọc chap truyện, bao gồm các trường:
                     *  name novel
                     *  title chapter
                     *  author
                     *  content chapter
                     */
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                    string[] arr = trans.chinese.Split("~/#/~");
#pragma warning restore CS8602 // Dereference of a possibly null reference.
                    string nameNovel = arr[0], titleChap = arr[1], author = arr[2], contentChap = arr[3];
                    result = myTI.ToTitleCase(LoadMain.Dich(arr[0], 0, trans.translationAlgorithm, trans.prioritizedName)) + "~/#/~" +
                        myTI.ToTitleCase(LoadMain.Dich(arr[1], 0, trans.translationAlgorithm, trans.prioritizedName)) + "~/#/~" +
                        myTI.ToTitleCase(LoadMain.Dich(arr[2], 0, trans.translationAlgorithm,0)) + "~/#/~" +
                        LoadMain.Dich(arr[3], trans.wrapType, trans.translationAlgorithm, trans.prioritizedName);

                    break;

                default:
                    /*  Dịch nội dung thường không phải object
                     * 
                     */
                    result = LoadMain.Dich(trans.chinese, trans.wrapType, trans.translationAlgorithm, trans.prioritizedName);
                    break;
                
            }
            //LoadMain.LoadDictionaries();
            return result;
        }
    }
}
