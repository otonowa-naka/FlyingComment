using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlyingComment.Model
{
    public class CommentQueueEntity : NotifyChangedBase
    {
        /// <summary>
        /// 排他用ロックオプジェクト
        /// </summary>
        private object _LockObject = new object();

        /// <summary>
        /// コメントのリスト
        /// </summary>
        private List<CommentTextEntiy> _TextList = new List<CommentTextEntiy>();
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
        public void PushText(CommentTextEntiy addText)
        {
            lock (_LockObject)
            {
                _TextList.Add(addText);
            }
            OnPropertyChanged("TextListCount");
        }

        /// <summary>
        /// コメントリストから１行分のコメントを取得
        /// </summary>
        /// <returns></returns>
        public CommentTextEntiy PopText()
        {
            CommentTextEntiy ret = null;
            lock (_LockObject)
            {
                if (_TextList.Count() > 0)
                {
                    ret = _TextList[0];
                    _TextList.RemoveAt(0);
                    OnPropertyChanged("TextListCount");

                }
            }
            return ret;
        }

    }
}
