using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FlyingComment.Model
{
    public class WindowsPositionEntiy
    {
        public WindowsPositionEntiy()
        {

        }

        public WindowsPositionEntiy( Rect rect, WindowState state) 
        {
            WindowRect = rect;
            State = state;
        }

        public WindowsPositionEntiy(Window wnd)
        {
            WindowRect = new Rect(wnd.Left, wnd.Top, wnd.Width, wnd.Height) ;
            State = wnd.WindowState;
        }


        public Rect WindowRect
        {
            get;      private set;
        } = new Rect(500, 0, 300, 300);

        public WindowState State
        {
            get; private set;
        } = WindowState.Normal;

        public void MoveWindow(Window window)
        {
            window.Left = WindowRect.Left;
            window.Top = WindowRect.Top;
            window.Width = WindowRect.Width;
            window.Height = WindowRect.Height;
            window.WindowState = State;

        }
    }
}
