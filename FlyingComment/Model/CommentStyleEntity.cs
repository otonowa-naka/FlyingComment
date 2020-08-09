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
    public class CommentStyleEntity : NotifyChangedBase
    {


        /// <summary>
        /// 保持データのバリテーションエラー
        /// </summary>
        /// <returns>ture = エラーあり　false = エラー無し</returns>
        public bool IsError()
        {
            bool ret = true;
            if (
                (FamilyStringErrorMessage == null)
                &&  (SizeStringErrorMessage == null)
                && (ColorStringErrorMessage == null)
                && (ThicknessColorStringErrorMessage == null)
                && (ThicknessStringErrorMessage == null)
                && (CommentTimeErrorMessage == null)
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


        /// <summary>
        /// フォルトファミリーオブジェクト
        /// </summary>
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
                    ret = $"フォントサイズ変換できません{ ex.Message}";
                }

                return ret;
            }
        }

        /// <summary>
        /// フォントイタリックフラグ
        /// </summary>
        private bool _Italic = false;
        public bool Italic
        {
            get
            {
                return _Italic;
            }
            set
            {
                SetProperty(ref _Italic, value);
              
            }
        }

        /// <summary>
        /// フォントボールド設定
        /// </summary>
        private bool _Bald = false;
        public bool Bald
        {
            get
            {
                return _Bald;
            }
            set
            {
                SetProperty(ref _Bald, value);
            }
        }

        /// <summary>
        /// 文字の色
        /// </summary>
        private string _ColorString = "#FFFFFF";
        public string ColorString
        {
            get
            {
                return _ColorString;
            }
            set
            {
                SetProperty(ref _ColorString, value);
            }
        }

        public Color Color
        {
            get
            {
                Color ret = new Color();
                try
                {
                    ret = StringToColor(_ColorString);
                }
                catch (Exception /*ex*/)
                {
                }

                return ret;
            }
        }

        public string ColorStringErrorMessage
        {
            get
            {
                string ret = null;
                //　フォントサイズ設定
                try
                {
                    if (string.IsNullOrWhiteSpace(_ColorString) == false)
                    {
                        StringToColor(_ColorString);
                    }
                    else
                    {
                        ret = "文字色が空白です。";
                    }
                }
                catch (Exception ex)
                {
                    ret = $"文字色が変換できません。{ ex.Message}";
                }

                return ret;
            }
        }

        private static Color StringToColor(string colorstr)
        {
            ColorConverter ColorConv = new ColorConverter();
            return (Color)ColorConv.ConvertFrom(colorstr);
        }

        /// <summary>
        /// 文字の縁の色
        /// </summary>
        private string _ThicknessColorrString = "#000000";
        public string ThicknessColorString
        {
            get
            {
                return _ThicknessColorrString;
            }
            set
            {
                SetProperty(ref _ThicknessColorrString, value);
            }
        }


        public Color ThicknessColor
        {
            get
            {
                Color ret = new Color();
                try
                {
                    ret = StringToColor(_ThicknessColorrString);
                }
                catch (Exception /*ex*/)
                {
                }

                return ret;
            }
        }

        public string ThicknessColorStringErrorMessage
        {
            get
            {
                string ret = null;
                //　フォントサイズ設定
                try
                {
                    if (string.IsNullOrWhiteSpace(_ThicknessColorrString) == false)
                    {
                        StringToColor(_ThicknessColorrString);
                    }
                    else
                    {
                        ret = "文字縁色がフォントサイズが空白です。";
                    }
                }
                catch (Exception ex)
                {
                    ret = $"文字縁色が変換できません。{ ex.Message}";
                }

                return ret;
            }
        }


        /// <summary>
        /// 文字の縁のピクセル数
        /// </summary>
        private string _ThicknessrString = "0";
        public string ThicknessString
        {
            get
            {
                return _ThicknessrString;
            }
            set
            {
                SetProperty(ref _ThicknessrString, value);
            }
        }

        public ushort Thickness
        {
            get
            {
                ushort ret = 0;
                ushort.TryParse(_ThicknessrString, out ret);
                return ret;
            }
        }
        public string ThicknessStringErrorMessage
        {
            get
            {
                string ret = null;
                //　フォントサイズ設定
                try
                {
                    if (string.IsNullOrWhiteSpace(_ThicknessrString) == false)
                    {
                        ushort.Parse(_ThicknessrString);
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

        private string _CommentTimeString = "4000";
        public string CommentTimeString {
            get
            {
                return _CommentTimeString;
            }
            set
            {
                SetProperty(ref _CommentTimeString, value);
            }
        }

        public long CommentTime {
            get
            {
                long ret = 0;
                long.TryParse(CommentTimeString, out ret);
                return ret;
            }
        }

        public string CommentTimeErrorMessage
        {
            get
            {
                string ret = null;
                //　フォントサイズ設定
                try
                {
                    if (string.IsNullOrWhiteSpace(CommentTimeString) == false)
                    {
                        long.Parse(CommentTimeString);
                    }
                    else
                    {
                        ret = "コメント待機時間が空白です。";
                    }
                }
                catch (Exception ex)
                {
                    ret = $"コメント待機時間の変換に失敗{ ex.Message}";
                }

                return ret;
            }
        }

    }
}
