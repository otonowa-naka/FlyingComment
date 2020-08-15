using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace FlyingComment.Model
{
 

    //------------------------------------------------------------------------------------
    /// <summary>
    /// スレッド引数付き処理関数デリゲート
    /// </summary>
    /// <param type="object" name="aArg">[i] 引数</param>
    /// <param type="CancellationToken" name="token">[i] キャンセルトークン</param>
    /// <returns></returns>
    /// <remarks>
    /// 
    /// History<para/>
    /// </remarks>
    public delegate Action OnRunActionArg(object aArg, CancellationToken token);


    public class DelegateTaskEntity : NotifyChangedBase, IDisposable
    {

        #region IDisposable Support
        private bool disposedValue = false; // 重複する呼び出しを検出するには

        protected virtual void Dispose(bool disposing)
        {
            if (disposedValue == false)
            {
                if (disposing == true)
                {
                    // TODO: マネージ状態を破棄します (マネージ オブジェクト)。
                }

                // キャンセルトークンは、スレッド側でDisposeする。
                CancelTask();

                disposedValue = true;
            }
        }

        ~DelegateTaskEntity()
        {
            // このコードを変更しないでください。クリーンアップ コードを上の Dispose(bool disposing) に記述します。
            Dispose(false);
        }

        // このコードは、破棄可能なパターンを正しく実装できるように追加されました。
        void IDisposable.Dispose()
        {
            // このコードを変更しないでください。クリーンアップ コードを上の Dispose(bool disposing) に記述します。
            Dispose(true);
            // TODO: 上のファイナライザーがオーバーライドされる場合は、次の行のコメントを解除してください。
            // GC.SuppressFinalize(this);
        }
        #endregion

        /// <summary>
        /// 排他オブジェクト
        /// </summary>
        private readonly object _m_LockObject = new object();

        /// <summary>
        /// スレッド 
        /// </summary>
        private Task _DelegateTask = null;
        private Task DelegateTask {
            get
            {
                return _DelegateTask;
            }
            set
            {
                _DelegateTask =  value;
                OnPropertyChanged(nameof(IsRunning));
            }
        }

        /// <summary>
        /// 通信スレッドのキャンセルトークン
        /// </summary>
        private CancellationTokenSource _m_tokenSource = null;


        public bool IsRunning
        {
            get
            {
                bool isRunning = false;
                lock (_m_LockObject)
                {
                    if (DelegateTask != null)
                    {
                        // Completedでなければ実行中状態
                        isRunning = (DelegateTask.IsCompleted == false);
                    }
                }

                return isRunning;

            }
        }

        public void CancelTask()
        {
            lock (_m_LockObject)
            {
                if (_m_tokenSource != null)
                {
                    // キャンセル発行
                    _m_tokenSource.Cancel();
                }
                else
                {
                    // すでにタスク終了済み
                }
            }            
        }

        public bool Wait(int timeout_ms)
        {
            return DelegateTask.Wait(timeout_ms);
        }


        public void Run(OnRunActionArg aOnRunActionArg, object aArg)
        {
            try
            {
                // ヌルチェックします
                if (aOnRunActionArg == null)
                {
                    throw new ArgumentNullException(string.Format("The specified argument cannot be null. name={0}", nameof(aOnRunActionArg)));
                }

                lock (_m_LockObject)
                {
                    if (DelegateTask != null)
                    {
                        // まだタスクが実行中のため、エラーとする
                        throw new InvalidOperationException("The task can not be started because the task is still running.");
                    }

                    // タスク処理開始
                    _m_tokenSource = new CancellationTokenSource();
                    
                    DelegateTask = Task.Run(() =>
                    {
                        // OnRun()後処理
                        Action afterOnRunProc = null;

                        try
                        {
                            afterOnRunProc = aOnRunActionArg(aArg, _m_tokenSource.Token);
                            // OnRun()後処理
                            if (afterOnRunProc != null)
                            {
                                Action tryWrappedAction = () =>
                                {
                                    try
                                    {
                                        afterOnRunProc();
                                    }
                                    catch (Exception ex)
                                    {
                                        // 関数の例外を潰す
                                    }
                                };

                                // メインスレッド側で処理する
                                Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal, tryWrappedAction);
                            }
                        }
                        catch (Exception /*ex*/)
                        {
                           //　アクションの例外は潰す
                        }
                        
                        // 実処理終わったら、スレッドとキャンセルトークンを開放
                        lock (_m_LockObject)
                        {
                            DelegateTask = null;
                            if (_m_tokenSource != null)
                            {
                                _m_tokenSource.Dispose();
                                _m_tokenSource = null;
                            }
                        }


                    });
                }

            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
