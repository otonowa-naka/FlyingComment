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

namespace FlyingComment
{


    public class AppModel : INotifyPropertyChanged, IDisposable
    {

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public AppModel()
        {

            // APP設定でデータの初期化
            try
            {
                FontName = Properties.Settings.Default.FontName;
                FontSize = Properties.Settings.Default.FontSize;
                FontItalic = Properties.Settings.Default.FontItalic;
                FontBald = Properties.Settings.Default.FontBald;
                FontColor = Properties.Settings.Default.FontColor;
                FontThicknessColor = Properties.Settings.Default.FontThicknessColor;
                FontThickness = Properties.Settings.Default.FontThickness;
                CommentTime = Properties.Settings.Default.CommentTime;
                YouTubeAPIKey = Properties.Settings.Default.APIKey;
                YouTubeVideoID = Properties.Settings.Default.VideoID;
                CommentWndRect = Properties.Settings.Default.CommentWndRect;
                SettingWndRect = Properties.Settings.Default.SettingWndRect;
                CommentWndStste = Properties.Settings.Default.CommentWinState;
                Topmost = Properties.Settings.Default.Topmost;
                BackColor = Properties.Settings.Default.BackColor;

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
        ~AppModel()
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
        private bool _Stealth = false;
        public bool Stealth
        {
            get
            {
                return _Stealth;
            }
            set
            {
                PropertyChangedIfSet(ref _Stealth, value);
            }
        }

        /// <summary>
        /// Windowsの非表示化フラグ　（True＝非表示）
        /// </summary>
        private bool _Visible = true;
        public bool Visible
        {
            get
            {
                return _Visible;
            }
            set
            {

                PropertyChangedIfSet(ref _Visible, value);
                NotifyPropertyChanged("WindowVisible");
            }
        }

        /// <summary>
        /// WIndowのTOPMost（強制最前面）設定
        /// </summary>
        private bool _Topmost = true;
        public bool Topmost
        {
            get
            {
                return _Topmost;
            }
            set
            {

                PropertyChangedIfSet(ref _Topmost, value);
            }
        }

        /// <summary>
        /// Window背景食の設定
        /// </summary>
        private string _BackColor;
        public string BackColor
        {
            get
            {
                string ret = "";
                if(Stealth == false)
                {
                    ret = _BackColor;
                }
                return ret;
            }
            set
            {
                PropertyChangedIfSet(ref _BackColor, value);
            }
        }

        /// <summary>
        /// フォント名
        /// </summary>
        private string _FontName = "";
        public string FontName {
            get
            {
                return _FontName;
            }
            set
            {
                PropertyChangedIfSet(ref _FontName, value);
            }
        }

        /// <summary>
        /// フォントサイズ
        /// </summary>
        private string _FontSzie = "";
        public string FontSize
        {
            get
            {
                return _FontSzie;
            }
            set
            {
                PropertyChangedIfSet(ref _FontSzie, value);
            }
        }


        /// <summary>
        /// フォントイタリックフラグ
        /// </summary>
        private bool _FontItalic = false;
        public bool FontItalic
        {
            get
            {
                return _FontItalic;
            }
            set
            {
                PropertyChangedIfSet(ref _FontItalic, value);
            }
        }

        /// <summary>
        /// フォントボールド設定
        /// </summary>
        private bool _FontBald;
        public bool FontBald
        {
            get
            {
                return _FontBald;
            }
            set
            {
                PropertyChangedIfSet(ref _FontBald, value);
            }
        }

        /// <summary>
        /// 文字の色
        /// </summary>
        private string _FontColor;
        public string FontColor
        {
            get
            {
                return _FontColor;
            }
            set
            {
                PropertyChangedIfSet(ref _FontColor, value);
            }
        }

        /// <summary>
        /// 文字の縁の色
        /// </summary>
        private string _FontThicknessColor;
        public string FontThicknessColor
        {
            get
            {
                return _FontThicknessColor;
            }
            set
            {
                PropertyChangedIfSet(ref _FontThicknessColor, value);
            }
        }

        /// <summary>
        /// 文字の縁のピクセル数
        /// </summary>
        private string _FontThickness;
        public string FontThickness
        {
            get
            {
                return _FontThickness;
            }
            set
            {
                PropertyChangedIfSet(ref _FontThickness, value);
            }
        }

        /// <summary>
        /// コメントの画面滞在時間
        /// </summary>
        private string _CommentTime;
        public string CommentTime
        {
            get
            {
                return _CommentTime;
            }
            set
            {
                PropertyChangedIfSet(ref _CommentTime, value);
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
        private Rect _CommentWndRect;
        public Rect CommentWndRect
        {
            get
            {
                return _CommentWndRect;
            }
            set
            {
                PropertyChangedIfSet(ref _CommentWndRect, value);
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
        /// 設定内容の保存処理
        /// </summary>
        public void Save()
        {
            // APP設定でデータの初期化
            try
            {
                Properties.Settings.Default.FontName = FontName;
                Properties.Settings.Default.FontSize = FontSize;
                Properties.Settings.Default.FontItalic = FontItalic;
                Properties.Settings.Default.FontBald = FontBald;
                Properties.Settings.Default.FontColor = FontColor;
                Properties.Settings.Default.FontThicknessColor = FontThicknessColor;
                Properties.Settings.Default.FontThickness = FontThickness;
                Properties.Settings.Default.CommentTime = CommentTime;
                Properties.Settings.Default.APIKey = YouTubeAPIKey;
                Properties.Settings.Default.VideoID = YouTubeVideoID;
                Properties.Settings.Default.CommentWndRect = CommentWndRect;
                Properties.Settings.Default.SettingWndRect = SettingWndRect;
                Properties.Settings.Default.CommentWinState = CommentWndStste;
                Properties.Settings.Default.Topmost = Topmost;
                Properties.Settings.Default.BackColor = BackColor;

                //　プロパティの保存
                Properties.Settings.Default.Save();
                _logger.Info("設定書き込み終了");
            }
            catch (Exception ex)
            {
                _logger.Error($"設定書き込み異常 例外メッセージ={ex.Message}");

            }
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
            }catch(OperationCanceledException ex)
            {
                _logger.Info($"Youtubes監視スレッドで終了要求検知");

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
