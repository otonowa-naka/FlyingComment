using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Animation;
using OutlineText;

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
        public FlyingCommentsWindow(AppModel model)
        {
            InitializeComponent();

            ///　Model設定
            DataContext = model;

            ///　コメントリストの変更通知を受け取るためにバインディングを設定
            Binding TextListCountBinding = new Binding("TextListCount");
            TextListCountBinding.Mode = BindingMode.OneWay;
            this.SetBinding(TextListCount, TextListCountBinding);

            ///　透明化の変更通知を受け取るためにバインディングを設定
            Binding StealthBinding = new Binding("Stealth");
            StealthBinding.Mode = BindingMode.OneWay;
            this.SetBinding(Stealth, StealthBinding);

            //　ウインドウの位置とサイズを設定
            Left = model.CommentWndRect.Left;
            Top = model.CommentWndRect.Top;
            Width = model.CommentWndRect.Width;
            Height = model.CommentWndRect.Height;
            WindowState = model.CommentWndStste;
            
            ///　透明化の初期状態を設定
            if (model.Stealth == true)
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

        }


        /// <summary>
        /// 飛ばすココメントリストの数が変わったことを受け取る為のプロパティを定義
        /// </summary>
        public static readonly DependencyProperty TextListCount =
            DependencyProperty.Register(
            "TextListCount",                     // プロパティ名
            typeof(int),                         //　プロパティの型情報
            typeof(FlyingCommentsWindow),
                new PropertyMetadata(
                0,                               // デフォルト値の設定
                TextListCountPropertyChanged)    // 変更のイベントハンドラ定義
            );

        /// <summary>
        /// TextListCountの変更イベントハンドラ
        /// </summary>
        /// <param name="ｄOjd"></param>
        /// <param name="eArgs"></param>
        private static void TextListCountPropertyChanged(DependencyObject ｄOjd, DependencyPropertyChangedEventArgs eArgs)
        {
            try
            {
                FlyingCommentsWindow wnd = ｄOjd as FlyingCommentsWindow;
               
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
            "Stealth",                       // プロパティ名
            typeof(bool),                    //　プロパティの型情報
            typeof(FlyingCommentsWindow),
                new PropertyMetadata(
                false,                       // デフォルト値の設定
                StealthPropertyChanged)    　// 変更のイベントハンドラ定義
            );

        /// <summary>
        /// ウインドウの透明化フラグの変更イベントハンドラ
        /// </summary>
        /// <param name="ｄOjd"></param>
        /// <param name="eArgs"></param>
        private static void StealthPropertyChanged(DependencyObject ｄOjd, DependencyPropertyChangedEventArgs eArgs)
        {
            try
            {
                FlyingCommentsWindow wnd = ｄOjd as FlyingCommentsWindow;

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

            AppModel model = DataContext as AppModel;
            if (model != null)
            {
                //　現在の状態と透明化フラグの状態に差異があるか確認
                if (
                    (model.Stealth == true && WindowStyle == WindowStyle.None) ||
                    (model.Stealth == false && WindowStyle == WindowStyle.SingleBorderWindow)

                    )
                {

                    // 差異が無い場合は、何もしない
                }
                else
                {

                    // 透明化の状態と現在のWindowの状態が異なる時

                    //　新しいWindowを作成して、自身のModelを渡す。
                    FlyingCommentsWindow newWind = new FlyingCommentsWindow(DataContext as AppModel);

                    //　位置とサイズをコピー
                    newWind.Top = Top;
                    newWind.Left = Left;
                    newWind.Width = Width;
                    newWind.Height = Height;
                    newWind.WindowState = WindowState;

                    //newWind.DataContext = DataContext;
                    //newWind.Owner = Owner;
                    
                    //　旧ウィンドウを閉じる
                    ManualColse();

                    //　旧ウィンドウとモデルを切断
                    DataContext = null;

                    //　新ウィンドウを表示する
                    newWind.Show();

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
            AppModel model = DataContext as AppModel;

            if (model != null)
            {
                string flytext = model.PopText();
                if(string.IsNullOrEmpty(flytext) == false)
                {
                    //　コメントのテキストコントロール
                    OutlineTextControl CommentBlock = new OutlineTextControl();
                    
                    // 表示文字設定
                    CommentBlock.Text = flytext;
                    // フォント設定
                    CommentBlock.Font = new FontFamily(model.FontName); ;
                    // ボールド設定
                    CommentBlock.Bold = model.FontBald;
                    //　イタリック設定
                    CommentBlock.Italic = model.FontItalic;

                    //　フォントサイズ設定
                    try
                    {
                        FontSizeConverter myFontSizeConverter = new FontSizeConverter();
                        CommentBlock.FontSize = (Double)myFontSizeConverter.ConvertFromString(model.FontSize);
                    }
                    catch (Exception ex)
                    {
                        _logger.Error($"フォントサイズ変換に失敗{ ex.Message}");
                    }
                    
                    //　文字色設定
                    ColorConverter ColorConv = new ColorConverter();
                    try
                    {
                        Color col = (Color)ColorConv.ConvertFrom(model.FontColor);
                        CommentBlock.Fill = new SolidColorBrush(col);
                    }
                    catch (Exception ex)
                    {
                        _logger.Error($"文字色変換に失敗{ ex.Message}");
                    }

                    // テキストの枠線の色
                    try
                    {
                        Color col = (Color)ColorConv.ConvertFrom(model.FontThicknessColor);
                        CommentBlock.Stroke = new SolidColorBrush(col);
                    }
                    catch (Exception ex)
                    {
                        _logger.Error($"文字の縁色変換に失敗{ ex.Message}");
                    }

                    // テキストの枠の太さ
                    try
                    {
                        ushort thic = 0;
                        ushort.TryParse(model.FontThickness, out thic);
                        CommentBlock.StrokeThickness = thic;
                    }
                    catch (Exception ex)
                    {
                        _logger.Error($"文字の縁の太さ変換に失敗{ ex.Message}");
                    }

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
                        try
                        {
                            long time = 0;
                            long.TryParse(model.CommentTime, out time);
                            myDoubleAnimation.Duration = new Duration(TimeSpan.FromMilliseconds(time));

                        }
                        catch (Exception ex)
                        {
                            _logger.Error($"コメント滞在時間の変換に失敗{ ex.Message}");
                            throw;
                        }

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
        private void ManualColse()
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

                AppModel model = DataContext as AppModel;

                //　モデルが設定されている場合
                if(model != null)
                {
                    //　位置情報の保存
                     model.CommentWndRect  = new Rect( Left, Top, Width, Height);
                     model.CommentWndStste = WindowState;
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
                _logger.Error($"カラー変換で例外 {ex.Message} ColorString={value?.ToString()}");
            }
            return ret;
            
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
