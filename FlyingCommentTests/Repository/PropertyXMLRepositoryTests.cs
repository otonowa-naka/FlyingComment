using Microsoft.VisualStudio.TestTools.UnitTesting;
using FlyingComment.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlyingComment.Model;

namespace FlyingComment.Repository.Tests
{
    [TestClass()]
    public class PropertyXMLRepositoryTests
    {
    
        [TestMethod()]
        public void LoadFontSettingEntityTest()
        {
            FontSettingEntity font = PropertyXMLRepository.LoadFontSettingEntity();

            Assert.AreNotEqual(null, font);
            Assert.AreEqual("Meiryo", font.FamilyString);
            Assert.AreEqual("48", font.SizeString);
            Assert.AreEqual(false, font.Italic);
            Assert.AreEqual(false, font.Bald);
            Assert.AreEqual("#FFFFFF", font.ColorString);
            Assert.AreEqual("Black", font.ThicknessColorString);
            Assert.AreEqual("1", font.ThicknessString);

            font.FamilyString = "MS Gothic";
            font.SizeString = "60";
            font.Italic = true;
            font.Bald = true;
            font.ColorString = "#FF0000";
            font.ThicknessColorString = "Red";
            font.ThicknessString = "2";

            PropertyXMLRepository.SaveFontSettingEntity(font);

            FontSettingEntity affont = PropertyXMLRepository.LoadFontSettingEntity();

            Assert.AreEqual("MS Gothic", affont.FamilyString);
            Assert.AreEqual("60", affont.SizeString);
            Assert.AreEqual(true, affont.Italic);
            Assert.AreEqual(true, affont.Bald);
            Assert.AreEqual("#FF0000", affont.ColorString);
            Assert.AreEqual("Red", affont.ThicknessColorString);
            Assert.AreEqual("2", affont.ThicknessString);

        }
    }
}