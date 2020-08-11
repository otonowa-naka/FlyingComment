using Microsoft.VisualStudio.TestTools.UnitTesting;
using FlyingComment.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows;
using FlyingComment.Model;

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
            MainViewModel = new MainWindowViewModel(new CommentStyleEntity(), new CommentWindowConfigurationEntity(), new YoutubeConnectEntiy(), new WindowsPositionEntiy() );
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
            MainViewModel.CommentStyle_CommentTimeString = "6000";

            Assert.AreEqual(nameof(MainViewModel.CommentStyle_CommentTimeString), LastPropertyChangeName);
        }

        [TestMethod()]
        public void BackColorを設定すると変更通知()
        {
            MainViewModel.CommentWnd_BackColor = "red";

            Assert.AreEqual(nameof(MainViewModel.CommentWnd_BackColor) , LastPropertyChangeName);
        }

        [TestMethod()]
        public void BackColorに異常値を代入するとエラー判定がTrueになる()
        {
            MainViewModel.CommentWnd_BackColor = "無効値";

            Assert.AreEqual(true, MainViewModel.HasErrors);
        }

        [TestMethod()]
        public void BackColorに異常値を代入するとエラーメッセージが取得できる()
        {
            MainViewModel.CommentWnd_BackColor = "無効文字";

            Assert.AreNotEqual(null, MainViewModel.GetErrors(nameof(MainViewModel.CommentWnd_BackColor)));
        }

        [TestMethod]
        public void TopMostを変更すると変更通知する()
        {
            MainViewModel.CommentWnd_TopMost = true;
            Assert.AreEqual(nameof(MainViewModel.CommentWnd_TopMost), LastPropertyChangeName);
        }

        [TestMethod]
        public void Visibleを変更すると変更通知をする()
        {
            MainViewModel.CommentWnd_Visible = true;
            Assert.AreEqual(nameof(MainViewModel.CommentWnd_Visible), LastPropertyChangeName);
        }


        [TestMethod]
        public void Stealthを変更すると変更通知をする()
        {
            MainViewModel.CommentWnd_Stealth = true;
            Assert.AreEqual(nameof(MainViewModel.CommentWnd_Stealth), LastPropertyChangeName);
        }

        [TestMethod]
        public void CommentWndRectを変更すると変更通知をする()
        {
            MainViewModel.CommentWnd_Position = new WindowsPositionEntiy( new Rect(10,10,10,10), WindowState.Minimized);
            Assert.AreEqual(nameof(MainViewModel.CommentWnd_Position), LastPropertyChangeName);
        }

        [TestMethod]
        public void YouTubeConnect_ApiKeyを変更すると変更通知をする()
        {
            MainViewModel.YouTubeConnect_ApiKey = "11111";
            Assert.AreEqual(nameof(MainViewModel.YouTubeConnect_ApiKey), LastPropertyChangeName);
        }

        [TestMethod]
        public void YouTubeConnect_VideoIDを変更すると変更通知をする()
        {
            MainViewModel.YouTubeConnect_VideoID = "11111";
            Assert.AreEqual(nameof(MainViewModel.YouTubeConnect_VideoID), LastPropertyChangeName);
        }
    }
}