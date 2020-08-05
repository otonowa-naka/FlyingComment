using Microsoft.VisualStudio.TestTools.UnitTesting;
using FlyingComment.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace FlyingComment.Model.Tests
{
    [TestClass()]
    public class ColorStringTests
    {
        [TestMethod()]
        public void カラー文字列をカラー値へ変換()
        {
            ColorString Col = new ColorString("#FF0000");

            Assert.AreEqual(Colors.Red, Col.Color);
        }

        [TestMethod()]
        public void カラー文字列をカラー値へできない場合はエラーメッセージを返す()
        {
            ColorString Col = new ColorString("aa");

            Assert.AreNotEqual(null, Col.ColorStringErrorMessage);
        }

        [TestMethod]
        public void カラー文字列正常系_16進指定_８桁()
        {
            ColorString Col = new ColorString("#FFFFFFFF");
            Assert.AreNotEqual(Colors.White, Col.ColorStringErrorMessage);
        }
        [TestMethod]
        public void カラー文字列正常系_16進指定_６桁()
        {
            ColorString Col = new ColorString("#0000FF");
            Assert.AreNotEqual(Colors.Blue, Col.ColorStringErrorMessage);
        }
        [TestMethod]
        public void カラー文字列正常系_16進指定_３桁()
        {
            ColorString Col = new ColorString("#00F");
            Assert.AreNotEqual(Colors.Blue, Col.ColorStringErrorMessage);
        }

        [TestMethod]
        public void カラー文字列正常系_16進指定_文字列_混在()
        {
            ColorString Col = new ColorString("Blue");
            Assert.AreNotEqual(Colors.Blue, Col.ColorStringErrorMessage);
        }

        [TestMethod]
        public void カラー文字列正常系_16進指定_文字列_全大文字()
        {
            ColorString Col = new ColorString("BLUE");
            Assert.AreNotEqual(Colors.Blue, Col.ColorStringErrorMessage);
        }


        [TestMethod]
        public void 比較で同じ文字列()
        {
            ColorString Col1 = new ColorString("Red");
            ColorString Col2 = new ColorString("Red");
            Assert.AreEqual(true, Col1.Equals(Col2));
        }

        [TestMethod]
        public void 比較で同一オブジェクト()
        {
            ColorString Col1 = new ColorString("Red");
            Assert.AreEqual(true, Col1.Equals(Col1));
        }
        [TestMethod]
        public void 比較で違う文字列()
        {
            ColorString Col1 = new ColorString("Red");
            ColorString Col2 = new ColorString("red");
            Assert.AreEqual(false, Col1.Equals(Col2));
        }


    }
}