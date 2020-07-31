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
        public static FontSettingEntity LoadFontSettingEntity()
        {
            FontSettingEntity ret = new FontSettingEntity();
            ret.FamilyString = Properties.Settings.Default.FontName;
            ret.SizeString = Properties.Settings.Default.FontSize;
            ret.Italic = Properties.Settings.Default.FontItalic;
            ret.Bald = Properties.Settings.Default.FontBald;
            ret.ColorString = Properties.Settings.Default.FontColor;
            ret.ThicknessColorString = Properties.Settings.Default.FontThicknessColor;
            ret.ThicknessString = Properties.Settings.Default.FontThickness;

            return ret;
        }

        public static void SaveFontSettingEntity(FontSettingEntity entity)
        {
            Properties.Settings.Default.FontName = entity.FamilyString;
            Properties.Settings.Default.FontSize = entity.SizeString;           
            Properties.Settings.Default.FontItalic = entity.Italic;             
            Properties.Settings.Default.FontBald = entity.Bald;                 
            Properties.Settings.Default.FontColor = entity.ColorString;          
            Properties.Settings.Default.FontThicknessColor = entity.ThicknessColorString;
            Properties.Settings.Default.FontThickness = entity.ThicknessString;  

        }
    }
}
