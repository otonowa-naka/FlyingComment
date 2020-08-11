using FlyingComment.Model;
using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FlyingComment.Service
{
    class YoutubeCommentPollingService: IDisposable
    {
        /// <summary>
        /// ログインスタンス
        /// </summary>
        private static readonly log4net.ILog _logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// 排他用ロックオプジェクト
        /// </summary>
        private object _LockObject = new object();

        /// <summary>
        /// コメント取得スレッド
        /// </summary>
        private Task _Task = null;
        /// <summary>
        /// コメント取得スレッドCancelトークン
        /// </summary>
        private CancellationTokenSource _TokenSource = null;

        private YoutubeConnectEntiy _ConnectParameter = null;
        private CommentQueueEntity _CommentQueue = null;

        public YoutubeCommentPollingService(YoutubeConnectEntiy connect, CommentQueueEntity CommentQueue)
        {
            _ConnectParameter = connect;
            _CommentQueue = CommentQueue;
        }

        /// <summary>
        /// デストラクタ
        /// </summary>
        ~YoutubeCommentPollingService()
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
                    if (string.IsNullOrEmpty(_ConnectParameter.VideoID) == false)
                    {

                        string VideoId = _ConnectParameter.VideoID;
                        _TokenSource = new CancellationTokenSource();

                        //スレッド内で通信を実行
                        _Task = Task.Run(() =>
                        {
                            CommentMonitorTask(VideoId);

                            // スレッドが終了したことを伝えるために
                      //      NotifyPropertyChanged(nameof(IsTaskRunning));
                        }
                        );

                        //スレッドが終了するので変更通知を実行
                       // NotifyPropertyChanged(nameof(IsTaskRunning));

                    }
                }
                else
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
                _logger.Info($"Youtubes監視スレッド実行開始 API={_ConnectParameter.ApiKey}, Video={videoId}");

                using (YouTubeService Youtube = new YouTubeService(new BaseClientService.Initializer() { ApiKey = _ConnectParameter.ApiKey }))
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
                                _CommentQueue.PushText(liveChat.Snippet.DisplayMessage);
                            }

                        }
                        else
                        {
                            fastflg = true;
                        }

                        System.Threading.Thread.Sleep((int)liveChatResponse.PollingIntervalMillis);
                        nextPageToken = liveChatResponse.NextPageToken;
                    }
                }
            }
            catch (OperationCanceledException /*ex*/)
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
                ret = videoID.LiveStreamingDetails.ActiveLiveChatId;
            }
            //動画情報取得できない場合はnullを返す
            return ret;
        }
    }
}
