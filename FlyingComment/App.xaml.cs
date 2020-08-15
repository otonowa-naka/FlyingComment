using FlyingComment.Model;
using FlyingComment.Repository;
using FlyingComment.ViewModel;
using System;
using System.Windows;

namespace FlyingComment
{
    /// <summary>
    /// App.xaml の相互作用ロジック
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// ログインスタンス
        /// </summary>
        private static readonly log4net.ILog _logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public CommentStyleEntity CommentStyle
            { 
            get;
            private set;
            } = null;


        public CommentWindowConfigurationEntity CommentWindowConfiguration
        {
            get;
            private set;
        }
        = null;

        public CommentQueueEntity CommentQueue
        {
            get;
            private set;
        } = null;

        public WindowsPositionEntiy SettingWindowPosition
        {
            get;
            private set;
        } = null;

        public DelegateTaskEntity CommentMonitorTask
        {
            get;
            private set;
        } = null;
        public YoutubeConnectEntiy YouTubeConnect
        {
            get;
            private set;
        } = null;
        /// <summary>
        /// メイン関数
        /// </summary>
        [STAThread]
        public static void Main()
        {
            try
            {
                App app = new App();

                app.CommentStyle = PropertyXMLRepository.LoadCommentStyleEntity();
                app.CommentWindowConfiguration = PropertyXMLRepository.LoadCommentWindowConfigurationEntity();
                app.SettingWindowPosition = PropertyXMLRepository.LoadSettingWindowPositionEntiy();
                app.CommentQueue = new CommentQueueEntity();
                app.CommentMonitorTask = new DelegateTaskEntity();
                app.YouTubeConnect = PropertyXMLRepository.LoadYoutubeConnectEntiy();
               

                app.InitializeComponent();
                app.CreateFlyingCommentWindow();
                app.Run();

                PropertyXMLRepository.Save(app.CommentStyle, app.CommentWindowConfiguration, app.YouTubeConnect, app.SettingWindowPosition);

                _logger.Info("アプリケーション終了");

            }catch (Exception ex)
            {
                _logger.Error($"不明のエラーが発生しました。　ex ={ ex.Message }");
            }

        }

        /// <summary>
        /// 
        /// </summary>
        private static FlyingCommentsWindow FlyingCommentWindow = null;

        /// <summary>
        /// 
        /// </summary>
        public void CreateFlyingCommentWindow()
        {
            if(FlyingCommentWindow != null)
            {
                CloseFlyingCommentWindow();
            }


            FlyingCommentWindow = new FlyingCommentsWindow(new FlyingCommentsViewModel(CommentWindowConfiguration, CommentQueue));
            FlyingCommentWindow.Show();
        }

        public void CloseFlyingCommentWindow()
        {
            FlyingCommentWindow.ManualColse();
            FlyingCommentWindow = null;
        }

    }
}
