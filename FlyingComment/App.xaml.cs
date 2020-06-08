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


      
        public AppModel Model { get; set; } = new AppModel();
  

        /// <summary>
        /// メイン関数
        /// </summary>
        [STAThread]
        public static void Main()
        {
            try
            {
                App app = new App();
                app.InitializeComponent();
                app.Run();

                app.Model.Save();

                _logger.Info("アプリケーション終了");

            }catch (Exception ex)
            {
                _logger.Error($"不明のエラーが発生しました。　ex ={ ex.Message }");
            }

        }
    }
}
