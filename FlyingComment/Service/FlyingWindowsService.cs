using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlyingComment.Service
{
    class FlyingWindowsService : IWindowService
    {
        public void CreateWindow(object context)
        {
            App ap = App.Current as App;
            ap?.CreateFlyingCommentWindow(context);
        }

        public void CloseWindow()
        {
            App ap = App.Current as App;
            ap?.CloseFlyingCommentWindow();
        }

    }
}
