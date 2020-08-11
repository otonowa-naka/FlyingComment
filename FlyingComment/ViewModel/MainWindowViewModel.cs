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


    public class MainWindowViewModel : INotifyPropertyChanged, INotifyDataErrorInfo
    {
        private CommentStyleEntity _CommentStyle = null;
        private CommentWindowConfigurationEntity _CommentWnd = null;
        private YoutubeConnectEntiy _YouTubeConnect = null;
        public WindowsPositionEntiy SettingWindowPosition { set; get; } = null;

        /// <summary>
        /// ログインスタンス
        /// </summary>
        private static readonly log4net.ILog _logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        /// <summary>
        /// コンストラクタ
        /// </summary>
        public MainWindowViewModel(CommentStyleEntity style, CommentWindowConfigurationEntity commentWnd, YoutubeConnectEntiy youTubeConnect, WindowsPositionEntiy settingWindowPosition)
        {
            // APP設定でデータの初期化
            try
            {
                _CommentStyle = style;
                _CommentWnd = commentWnd;
                _YouTubeConnect = youTubeConnect;
                SettingWindowPosition = settingWindowPosition;

                _CommentStyle.PropertyChanged += OnPropertyChanged_CommentStyle;
                _CommentWnd.PropertyChanged += OnPropertyChanged_CommentWnd;
                _YouTubeConnect.PropertyChanged += OnPropertyChanged_YouTubeConnect;

                
            }
            catch (Exception ex)
            {
                _logger.Error($"MainWindowViewModelの例外メッセージ={ex.Message}");

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
        private void OnPropertyChanged_YouTubeConnect(object sender, PropertyChangedEventArgs arg)
        {
            if (arg == null)
            {
                throw new ArgumentException("proarg is null");
            }

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("YouTubeConnect_" + arg.PropertyName));
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs("YouTubeConnect_" + arg.PropertyName));

        }


 

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



 
        #region _CommentStyleプロパティ
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
        #endregion


        #region _CommentWndプロパティ

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
        /// コメントウィンドウの位置とサイズ
        /// </summary>
        public WindowsPositionEntiy CommentWnd_Position
        {
            get
            {
                return _CommentWnd.Position;
            }
            set
            {
                _CommentWnd.Position = value;
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
        #endregion


        #region _YouTubeConnect プロパティ
        /// <summary>
        /// YouTubeAPIKey
        /// </summary>
        public string YouTubeConnect_ApiKey
        {
            get
            {
                return _YouTubeConnect.ApiKey;
            }
            set
            {
                _YouTubeConnect.ApiKey = value;
            }
        }

        /// <summary>
        /// YouTubeのVideoID
        /// </summary>
        public string YouTubeConnect_VideoID
        {
            get
            {
                return _YouTubeConnect.VideoID;
            }
            set
            {
                _YouTubeConnect.VideoID = value;
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

 

    }
}
