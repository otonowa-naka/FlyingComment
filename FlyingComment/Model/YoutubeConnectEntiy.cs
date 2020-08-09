using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlyingComment.Model
{
    public class YoutubeConnectEntiy : NotifyChangedBase
    {

        private string _ApiKey = "AIzaSyBnScGlhDjBtIPAtTZ1XjxhKocaDyJgQUw";
        public string ApiKey {
            get
            {
                return _ApiKey;
            }
            set
            {
                SetProperty(ref _ApiKey, value);
            }
        } 

        private string _Video = "1gLu-N7Naaa";
        public string VideoID {
            get
            {
                return _Video;
            }
            set
            {
                SetProperty(ref _Video, value);
            }
        }
    }
}
