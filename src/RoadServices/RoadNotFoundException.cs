﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RouteChecker.RoadServices
{
    public class RoadNotFoundException : Exception
    {
        public RoadNotFoundException(string message) : base(message) { }
    }
}
