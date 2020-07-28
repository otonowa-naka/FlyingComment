using Microsoft.VisualStudio.TestTools.UnitTesting;
using FlyingComment.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

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
                fontset._m_notifyPropertyChanged += (send, arg) =>
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
                fontset._m_notifyPropertyChanged += (send, arg) =>
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
    }
}