using FlyingComment.Model;
using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Collections;
using FlyingComment.Repository;
using FlyingComment.Service;
using System.Windows.Input;

namespace FlyingComment.ViewModel
{


    public class MainWindowViewModel : INotifyPropertyChanged, INotifyDataErrorInfo
    {
        private CommentStyleEntity CommentStyle { set; get; } = null;
        private CommentWindowConfigurationEntity CommentWnd { set; get; } = null;
        private YoutubeConnectEntiy YouTubeConnect { set; get; } = null;
        private CommentQueueEntity CommentQueue { set; get; } = null;
        public WindowsPositionEntiy SettingWindowPosition { set; get; } = null;
        public DelegateTaskEntity CommnetMonitor { get; private set; } = null;

        /// <summary>
        /// ログインスタンス
        /// </summary>
        private static readonly log4net.ILog _logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        /// <summary>
        /// コンストラクタ
        /// </summary>
        public MainWindowViewModel(
            CommentStyleEntity style, 
            CommentWindowConfigurationEntity commentWnd, 
            WindowsPositionEntiy settingWindowPosition,
            CommentQueueEntity commentQueue,
            DelegateTaskEntity commnetMonitor,
            YoutubeConnectEntiy youTubeConnect
            )
        {
            // APP設定でデータの初期化
            try
            {
                CommentStyle = style;
                CommentWnd = commentWnd;
                SettingWindowPosition = settingWindowPosition;
                CommentQueue = commentQueue;
                CommnetMonitor = commnetMonitor;
                YouTubeConnect = youTubeConnect;

                CommentStyle.PropertyChanged += OnPropertyChanged_CommentStyle;
                CommentWnd.PropertyChanged += OnPropertyChanged_CommentWnd;
                // SettingWindowPosition    // 画面に表示する項目が無いので変更通知は無し
                // CommentQueue             // 画面に表示する項目が無いので変更通知は無し
                CommnetMonitor.PropertyChanged += OnPropertyChanged_CommnetMonitor;
                YouTubeConnect.PropertyChanged += OnPropertyChanged_YouTubeConnect;


            }
            catch (Exception ex)
            {
                _logger.Error($"MainWindowViewModelの例外メッセージ={ex.Message}");

            }
        }


        #region INotifyDataErrorInfo実装
        public IEnumerable GetErrors(string propertyName)
        {
            IEnumerable ret = null;
            string errmsg = null;

            if (propertyName == nameof(CommentStyle_FamilyString))
            {
                errmsg = CommentStyle.FamilyStringErrorMessage;
            }
            else
            if (propertyName == nameof(CommentStyle_SizeString))
            {
                errmsg = CommentStyle.SizeStringErrorMessage;
            }
            else
            if (propertyName == nameof(CommentStyle_ColorString))
            {
                errmsg = CommentStyle.ColorStringErrorMessage;
            }
            else
            if (propertyName == nameof(CommentStyle_ThicknessColorString))
            {
                errmsg = CommentStyle.ThicknessColorStringErrorMessage;
            }
            else
            if (propertyName == nameof(CommentStyle_ThicknessString))
            {
                errmsg = CommentStyle.ThicknessStringErrorMessage;
            }
            else
            if (propertyName == nameof(CommentWnd_BackColor))
            {
                errmsg = CommentWnd.BackColor.ColorStringErrorMessage;
            }
            if (errmsg != null)
            {
                ret = new List<string> { errmsg };
            }

            return ret;
        }


        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;


        public bool HasErrors
        {
            get
            {
                //　TODO　YouTubeのエラーチェック
                return CommentStyle.IsError() || CommentWnd.IsError();
            }
        }
        #endregion

