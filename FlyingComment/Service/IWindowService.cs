﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlyingComment.Service
{
    interface IWindowService
    {
        void CreateWindow(object context);

        void CloseWindow();
    }
}
