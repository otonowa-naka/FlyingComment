using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace FlyingComment.Model
{
    public class FontSettingEntity : NotifyChangedBase
    {


        public bool IsError()
        {
            bool ret = true;
            if (
                (FamilyStringErrorMessage == null)
                &&  (SizeStringErrorMessage == null)
                )
            {
                ret = false;
            }
            return ret;
        }


        /// <summary>
        /// フォント名
        /// </summary>
        private string _FamilyString = "Meiryo";
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
                return (new FontFamily(_FamilyString));
            }
        }

        /// <summary>
        /// フォントファミリー名が有効な文字か確認する
        /// </summary>
        /// <param name="val">フォントファミリー文字列</param>
        /// <returns>正常　＝　null</returns>
        public string FamilyStringErrorMessage
        {
            get
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
                            ret = "フォントファミリーが変換できませんでした。";
                        }
                    }
                    else
                    {
                        ret = "フォントファミリー名が空白です。";
                    }
                }
                catch (Exception)
                {
                    ret = "例外が発生しました。";
                }

                return ret;

            }
        }

        /// <summary>
        /// フォントサイズ
        /// </summary>
        private string _SzierString = "48";
        public string SizeString
        {
            get
            {
                return _SzierString;
            }
            set
            {
                SetProperty(ref _SzierString, value);
            }
        }

        public Double Size
        {
            get
            {
                Double ret = 0;
                try
                {
                    FontSizeConverter myFontSizeConverter = new FontSizeConverter();
                    ret =  (Double)myFontSizeConverter.ConvertFromString(_SzierString);

                }catch(Exception /*ex*/)
                {
                    ret = 0;
                }
                return ret;
            }
        }
  

        public string SizeStringErrorMessage
        {
            get
            {
                string ret = null;
                //　フォントサイズ設定
                try
                {
                    if (string.IsNullOrWhiteSpace(_SzierString) == false)
                    {
                        FontSizeConverter myFontSizeConverter = new FontSizeConverter();
                        var FontSize = (Double)myFontSizeConverter.ConvertFromString(_SzierString);
                    }
                    else
                    {
                        ret = "フォントサイズが空白です。";
                    }
                }
                catch (Exception ex)
                {
                    ret = $"フォントサイズ変換に失敗{ ex.Message}";
                }

                return ret;
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
