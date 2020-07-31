using Microsoft.VisualStudio.TestTools.UnitTesting;
using FlyingComment.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows.Media;

namespace FlyingComment.Model.Tests
{
    [TestClass()]
    public class FontSettingEntityTests
    {
        [TestMethod()]
        public void FamilyStringTest()
        {
            // 初期値はエラー
            FontSettingEntity fontset = new FontSettingEntity();
            Assert.AreEqual(false, fontset.IsError());


            bool ChangeEvent = false;

            //正常系
            {
                fontset.PropertyChanged += (send, arg) =>
                {
                    PropertyChangedEventArgs proarg = arg as PropertyChangedEventArgs;
                    Assert.AreEqual("FamilyString", proarg.PropertyName);
                    ChangeEvent = true;
                };

                fontset.FamilyString = "MS ゴシック";
                Assert.AreEqual(false, fontset.IsError());
                Assert.IsTrue(ChangeEvent);

                // 正常から正常へ
                // イベント状態のリセット
                ChangeEvent = false;
                fontset.FamilyString = "メイリオ";
                Assert.AreEqual(false, fontset.IsError());
                Assert.IsTrue(ChangeEvent);


                // 値の変更なし
                // イベント状態のリセット
                ChangeEvent = false;
                fontset.FamilyString = "メイリオ";
                Assert.AreEqual(false, fontset.IsError());
                Assert.IsFalse(ChangeEvent);

            }

            //　正常から異常イベントの確認
            {
                // イベント状態のリセット
                ChangeEvent = false;

                fontset.FamilyString = null;
                Assert.AreEqual(true, fontset.IsError());
                Assert.IsTrue(ChangeEvent);

                //異常から異常イベントの確認
                ChangeEvent = false;

                fontset.FamilyString = "";
                Assert.AreEqual(true, fontset.IsError());
                Assert.IsTrue(ChangeEvent);


            }

            //　値パターンの確認

            fontset.FamilyString = "";
            Assert.AreEqual(true, fontset.IsError());

            fontset.FamilyString = "MS ゴシック";
            Assert.AreEqual(false, fontset.IsError());

            fontset.FamilyString = "メイリオ";
            Assert.AreEqual(false, fontset.IsError());

            fontset.FamilyString = "Meiryo";
            Assert.AreEqual(false, fontset.IsError());

            // システムで適当なフォントを割り当てるのでエラーにならない
            fontset.FamilyString = "適当";
            Assert.AreEqual(false, fontset.IsError());
        }

        [TestMethod()]
        public void SizeStringErrorTest()
        {
            // 初期値はエラー
            FontSettingEntity fontset = new FontSettingEntity();


            bool ChangeEvent = false;

            //正常系
            {
                fontset.PropertyChanged += (send, arg) =>
                {
                    PropertyChangedEventArgs proarg = arg as PropertyChangedEventArgs;
                    Assert.AreEqual("SizeString", proarg.PropertyName);
                    ChangeEvent = true;
                };

                fontset.SizeString = "18.0";
                Assert.AreEqual("18.0", fontset.SizeString);
                Assert.AreEqual(18.0, fontset.Size);
                Assert.AreEqual(false, fontset.IsError());
                Assert.IsTrue(ChangeEvent);

                // 正常から正常へ
                // イベント状態のリセット
                ChangeEvent = false;
                fontset.SizeString = "20";
                Assert.AreEqual("20", fontset.SizeString);
                Assert.AreEqual(20, fontset.Size);
                Assert.AreEqual(false, fontset.IsError());
                Assert.IsTrue(ChangeEvent);


                // 値の変更なし
                // イベント状態のリセット
                ChangeEvent = false;
                fontset.SizeString = "20";
                Assert.AreEqual("20", fontset.SizeString);
                Assert.AreEqual(20, fontset.Size);
                Assert.AreEqual(false, fontset.IsError());
                Assert.IsFalse(ChangeEvent);

            }

            //　正常から異常イベントの確認
            {
                // イベント状態のリセット
                ChangeEvent = false;

                fontset.SizeString = null;
                Assert.AreEqual(null, fontset.SizeString);
                Assert.AreEqual(0, fontset.Size);
                Assert.AreEqual(true, fontset.IsError());
                Assert.AreNotEqual(null, fontset.SizeStringErrorMessage);
                Assert.IsTrue(ChangeEvent);

                //異常から異常イベントの確認
                ChangeEvent = false;

                fontset.SizeString = "";
                Assert.AreEqual("", fontset.SizeString);
                Assert.AreEqual(0, fontset.Size);
                Assert.AreEqual(true, fontset.IsError());
                Assert.AreNotEqual(null, fontset.SizeStringErrorMessage);
                Assert.IsTrue(ChangeEvent);


            }

            //　値パターンの確認

            fontset.SizeString = "  10.1  ";
            Assert.AreEqual(10.1, fontset.Size);
            Assert.AreEqual(false, fontset.IsError());

            fontset.SizeString = "10.1000000";
            Assert.AreEqual(10.1, fontset.Size);
            Assert.AreEqual(false, fontset.IsError());

            fontset.SizeString = ".5";
            Assert.AreEqual(0.5, fontset.Size);
            Assert.AreEqual(false, fontset.IsError());

            fontset.SizeString = "1..0";
            Assert.AreEqual(true, fontset.IsError());

            fontset.SizeString = "1aa";
            Assert.AreEqual(true, fontset.IsError());

            fontset.SizeString = "オ";
            Assert.AreEqual(true, fontset.IsError());

            fontset.SizeString = "!";
            Assert.AreEqual(true, fontset.IsError());

            // システムで適当なフォントを割り当てるのでエラーにならない
            fontset.SizeString = "22a22";
            Assert.AreEqual(true, fontset.IsError());
        }

