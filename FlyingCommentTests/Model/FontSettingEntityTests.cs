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
            FontSettingEntity fontset = new FontSettingEntity();
            Assert.AreEqual(true, fontset.ValidationErrorMessage().HasError());


            bool ChangeEvent = false;
            bool ErrorEvent = false;

            //正常系
            {
                fontset._m_notifyPropertyChanged += (send, arg) =>
                {
                    PropertyChangedEventArgs proarg = arg as PropertyChangedEventArgs;
                    Assert.AreEqual("FamilyString", proarg.PropertyName);
                    ChangeEvent = true;
                };
                fontset._m_notifyError += (send, arg) =>
                {
                    PropertyChangedEventArgs proarg = arg as PropertyChangedEventArgs;
                    Assert.AreEqual("FamilyString", proarg.PropertyName);
                    ErrorEvent = true;
                };

                fontset.FamilyString = "MS ゴシック";
                Assert.AreEqual(false, fontset.ValidationErrorMessage().HasError());
                Assert.IsTrue(ChangeEvent);
                Assert.IsTrue(ErrorEvent);  // 初期がエラーなので変更イベントが発生

                // 正常から正常へ
                // イベント状態のリセット
                ChangeEvent = false;
                ErrorEvent = false;
                fontset.FamilyString = "メイリオ";
                Assert.AreEqual(false, fontset.ValidationErrorMessage().HasError());
                Assert.IsTrue(ChangeEvent);
                Assert.IsFalse(ErrorEvent);


                // 値の変更なし
                // イベント状態のリセット
                ChangeEvent = false;
                ErrorEvent = false;
                fontset.FamilyString = "メイリオ";
                Assert.AreEqual(false, fontset.ValidationErrorMessage().HasError());
                Assert.IsFalse(ChangeEvent);
                Assert.IsFalse(ErrorEvent);

            }

            //　正常から異常イベントの確認
            {
                // イベント状態のリセット
                ChangeEvent = false;
                ErrorEvent = false;

                fontset.FamilyString = null;
                Assert.AreEqual(true, fontset.ValidationErrorMessage().HasError());
                Assert.IsTrue(ChangeEvent);
                Assert.IsTrue(ErrorEvent);

                //異常から異常イベントの確認
                ChangeEvent = false;
                ErrorEvent = false;

                fontset.FamilyString = "";
                Assert.AreEqual(true, fontset.ValidationErrorMessage().HasError());
                Assert.IsTrue(ChangeEvent);
                Assert.IsFalse(ErrorEvent);


            }

            //　値パターンの確認

            fontset.FamilyString = "";
            Assert.AreEqual(true, fontset.ValidationErrorMessage().HasError());

            fontset.FamilyString = "MS ゴシック";
            Assert.AreEqual(false, fontset.ValidationErrorMessage().HasError());

            fontset.FamilyString = "メイリオ";
            Assert.AreEqual(false, fontset.ValidationErrorMessage().HasError());

            fontset.FamilyString = "Meiryo";
            Assert.AreEqual(false, fontset.ValidationErrorMessage().HasError());

            // システムで適当なフォントを割り当てるのでエラーにならない
            fontset.FamilyString = "適当";
            Assert.AreEqual(false, fontset.ValidationErrorMessage().HasError());
         }

    }
}