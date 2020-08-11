using FlyingComment.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlyingComment.Repository
{
    public static class PropertyXMLRepository
    {
        public static void Save(CommentStyleEntity style, CommentWindowConfigurationEntity commentWnd, YoutubeConnectEntiy youTube, WindowsPositionEntiy settingWindowPos)
        {
            SaveCommentStyleEntity(style);
            SaveCommentWindowConfigurationEntity(commentWnd);
            SaveYoutubeConnectEntiy(youTube);
            SaveSettingWindowPositionEntiy(settingWindowPos);

            Properties.Settings.Default.Save();
        }

        public static CommentStyleEntity LoadCommentStyleEntity()
        {
            CommentStyleEntity ret = new CommentStyleEntity();
            ret.FamilyString = Properties.Settings.Default.FontName;
            ret.SizeString = Properties.Settings.Default.FontSize;
            ret.Italic = Properties.Settings.Default.FontItalic;
            ret.Bald = Properties.Settings.Default.FontBald;
            ret.ColorString = Properties.Settings.Default.FontColor;
            ret.ThicknessColorString = Properties.Settings.Default.FontThicknessColor;
            ret.ThicknessString = Properties.Settings.Default.FontThickness;
            ret.CommentTimeString = Properties.Settings.Default.CommentTime;

            return ret;
        }
        internal static WindowsPositionEntiy LoadSettingWindowPositionEntiy()
        {
            return new WindowsPositionEntiy(Properties.Settings.Default.SettingWndRect, Properties.Settings.Default.SettingState);
        }

        private static void SaveCommentStyleEntity(CommentStyleEntity entity)
        {
            Properties.Settings.Default.FontName = entity.FamilyString;
            Properties.Settings.Default.FontSize = entity.SizeString;           
            Properties.Settings.Default.FontItalic = entity.Italic;             
            Properties.Settings.Default.FontBald = entity.Bald;                 
            Properties.Settings.Default.FontColor = entity.ColorString;          
            Properties.Settings.Default.FontThicknessColor = entity.ThicknessColorString;
            Properties.Settings.Default.FontThickness = entity.ThicknessString;
            Properties.Settings.Default.CommentTime = entity.CommentTimeString;
        }



        public static CommentWindowConfigurationEntity LoadCommentWindowConfigurationEntity()
        {
            CommentWindowConfigurationEntity ret = new CommentWindowConfigurationEntity();
            ret.Stealth = Properties.Settings.Default.Stealth;
            ret.Visible = Properties.Settings.Default.Visible;
            ret.TopMost = Properties.Settings.Default.Topmost;
            ret.BackColor = new ColorString( Properties.Settings.Default.BackColor);
            ret.Position = new WindowsPositionEntiy(Properties.Settings.Default.CommentWndRect, Properties.Settings.Default.CommentWinState);

            return ret;
        }

        private static void SaveCommentWindowConfigurationEntity(CommentWindowConfigurationEntity entity)
        {
             Properties.Settings.Default.Stealth = entity.Stealth;
             Properties.Settings.Default.Visible = entity.Visible;
             Properties.Settings.Default.Topmost = entity.TopMost;
             Properties.Settings.Default.BackColor = entity.BackColor.ValueString;
             Properties.Settings.Default.CommentWndRect = entity.Position.WindowRect;
             Properties.Settings.Default.CommentWinState = entity.Position.State;
        }
        public static YoutubeConnectEntiy LoadYoutubeConnectEntiy()
        {
            YoutubeConnectEntiy ret = new YoutubeConnectEntiy();
            ret.ApiKey = Properties.Settings.Default.APIKey;
            ret.VideoID = Properties.Settings.Default.VideoID;

            return ret;
        }

        private static void SaveYoutubeConnectEntiy(YoutubeConnectEntiy entity)
        {
            Properties.Settings.Default.APIKey = entity.ApiKey;
            Properties.Settings.Default.VideoID = entity.VideoID;
        }

        private static void SaveSettingWindowPositionEntiy(WindowsPositionEntiy entity)
        {
            Properties.Settings.Default.SettingWndRect = entity.WindowRect;
            Properties.Settings.Default.SettingState = entity.State;
        }

    }
}
