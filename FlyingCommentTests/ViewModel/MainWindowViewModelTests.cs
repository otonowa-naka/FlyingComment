using Microsoft.VisualStudio.TestTools.UnitTesting;
using FlyingComment.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace FlyingComment.ViewModel.Tests
{
    [TestClass()]
    public class MainWindowViewModelTests
    {
        string LastPropertyChangeName = null;
        MainWindowViewModel MainViewModel;


        [TestInitialize]
        public void setup()
        {
            MainViewModel = new MainWindowViewModel();
            MainViewModel.PropertyChanged += (send, arg) =>
            {
                PropertyChangedEventArgs proarg = arg as PropertyChangedEventArgs;
                LastPropertyChangeName = proarg.PropertyName;
            };

            LastPropertyChangeName = "";
        }

        [TestMethod()]
        public void CommentTimeを設定すると変更通知()
        {
            MainViewModel.CommentTime = "6000";

            Assert.AreEqual(nameof(MainViewModel.CommentTime), LastPropertyChangeName);
        }

        [TestMethod()]
        public void BackColorを設定すると変更通知()
        {
            MainViewModel.BackColor = "red";

            Assert.AreEqual(nameof(MainViewModel.BackColor) , LastPropertyChangeName);
        }

        [TestMethod()]
        public void BackColorに異常値を代入するとエラー判定がTrueになる()
        {
            MainViewModel.BackColor = "無効値";

            Assert.AreEqual(true, MainViewModel.HasErrors);
        }

        [TestMethod()]
        public void BackColorに異常値を代入するとエラーメッセージが取得できる()
        {
            MainViewModel.BackColor = "無効文字";

            Assert.AreNotEqual(null, MainViewModel.GetErrors(nameof(MainViewModel.BackColor)));
        }

        [TestMethod]
        public void TopMostを変更すると変更通知する()
        {
            MainViewModel.TopMost = false;
            Assert.AreEqual(nameof(MainViewModel.TopMost), LastPropertyChangeName);
        }

        [TestMethod]
        public void Visibleを変更すると変更通知をする()
        {
            MainViewModel.Visible = true;
            Assert.AreEqual(nameof(MainViewModel.Visible), LastPropertyChangeName);
        }


        [TestMethod]
        public void Stealthを変更すると変更通知をする()
        {
            MainViewModel.Stealth = true;
            Assert.AreEqual(nameof(MainViewModel.Stealth), LastPropertyChangeName);
        }
    }
}