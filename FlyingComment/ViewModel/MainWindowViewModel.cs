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

namespace FlyingComment.ViewModel
{


    public class MainWindowViewModel : INotifyPropertyChanged, IDisposable, INotifyDataErrorInfo
    {
        private FontSettingEntity _FontSetting = new FontSettingEntity();
        private CommentWindowsEntity _CommentWnd = new CommentWindowsEntity();


        public IEnumerable GetErrors(string propertyName)
        {
            IEnumerable ret = null;
            string errmsg = null;

            if (propertyName == nameof(FamilyString))
            {
                errmsg = _FontSetting.FamilyStringErrorMessage;
            }else
            if (propertyName == nameof(SizeString))
            {
                errmsg = _FontSetting.SizeStringErrorMessage;
            }
            else
            if (propertyName == nameof(ColorString))
            {
                errmsg = _FontSetting.ColorStringErrorMessage;
            }
            else
            if (propertyName == nameof(ThicknessColorString))
            {
                errmsg = _FontSetting.ThicknessColorStringErrorMessage;
            }
            else
            if (propertyName == nameof(ThicknessString))
            {
                errmsg = _FontSetting.ThicknessStringErrorMessage;
            }
            else
            if (propertyName == nameof(BackColor))
            {
                errmsg = _CommentWnd.BackColor.ColorStringErrorMessage ;
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
                return _FontSetting.IsError() || _CommentWnd.IsError();
            }
        }


        

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public MainWindowViewModel()
        {
    


            // APP設定でデータの初期化
            try
            {
                _FontSetting = PropertyXMLRepository.LoadFontSettingEntity();
                _FontSetting.PropertyChanged += OnPropertyChanged;
                _CommentWnd.PropertyChanged += OnPropertyChanged;



                CommentWndRect = Properties.Settings.Default.CommentWndRect;
                SettingWndRect = Properties.Settings.Default.SettingWndRect;
                CommentWndStste = Properties.Settings.Default.CommentWinState;

                TopMost = Properties.Settings.Default.Topmost;
                BackColor = Properties.Settings.Default.BackColor;


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

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs arg)
        {
            if (arg == null)
            {
                throw new ArgumentException("proarg is null");
            }

            PropertyChanged?.Invoke(this, arg);
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(arg.PropertyName));

            //if(nameof(_CommentWnd.ShowFlg) == arg.PropertyName)
            //{
            //    IWindowService winService = new Service.FlyingWindowsService();
            //    if(_CommentWnd.ShowFlg == true)
            //    {
            //        winService.CreateWindow(new FlyingCommentsViewModel());
            //    }else
            //    {
            //        winService.CloseWindow();

            //    }

            //}
        }



        /// <summary>
        /// フォント名
        /// </summary>
        public string FamilyString
        {
            get
            {
                return _FontSetting.FamilyString;
            }
            set
            {
                _FontSetting.FamilyString = value;

            }
        }


        /// <summary>
        /// フォントサイズ
        /// </summary>
        public string SizeString
        {
            get
            {
                return _FontSetting.SizeString;
            }
            set
            {
                _FontSetting.SizeString = value;

            }
        }


        /// <summary>
        /// フォントイタリックフラグ
        /// </summary>
        public bool Italic
        {
            get
            {
                return _FontSetting.Italic;
            }
            set
            {
                _FontSetting.Italic = value;

            }
        }

        /// <summary>
        /// フォントボールド設定
        /// </summary>
        public bool Bald
        {
            get
            {
                return _FontSetting.Bald;
            }
            set
            {
                _FontSetting.Bald = value;
            }
        }

        /// <summary>
        /// 文字の色
        /// </summary>
        public string ColorString
        {
            get
            {
                return _FontSetting.ColorString;
            }
            set
            {
                _FontSetting.ColorString = value;
            }
        }

        /// <summary>
        /// 文字の縁の色
        /// </summary>
        public string ThicknessColorString
        {
            get
            {
                return _FontSetting.ThicknessColorString;
            }
            set
            {
                _FontSetting.ThicknessColorString = value;
            }
        }

        /// <summary>
        /// 文字の縁のピクセル数
        /// </summary>
        public string ThicknessString
        {
            get
            {
                return _FontSetting.ThicknessString;
            }
            set
            {
                _FontSetting.ThicknessString = value;
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
        public bool Stealth
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
        public bool Visible
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
        public bool TopMost
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
        public string BackColor
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
                PropertyXMLRepository.SaveFontSettingEntity(_FontSetting);


                Properties.Settings.Default.CommentTime = CommentTime;
                Properties.Settings.Default.APIKey = YouTubeAPIKey;
                Properties.Settings.Default.VideoID = YouTubeVideoID;
                Properties.Settings.Default.CommentWndRect = CommentWndRect;
                Properties.Settings.Default.SettingWndRect = SettingWndRect;
                Properties.Settings.Default.CommentWinState = CommentWndStste;
                Properties.Settings.Default.Topmost = TopMost;
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
