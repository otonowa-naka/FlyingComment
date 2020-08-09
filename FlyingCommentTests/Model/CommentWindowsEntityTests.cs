using Microsoft.VisualStudio.TestTools.UnitTesting;
using FlyingComment.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows.Media;
using System.Windows;

namespace FlyingComment.Model.Tests
{
    [TestClass()]
    public class CommentWindowsEntityTests
    {
        CommentWindowConfigurationEntity CommentWnd = null;
        string LastPropertyChangeName;

        [TestInitialize]
        public void 前準備()
        {
            CommentWnd = new CommentWindowConfigurationEntity();
            CommentWnd.PropertyChanged += (send, arg) =>
            {
                PropertyChangedEventArgs proarg = arg as PropertyChangedEventArgs;
                LastPropertyChangeName = proarg.PropertyName;
            };

            EventClear();
        }

        void EventClear()
        {
            LastPropertyChangeName = "";
           
        }
        [TestMethod]
        public void 同一値の場合は変更通知をしない()
        {
            CommentWnd.Stealth = true;
            EventClear();
            CommentWnd.Stealth = true;
            Assert.AreEqual("", LastPropertyChangeName);
        }

        [TestMethod]
        public void 透明化フラグの設定保持()
        {
            CommentWnd.Stealth = true;
            Assert.AreEqual(true, CommentWnd.Stealth);
        }

        [TestMethod]
        public void 透明化フラグの変更通知()
        {
            CommentWnd.Stealth = true;
            Assert.AreEqual(nameof(CommentWnd.Stealth), LastPropertyChangeName);
        }

        [TestMethod]
        public void 非表示フラグの設定保持()
        {
            CommentWnd.Visible = true;
            Assert.AreEqual(true, CommentWnd.Visible);

        }
        [TestMethod]
        public void 非表示フラグの変更通知()
        {
            CommentWnd.Visible = true;
            Assert.AreEqual(nameof(CommentWnd.Visible), LastPropertyChangeName);
        }
        [TestMethod]
        public void TopMostフラグの設定保持()
        {
            CommentWnd.TopMost = false;
            Assert.AreEqual(false, CommentWnd.TopMost);

        }

        [TestMethod]
        public void TopMostフラグの変更通知()
        {
            CommentWnd.TopMost = true;
            Assert.AreEqual(nameof(CommentWnd.TopMost), LastPropertyChangeName);
        }


        [TestMethod]
        public void Windowバックカラーの設定保持()
        {

            CommentWnd.BackColor = new ColorString("Green");
            Assert.AreEqual("Green", CommentWnd.BackColor.ValueString);

        }

        [TestMethod]
        public void Windowバックカラーの変更通知()
        {
            CommentWnd.BackColor = new ColorString("Green"); ;
            Assert.AreEqual(nameof(CommentWnd.BackColor), LastPropertyChangeName);
        }

        [TestMethod]
        public void Windows位置の保持()
        {
            CommentWnd.WindowRect = new Rect(100, 200, 300, 400);
            Assert.AreEqual(new Rect(100, 200, 300, 400), CommentWnd.WindowRect);
        }

        [TestMethod]
        public void Windows位置の変更通知()
        {
            CommentWnd.WindowRect = new Rect(100, 200, 300, 400);
            Assert.AreEqual(nameof(CommentWnd.WindowRect), LastPropertyChangeName);

        }


        [TestMethod]
        public void Windows状態の保持()
        {
            CommentWnd.State = WindowState.Minimized; 
            Assert.AreEqual(WindowState.Minimized, CommentWnd.State);
        }

        [TestMethod]
        public void Windows状態の変更通知()
        {
            CommentWnd.State = WindowState.Maximized; 
            Assert.AreEqual(nameof(CommentWnd.State), LastPropertyChangeName);

        }
    }
}