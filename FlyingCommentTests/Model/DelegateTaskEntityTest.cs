using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FlyingComment.Model;
using System.Threading;
using System.ComponentModel;
using System.Threading.Tasks;

namespace FlyingCommentTests.Model
{
    /// <summary>
    /// DelegateTaskEntity の概要の説明
    /// </summary>
    [TestClass]
    public class DelegateTaskEntityTest
    {
        public class MockTack
        {
            public string Parameter { get; private set; } = "";
            private int SleepTime= 500;

            public MockTack(int sleep)
            {
                SleepTime = sleep;
            }

            public Action TaskMain(object obj, CancellationToken token)
            {
                Action ret = null;

                WaitHandle.WaitAll(new WaitHandle[] { token.WaitHandle }, SleepTime);
                Parameter = obj as string;


                return ret;
            }
        }


        DelegateTaskEntity task = null;
        string LastPropertyChangeName;

        #region 追加のテスト属性
        // 各テストを実行する前に、TestInitialize を使用してコードを実行してください
        [TestInitialize()]
        public void MyTestInitialize()
        {
            task = new DelegateTaskEntity();
            task.PropertyChanged += (send, arg) =>
            {
                PropertyChangedEventArgs proarg = arg as PropertyChangedEventArgs;
                LastPropertyChangeName = proarg.PropertyName;
            };
        }

        #endregion

        [TestMethod]
        public void タスクの実行()
        {
            MockTack moc = new MockTack(50);

            task.Run(moc.TaskMain, "テスト");

            Assert.AreEqual(true,  task.Wait(1000));
            Assert.AreEqual("テスト", moc.Parameter);

        }

        [TestMethod]
        public void タスクの状態変更通知_スレッド開始()
        {
            MockTack moc = new MockTack(50);

            task.Run(moc.TaskMain, "テスト");
            
            Assert.AreEqual(nameof(task.IsRunning), LastPropertyChangeName);
        }

        [TestMethod]
        public void タスクの状態変更通知_スレッド終了()
        {
            MockTack moc = new MockTack(1000);

            task.Run(moc.TaskMain, "テスト");

            Assert.AreEqual(nameof(task.IsRunning), LastPropertyChangeName);

            LastPropertyChangeName = "";
            Assert.AreEqual("", LastPropertyChangeName);

            Assert.AreEqual(true, task.Wait(2000));
            Assert.AreEqual(false, task.IsRunning);
            Assert.AreEqual(nameof(task.IsRunning), LastPropertyChangeName);
        }
        [TestMethod]
        public void Wait関数のタイムアウト()
        {
            MockTack moc = new MockTack(1000);

            task.Run(moc.TaskMain, "テスト");

            Assert.AreEqual(false, task.Wait(10));
            Assert.AreEqual(true, task.IsRunning);
        }


        [TestMethod]
        public void タスクのキャンセル確認()
        {
            MockTack moc = new MockTack(10000);

            task.Run(moc.TaskMain, "テスト");

            task.CancelTask();

            Assert.AreEqual(true, task.Wait(10));
            Assert.AreEqual(false, task.IsRunning);
        }
    }
}
