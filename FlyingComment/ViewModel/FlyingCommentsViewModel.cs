using FlyingComment.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace FlyingComment.ViewModel
{
    public class FlyingCommentsViewModel: INotifyPropertyChanged
    {
        public CommentWindowConfigurationEntity CommentWnd { private set; get; } = null;
        private CommentQueueEntity CommentQueue { set; get; } = null;

        public FlyingCommentsViewModel(
            CommentWindowConfigurationEntity commentWnd,
            CommentQueueEntity commentQueue
            )
        {
            CommentWnd = commentWnd;
            CommentQueue = commentQueue;

            CommentWnd.PropertyChanged += OnPropertyChanged_CommentWnd;
            CommentQueue.PropertyChanged += OnPropertyChanged_CommentQueue;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged_CommentWnd(object sender, PropertyChangedEventArgs arg)
        {
            if (arg == null)
            {
                throw new ArgumentException("proarg is null");
            }

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CommentWnd) + "_" + arg.PropertyName));

        }

        private void OnPropertyChanged_CommentQueue(object sender, PropertyChangedEventArgs arg)
        {
            if (arg == null)
            {
                throw new ArgumentException("proarg is null");
            }

              PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CommentQueue) + "_" + arg.PropertyName));

        }

        public bool CommentWnd_Stealth {
            get
            {
                return CommentWnd.Stealth;
            }
            private set 
            {
                // 変更しない
            }
        }

        public bool CommentWnd_TopMost
        {
            get
            {
                return CommentWnd.TopMost;
            }
            private set
            {
                // 変更しない
            }
        }

        public bool CommentWnd_Visible
        {
            get
            {
                return CommentWnd.Visible;
            }
            private set
            {
                // 変更しない
            }
        }
        public Color CommentWnd_BackColor
        {
            get
            {
                return CommentWnd.BackColor.Color;
            }
            private set
            {
                // 変更しない
            }
        }

        public CommentTextEntiy PopComment
        {
            get
            {
                return CommentQueue.PopText();
            }
        }
    }
}
