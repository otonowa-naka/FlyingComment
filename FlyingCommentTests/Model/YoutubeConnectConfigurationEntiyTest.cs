using FlyingComment.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlyingCommentTests.Model
{
    [TestClass()]
    public class YoutubeConnectConfigurationEntiyTest
    {
        private YoutubeConnectEntiy _YouTube = null;
        string LastPropertyChangeName;

        [TestInitialize]
        public void setup()
        {
            _YouTube = new YoutubeConnectEntiy();
            _YouTube.PropertyChanged += (send, arg) =>
            {
                PropertyChangedEventArgs proarg = arg as PropertyChangedEventArgs;
                LastPropertyChangeName = proarg.PropertyName;
            };
        }

        [TestMethod()]
        public void YouTubeKeyを保持する()
        {
            const string testkey = "AIzaSyBnScGlhDjBtIPAtTZ1XjxhKocaDyJgQUz";

            _YouTube.ApiKey = testkey;
            Assert.AreEqual(testkey, _YouTube.ApiKey);
        }

        [TestMethod]
        public void VideoIDを保持する()
        {
            const string Video = "1gLu-N7Naab";

            _YouTube.VideoID = Video;
            Assert.AreEqual(Video, _YouTube.VideoID);
        }

        [TestMethod]
        public void YouTubeKeyの変更通知()
        {
            const string testkey = "AIzaSyBnScGlhDjBtIPAtTZ1XjxhKocaDyJgQUz";

            _YouTube.ApiKey = testkey;
            Assert.AreEqual(nameof(_YouTube.ApiKey), LastPropertyChangeName);
        }

        [TestMethod]
        public void VideIDの変更通知()
        {
            const string Video = "1gLu-N7Naab";

            _YouTube.VideoID = Video;
            Assert.AreEqual(nameof(_YouTube.VideoID), LastPropertyChangeName);
        }
    }
}
