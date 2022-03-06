using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using Translation;
using Newtonsoft.Json;
using Translation.TranslatorEngine;

namespace Display
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            //rtbDisplay.Text = Translation.LoadMain.test

            string text = "<br><img src=\"http://rs.sfacg.com/web/novel/images/UploadPic/2017/06/44eb419e-f124-4d13-a52e-b5380edf3e12.jpg\"><p>君夜万花筒写轮眼的图案如上图所示......这算不算是剧透呢？</p><p>————————————————————————.</p><p>  “看样子白板1级的新手‘真祖’，也勉强有影级的实力。”君夜抚着下巴嘀咕道。</p><p>  不过遗憾的是，<img src=\"http://111rs.sfacg.com/web/novel/images/UploadPic/2017/06/44eb419e-f124-4d13-a52e-b5380edf3e12.jpg\">他纯粹是依靠强悍的肉身和速度，虐杀这帮人的。如果真的遇到影级高手，一大波忍术就能让他措手不及，甚至可能没法击败准影<img src=\"http://rs.sfacg.com/web/novel/images/UploadPic/2017/06/44eb419e-f124-4d13-a52e-b5380edf3e12.jpg\">，君夜能使用“真祖本质”的时间实在有限。</p><p>  “那么，也该收收战利品了。”</p><p>  君夜悠悠然地俯下身，右手对准了那位领头人死不瞑目的头颅，手掌散发出淡淡的蓝色光芒，领头人的双眼中，某些东西化为了晶莹的粒子，涌向君夜的手。</p><p>  “写轮眼的本质，我就不客气地拿下了。”</p><p>  运转体内微小的界限之力，将名为“写轮眼”的本质，添加进来。<img src=\"http://222rs.sfacg.com/web/novel/images/UploadPic/2017/06/44eb419e-f124-4d13-a52e-b5380edf3e12.jpg\">它就像一件装备，无论人、吸血鬼真祖、纯血龙种亦或是别的本质，都可以使用。</p>";

            //string json = JsonConvert.SerializeObject(tempArr);
            //rtbDisplay.Text = JsonConvert.SerializeObject(skipTag(text));

            string[] arr = TranslatorEngine.skipTag(text);
            for (int k = 0; k < arr.Length; k++)
            {
                if (arr[k].Length > 0)
                {
                    text = text.Replace(arr[k].ToString(), "asdasda");
                }
                
            }
            rtbDisplay.Text = text;
        }

        public string[] skipTag(string text)
        {
            string tempText = text;
            StringBuilder stringBuilder = new StringBuilder();
            int index = 0;
            findtag(tempText, stringBuilder, index);
            string[] result = stringBuilder.ToString().Split('\n').ToArray();
            return result;
        }

        public void findtag(string text, StringBuilder arr,int index)
        {
            if (text.Contains("<img"))
            {
                index = text.IndexOf("<img");
                for (int i = index; i < text.Length; i++)
                {
                    if (text[i] == '>')
                    {
                        arr.Append(text.Substring(index, i - index+1)+"\n");
                        string a = text.Replace(text.Substring(index, i - index + 1), "");
                        findtag(a, arr, index);
                        break;
                    }
                }
            }
        }
    }
}
