using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Animation;
using OutlineText;
using FlyingComment.ViewModel;
using FlyingComment.Model;

namespace FlyingComment
{
    /// <summary>
    /// FlyingCommentsWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class FlyingCommentsWindow : Window
    {

        /// <summary>
        /// ログインスタンス
        /// </summary>
        private static readonly log4net.ILog _logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="model"></param>
        public FlyingCommentsWindow(FlyingCommentsViewModel model)
        {
            InitializeComponent();

            ///　Model設定
            DataContext = model;



            //　ウインドウの位置とサイズを設定
            model.CommentWnd.Position.MoveWindow(this);
            
            ///　透明化の初期状態を設定
            if (model.CommentWnd.Stealth == true)
            {
                WindowStyle = WindowStyle.None;
                AllowsTransparency = true;
                Background = new SolidColorBrush(Colors.Transparent);   //　透明を設定
            }
            else
            {
                WindowStyle = WindowStyle.SingleBorderWindow;
                AllowsTransparency = false;
                
            }

            // バイディングを設定するとChangeイベントが実行される。
            // イベント処理の中でWindowStyleの設定値と比較処理をしているので、
            // WindowStyle後にしないと無限ループになる。
            // コメントリストの変更通知を受け取るためにバインディングを設定
            Binding CommentQueue_CountBinding = new Binding("CommentQueue_Count");
            CommentQueue_CountBinding.Mode = BindingMode.OneWay;
            this.SetBinding(CommentQueue_Count, CommentQueue_CountBinding);

            ///　透明化の変更通知を受け取るためにバインディングを設定
            Binding StealthBinding = new Binding("CommentWnd_Stealth");
            StealthBinding.Mode = BindingMode.OneWay;
            this.SetBinding(Stealth, StealthBinding);
        }


        /// <summary>
        /// 飛ばすココメントリストの数が変わったことを受け取る為のプロパティを定義
        /// </summary>
        public static readonly DependencyProperty CommentQueue_Count =
            DependencyProperty.Register(
            "CommentQueue_Count",                     // プロパティ名
            typeof(CommentTextEntiy),                         //　プロパティの型情報
            typeof(FlyingCommentsWindow),
                new PropertyMetadata(
                new CommentTextEntiy("", new CommentStyleEntity()),                               // デフォルト値の設定
                CommentQueue_CountPropertyChanged)    // 変更のイベントハンドラ定義
            );

        /// <summary>
        /// CommentQueue_Countの変更イベントハンドラ
        /// </summary>
        /// <param name="dobj"></param>
        /// <param name="eArgs"></param>
        private static void CommentQueue_CountPropertyChanged(DependencyObject dobj, DependencyPropertyChangedEventArgs eArgs)
        {
            try
            {
                FlyingCommentsWindow wnd = dobj as FlyingCommentsWindow;
               
                //　コメントを飛ばす処理を実行
                wnd?.FlyingComment();
            }
            catch (Exception ex)
            {

                _logger.Error($"ウインドウの透明化フラグの変更イベントハンドラで例外　{ex.Message}");
            }

        }

        /// <summary>
        /// ウインドウの透明化フラグが変わったことを受け取る為のプロパティを定義
        /// </summary>
        public static readonly DependencyProperty Stealth =
            DependencyProperty.Register(
            "CommentWnd_Stealth",                // プロパティ名
            typeof(bool),                        //　プロパティの型情報
            typeof(FlyingCommentsWindow),
                new PropertyMetadata(
                false,                           // デフォルト値の設定
                StealthPropertyChanged)    　    // 変更のイベントハンドラ定義
            );

        /// <summary>
        /// ウインドウの透明化フラグの変更イベントハンドラ
        /// </summary>
        /// <param name="dobj"></param>
        /// <param name="eArgs"></param>
        private static void StealthPropertyChanged(DependencyObject dobj, DependencyPropertyChangedEventArgs eArgs)
        {
            try
            {
                FlyingCommentsWindow wnd = dobj as FlyingCommentsWindow;

                //　コメントウィンドウの再作成
                wnd.RenewWindow();
            }
            catch (Exception ex)
            {

                _logger.Error($"ウインドウの透明化フラグの変更イベントハンドラで例外　{ex.Message}");
            }
        }
        /// <summary>
        /// 透明化の為にウインドウの再作成
        /// ※　WindowStyleの変更はShowをする前にしか変更できないため
        /// </summary>
        private void RenewWindow()
        {

            FlyingCommentsViewModel model = DataContext as FlyingCommentsViewModel;
            if (model != null)
            {
                //　現在の状態と透明化フラグの状態に差異があるか確認
                if (
                    (model.CommentWnd.Stealth == true && WindowStyle == WindowStyle.None) ||
                    (model.CommentWnd.Stealth == false && WindowStyle == WindowStyle.SingleBorderWindow)

                    )
                {

                    // 差異が無い場合は、何もしない
                }
                else
                {

                    // 透明化の状態と現在のWindowの状態が異なる時

                    //　現在の位置とサイズをモデルに保存
                    model.CommentWnd.Position = new Model.WindowsPositionEntiy(this);

                    App ap = Application.Current as App;
                    ap.CreateFlyingCommentWindow();                   
                }

            }
        }

        /// <summary>
        /// コメント登場位置生成乱数
        /// </summary>
        private Random _rnd = new System.Random();

        /// <summary>
        /// 最後の行数
        /// </summary>
        private long _LastLine = -1;

        /// <summary>
        /// コメント表示
        /// </summary>
        private void FlyingComment()
        {
            FlyingCommentsViewModel model = DataContext as FlyingCommentsViewModel;

            if (model != null)
            {
                CommentTextEntiy comment = model.PopComment;
                while(comment != null)
                {
                    //　コメントのテキストコントロール
                    OutlineTextControl CommentBlock = new OutlineTextControl();
                    
                    // 表示文字設定
                    CommentBlock.Text = comment.Comment;
                    // フォント設定
                    CommentBlock.Font = comment.Style.Family;
                    // ボールド設定
                    CommentBlock.Bold = comment.Style.Bald;
                    //　イタリック設定
                    CommentBlock.Italic = comment.Style.Italic;

                    //　フォントサイズ設定
                    CommentBlock.FontSize = comment.Style.Size;

                    //　文字色設定
                    CommentBlock.Fill = new SolidColorBrush(comment.Style.Color);

                    // テキストの枠線の色
                    CommentBlock.Stroke = new SolidColorBrush(comment.Style.ThicknessColor);

                    // テキストの枠の太さ
                    CommentBlock.StrokeThickness = comment.Style.Thickness;


                    try
                    {
                        //　位置設定Y軸
                        //　全体のサイズ／コメントの縦幅で、画面の行数を計算
                        long linecount = (long)(m_Canvas.ActualHeight / CommentBlock.FormattedTextHeight);


                        long pos = 0;
                        if ( linecount >= 2)
                        {
                            // 前回の行数と異なる場所になるようにする。
                            pos = _LastLine;
                            while (_LastLine == pos)
                            {
                                pos = _rnd.Next(0, (int)linecount);
                            }
                            _LastLine = pos;

                        }
                        double ypos = pos * CommentBlock.FormattedTextHeight;

                        //CommentBlock.SetValue(Canvas.LeftProperty, 200.0);
                        CommentBlock.SetValue(Canvas.TopProperty, ypos);


                        // アニメーション設定
                        DoubleAnimation myDoubleAnimation = new DoubleAnimation();

                        //　キャンバスの右端から
                        myDoubleAnimation.From = m_Canvas.ActualWidth;
                        //　キャンバスの左端－コメントの長さ＝コメントが全て見きれる位置
                        myDoubleAnimation.To = 0 - CommentBlock.FormattedTextWidth;

                        // コメント滞在時間
                        myDoubleAnimation.Duration = new Duration(TimeSpan.FromMilliseconds(comment.Style.CommentTime));

                        // 繰り返しを無しに設定
                        myDoubleAnimation.AutoReverse = false;

                        //　アニメーションの終了イベントを設定
                        myDoubleAnimation.Completed += (s, _) => CommentScrollCompleted(CommentBlock);

                        //　アニメーションを開始
                        CommentBlock.BeginAnimation(Canvas.LeftProperty, myDoubleAnimation);

                   

                        m_Canvas.Children.Add(CommentBlock);
                    }
                    catch (Exception ex)
                    {
                        _logger.Error($"コメントコントロールの作成に失敗{ ex.Message}");
                    }

                    comment = model.PopComment;
                }

            }

        }
        
        /// <summary>
        /// コメントのアニメーション終了イベント
        /// </summary>
        /// <param name="element"></param>
        private void CommentScrollCompleted(UIElement element)
        {
            try
            {
                //　表示の終わったコメントコントロールをキャンバスから削除
                m_Canvas.Children.Remove(element);
            }
            catch (Exception ex)
            {
                _logger.Error($"例外　{ ex.Message}");
            }
        }

        /// <summary>
        /// 手動削除フラグ
        /// </summary>
        private bool _ManualColse = false;
        /// <summary>
        /// ウィンドウの閉じる。（アプリケーションを終了させない）
        /// </summary>
        public void ManualColse()
        {
            _ManualColse = true;
            Close();

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Closed(object sender, EventArgs e)
        {
            try
            {
                if (_ManualColse == false)
                {
                    Application.Current.Shutdown();
                }
            }
            catch (Exception ex)
            {
                _logger.Error($"例外　{ ex.Message}");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {

                MainWindowViewModel model = DataContext as MainWindowViewModel;

                //　モデルが設定されている場合
                if(model != null)
                {
                    //　位置情報の保存
                    model.CommentWnd_Position = new WindowsPositionEntiy(this);
                }
            }
            catch (Exception ex)
            {
                _logger.Error($"例外　{ ex.Message}");
            }

        }

    
    }

    /// <summary>
    /// キャンバスの背景色カラーコンバーター
    /// </summary>
    public class ColorStringToBrushConverter : IValueConverter
    {
        /// <summary>
        /// ログインスタンス
        /// </summary>
        private static readonly log4net.ILog _logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {

            Brush ret = null;
            string coloerString = value as string;
            //　文字色
            ColorConverter ColorConv = new ColorConverter();
            try
            {
                //　透明化フラグが有効の場合は、NULLが返ってくる。
                if(string.IsNullOrEmpty(coloerString) == false)
                {
                    Color col = (Color)ColorConv.ConvertFrom(coloerString);
                    ret = new SolidColorBrush(col);
                }
            }
            catch (Exception ex)
            {
                _logger.Error($"カラー変換で例外 {ex.Message} CommentStyle_ColorString={value?.ToString()}");
            }
            return ret;
            
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
