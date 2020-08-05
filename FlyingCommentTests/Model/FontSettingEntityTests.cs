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
        FontSettingEntity FontSet;
        string LastPropertyChangeName ;
        bool ChangeEvent = false;

        [TestInitialize]
        public void setup()
        {
            FontSet = new FontSettingEntity();
            FontSet.PropertyChanged += (send, arg) =>
            {
                PropertyChangedEventArgs proarg = arg as PropertyChangedEventArgs;
                LastPropertyChangeName = proarg.PropertyName;
                ChangeEvent = true;
            };

            EventClear();
        }

        void EventClear()
        {
            LastPropertyChangeName = "";
            ChangeEvent = false;

        }

        [TestMethod()]
        public void コメント滞在時間にデータセットしたら値を変更して変更通知()
        {
            FontSet.CommentTimeString = "3000";

            Assert.AreEqual("CommentTimeString", LastPropertyChangeName);
            Assert.IsTrue(ChangeEvent);
        }

        [TestMethod]
        public void コメント滞在時間に異常データがセットしたら値を変更して変更通知()
        {
            FontSet.CommentTimeString = "";
            Assert.AreEqual("CommentTimeString", LastPropertyChangeName);
            Assert.IsTrue(ChangeEvent);
        }

        [TestMethod]
        public void コメント滞在時間の文字列をLongに変換()
        {
            FontSet.CommentTimeString = "9999";
            Assert.AreEqual(9999, FontSet.CommentTime);
        }

        [TestMethod]
        public void コメント滞在時間に異常値設定_小数点()
        {
            FontSet.CommentTimeString = "1.1";
            Assert.AreEqual(true, FontSet.IsError());
            Assert.AreNotEqual(null, FontSet.CommentTimeErrorMessage);
        }

        [TestMethod]
        public void コメント滞在時間に異常値設定_数字以外()
        {
            FontSet.CommentTimeString = "1a1";
            Assert.AreEqual(true, FontSet.IsError());
            Assert.AreNotEqual(null, FontSet.CommentTimeErrorMessage);
        }


        [TestMethod()]
        public void フォントファミリーにデータがセットしたら値を変更して変更通知()
        {
            FontSet.FamilyString = "MS ゴシック";
            Assert.AreEqual("FamilyString", LastPropertyChangeName);
            Assert.IsTrue(ChangeEvent);
        }

        [TestMethod()]
        public void フォントファミリーに異常データがセットしたら値を変更して変更通知()
        {
            FontSet.FamilyString = "";
            Assert.AreEqual("FamilyString", LastPropertyChangeName);
            Assert.IsTrue(ChangeEvent);
        }

        [TestMethod()]
        public void フォントファミリーに同じデータがセットしたら値を変更して変更通知を出さない()
        {
            //同じ値を２回セットする
            FontSet.FamilyString = "メイリオ";

            EventClear();
            FontSet.FamilyString = "メイリオ";
            Assert.IsFalse(ChangeEvent);
        }

        //　値パターンの確認
        [TestMethod()]
        public void フォントファミリーに異常値設定_空文字()
        {
            FontSet.FamilyString = "";
            Assert.AreEqual(true, FontSet.IsError());
            Assert.AreNotEqual(null, FontSet.FamilyStringErrorMessage);

        }

        [TestMethod()]
        public void フォントファミリーに異常値設定_NULL()
        {
            FontSet.FamilyString = null; 
            Assert.AreEqual(true, FontSet.IsError());
            Assert.AreNotEqual(null, FontSet.FamilyStringErrorMessage);
        }

        [TestMethod()]
        public void フォントファミリーに正常値設定_メイリオ()
        {
            FontSet.FamilyString = "メイリオ";
            Assert.AreEqual(false, FontSet.IsError());
            Assert.AreEqual(null, FontSet.FamilyStringErrorMessage);

        }

        [TestMethod()]
        public void フォントファミリーに正常値設定_英語()
        {
            FontSet.FamilyString = "Meiryo";
            Assert.AreEqual(false, FontSet.IsError());
            Assert.AreEqual(null, FontSet.FamilyStringErrorMessage);

        }

        [TestMethod()]
        public void フォントファミリーに異常値設定_適当()
        {
            FontSet.FamilyString = "適当";
            // システムで適当なフォントを割り当てるのでエラーにならない
            Assert.AreEqual(false, FontSet.IsError());
            Assert.AreEqual(null, FontSet.FamilyStringErrorMessage);

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