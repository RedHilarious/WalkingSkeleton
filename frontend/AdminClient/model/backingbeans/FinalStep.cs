﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminClient.model
{
    /// <summary>
    /// Backing bean implementation for FinalStep.
    /// </summary>
    class FinalStep : AbstractFinalStep
    {
        private string _label = "Endzustand";
        public string label { get { return _label; } set { } }
    }
}
