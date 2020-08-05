using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace FlyingComment.Model
{
    public class ColorString
    {
        public ColorString(string colorstr)
        {
            _ValueString = colorstr;
        }

        private string _ValueString = "";
        public string ValueString {
            get
            {
                return _ValueString;
            }
        }

        public Color Color
        {
            get
            {
                Color ret = new Color();
                try
                {
                    ret = StringToColor(ValueString);
                }
                catch (Exception /*ex*/)
                {
                }

                return ret;
            }
        }

        public string ColorStringErrorMessage {
            get
            {
                string ret = null;
                //　フォントサイズ設定
                try
                {
                    if (string.IsNullOrWhiteSpace(ValueString) == false)
                    {
                        StringToColor(ValueString);
                    }
                    else
                    {
                        ret = "空白です。色を指定してください。";
                    }
                }
                catch (Exception ex)
                {
                    ret = $"色に変換できません。{ ex.Message}";
                }

                return ret;
            }
        }

        private static Color StringToColor(string colorstr)
        {
            ColorConverter ColorConv = new ColorConverter();
            return (Color)ColorConv.ConvertFrom(colorstr);
        }
        // override object.Equals
        public override bool Equals(object obj)
        {
            bool ret = false;
            if (obj == null || GetType() != obj.GetType())
            {
                ret = false;
            }
            else
            {
                ColorString tobj = obj as ColorString;
                ret = ( this.ValueString == tobj.ValueString);
            }

            return ret;
        }
    }
}
