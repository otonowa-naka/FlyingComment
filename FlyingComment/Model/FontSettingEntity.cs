using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace FlyingComment.Model
{
    public class FontSettingEntity : NotifyChangedBase
    {


        public override bool IsError()
        {
            bool ret = true;
            if(FamilyValidation() == null)
            {
                ret = false;
            }
            return ret;
        }


        /// <summary>
        /// フォント名
        /// </summary>
        private string _FamilyString = "";
        public string FamilyString {
            get
            {
                return _FamilyString;
            }
            set
            {
                SetProperty(ref _FamilyString, value);
            }
        }

        public FontFamily Family
        {
            get
            {
                return ( new FontFamily(_FamilyString) );
            }
        }

        /// <summary>
        /// フォントファミリー名が有効な文字か確認する
        /// </summary>
        /// <param name="val">フォントファミリー文字列</param>
        /// <returns>正常　＝　null</returns>
        public string FamilyValidation()
        {
            string ret = null;
            try
            {
                if (string.IsNullOrWhiteSpace(FamilyString) == false)
                {
                    FontFamily fam = new FontFamily(FamilyString);
                    if (fam != null)
                    {
                       // 正常
                    }
                    else
                    {
                        ret =  "フォントファミリーが変換できませんでした。";
                    }
                }
                else
                {
                    ret =  "フォントファミリー名が空白です。";
                }
            }
            catch (Exception)
            {
                ret =  "例外が発生しました。";
            }

            return ret;
        }

        /// <summary>
        /// フォントサイズ
        /// </summary>
        private string _SzierString = "";
        public string SizeString
        {
            get
            {
                return _SzierString;
            }
            set
            {
                _SzierString = value;
            }
        }


        /// <summary>
        /// フォントイタリックフラグ
        /// </summary>
        private bool _Italic = false;
        public bool ItalicString
        {
            get
            {
                return _Italic;
            }
            set
            {
                _Italic =  value;
            }
        }

        /// <summary>
        /// フォントボールド設定
        /// </summary>
        private bool _Bald;
        public bool BaldString
        {
            get
            {
                return _Bald;
            }
            set
            {
                _Bald =  value;
            }
        }

        /// <summary>
        /// 文字の色
        /// </summary>
        private string _ColorString;
        public string ColorStringString
        {
            get
            {
                return _ColorString;
            }
            set
            {
                _ColorString =  value;
            }
        }

        /// <summary>
        /// 文字の縁の色
        /// </summary>
        private string _ThicknessColorrString;
        public string ThicknessColorString
        {
            get
            {
                return _ThicknessColorrString;
            }
            set
            {
                _ThicknessColorrString = value;
            }
        }

        /// <summary>
        /// 文字の縁のピクセル数
        /// </summary>
        private string _ThicknessrString;
        public string ThicknessString
        {
            get
            {
                return _ThicknessrString;
            }
            set
            {
                _ThicknessrString =  value;
            }
        }
    }
}
