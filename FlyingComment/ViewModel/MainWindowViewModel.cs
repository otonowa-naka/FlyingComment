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

namespace FlyingComment.ViewModel
{


    public class MainWindowViewModel : INotifyPropertyChanged, IDisposable, INotifyDataErrorInfo
    {
        private CommentStyleEntity _CommentStyle = null;
        private CommentWindowConfigurationEntity _CommentWnd = null;
        private YoutubeConnectEntiy _YouTubeConnect = null; 
                  

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public MainWindowViewModel(CommentStyleEntity style, CommentWindowConfigurationEntity commentWnd, YoutubeConnectEntiy youTubeConnect)
        {
            // APP設定でデータの初期化
            try
            {
                _CommentStyle = style;
                _CommentWnd = commentWnd;
                _YouTubeConnect = youTubeConnect;


                _CommentStyle.PropertyChanged += OnPropertyChanged_CommentStyle;
                _CommentWnd.PropertyChanged += OnPropertyChanged_CommentWnd;

                CommentWnd_WindowRect = Properties.Settings.Default.CommentWndRect;
                SettingWndRect = Properties.Settings.Default.SettingWndRect;
                CommentWndStste = Properties.Settings.Default.CommentWinState;

                CommentWnd_TopMost = Properties.Settings.Default.Topmost;
                CommentWnd_BackColor = Properties.Settings.Default.BackColor;


                YouTubeAPIKey = Properties.Settings.Default.APIKey;
                YouTubeVideoID = Properties.Settings.Default.VideoID;


                _logger.Info("設定読み込み終了");
            }
            catch (Exception ex)
            {
                _logger.Error($"設定読み込み異常 例外メッセージ={ex.Message}");

            }
        }



