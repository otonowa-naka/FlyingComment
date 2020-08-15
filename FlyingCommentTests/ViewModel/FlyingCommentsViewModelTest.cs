using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FlyingComment.ViewModel;
using FlyingComment.Model;
using System.ComponentModel;

namespace FlyingCommentTests.ViewModel
{
    [TestClass]
    public class FlyingCommentsViewModelTest
    {
        string LastPropertyChangeName = null;
        FlyingCommentsViewModel ViewModel;
        private CommentWindowConfigurationEntity CommentWnd {  set; get; } = null;

        private  CommentQueueEntity CommentQueue { set; get; } = null;

        [TestInitialize]
        public void setup()
        {
            CommentWnd = new CommentWindowConfigurationEntity();
            CommentQueue = new CommentQueueEntity();
            ViewModel = new FlyingCommentsViewModel(CommentWnd, CommentQueue);
            ViewModel.PropertyChanged += (send, arg) =>
            {
                PropertyChangedEventArgs proarg = arg as PropertyChangedEventArgs;
                LastPropertyChangeName = proarg.PropertyName;
            };

            LastPropertyChangeName = "";
        }

        [TestMethod]
        public void Commentの追加の変更通知を受ける()
        {
            CommentQueue.PushText(new CommentTextEntiy("追加テキスト", new CommentStyleEntity()));

            Assert.AreEqual($"{ nameof(CommentQueue)}_Count", LastPropertyChangeName);
        }

        [TestMethod]
        public void Commentの追加の情報を受け取りその後キューが空になる()
        {
            CommentQueue.PushText(new CommentTextEntiy("追加テキスト", new CommentStyleEntity()));

            
            Assert.AreEqual("追加テキスト", ViewModel.PopComment.Comment);
            Assert.AreEqual(null, ViewModel.PopComment);
        }

        [TestMethod]
        public void Windowsの透明フラグの変更通知を受けとる()
        {
            CommentWnd.Stealth = true;

            Assert.AreEqual($"{nameof(CommentWnd)}_{nameof(CommentWnd.Stealth)}", LastPropertyChangeName);

        }

    }
}