        #region INotifyPropertyChanged実装
        /// <summary>
        /// プロパティ変更イベントハンドラ
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged_CommentStyle(object sender, PropertyChangedEventArgs arg)
        {
            if (arg == null)
            {
                throw new ArgumentException("proarg is null");
            }

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CommentStyle) + "_" + arg.PropertyName));
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(CommentStyle) + "_" + arg.PropertyName));

        }
        private void OnPropertyChanged_CommentWnd(object sender, PropertyChangedEventArgs arg)
        {
            if (arg == null)
            {
                throw new ArgumentException("proarg is null");
            }

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CommentWnd) + "_" + arg.PropertyName));
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(CommentWnd) + "_" + arg.PropertyName));

        }
        private void OnPropertyChanged_YouTubeConnect(object sender, PropertyChangedEventArgs arg)
        {
            if (arg == null)
            {
                throw new ArgumentException("proarg is null");
            }

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(YouTubeConnect) + "_" + arg.PropertyName));
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(YouTubeConnect) + "_" + arg.PropertyName));

        }

        private void OnPropertyChanged_CommnetMonitor(object sender, PropertyChangedEventArgs arg)
        {
            if (arg == null)
            {
                throw new ArgumentException("proarg is null");
            }

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CommnetMonitor) + "_" + arg.PropertyName));
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(CommnetMonitor) + "_" + arg.PropertyName));

        }

        #endregion


        #region _CommentStyleプロパティ
        /// <summary>
        /// フォント名
        /// </summary>
        public string CommentStyle_FamilyString
        {
            get
            {
                return CommentStyle.FamilyString;
            }
            set
            {
                CommentStyle.FamilyString = value;

            }
        }


        /// <summary>
        /// フォントサイズ
        /// </summary>
        public string CommentStyle_SizeString
        {
            get
            {
                return CommentStyle.SizeString;
            }
            set
            {
                CommentStyle.SizeString = value;

            }
        }


        /// <summary>
        /// フォントイタリックフラグ
        /// </summary>
        public bool CommentStyle_Italic
        {
            get
            {
                return CommentStyle.Italic;
            }
            set
            {
                CommentStyle.Italic = value;

            }
        }

        /// <summary>
        /// フォントボールド設定
        /// </summary>
        public bool CommentStyle_Bald
        {
            get
            {
                return CommentStyle.Bald;
            }
            set
            {
                CommentStyle.Bald = value;
            }
        }

        /// <summary>
        /// 文字の色
        /// </summary>
        public string CommentStyle_ColorString
        {
            get
            {
                return CommentStyle.ColorString;
            }
            set
            {
                CommentStyle.ColorString = value;
            }
        }

        /// <summary>
        /// 文字の縁の色
        /// </summary>
        public string CommentStyle_ThicknessColorString
        {
            get
            {
                return CommentStyle.ThicknessColorString;
            }
            set
            {
                CommentStyle.ThicknessColorString = value;
            }
        }

        /// <summary>
        /// 文字の縁のピクセル数
        /// </summary>
        public string CommentStyle_ThicknessString
        {
            get
            {
                return CommentStyle.ThicknessString;
            }
            set
            {
                CommentStyle.ThicknessString = value;
            }
        }

        /// <summary>
        /// コメントの画面滞在時間
        /// </summary>
        public string CommentStyle_CommentTimeString
        {
            get
            {
                return CommentStyle.CommentTimeString;
            }
            set
            {
                CommentStyle.CommentTimeString = value;
            }
        }
        #endregion


        #region _CommentWndプロパティ

        /// <summary>
        /// Window背景の透明化フラグ（True＝透明）
        /// </summary>
        public bool CommentWnd_Stealth
        {
            get
            {
                return CommentWnd.Stealth;
            }
            set
            {
                CommentWnd.Stealth = value;
            }
        } 

        /// <summary>
        /// Windowsの非表示化フラグ　（True＝非表示）
        /// </summary>
        public bool CommentWnd_Visible
        {
            get
            {
                return CommentWnd.Visible;
            }
            set
            {

                CommentWnd.Visible = value;
            }
        }

        /// <summary>
        /// WIndowのTOPMost（強制最前面）設定
        /// </summary>
        public bool CommentWnd_TopMost
        {
            get
            {
                return CommentWnd.TopMost;
            }
            set
            {

                CommentWnd.TopMost = value;
            }
        }

        /// <summary>
        /// Window背景食の設定
        /// </summary>
        public string CommentWnd_BackColor
        {
            get
            {
                return CommentWnd.BackColor.ValueString;
            }
            set
            {
                CommentWnd.BackColor =  new ColorString(value);
            }
        }

        /// <summary>
        /// コメントウィンドウの位置とサイズ
        /// </summary>
        public WindowsPositionEntiy CommentWnd_Position
        {
            get
            {
                return CommentWnd.Position;
            }
            set
            {
                CommentWnd.Position = value;
            }
        }



        ///// <summary>
        ///// コメントウィンドウの状態
        ///// </summary>
        //private WindowState _CommentWndStste = WindowState.Normal;
        //public WindowState CommentWndStste
        //{
        //    get
        //    {
        //        return _CommentWndStste;
        //    }
        //    set
        //    {
        //        PropertyChangedIfSet(ref _CommentWndStste, value);
        //    }
        //}
        #endregion


        #region YouTubeConnect プロパティ
        /// <summary>
        /// YouTubeAPIKey
        /// </summary>
        public string YouTubeConnect_ApiKey
        {
            get
            {
                return YouTubeConnect.ApiKey;
            }
            set
            {
                YouTubeConnect.ApiKey = value;
            }
        }

        /// <summary>
        /// YouTubeのVideoID
        /// </summary>
        public string YouTubeConnect_VideoID
        {
            get
            {
                return YouTubeConnect.VideoID;
            }
            set
            {
                YouTubeConnect.VideoID = value;
            }
        }

        #endregion

        public bool CommnetMonitor_IsTaskRunning
        {
            get
            {
                return CommnetMonitor.IsRunning;
            }
            private set
            {
                // 処理無し
            }
        }


        DelegateCommand _TestCommentPushCommand = null;

        public ICommand TestCommentPushCommand
        {
            get
            {
                if( _TestCommentPushCommand == null)
                {
                    _TestCommentPushCommand = new DelegateCommand(TestCommentPush, null);
                }
                return _TestCommentPushCommand;
            }
           
        } 

        private void TestCommentPush(object comment)
        {
            string commentStr = comment as string;

            CommentQueue.PushText(new CommentTextEntiy(commentStr, CommentStyle));

        }

        private void StartYouTube()
        {
            if( CommnetMonitor.IsRunning == false)
            {
                YoutubeCommentPollingService monitor = new YoutubeCommentPollingService(YouTubeConnect, CommentQueue);
                CommnetMonitor.Run(monitor.CommentMonitorTask, null);
            }
}

    }
}