        /// <summary>
        /// デストラクタ
        /// </summary>
        ~MainWindowViewModel()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                ClearTask();
            }

        }

        public IEnumerable GetErrors(string propertyName)
        {
            IEnumerable ret = null;
            string errmsg = null;

            if (propertyName == nameof(CommentStyle_FamilyString))
            {
                errmsg = _CommentStyle.FamilyStringErrorMessage;
            }
            else
            if (propertyName == nameof(CommentStyle_SizeString))
            {
                errmsg = _CommentStyle.SizeStringErrorMessage;
            }
            else
            if (propertyName == nameof(CommentStyle_ColorString))
            {
                errmsg = _CommentStyle.ColorStringErrorMessage;
            }
            else
            if (propertyName == nameof(CommentStyle_ThicknessColorString))
            {
                errmsg = _CommentStyle.ThicknessColorStringErrorMessage;
            }
            else
            if (propertyName == nameof(CommentStyle_ThicknessString))
            {
                errmsg = _CommentStyle.ThicknessStringErrorMessage;
            }
            else
            if (propertyName == nameof(CommentWnd_BackColor))
            {
                errmsg = _CommentWnd.BackColor.ColorStringErrorMessage;
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
                return _CommentStyle.IsError() || _CommentWnd.IsError();
            }
        }



        private void OnPropertyChanged_CommentStyle(object sender, PropertyChangedEventArgs arg)
        {
            if (arg == null)
            {
                throw new ArgumentException("proarg is null");
            }

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("CommentStyle_" + arg.PropertyName));
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs("CommentStyle_" + arg.PropertyName));

        }
        private void OnPropertyChanged_CommentWnd(object sender, PropertyChangedEventArgs arg)
        {
            if (arg == null)
            {
                throw new ArgumentException("proarg is null");
            }

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("CommentWnd_" + arg.PropertyName));
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs("CommentWnd_" + arg.PropertyName));

        }


        /// <summary>
        /// フォント名
        /// </summary>
        public string CommentStyle_FamilyString
        {
            get
            {
                return _CommentStyle.FamilyString;
            }
            set
            {
                _CommentStyle.FamilyString = value;

            }
        }


        /// <summary>
        /// フォントサイズ
        /// </summary>
        public string CommentStyle_SizeString
        {
            get
            {
                return _CommentStyle.SizeString;
            }
            set
            {
                _CommentStyle.SizeString = value;

            }
        }


        /// <summary>
        /// フォントイタリックフラグ
        /// </summary>
        public bool CommentStyle_Italic
        {
            get
            {
                return _CommentStyle.Italic;
            }
            set
            {
                _CommentStyle.Italic = value;

            }
        }

        /// <summary>
        /// フォントボールド設定
        /// </summary>
        public bool CommentStyle_Bald
        {
            get
            {
                return _CommentStyle.Bald;
            }
            set
            {
                _CommentStyle.Bald = value;
            }
        }

        /// <summary>
        /// 文字の色
        /// </summary>
        public string CommentStyle_ColorString
        {
            get
            {
                return _CommentStyle.ColorString;
            }
            set
            {
                _CommentStyle.ColorString = value;
            }
        }

        /// <summary>
        /// 文字の縁の色
        /// </summary>
        public string CommentStyle_ThicknessColorString
        {
            get
            {
                return _CommentStyle.ThicknessColorString;
            }
            set
            {
                _CommentStyle.ThicknessColorString = value;
            }
        }

        /// <summary>
        /// 文字の縁のピクセル数
        /// </summary>
        public string CommentStyle_ThicknessString
        {
            get
            {
                return _CommentStyle.ThicknessString;
            }
            set
            {
                _CommentStyle.ThicknessString = value;
            }
        }

        /// <summary>
        /// コメントの画面滞在時間
        /// </summary>
        public string CommentStyle_CommentTimeString
        {
            get
            {
                return _CommentStyle.CommentTimeString;
            }
            set
            {
                _CommentStyle.CommentTimeString = value;
            }
        }

        /// <summary>
        /// 排他用ロックオプジェクト
        /// </summary>
        private object _LockObject = new object();
        

        /// <summary>
        /// ログインスタンス
        /// </summary>
        private static readonly log4net.ILog _logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        /// <summary>
        /// プロパティ変更イベントハンドラ
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// プロパティの更新通知を発行します。
        /// </summary>
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }



        #region プロパティ
        /// <summary>
        /// コメントのリスト
        /// </summary>
        private List<string> _TextList = new List<string>();
        public int TextListCount
        {
            get
            {
                return _TextList.Count;
            }
            
        }

        /// <summary>
        /// コメントリストに追加
        /// </summary>
        /// <param name="addText"></param>
        public void PushText(string addText)
        {
            lock (_LockObject)
            {
                _TextList.Add(addText);
            }
            NotifyPropertyChanged("TextListCount");
        }

        /// <summary>
        /// コメントリストから１行分のコメントを取得
        /// </summary>
        /// <returns></returns>
        public string PopText( )
        {
            string ret = "";
            lock (_LockObject)
            {
                if (_TextList.Count() > 0)
                {
                    ret = _TextList[0];
                    _TextList.RemoveAt(0);
                    NotifyPropertyChanged("TextListCount");

                }
            }
            return ret;
        }


        /// <summary>
        /// Window背景の透明化フラグ（True＝透明）
        /// </summary>
        public bool CommentWnd_Stealth
        {
            get
            {
                return _CommentWnd.Stealth;
            }
            set
            {
                _CommentWnd.Stealth = value;
            }
        }

        /// <summary>
        /// Windowsの非表示化フラグ　（True＝非表示）
        /// </summary>
        public bool CommentWnd_Visible
        {
            get
            {
                return _CommentWnd.Visible;
            }
            set
            {

                _CommentWnd.Visible = value;
            }
        }

        /// <summary>
        /// WIndowのTOPMost（強制最前面）設定
        /// </summary>
        public bool CommentWnd_TopMost
        {
            get
            {
                return _CommentWnd.TopMost;
            }
            set
            {

                _CommentWnd.TopMost = value;
            }
        }

        /// <summary>
        /// Window背景食の設定
        /// </summary>
        public string CommentWnd_BackColor
        {
            get
            {
                return _CommentWnd.BackColor.ValueString;
            }
            set
            {
                _CommentWnd.BackColor =  new ColorString(value);
            }
        }





        /// <summary>
        /// YouTubeAPIのKey
        /// </summary>
        private string _YouTubeAPIKey;
        public string YouTubeAPIKey
        {
            get
            {
                return _YouTubeAPIKey;
            }
            set
            {
                PropertyChangedIfSet(ref _YouTubeAPIKey, value);
            }
        }

        /// <summary>
        /// YouTubeのVideoKey
        /// </summary>
        private string _YouTubeVideoID;
        public string YouTubeVideoID
        {
            get
            {
                return _YouTubeVideoID;
            }
            set
            {
                PropertyChangedIfSet(ref _YouTubeVideoID, value);
            }
        }

        /// <summary>
        /// コメントウィンドウの位置とサイズ
        /// </summary>
        public Rect CommentWnd_WindowRect
        {
            get
            {
                return _CommentWnd.WindowRect;
            }
            set
            {
                _CommentWnd.WindowRect = value;
            }
        }

        /// <summary>
        /// 設定ウィンドウの位置
        /// </summary>
        private Rect _SettingWndRect;
        public Rect SettingWndRect
        {
            get
            {
                return _SettingWndRect;
            }
            set
            {
                PropertyChangedIfSet(ref _SettingWndRect, value);
            }
        }

        /// <summary>
        /// コメントウィンドウの状態
        /// </summary>
        private WindowState _CommentWndStste = WindowState.Normal;
        public WindowState CommentWndStste
        {
            get
            {
                return _CommentWndStste;
            }
            set
            {
                PropertyChangedIfSet(ref _CommentWndStste, value);
            }
        }

        /// <summary>
        /// コメント取得スレッド
        /// </summary>
        private Task _Task = null;
        /// <summary>
        /// コメント取得スレッドCancelトークン
        /// </summary>
        private CancellationTokenSource _TokenSource = null;

        /// <summary>
        ///　コメント取得スレッド状態取得
        /// </summary>
        public bool IsTaskRunning
        {
            get
            {
                bool ret = false;
                lock (_LockObject)
                {
                    if (_Task != null)
                    {
                        // Completedでなければ実行中状態
                        ret = (_Task.IsCompleted == false);
                    }
                }
                return ret;
            }
        }


        #endregion

        /// <summary>
        /// 前と値が違うなら変更してイベントを発行する
        /// </summary>
        /// <typeparam name="TResult">プロパティの型</typeparam>
        /// <param name="source">元の値</param>
        /// <param name="value">新しい値</param>
        /// <param name="propertyName">プロパティ名/param>
        /// <returns>値の変更有無</returns>
        private bool PropertyChangedIfSet<TResult>(ref TResult source, TResult value, [CallerMemberName]string propertyName = null)
        {
            bool ret = false;
            //値が同じだったら何もしない
            if (EqualityComparer<TResult>.Default.Equals(source, value) == false)
            {
                source = value;
                //イベント発行
                NotifyPropertyChanged(propertyName);
                ret = true;
            }

            return ret;
        }

 
        /// <summary>
        /// YouTube　コメント開始/終了処理
        /// </summary>
        public void RunYouTube()
        {
            try
            {
                //　現行の起動状態を確認
                if (IsTaskRunning == false)
                {
                    //　実行済みの可能性もあるのでTaskをクリアする
                    ClearTask();

                    //　ビデオIDが設定されていることを確認する
                    if (string.IsNullOrEmpty(YouTubeVideoID) == false)
                    {

                        string VideoId = YouTubeVideoID;
                        _TokenSource = new CancellationTokenSource();
                    
                        //スレッド内で通信を実行
                        _Task = Task.Run(() =>
                        {
                            CommentMonitorTask(VideoId);

                            // スレッドが終了したことを伝えるために
                            NotifyPropertyChanged(nameof(IsTaskRunning));
                        }
                        );

                        //スレッドが終了するので変更通知を実行
                        NotifyPropertyChanged(nameof(IsTaskRunning));

                    }
                }else
                {
                    ClearTask();
                }

            }
            catch (Exception ex)
            {
                _logger.Error($"コメント取得実行で例外 例外メッセージ={ex.Message}");

            }
        }



        /// <summary>
        /// タスクを終了させてNULL化
        /// </summary>
        private void ClearTask()
        {
            try
            {
                if (_Task != null)
                {
                    _TokenSource.Cancel();
                    _Task.Wait();

                    _Task.Dispose();
                    _TokenSource.Dispose();

                    _TokenSource = null;
                    _Task = null;
                }
            }
            catch (Exception ex)
            {
                _logger.Error($"タスクキャンセル実行で例外メッセージ={ex.Message}");
                throw;
            }
        }



        
        /// <summary>
        /// Youtubeのチャット監視スレッドメイン処理
        /// </summary>
        /// <param name="videoId"></param>
        private void CommentMonitorTask(string videoId)
        {
            try
            {
                _logger.Info($"Youtubes監視スレッド実行開始 API={YouTubeAPIKey}, Video={videoId}");

                using (YouTubeService Youtube = new YouTubeService(new BaseClientService.Initializer() { ApiKey = YouTubeAPIKey }))
                {
                    string liveChatId = GetliveChatID(videoId, Youtube);

                    _logger.Info($"YouTubeチャットID成功 ChatId={liveChatId}");
                    string nextPageToken = null;

                    LiveChatMessagesResource.ListRequest liveChatRequest = Youtube.LiveChatMessages.List(liveChatId, "snippet,authorDetails");

                    bool fastflg = false;

                    while (true)
                    {
                        liveChatRequest.PageToken = nextPageToken;

                        var task = liveChatRequest.ExecuteAsync();
                        task.Wait(_TokenSource.Token);
                        var liveChatResponse = task.Result;

                        // 途中起動を考慮して初回の読み込みコメントは無視
                        if (fastflg == true)
                        {

                            foreach (var liveChat in liveChatResponse.Items)
                            {
                                _logger.Info($"コメント：{liveChat.Snippet.DisplayMessage},{liveChat.AuthorDetails.DisplayName}");
                                // コメントを出力
                                PushText(liveChat.Snippet.DisplayMessage);
                            }

                        }else
                        {
                            fastflg = true; 
                        }

                        System.Threading.Thread.Sleep((int)liveChatResponse.PollingIntervalMillis);
                        nextPageToken = liveChatResponse.NextPageToken;
                    }
                }
            }catch(OperationCanceledException /*ex*/)
            {
                _logger.Info($"Youtubes監視スレッドで終了要求検知 {0} ");

            }
            catch (Exception ex)
            {
           
                _logger.Error($"Youtubes監視スレッドで例外　{ex.Message}");
            }


        }

        /// <summary>
        /// YouTubeのVideoIDからチャットIDに変換する。
        /// </summary>
        /// <param name="videoId">VideoID文字列</param>
        /// <param name="youtubeService">YouTubeサービスインスタンス</param>
        /// <returns></returns>
        static private string GetliveChatID(string videoId, YouTubeService youtubeService)
        {
            string ret = null;
            //引数で取得したい情報を指定
            var videosList = youtubeService.Videos.List("LiveStreamingDetails");
            videosList.Id = videoId;

            //動画情報の取得
            var videoListResponse = videosList.Execute();
            //LiveChatIDを返す
            foreach (var videoID in videoListResponse.Items)
            {
                ret=  videoID.LiveStreamingDetails.ActiveLiveChatId;
            }
            //動画情報取得できない場合はnullを返す
            return ret;
        }

    }
}
