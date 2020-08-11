using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace FlyingComment.Model
{
    public class CommentWindowConfigurationEntity : NotifyChangedBase
    {
        public CommentWindowConfigurationEntity()
        {

        }

        /// <summary>
        /// 保持データのバリテーションエラー
        /// </summary>
        /// <returns>ture = エラーあり　false = エラー無し</returns>
        public bool IsError()
        {
            bool ret = true;
            if (
                (_BackColor.ColorStringErrorMessage == null)
                )
            {
                ret = false;
            }
            return ret;
        }

        private bool _Stealth = false;
        public bool Stealth
        {
            get
            {
                return _Stealth;
            }
            set
            {
                SetProperty(ref _Stealth, value);
            }
        }

        private bool _Visible = false;
        public bool Visible
        {
            get
            {
                return _Visible;
            }
            set
            {
                SetProperty(ref _Visible, value);
            }
        }

        private bool _TopMost = false;

        public bool TopMost
        {
            get
            {
                return _TopMost;
            }
            set
            {
                SetProperty(ref _TopMost, value);
            }
        }

        private ColorString _BackColor = new ColorString("#00FF00");

        public ColorString BackColor
        {
            get
            {
                return _BackColor;
            }
            set
            {
                SetProperty(ref _BackColor, value);
            }
        }

        private WindowsPositionEntiy _Position = new WindowsPositionEntiy( new Rect(500, 0, 300, 300), WindowState.Normal);
        public WindowsPositionEntiy Position
        {
            get { return _Position; }
            set { SetProperty(ref _Position, value); }
        }

    }
}
