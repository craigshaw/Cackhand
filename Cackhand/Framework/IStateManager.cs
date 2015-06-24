﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cackhand.Framework
{
    internal interface IStateManager
    {
        void RegisterNextState(IState nextState);
    }
}
