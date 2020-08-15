using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Data;
using System.Windows.Markup;
using System.Linq;
using System.Globalization;
using FlyingComment.ViewModel;

namespace FlyingComment
{

    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();

            App ap= Application.Current as App;
            MainWindowViewModel Viewmodel =  new MainWindowViewModel(ap.CommentStyle, ap.CommentWindowConfiguration, ap.SettingWindowPosition, ap.CommentQueue, ap.CommentMonitorTask, ap.YouTubeConnect);
            DataContext = Viewmodel;

            //　ウインドウの位置とサイズを指定
            Viewmodel.SettingWindowPosition.MoveWindow(this);
        
            // フォント選択コンボBoxにシステムのフォント一覧を設定
            m_FontName.ItemsSource = Fonts.SystemFontFamilies;
            
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            MainWindowViewModel model = DataContext as MainWindowViewModel;

            if (model != null)
            {
                //　現在のウインドウ位置とサイズを保存
                model.SettingWindowPosition = new Model.WindowsPositionEntiy(this);
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Application.Current.Shutdown();
        }


        /// <summary>
        /// YouTubeを監視を実行
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RunYouTbbe_Click(object sender, RoutedEventArgs e)
        {

            MainWindowViewModel model = DataContext as MainWindowViewModel;

            //model?.RunYouTube();
        }


        /// <summary>
        /// テストコメントを送信
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void m_TestSendButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindowViewModel model = DataContext as MainWindowViewModel;
          //  model.PushText(m_TestComment.Text);
        }
        



    }

    /// <summary>
    /// フォント名を日本語変換コンバータ
    /// </summary>
    public class FontFamilyToNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            FontFamily fontlist = value as FontFamily;
            XmlLanguage currentLang = XmlLanguage.GetLanguage(CultureInfo.CurrentCulture.IetfLanguageTag);
            return fontlist.FamilyNames.FirstOrDefault(o => o.Key == currentLang).Value ?? fontlist.Source;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }


    /// <summary>
    /// YouTube実行状態からボタンテキスト変化コンバーター
    /// </summary>
    public class YouTubeRunButtonConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string ret;
            bool? running = value as bool?;

            if (running != null && running == true)
            {
                ret = "Youtubeコメント監視を止める";
            }
            else
            {
                ret = "Youtubeコメント監視を始める";
            }
            return ret;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
