﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataVisualizer{
    interface IVisualFeatureHolderPrivate
    {
        Dictionary<string, VisualFeatureBase> Properties
        {
            get;
        }
    }
}