        [TestMethod()]
        public void ColorStringStringTest()
        {
            // 初期値はエラー
            FontSettingEntity fontset = new FontSettingEntity();


            bool ChangeEvent = false;

            //正常系
            {
                fontset.PropertyChanged += (send, arg) =>
                {
                    PropertyChangedEventArgs proarg = arg as PropertyChangedEventArgs;
                    Assert.AreEqual("ColorString", proarg.PropertyName);
                    ChangeEvent = true;
                };

                fontset.ColorString = "#FFFFFFFF";
                Assert.AreEqual("#FFFFFFFF", fontset.ColorString);
                Assert.AreEqual(Colors.White, fontset.Color);
                Assert.AreEqual(false, fontset.IsError());
                Assert.IsTrue(ChangeEvent);

                // 正常から正常へ
                // イベント状態のリセット
                ChangeEvent = false;
                fontset.ColorString = "red";
                Assert.AreEqual("red", fontset.ColorString);
                Assert.AreEqual(Colors.Red, fontset.Color);
                Assert.AreEqual(false, fontset.IsError());
                Assert.IsTrue(ChangeEvent);


                // 値の変更なし
                // イベント状態のリセット
                ChangeEvent = false;
                fontset.ColorString = "red";
                Assert.AreEqual("red", fontset.ColorString);
                Assert.AreEqual(Colors.Red, fontset.Color);
                Assert.AreEqual(false, fontset.IsError());
                Assert.IsFalse(ChangeEvent);

            }

            //　正常から異常イベントの確認
            {
                // イベント状態のリセット
                ChangeEvent = false;

                fontset.ColorString = null;
                Assert.AreEqual(null, fontset.ColorString);
                Assert.AreEqual(new Color(), fontset.Color);

                Assert.AreEqual(true, fontset.IsError());
                Assert.AreNotEqual(null, fontset.ColorStringErrorMessage);
                Assert.IsTrue(ChangeEvent);

                //異常から異常イベントの確認
                ChangeEvent = false;

                fontset.ColorString = "";
                Assert.AreEqual("", fontset.ColorString);
                Assert.AreEqual(new Color(), fontset.Color);
                Assert.AreEqual(true, fontset.IsError());
                Assert.AreNotEqual(null, fontset.ColorStringErrorMessage);
                Assert.IsTrue(ChangeEvent);


            }

            //　値パターンの確認

            fontset.ColorString = "Red";
            Assert.AreEqual(Colors.Red, fontset.Color);
            Assert.AreEqual(false, fontset.IsError());

            fontset.ColorString = "RED";
            Assert.AreEqual(Colors.Red, fontset.Color);
            Assert.AreEqual(false, fontset.IsError());

            fontset.ColorString = "#FF0000";
            Assert.AreEqual(Colors.Red, fontset.Color);
            Assert.AreEqual(false, fontset.IsError());
            fontset.ColorString = "#00FF00";
            Assert.AreEqual(Colors.Lime, fontset.Color);
            Assert.AreEqual(false, fontset.IsError());
            fontset.ColorString = "#0000FF";
            Assert.AreEqual(Colors.Blue, fontset.Color);
            Assert.AreEqual(false, fontset.IsError());

            fontset.ColorString = "#F00";
            Assert.AreEqual(Colors.Red, fontset.Color);
            Assert.AreEqual(false, fontset.IsError());
            fontset.ColorString = "#0F0";
            Assert.AreEqual(Colors.Lime, fontset.Color);
            Assert.AreEqual(false, fontset.IsError());
            fontset.ColorString = "#00F";
            Assert.AreEqual(Colors.Blue, fontset.Color);
            Assert.AreEqual(false, fontset.IsError());

            fontset.ColorString = "#FFFF0000";
            Assert.AreEqual(Colors.Red, fontset.Color);
            Assert.AreEqual(false, fontset.IsError());


            fontset.ColorString = "#FFFFFFFFF";
            Assert.AreEqual(true, fontset.IsError());
            fontset.ColorString = "#FFFFFFFF";
            Assert.AreEqual(false, fontset.IsError());
            fontset.ColorString = "#FFFFFFF";
            Assert.AreEqual(true, fontset.IsError());
            fontset.ColorString = "#FFFFFF";
            Assert.AreEqual(false, fontset.IsError());
            fontset.ColorString = "#FFFFF";
            Assert.AreEqual(true, fontset.IsError());
            fontset.ColorString = "#FFFF";
            Assert.AreEqual(false, fontset.IsError());
            fontset.ColorString = "#FFF";
            Assert.AreEqual(false, fontset.IsError());
            fontset.ColorString = "#FF";
            Assert.AreEqual(true, fontset.IsError());
            fontset.ColorString = "#F";
            Assert.AreEqual(true, fontset.IsError());



            fontset.ColorString = "F";
            Assert.AreEqual(true, fontset.IsError());

            fontset.ColorString = "FFFFFF";
            Assert.AreEqual(true, fontset.IsError());

            fontset.ColorString = "オ";
            Assert.AreEqual(true, fontset.IsError());

            fontset.ColorString = "!";
            Assert.AreEqual(true, fontset.IsError());
        }

    }
}