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
    class YoutubeCommentPollingService
    {
        /// <summary>
        /// ログインスタンス
        /// </summary>
        private static readonly log4net.ILog _logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

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
        /// Youtubeのチャット監視スレッドメイン処理
        /// </summary>
        /// <param name="videoId"></param>
        public Action CommentMonitorTask(object aArg, CancellationToken token)
        {
            try
            {

                string videoId = _ConnectParameter.VideoID;

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
                                // TODO　フォントの設定が必要
                                _CommentQueue.PushText(new CommentTextEntiy( liveChat.Snippet.DisplayMessage, new CommentStyleEntity()));
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

            return null;

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
